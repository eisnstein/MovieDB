import { store } from '../services/store'

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
}

export async function apiFetch<T = void>(
  path: string,
  { body, auth = true, headers, ...init }: ApiFetchOptions = {}
): Promise<T> {
  const requestHeaders = new Headers(headers)
  requestHeaders.set('Accept', 'application/json')

  if (auth && store.state.account) {
    requestHeaders.set('Authorization', `Bearer ${store.state.account.jwtToken}`)
  }

  if (body !== undefined) {
    requestHeaders.set('Content-Type', 'application/json;charset=utf-8')
  }

  const res = await fetch(`${import.meta.env.VITE_API_URL}${path}`, {
    ...init,
    headers: requestHeaders,
    body: body !== undefined ? JSON.stringify(body) : undefined,
  })

  const data = parseJson(await res.text())

  if (!res.ok) {
    if (res.status === 401 && auth && store.state.isAuthenticated) {
      store.dispatch('logout')
    }

    const message = data?.message ?? data?.title ?? `Request failed with status ${res.status}`
    throw new ApiError(res.status, message, data)
  }

  return data as T
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
