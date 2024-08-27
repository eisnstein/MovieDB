import { store } from '../services/store'
import { TMovie } from '../types/movie'

export async function fetchMovies(): Promise<Array<TMovie>> {
  const url = `${import.meta.env.VITE_API_URL}/api/movies`
  const res = await fetch(url, {
    method: 'GET',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
    },
  })

  if (!res.ok) {
    return Promise.reject({ message: res.statusText })
  }

  return (await res.json()) as Array<TMovie>
}

export async function fetchMovie(movieId: number): Promise<TMovie> {
  const url = `${import.meta.env.VITE_API_URL}/api/movies/${movieId}`
  const res = await fetch(url, {
    method: 'GET',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
    },
  })

  if (!res.ok) {
    return Promise.reject({ message: 'Could not find movie' })
  }

  const movie = await res.json() as TMovie

  return movie
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

  const movieData = await res.json()
  return movieData.Poster ?? undefined
}

export async function storeMovie(movie: Omit<TMovie, 'id'>): Promise<boolean> {
  const url = `${import.meta.env.VITE_API_URL}/api/movies`
  const res = await fetch(url, {
    method: 'POST',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
      'Content-Type': 'application/json;charset=utf-8',
    },
    body: JSON.stringify(movie),
  })

  return res.ok
}

export async function deleteMovie(movieId: number): Promise<boolean> {
  const url = `${import.meta.env.VITE_API_URL}/api/movies/${movieId}`
  const res = await fetch(url, {
    method: 'DELETE',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
    }
  })

  return res.ok
}
