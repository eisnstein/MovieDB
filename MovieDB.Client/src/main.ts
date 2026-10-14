import { VueQueryPlugin } from '@tanstack/vue-query'
import { createPinia } from 'pinia'
import { createApp } from 'vue'
import { configureApiClient } from '@/api/client'
import { queryClient } from '@/services/queryClient'
import router from '@/services/router'
import { useAuthStore } from '@/services/store'
import App from '@/App.vue'
import './index.css'

const app = createApp(App)

// Pinia must be installed before the router, whose guard uses the auth store
app.use(createPinia())

const authStore = useAuthStore()
configureApiClient({
  getToken: () => authStore.account?.jwtToken,
  onUnauthorized: () => authStore.logout(),
})

app.use(router)
app.use(VueQueryPlugin, { queryClient })

await router.isReady()
app.mount('#app')
