import { createApp } from 'vue'
import router from './services/router'
import { store } from './services/store'
import App from './App.vue'
import './index.css'

const app = createApp(App)

app.use(router).use(store)

await router.isReady()

app.mount('#app')
