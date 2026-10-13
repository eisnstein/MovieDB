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

export function storeMovie(movie: Omit<TMovie, 'id'>): Promise<void> {
  return apiFetch('/api/movies', { method: 'POST', body: movie })
}

export function updateMovie(
  id: number,
  movie: Omit<TMovie, 'id'>
): Promise<void> {
  return apiFetch(`/api/movies/${id}`, { method: 'PUT', body: movie })
}

export function deleteMovie(movieId: number): Promise<void> {
  return apiFetch(`/api/movies/${movieId}`, { method: 'DELETE' })
}
