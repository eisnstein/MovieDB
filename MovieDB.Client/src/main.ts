import { QueryClient, VueQueryPlugin } from '@tanstack/vue-query'
import { createApp } from 'vue'
import { ApiError } from './api/client'
import router from './services/router'
import { key, store } from './services/store'
import App from './App.vue'
import './index.css'

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: (failureCount, error) => {
        // Client errors (401, 404, ...) won't succeed on retry
        if (error instanceof ApiError && error.status < 500) {
          return false
        }

        return failureCount < 3
      },
    },
  },
})

const app = createApp(App)

app.use(router)
app.use(store, key)
app.use(VueQueryPlugin, { queryClient })

await router.isReady()
app.mount('#app')
