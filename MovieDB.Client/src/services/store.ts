import { InjectionKey } from 'vue'
import {
  createStore,
  useStore as baseUseStore,
  Store,
  ActionTree,
  MutationTree,
} from 'vuex'
import { login, logout } from '../api/account'
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

const accountJson = LocalStorage.get('account')
const initialState: State = {
  account: accountJson ? JSON.parse(accountJson) : null,
  isAuthenticated: accountJson !== null,
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
    } catch (error: any) {
      commit('loginFailure', error)
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
