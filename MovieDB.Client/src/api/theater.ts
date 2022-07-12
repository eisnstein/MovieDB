import { store } from '../services/store'
import { TTheater } from '../types/theater'

export async function fetchTheaters(): Promise<Array<TTheater>> {
  const url = `${import.meta.env.VITE_API_URL}/api/theaters`
  const res = await fetch(url, {
    method: 'GET',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
    },
  })

  return (await res.json()) as Array<TTheater>
}

export async function storeTheater(
  theater: Omit<TTheater, 'id'>
): Promise<boolean> {
  const url = `${import.meta.env.VITE_API_URL}/api/theaters`
  const res = await fetch(url, {
    method: 'POST',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
      'Content-Type': 'application/json;charset=utf-8',
    },
    body: JSON.stringify(theater),
  })

  return res.ok
}
