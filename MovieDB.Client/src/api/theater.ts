import { TTheater } from '../types/theater'
import { apiFetch } from './client'

export function fetchTheaters(): Promise<Array<TTheater>> {
  return apiFetch<Array<TTheater>>('/api/theaters')
}

export function storeTheater(theater: Omit<TTheater, 'id'>): Promise<void> {
  return apiFetch('/api/theaters', { method: 'POST', body: theater })
}
