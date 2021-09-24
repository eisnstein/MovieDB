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

  return (await res.json()) as Array<TMovie>
}

export async function fetchMoviePoster(
  imdbIdentifier: string
): Promise<string | undefined> {
  const url = `http://www.omdbapi.com/?i=${imdbIdentifier}&apiKey=${
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
