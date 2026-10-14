// Values match the MovieGenre enum in the API
export const movieGenres = [
  { text: 'Action', value: 0 },
  { text: 'Comedy', value: 1 },
  { text: 'Drama', value: 2 },
  { text: 'Fantasy', value: 3 },
  { text: 'Horror', value: 4 },
  { text: 'SciFi', value: 5 },
  { text: 'Thriller', value: 6 },
] as const

export type TMovieGenre = (typeof movieGenres)[number]['value']

export type TMovie = {
  id: number
  title: string
  seenAt: string
  imdbIdentifier: string
  genre: TMovieGenre
  rating: number
  posterUrl: string | null
}
