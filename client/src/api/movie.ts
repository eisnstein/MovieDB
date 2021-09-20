import { store } from '../services/store'
import { TMovie } from '../types/movie'

export async function fetchMovies(): Promise<Array<TMovie>> {
  const res = await fetch('http://localhost:4000/api/movies', {
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
  const url = `http://www.omdbapi.com/?i=${imdbIdentifier}&apiKey=`
  const res = await fetch(url, {
    method: 'GET',
    headers: {
      accept: 'application/json',
    },
  })

  const movieData = await res.json()
  return movieData.Poster ?? undefined
}

export async function storeMovie(movie: Omit<TMovie, 'id'>): Promise<any> {
  console.log(movie)
  const res = await fetch('http://localhost:4000/api/movies', {
    method: 'POST',
    headers: {
      Authorization: 'Bearer ' + store.state.account?.jwtToken,
      'Content-Type': 'application/json;charset=utf-8',
    },
    body: JSON.stringify(movie),
  })

  console.log(res)
}
