import { isTokenExpired } from '@/services/jwt'

type ApiClientConfig = {
  getToken: () => string | undefined
  // Gets a new JWT via the refresh token cookie; resolves to undefined if that fails
  refreshToken: () => Promise<string | undefined>
  onUnauthorized: () => void
}

// Set from main.ts, so this module doesn't import the auth store (which imports it)
let config: ApiClientConfig = {
  getToken: () => undefined,
  refreshToken: async () => undefined,
  onUnauthorized: () => {},
}

export function configureApiClient(apiClientConfig: ApiClientConfig) {
  config = apiClientConfig
}

// The API rotates refresh tokens, so parallel requests must share one refresh:
// a second refresh would send the token the first one just replaced
let pendingRefresh: Promise<string | undefined> | null = null

function refreshToken(): Promise<string | undefined> {
  pendingRefresh ??= config.refreshToken().finally(() => {
    pendingRefresh = null
  })
  return pendingRefresh
}

export class ApiError extends Error {
  constructor(
    public status: number,
    message: string,
    public body?: unknown
  ) {
    super(message)
    this.name = 'ApiError'
  }
}

type ApiFetchOptions = Omit<RequestInit, 'body'> & {
  body?: unknown
  // Set to false for endpoints that don't need a token (e.g. login)
  auth?: boolean
  // Set to false to skip the refresh and logout on a 401 (e.g. for the logout call itself)
  handleUnauthorized?: boolean
}

export async function apiFetch<T = void>(
  path: string,
  { body, auth = true, handleUnauthorized = true, headers, ...init }: ApiFetchOptions = {}
): Promise<T> {
  const send = (token: string | undefined) => {
    const requestHeaders = new Headers(headers)
    requestHeaders.set('Accept', 'application/json')

    if (token) {
      requestHeaders.set('Authorization', `Bearer ${token}`)
    }

    if (body !== undefined) {
      requestHeaders.set('Content-Type', 'application/json;charset=utf-8')
    }

    return fetch(`${import.meta.env.VITE_API_URL ?? ''}${path}`, {
      ...init,
      headers: requestHeaders,
      body: body !== undefined ? JSON.stringify(body) : undefined,
    })
  }

  let token = auth ? config.getToken() : undefined
  // Only requests made while logged in can refresh or log out
  const canRefresh = handleUnauthorized && token !== undefined

  // Refresh up front instead of sending a request that's bound to fail
  if (canRefresh && isTokenExpired(token!)) {
    token = await refreshToken()
    if (!token) {
      config.onUnauthorized()
      throw new ApiError(401, 'Your session has expired')
    }
  }

  let res = await send(token)

  // The token may have expired or been revoked in the meantime: refresh once and retry
  if (res.status === 401 && canRefresh) {
    const newToken = await refreshToken()
    if (newToken) {
      res = await send(newToken)
    }
  }

  const data = parseJson(await res.text())

  if (!res.ok) {
    if (res.status === 401 && canRefresh) {
      config.onUnauthorized()
    }

    const message = data?.message ?? data?.title ?? `Request failed with status ${res.status}`
    throw new ApiError(res.status, message, data)
  }

  return data as T
}

// Turns an error into a message for the UI, including validation errors
export function errorMessage(error: unknown, fallback = 'Something went wrong'): string {
  if (error instanceof ApiError) {
    const errors = (error.body as any)?.errors
    if (errors && typeof errors === 'object') {
      return Object.values(errors).flat().join('\n')
    }
  }

  return error instanceof Error && error.message ? error.message : fallback
}

function parseJson(text: string): any {
  if (!text) {
    return undefined
  }

  try {
    return JSON.parse(text)
  } catch {
    // e.g. an HTML error page from a proxy
    return undefined
  }
}
