import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import { login as apiLogin, logout as apiLogout } from '../api/account'
import { errorMessage } from '../api/client'
import { TAccount } from '../types/account'
import { LocalStorage } from './localStorage'
import { queryClient } from './queryClient'

type TAlert = {
  type: 'success' | 'error'
  message: string
}

function isTokenExpired(jwtToken: string): boolean {
  try {
    const payload = jwtToken.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    const { exp } = JSON.parse(atob(payload))
    return typeof exp === 'number' && exp * 1000 <= Date.now()
  } catch {
    return true
  }
}

function loadAccount(): TAccount | null {
  const accountJson = LocalStorage.get('account')
  if (!accountJson) {
    return null
  }

  try {
    const account = JSON.parse(accountJson) as TAccount
    if (!isTokenExpired(account.jwtToken)) {
      return account
    }
  } catch {
    // Corrupt entry, treat as logged out
  }

  LocalStorage.remove('account')
  return null
}

export const useAuthStore = defineStore('auth', () => {
  const account = ref<TAccount | null>(loadAccount())
  const loading = ref(false)
  const alert = ref<TAlert | null>(null)
  const isAuthenticated = computed(() => account.value !== null)

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

  // App.vue redirects to /login once isAuthenticated turns false
  function logout() {
    apiLogout()
    // Drop cached server data so the next user doesn't see it
    queryClient.clear()
    account.value = null
    alert.value = null
  }

  return { account, loading, alert, isAuthenticated, login, logout }
})
