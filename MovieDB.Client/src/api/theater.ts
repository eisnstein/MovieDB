import { TTheater } from '../types/theater'
import { apiFetch } from './client'

export function fetchTheaters(): Promise<Array<TTheater>> {
  return apiFetch<Array<TTheater>>('/api/theaters')
}

export async function storeTheater(
  theater: Omit<TTheater, 'id'>
): Promise<boolean> {
  try {
    await apiFetch('/api/theaters', { method: 'POST', body: theater })
    return true
  } catch {
    return false
  }
}
