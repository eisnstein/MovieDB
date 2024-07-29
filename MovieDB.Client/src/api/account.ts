import { LocalStorage } from '../services/localStorage'
import { TAccount } from '../types/account'

export async function login(
  email: string,
  password: string
): Promise<TAccount> {
  const url = `${import.meta.env.VITE_API_URL}/api/accounts/authenticate`
  const res = await fetch(url, {
    method: 'POST',
    headers: {
      'content-type': 'application/json;charset=UTF-8',
    },
    body: JSON.stringify({ email, password }),
  })

  if (!res.ok) {
    return Promise.reject(await res.json())
  }

  const account = (await res.json()) as TAccount
  LocalStorage.set('account', JSON.stringify(account))
  return Promise.resolve(account)
}

export function logout() {
  LocalStorage.remove('account')
}
