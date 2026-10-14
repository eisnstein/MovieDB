// Values match the ConcertGenre enum in the API
export const concertGenres = [
  { text: 'Rock', value: 0 },
  { text: 'Classic', value: 1 },
  { text: 'Reggae', value: 2 },
  { text: 'Pop', value: 3 },
  { text: 'Latin', value: 4 },
  { text: 'Electro', value: 5 },
  { text: 'DrumnBass', value: 6 },
] as const

export type TConcertGenre = (typeof concertGenres)[number]['value']

export type TConcert = {
  id: number
  title: string
  seenAt: string
  location: string
  genre: TConcertGenre
  rating: number
}
