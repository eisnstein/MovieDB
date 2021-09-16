import { TAccount } from 'src/types/account'

export async function login(
  email: string,
  password: string
): Promise<TAccount> {
  const res = await fetch('http://localhost:4000/api/accounts/authenticate', {
    method: 'POST',
    body: JSON.stringify({ email, password }),
    headers: {
      'Content-Type': 'application/json',
    },
  })

  return await res.json()
}
