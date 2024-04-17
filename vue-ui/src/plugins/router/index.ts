import type { App } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import { routes } from './routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

router.beforeEach(async (to, from, next) => {
  console.log(`loading page ${to.path}`)
  if (to.path === '/login') {
    next()

    return
  }
  if (!to.meta.requireAuth || localStorage.getItem('isLoggedIn')) {
    next()

    return
  }

  next('/login')
})
export default function (app: App) {
  app.use(router)
}

export { router }
