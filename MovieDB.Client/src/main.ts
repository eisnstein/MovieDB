import { VueQueryPlugin } from '@tanstack/vue-query'
import { createApp } from 'vue'
import router from './services/router'
import { key, store } from './services/store'
import App from './App.vue'
import './index.css'

const app = createApp(App)

app.use(router)
app.use(store, key)
app.use(VueQueryPlugin)

await router.isReady()
app.mount('#app')
