import { TConcert } from '../types/concert'
import { apiFetch } from './client'

export function fetchConcerts(): Promise<Array<TConcert>> {
  return apiFetch<Array<TConcert>>('/api/concerts')
}

export async function storeConcert(
  concert: Omit<TConcert, 'id'>
): Promise<boolean> {
  try {
    await apiFetch('/api/concerts', { method: 'POST', body: concert })
    return true
  } catch {
    return false
  }
}
