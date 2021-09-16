import { login } from '../api/api'
import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { TAccount } from '../types/account'
import { LocalStorage } from './localStorage'

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

export const store = createStore<State>({
  state: initialState,
  mutations: {
    loginSuccess(state, payload: { account: TAccount }) {
      state.account = payload.account
      state.isAuthenticated = true
      state.loading = false
    },
    loginFailure(state, error: any) {
      state.account = null
      state.isAuthenticated = false
      state.loading = false
      state.alert = {
        type: 'success',
        message: error,
      }
    },
    logout(state) {
      state.account = null
      state.isAuthenticated = false
      state.loading = false
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
  },
  actions: {
    async login({ commit }, payload: { email: string; password: string }) {
      try {
        const account = await login(payload.email, payload.password)
        LocalStorage.set('account', JSON.stringify(account))
        commit('loginSuccess', { account })
      } catch (error: any) {
        console.log(error)
        commit('loginFailure', error)
      }
    },
    logout({ commit }) {
      LocalStorage.remove('account')
      commit('logout')
    },
    setLoading({ commit }, value: boolean) {
      commit('setLoading', value)
    },
    clearAlert({ commit }) {
      commit('clearAlert')
    },
  },
  strict: true,
})

export function useStore() {
  return baseUseStore(key)
}
