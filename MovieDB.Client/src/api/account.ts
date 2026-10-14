import { TAccount } from '@/types/account'
import { apiFetch } from '@/api/client'

// Also sets the refresh token as an HttpOnly cookie
export function login(email: string, password: string): Promise<TAccount> {
  return apiFetch<TAccount>('/api/accounts/authenticate', {
    method: 'POST',
    body: { email, password },
    auth: false,
  })
}

// Sends the refresh token cookie; the API rotates it and returns a new JWT
export async function refreshToken(): Promise<string> {
  const { jwtToken } = await apiFetch<{ jwtToken: string }>('/api/accounts/refresh-token', {
    method: 'POST',
    auth: false,
  })
  return jwtToken
}

// Revokes the refresh token cookie, so it can't be used after logging out
export function revokeToken(): Promise<void> {
  return apiFetch('/api/accounts/revoke-token', {
    method: 'POST',
    body: {},
    handleUnauthorized: false,
  })
}
