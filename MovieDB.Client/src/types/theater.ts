export type TTheater = {
  id: number
  title: string
  seenAt: string
  location: string
  genre: number
  rating: number
}

export const theaterGenres = [
  { text: 'Opera', value: 0 },
  { text: 'Operette', value: 1 },
  { text: 'Musical', value: 2 },
  { text: 'Theater', value: 3 },
  { text: 'Ballet', value: 4 },
]
