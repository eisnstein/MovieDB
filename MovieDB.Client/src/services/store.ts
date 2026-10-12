import { InjectionKey } from 'vue'
import {
  createStore,
  useStore as baseUseStore,
  Store,
  ActionTree,
  MutationTree,
} from 'vuex'
import { login, logout } from '../api/account'
import { ApiError } from '../api/client'
import { TAccount } from '../types/account'
import { LocalStorage } from './localStorage'
import router from './router'

type TAlert = 'success' | 'error'

export interface State {
  account: TAccount | null
  isAuthenticated: boolean
  loading: boolean
  alert: {
    type: TAlert
    message: string
  } | null
}

export const key: InjectionKey<Store<State>> = Symbol()

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

const storedAccount = loadAccount()
const initialState: State = {
  account: storedAccount,
  isAuthenticated: storedAccount !== null,
  loading: false,
  alert: null,
}

const actions: ActionTree<State, State> = {
  async login({ commit }, payload: { email: string; password: string }) {
    commit('setLoading', true)
    commit('clearAlert')

    try {
      const account = await login(payload.email, payload.password)
      commit('loginSuccess', { account })
      router.push('/movies')
    } catch (error) {
      const validationErrors =
        error instanceof ApiError ? (error.body as any)?.errors : undefined

      if (validationErrors) {
        const messages = Object.values(validationErrors).flat().join('\n')
        commit('loginFailure', { message: messages })
      } else {
        const message = error instanceof Error ? error.message : 'Login failed'
        commit('loginFailure', { message })
      }
    }
  },
  logout({ commit }) {
    logout()
    commit('logout')
    router.push('/login')
  },
  setLoading({ commit }, value: boolean) {
    commit('setLoading', value)
  },
  clearAlert({ commit }) {
    commit('clearAlert')
  },
}

const mutations: MutationTree<State> = {
  loginSuccess(state, payload: { account: TAccount }) {
    state.account = payload.account
    state.isAuthenticated = true
    state.loading = false
  },
  loginFailure(state, error: { message: string }) {
    state.account = null
    state.isAuthenticated = false
    state.loading = false
    state.alert = {
      type: 'error',
      message: error.message,
    }
  },
  logout(state) {
    state.account = null
    state.isAuthenticated = false
    state.loading = false
    state.alert = null
  },
  setLoading(state, payload: boolean) {
    state.loading = payload
  },
  setAlert(state, alert: { type: TAlert; message: string }) {
    state.alert = alert
  },
  clearAlert(state) {
    state.alert = null
  },
}

export const store = createStore<State>({
  state: initialState,
  mutations,
  actions,
  strict: true,
})

export function useStore() {
  return baseUseStore(key)
}
