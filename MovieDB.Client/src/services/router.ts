import { createRouter, createWebHistory } from 'vue-router'
import IndexConcert from '../pages/concerts/Index.vue'
import CreateConcert from '../pages/concerts/Create.vue'
import IndexMovie from '../pages/movies/Index.vue'
import CreateMovie from '../pages/movies/Create.vue'
import ShowMovie from '../pages/movies/Show.vue'
import EditMovie from '../pages/movies/Edit.vue'
import IndexTheater from '../pages/theaters/Index.vue'
import CreateTheater from '../pages/theaters/Create.vue'
import Login from '../pages/Login.vue'
import { store } from './store'

const routes = [
  { path: '/', component: IndexMovie },
  { path: '/concerts', component: IndexConcert },
  { path: '/concerts/new', component: CreateConcert },
  { path: '/login', component: Login },
  { path: '/movies', component: IndexMovie },
  { path: '/movies/new', component: CreateMovie },
  { path: '/movies/:id(\\d+)', name: 'movies-show', component: ShowMovie },
  { path: '/movies/:id(\\d+)/edit', name: 'movies-edit', component: EditMovie },
  { path: '/theaters', component: IndexTheater },
  { path: '/theaters/new', component: CreateTheater },
  { path: '/:pathMatch(.*)*', redirect: '/' },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
  strict: true,
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
