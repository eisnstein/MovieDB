import { createRouter, createWebHistory } from 'vue-router'
import IndexConcert from '../pages/concerts/Index.vue'
import ConcertForm from '../components/concerts/ConcertForm.vue'
import IndexMovie from '../pages/movies/Index.vue'
import NewMovie from '../pages/movies/New.vue'
import ShowMovie from '../pages/movies/Show.vue'
import IndexTheater from '../pages/theaters/Index.vue'
import TheaterForm from '../components/theaters/TheaterForm.vue'
import Login from '../components/Login.vue'
import { store } from './store'

const routes = [
  { path: '/', component: IndexMovie },
  { path: '/concerts', component: IndexConcert },
  { path: '/concerts/new', component: ConcertForm },
  { path: '/login', component: Login },
  { path: '/movies', component: IndexMovie },
  { path: '/movies/new', component: NewMovie },
  { path: '/movies/:id(\\d+)', name: 'movies-show', component: ShowMovie },
  { path: '/theaters', component: IndexTheater },
  { path: '/theaters/new', component: TheaterForm },
  { path: '/:pathMatch(.*)*', redirect: '/' },
]

const router = createRouter({
  history: createWebHistory(),
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
