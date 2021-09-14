import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { TAccount } from '../types/account'

export interface State {
  account: TAccount | null
  isAuthenticated: boolean
  loading: boolean
}

export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
  state: {
    account: null,
    isAuthenticated: false,
    loading: false,
  },
  mutations: {
    loginSuccess(state, payload: { account: TAccount }) {
      state.account = payload.account
      state.isAuthenticated = true
      state.loading = false
    },
    loginFailure(state) {
      state.account = null
      state.isAuthenticated = false
      state.loading = false
    },
    setLoading(state, payload: boolean) {
      state.loading = payload
    },
  },
  strict: true,
})

export function useStore() {
  return baseUseStore(key)
}
