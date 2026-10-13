import { TConcert } from '../types/concert'
import { apiFetch } from './client'

export function fetchConcerts(): Promise<Array<TConcert>> {
  return apiFetch<Array<TConcert>>('/api/concerts')
}

export function storeConcert(concert: Omit<TConcert, 'id'>): Promise<void> {
  return apiFetch('/api/concerts', { method: 'POST', body: concert })
}
