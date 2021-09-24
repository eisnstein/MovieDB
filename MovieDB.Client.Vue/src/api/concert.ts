import { store } from '../services/store'
import { TConcert } from '../types/concert'

export async function fetchConcerts(): Promise<Array<TConcert>> {
  const url = `${import.meta.env.VITE_API_URL}/api/concerts`
  const res = await fetch(url, {
    method: 'GET',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
    },
  })

  return (await res.json()) as Array<TConcert>
}

export async function storeConcert(
  concert: Omit<TConcert, 'id'>
): Promise<boolean> {
  const url = `${import.meta.env.VITE_API_URL}/api/concerts`
  const res = await fetch(url, {
    method: 'POST',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
      'Content-Type': 'application/json;charset=utf-8',
    },
    body: JSON.stringify(concert),
  })

  return res.ok
}
