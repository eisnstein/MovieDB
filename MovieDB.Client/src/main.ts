import { VueQueryPlugin } from '@tanstack/vue-query'
import { createApp } from 'vue'
import { queryClient } from './services/queryClient'
import router from './services/router'
import { key, store } from './services/store'
import App from './App.vue'
import './index.css'

const app = createApp(App)

app.use(router)
app.use(store, key)
app.use(VueQueryPlugin, { queryClient })

await router.isReady()
app.mount('#app')
