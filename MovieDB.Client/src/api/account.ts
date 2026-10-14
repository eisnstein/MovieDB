import { LocalStorage } from '@/services/localStorage'
import { TAccount } from '@/types/account'
import { apiFetch } from '@/api/client'

export async function login(
  email: string,
  password: string
): Promise<TAccount> {
  const account = await apiFetch<TAccount>('/api/accounts/authenticate', {
    method: 'POST',
    body: { email, password },
    auth: false,
  })

  LocalStorage.set('account', JSON.stringify(account))
  return account
}

export function logout() {
  LocalStorage.remove('account')
}
