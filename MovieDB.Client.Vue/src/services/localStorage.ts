type TIdentifiers = 'account'

export class LocalStorage {
  static set(name: TIdentifiers, value: string) {
    localStorage.setItem(name, value)
  }

  static get(name: TIdentifiers) {
    return localStorage.getItem(name)
  }

  static remove(name: TIdentifiers) {
    localStorage.removeItem(name)
  }
}
