// Case-insensitive match anywhere in the title, e.g. "ring" finds "The Lord of the Rings"
export function filterByTitle<T extends { title: string }>(items: T[], search: string): T[] {
  const query = search.trim().toLowerCase()
  if (!query) {
    return items
  }

  return items.filter((item) => item.title.toLowerCase().includes(query))
}
