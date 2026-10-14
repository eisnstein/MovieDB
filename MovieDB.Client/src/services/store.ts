import { defineStore } from 'pinia'
import { computed, ref, watch } from 'vue'
import { login as apiLogin, refreshToken as apiRefreshToken, revokeToken as apiRevokeToken } from '@/api/account'
import { errorMessage } from '@/api/client'
import { TAccount } from '@/types/account'
import { LocalStorage } from '@/services/localStorage'
import { queryClient } from '@/services/queryClient'

type TAlert = {
  type: 'success' | 'error'
  message: string
}

// An expired JWT is kept: the first API call renews it with the refresh token cookie
function loadAccount(): TAccount | null {
  const accountJson = LocalStorage.get('account')
  if (!accountJson) {
    return null
  }

  try {
    return JSON.parse(accountJson) as TAccount
  } catch {
    // Corrupt entry, treat as logged out
    LocalStorage.remove('account')
    return null
  }
}

export const useAuthStore = defineStore('auth', () => {
  const account = ref<TAccount | null>(loadAccount())
  const loading = ref(false)
  const alert = ref<TAlert | null>(null)
  const isAuthenticated = computed(() => account.value !== null)

  // Keep localStorage in sync on login, token refresh and logout
  watch(account, (value) => {
    if (value) {
      LocalStorage.set('account', JSON.stringify(value))
    } else {
      LocalStorage.remove('account')
    }
  })

  async function login(email: string, password: string) {
    loading.value = true
    alert.value = null

    try {
      account.value = await apiLogin(email, password)
    } catch (error) {
      account.value = null
      alert.value = { type: 'error', message: errorMessage(error, 'Login failed') }
    } finally {
      loading.value = false
    }
  }

  // Called by the API client; resolves to undefined if the session can't be renewed
  async function refresh(): Promise<string | undefined> {
    try {
      const jwtToken = await apiRefreshToken()
      // Logged out while the refresh was running
      if (!account.value) {
        return undefined
      }

      account.value = { ...account.value, jwtToken }
      return jwtToken
    } catch {
      return undefined
    }
  }

  // App.vue redirects to /login once isAuthenticated turns false
  function logout({ revoke = true } = {}) {
    if (revoke && account.value) {
      // Sent with the current token, so it has to start before the account is cleared
      apiRevokeToken().catch(() => {
        // The session is ending anyway
      })
    }

    // Drop cached server data so the next user doesn't see it
    queryClient.clear()
    account.value = null
    alert.value = null
  }

  return { account, loading, alert, isAuthenticated, login, refresh, logout }
})
