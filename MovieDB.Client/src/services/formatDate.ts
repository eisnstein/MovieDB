const formatter = new Intl.DateTimeFormat('de-AT', {
  year: 'numeric',
  month: 'numeric',
  day: 'numeric',
})

// Intl.DateTimeFormat.format throws a RangeError on invalid dates,
// so a single malformed value would otherwise break a whole list
export function formatDate(value: string | null | undefined): string {
  const date = value ? Date.parse(value) : NaN
  return Number.isNaN(date) ? '' : formatter.format(date)
}
