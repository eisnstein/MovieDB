import { createRouter, createWebHashHistory } from 'vue-router'
import Movies from '../components/movies/Movies.vue'
import Login from '../components/Login.vue'
import { store } from './store'

const routes = [
  { path: '/', component: Movies },
  { path: '/login', component: Login },
  { path: '/movies', component: Movies },
  { path: '/concerts', component: Movies },
  { path: '/theaters', component: Movies },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes,
})

router.beforeEach((to, from, next) => {
  if (to.path !== '/login' && !store.state.isAuthenticated) {
    next('/login')
  } else if (to.path === '/login' && store.state.isAuthenticated) {
    next('/')
  } else {
    next()
  }
})

export default router
