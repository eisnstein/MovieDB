<script setup lang="ts">
import { onMounted, onUnmounted, ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/services/store'
import profileImgUrl from '@/assets/profile.jpg'

const authStore = useAuthStore()
const route = useRoute()
const open = ref(false)
const openMobileMenu = ref(false)
const accountMenu = ref<HTMLElement | null>(null)

const links = [
  { to: '/movies', label: 'Movies' },
  { to: '/concerts', label: 'Concerts' },
  { to: '/theaters', label: 'Theaters' },
]

// Close both menus after navigating
watch(() => route.fullPath, () => {
  open.value = false
  openMobileMenu.value = false
})

function onDocumentClick(event: MouseEvent) {
  if (open.value && !accountMenu.value?.contains(event.target as Node)) {
    open.value = false
  }
}

function onKeydown(event: KeyboardEvent) {
  if (event.key === 'Escape') {
    open.value = false
    openMobileMenu.value = false
  }
}

onMounted(() => {
  document.addEventListener('click', onDocumentClick)
  document.addEventListener('keydown', onKeydown)
})

onUnmounted(() => {
  document.removeEventListener('click', onDocumentClick)
  document.removeEventListener('keydown', onKeydown)
})

function logout() {
  open.value = false
  openMobileMenu.value = false
  authStore.logout()
}
</script>

<template>
  <nav class="fixed top-0 z-10 w-full bg-gray-800">
    <div class="container mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between h-16">
        <div class="flex items-center">
          <div class="shrink-0">
            <router-link to="/" aria-label="Home">
              <i class="fad fa-dice-d10 fa-2x" style="color: dodgerblue;"></i>
            </router-link>
          </div>
          <div class="hidden md:block">
            <div class="ml-10 flex items-baseline gap-4">
              <router-link v-for="link in links" :key="link.to" :to="link.to" active-class="text-white bg-gray-700"
                           class="px-3 py-2 rounded-md text-sm font-medium text-gray-300 hover:text-white hover:bg-gray-700 focus:outline-hidden">
                {{ link.label }}
              </router-link>
            </div>
          </div>
        </div>
        <div class="hidden md:block">
          <div class="ml-4 flex items-center md:ml-6">
            <div ref="accountMenu" class="ml-3 relative">
              <button
                type="button"
                aria-label="Account menu"
                aria-haspopup="true"
                :aria-expanded="open"
                class="max-w-xs flex items-center text-sm rounded-full text-white focus:outline-hidden focus:shadow-solid"
                @click="open = !open">
                <img class="h-8 w-8 rounded-full" :src="profileImgUrl" alt=""/>
              </button>
              <div v-show="open" class="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg">
                <div class="py-1 rounded-md bg-white shadow-xs">
                  <div v-if="authStore.account" class="px-4 py-2 text-xs text-gray-500 truncate">{{ authStore.account.email }}</div>
                  <button type="button" @click="logout" class="block w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">
                    Sign out
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="-mr-2 flex md:hidden">
          <button
            type="button"
            :aria-label="openMobileMenu ? 'Close menu' : 'Open menu'"
            :aria-expanded="openMobileMenu"
            @click="openMobileMenu = !openMobileMenu"
            class="inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-white hover:bg-gray-700 focus:outline-hidden focus:bg-gray-700 focus:text-white">
            <svg class="h-6 w-6" stroke="currentColor" fill="none" viewBox="0 0 24 24" aria-hidden="true">
              <path v-if="!openMobileMenu" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"/>
              <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
            </svg>
          </button>
        </div>
      </div>
    </div>
    <div v-show="openMobileMenu" class="md:hidden">
      <div class="px-2 pt-2 pb-3 sm:px-3 space-y-1">
        <router-link v-for="link in links" :key="link.to" :to="link.to" active-class="text-white bg-gray-900"
           class="block px-3 py-2 rounded-md text-base font-medium text-gray-300 hover:text-white hover:bg-gray-700 focus:outline-hidden focus:text-white focus:bg-gray-700">
          {{ link.label }}
        </router-link>
      </div>
      <div class="pt-4 pb-3 border-t border-gray-700">
        <div class="flex items-center px-5">
          <div class="shrink-0">
            <img class="h-10 w-10 rounded-full" :src="profileImgUrl" alt=""/>
          </div>
          <div class="ml-3">
            <div class="text-sm font-medium leading-none text-gray-400">{{ authStore.account?.email ?? '' }}</div>
          </div>
        </div>
        <div class="mt-3 px-2">
          <button type="button" @click="logout"
             class="mt-1 block w-full text-left px-3 py-2 rounded-md text-base font-medium text-gray-400 hover:text-white hover:bg-gray-700 focus:outline-hidden focus:text-white focus:bg-gray-700">
            Sign out
          </button>
        </div>
      </div>
    </div>
  </nav>
</template>
