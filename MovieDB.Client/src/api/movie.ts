import { TMovie } from '../types/movie'
import { apiFetch } from './client'

export function fetchMovies(): Promise<Array<TMovie>> {
  return apiFetch<Array<TMovie>>('/api/movies')
}

export function fetchMovie(movieId: number): Promise<TMovie> {
  return apiFetch<TMovie>(`/api/movies/${movieId}`)
}

export async function fetchMoviePoster(
  imdbIdentifier: string
): Promise<string | undefined> {
  const url = `https://www.omdbapi.com/?i=${imdbIdentifier}&apiKey=${
    import.meta.env.VITE_OMDB_API_KEY
  }`
  const res = await fetch(url, {
    method: 'GET',
    headers: {
      accept: 'application/json',
    },
  })

  if (!res.ok) {
    return Promise.reject({ message: 'Could not load poster data' })
  }

  const movieData = await res.json()
  return movieData.Poster ?? undefined
}

export async function storeMovie(movie: Omit<TMovie, 'id'>): Promise<boolean> {
  try {
    await apiFetch('/api/movies', { method: 'POST', body: movie })
    return true
  } catch {
    return false
  }
}

export async function updateMovie(
  id: number,
  movie: Omit<TMovie, 'id'>
): Promise<boolean> {
  try {
    await apiFetch(`/api/movies/${id}`, { method: 'PUT', body: movie })
    return true
  } catch {
    return false
  }
}

export async function deleteMovie(movieId: number): Promise<boolean> {
  try {
    await apiFetch(`/api/movies/${movieId}`, { method: 'DELETE' })
    return true
  } catch {
    return false
  }
}
