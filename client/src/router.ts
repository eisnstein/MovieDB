import { createRouter, createWebHashHistory } from 'vue-router'
import Movies from './components/movies/Movies.vue'

const routes = [
  { path: '/', component: Movies },
  { path: '/movies', component: Movies },
  { path: '/concerts', component: Movies },
  { path: '/theaters', component: Movies },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes,
})

export default router
