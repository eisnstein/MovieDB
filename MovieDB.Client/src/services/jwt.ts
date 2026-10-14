// Reads the "exp" claim; a token that can't be parsed counts as expired
export function isTokenExpired(jwtToken: string): boolean {
  try {
    const payload = jwtToken.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    const { exp } = JSON.parse(atob(payload))
    return typeof exp === 'number' && exp * 1000 <= Date.now()
  } catch {
    return true
  }
}
