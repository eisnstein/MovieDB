import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/services/store'

// Pages are loaded on demand, so the first load only fetches what it shows
const IndexMovie = () => import('@/pages/movies/Index.vue')

const routes = [
  { path: '/', component: IndexMovie },
  { path: '/concerts', component: () => import('@/pages/concerts/Index.vue') },
  { path: '/concerts/new', component: () => import('@/pages/concerts/Create.vue') },
  { path: '/login', component: () => import('@/pages/Login.vue') },
  { path: '/movies', component: IndexMovie },
  { path: '/movies/new', component: () => import('@/pages/movies/Create.vue') },
  { path: '/movies/:id(\\d+)', name: 'movies-show', component: () => import('@/pages/movies/Show.vue') },
  { path: '/movies/:id(\\d+)/edit', name: 'movies-edit', component: () => import('@/pages/movies/Edit.vue') },
  { path: '/theaters', component: () => import('@/pages/theaters/Index.vue') },
  { path: '/theaters/new', component: () => import('@/pages/theaters/Create.vue') },
  { path: '/:pathMatch(.*)*', redirect: '/' },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
  strict: true,
})

router.beforeEach((to) => {
  const { isAuthenticated } = useAuthStore()

  if (to.path !== '/login' && !isAuthenticated) {
    // Remember where the user wanted to go, Login.vue sends them back after signing in
    return to.fullPath === '/' ? '/login' : { path: '/login', query: { redirect: to.fullPath } }
  }

  if (to.path === '/login' && isAuthenticated) {
    return '/'
  }
})

export default router
