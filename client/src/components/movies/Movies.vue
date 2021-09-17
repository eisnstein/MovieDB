<script setup lang="ts">
import Movie from './Movie.vue'
import { ref, onMounted } from 'vue'
import { TMovie } from '../../types/movie'
import { fetchMovies } from '../../api/movie'

let totalCount = ref<number | undefined>()
let yearCount = ref<number | undefined>()
let monthCount = ref<number | undefined>()

let searchValue = ''
let loading = false

const movies = ref<Array<TMovie>>([])

onMounted(async () => {
  movies.value = await fetchMovies()
})

</script>
<template>
  <header class="bg-gray-100">
      <div class="container mx-auto py-6 px-2 sm:px-4 lg:px-6">
          <div class="flex justify-center align-items">
              <div class="w-1/3 rounded bg-white p-2 md:p-4 border">
                  <div class="text-gray-600 text-sm">Total</div>
                  <div v-if="totalCount !== undefined" class="text-4xl font-bold leading-tight">
                      {{ totalCount }}
                  </div>
                  <i v-else class="mt-2 fad fa-spinner-third fa-spin fa-lg" style="color: blue;"></i>
              </div>
              <div class="w-1/3 ml-4 rounded bg-white p-2 md:p-4 border">
                  <div class="text-gray-600 text-sm">This Year</div>
                  <div v-if="yearCount !== undefined" class="text-4xl font-bold leading-tight">
                      {{ yearCount }}
                  </div>
                  <i v-else class="mt-2 fad fa-spinner-third fa-spin fa-lg" style="color: blue;"></i>
              </div>
              <div class="w-1/3 ml-4 rounded bg-white p-2 md:p-4 border">
                  <div class="text-gray-600 text-sm">This Month</div>
                  <div v-if="monthCount !== undefined" class="text-4xl font-bold leading-tight">
                      monthCount
                  </div>
                  <i v-else class="mt-2 fad fa-spinner-third fa-spin fa-lg" style="color: blue;"></i>
              </div>
          </div>
      </div>
  </header>
  <div class="container mx-auto sm:px-4 lg:px-6">
      <div class="py-2">
          <input
              class="bg-white appearance-none border-2 border-l-0 border-r-0 md:border-l-2 md:border-r-2 border-gray-300 md:rounded w-full py-2 px-4 text-gray-700 leading-tight focus:outline-none focus:border-purple-500"
              id="filter"
              placeholder="Filter by Title..."
              type="text"
              value="searchValue" />
      </div>
      <ul class="py-6">
        <li v-for="movie in movies" :key="movie.id" class="mb-4" >
          <Movie :movie="movie" />
        </li>
      </ul>
      <div v-if="loading" class="p-6 flex align-items justify-center">
        <i class="fad fa-spinner-third fa-spin fa-2x" style="color: blue;"></i>
      </div>
  </div>
</template>