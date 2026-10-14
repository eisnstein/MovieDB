<script lang="ts" setup>
import { useQuery } from '@tanstack/vue-query'
import { ref, computed } from 'vue'
import { fetchMovies } from '../../api/movie'
import FilterBar from '../../components/FilterBar.vue'
import StatsHeader from '../../components/StatsHeader.vue'
import Movie from '../../components/movies/Movie.vue'

const searchValue = ref('')

const { isPending, isError, data } = useQuery({
  queryKey: ['movies'],
  queryFn: fetchMovies,
  staleTime: 1000 * 60 * 60, // Cache for 1h
})

const movies = computed(() => data.value ?? [])
const filteredMovies = computed(() => {
  if (searchValue.value.length < 3) {
    return movies.value
  }

  return movies.value.filter((m) => m.title.toLowerCase().startsWith(searchValue.value.toLowerCase()))
})
</script>
<template>
  <StatsHeader :items="movies" :loading="isPending" />
  <div class="container mx-auto px-2 sm:px-4 lg:px-6">
    <div v-if="isPending" class="p-6 flex align-items justify-center">
      <i class="fad fa-spinner-third fa-spin fa-2x" style="color: blue;"></i>
    </div>
    <div v-else-if="isError" class="p-6 flex justify-center">
      <div class="text-red-600 font-bold">Failed to load movies</div>
    </div>
    <div v-else>
      <FilterBar v-model="searchValue" add-to="/movies/new" add-label="Add Movie" />
      <div class="py-6 flex flex-wrap justify-between">
        <router-link v-for="movie in filteredMovies" :key="movie.id" :to="{ name: 'movies-show', params: { id: movie.id } }">
          <Movie :movie="movie"/>
        </router-link>
      </div>
    </div>
  </div>
</template>
