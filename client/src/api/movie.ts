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
