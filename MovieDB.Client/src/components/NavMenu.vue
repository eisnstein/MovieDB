<script setup lang="ts">
import { ref } from 'vue'
import { useStore } from '../services/store';
import profileImgUrl from '../assets/profile.jpg'

const store = useStore()
const open = ref(false)
const openMobileMenu = ref(false)

function logout() {
  store.dispatch('logout')
}
</script>

<template>
  <nav class="fixed top-0 z-10 w-full bg-gray-800">
    <div class="container mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between h-16">
        <div class="flex items-center">
          <div class="shrink-0">
            <router-link to="/">
              <i class="fad fa-dice-d10 fa-2x" style="color: dodgerblue;"></i>
            </router-link>
          </div>
          <div class="hidden md:block">
            <div class="ml-10 flex items-baseline">
              <router-link to="/movies" active-class="text-white bg-gray-700"
                           class="px-3 py-2 rounded-md text-sm font-medium text-gray-300 hover:text-white hover:bg-gray-700 focus:outline-hidden">
                Movies
              </router-link>
              <router-link to="/concerts" active-class="text-white bg-gray-700"
                           class="ml-4 px-3 py-2 rounded-md text-sm font-medium text-gray-300 hover:text-white hover:bg-gray-700 focus:outline-hidden">
                Concerts
              </router-link>
              <router-link to="/theaters" active-class="text-white bg-gray-700"
                           class="ml-4 px-3 py-2 rounded-md text-sm font-medium text-gray-300 hover:text-white hover:bg-gray-700 focus:outline-hidden">
                Theaters
              </router-link>
            </div>
          </div>
        </div>
        <div class="hidden md:block">
          <div class="ml-4 flex items-center md:ml-6">
            <div @click="open = !open" class="ml-3 relative">
              <button
                class="max-w-xs flex items-center text-sm rounded-full text-white focus:outline-hidden focus:shadow-solid">
                <img class="h-8 w-8 rounded-full" :src="profileImgUrl" alt=""/>
              </button>
              <div :class="{'hidden': !open, 'block': open}"
                   class="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg">
                <div class="py-1 rounded-md bg-white shadow-xs">
                  <a href="#" @click="logout" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">
                    Sign Out
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="-mr-2 flex md:hidden">
          <button
            @click="openMobileMenu = !openMobileMenu"
            class="inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-white hover:bg-gray-700 focus:outline-hidden focus:bg-gray-700 focus:text-white">
            <svg class="h-6 w-6" stroke="currentColor" fill="none" viewBox="0 0 24 24">
              <path :class="{'hidden': openMobileMenu, 'inline-flex': !openMobileMenu }" stroke-linecap="round" stroke-linejoin="round"
                    stroke-width="2" d="M4 6h16M4 12h16M4 18h16"/>
              <path :class="{'hidden': !openMobileMenu, 'inline-flex': openMobileMenu }" stroke-linecap="round" stroke-linejoin="round"
                    stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
            </svg>
          </button>
        </div>
      </div>
    </div>
    <div :class="{'hidden': !openMobileMenu}">
      <div class="px-2 pt-2 pb-3 sm:px-3">
        <router-link to="/movies"
                     active-class="text-white bg-gray-900 hover:bg-gray-100"
           class="block px-3 py-2 rounded-md text-base font-medium text-gray-300 hover:text-white hover:bg-gray-700 focus:outline-hidden focus:text-white focus:bg-gray-700">
          Cinema
        </router-link>
        <router-link to="/theaters"
                     active-class="text-white bg-gray-900 hover:bg-gray-100"
           class="mt-1 block px-3 py-2 rounded-md text-base font-medium text-gray-300 hover:text-white hover:bg-gray-700 focus:outline-hidden focus:text-white focus:bg-gray-700">
          Theater
        </router-link>
        <router-link to="/concerts"
                     active-class="text-white bg-gray-900 hover:bg-gray-100"
           class="mt-1 block px-3 py-2 rounded-md text-base font-medium text-gray-300 hover:text-white hover:bg-gray-700 focus:outline-hidden focus:text-white focus:bg-gray-700">
          Concerts
        </router-link>
      </div>
      <div class="pt-4 pb-3 border-t border-gray-700">
        <div class="flex items-center px-5">
          <div class="shrink-0">
            <img class="h-10 w-10 rounded-full" :src="profileImgUrl" alt=""/>
          </div>
          <div class="ml-3">
            <div class="text-sm font-medium leading-none text-gray-400">{{ store.state.account?.email ?? '' }}</div>
          </div>
        </div>
        <div class="mt-3 px-2">
          <a href="#" @click="logout"
             class="mt-1 block px-3 py-2 rounded-md text-base font-medium text-gray-400 hover:text-white hover:bg-gray-700 focus:outline-hidden focus:text-white focus:bg-gray-700">
            Sign out
          </a>
        </div>
      </div>
    </div>
  </nav>
</template>
