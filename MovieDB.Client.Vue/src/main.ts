import { createApp } from 'vue'
import router from './services/router'
import { key, store } from './services/store'
import App from './App.vue'
import './index.css'

const app = createApp(App).use(router).use(store, key)

await router.isReady()
app.mount('#app')
