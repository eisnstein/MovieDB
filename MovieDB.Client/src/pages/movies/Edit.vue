<script setup lang="ts">
import { ref } from 'vue'
import { useStore } from '@/services/store'
import { fetchMoviePoster, updateMovie } from '@/api/movie'
import router from '@/services/router'
import { genreOptions } from '@/constants'
import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { TMovie } from '@/types/movie'
import { fetchMovie } from '@/api/movie'

const store = useStore()
const route = useRoute()

const movie = ref<TMovie | null>(null)
const loading = ref(true)
const errorMsg = ref<string | null>(null)
const isSubmitting = ref(false)

onMounted(async () => {
  try {
    let id = route.params.id
    if (!id) {
      throw new Error('No movie id provided')
    }
    if (Array.isArray(id)) {
      id = id[0]
    }
    const fetchedMovie = await fetchMovie(Number(id))
    movie.value = fetchedMovie
    movie.value.seenAt = movie.value.seenAt.split('T')[0]
  } catch (error) {
    console.error(error)
    errorMsg.value = 'Failed to fetch movie'
  } finally {
    loading.value = false
  }
})

async function onSubmit() {
  if (!movie.value) {
    return
  }

  isSubmitting.value = true

  await updateMovie(movie.value!.id, {
    title: movie.value!.title,
    seenAt: movie.value!.seenAt,
    imdbIdentifier: movie.value!.imdbIdentifier,
    genre: movie.value!.genre,
    rating: movie.value!.rating,
    posterUrl: movie.value!.posterUrl ?? null
  })

  isSubmitting.value = false

  router.push('/movies')
}

function setRating(value: number) {
  movie.value!.rating = value
}

function fetchPoster() {
  if (movie.value!.imdbIdentifier.length === 0) {
    return
  }

  fetchMoviePoster(movie.value!.imdbIdentifier).then((url) => movie.value!.posterUrl = (url ?? null))
}
</script>
<template>
  <div class="container w-full lg:w-1/2 mx-auto py-6 px-2 sm:px-4 lg:px-6">
    <div class="flex flex-col md:flex-row bg-white shadow-md rounded-sm p-3 md:p-6">
      <div class="w-full md:w-1/2">
        <div v-if="loading" class="p-6 flex align-items justify-center">
          <i class="fad fa-spinner-third fa-spin fa-2x" style="color: blue;"></i>
        </div>
        <div v-if="errorMsg" class="p-6 flex align-items justify-center">
          <div class="text-red-600 font-bold">{{ errorMsg }}</div>
        </div>
        <form v-if="movie" class="" @submit.prevent="onSubmit">
          <div>
            <input type="text" v-model="movie.title" id="title" placeholder="Title of the movie" class="focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-sm">
          </div>
          <div class="mt-4">
            <input type="date" v-model="movie.seenAt" id="date" placeholder="Seen at" lang="de-DE" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-sm">
          </div>
          <div class="mt-4">
            <input type="text" v-model="movie.imdbIdentifier" id="imdb" placeholder="Imdb Identifier" @blur="fetchPoster" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-md">
          </div>
          <div class="mt-4">
            <select v-model="movie.genre" class="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-xs focus:outline-hidden focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
              <option v-for="option in genreOptions" :value="option.value">{{ option.text }}</option>
            </select>
          </div>
          <div class="mt-4 flex items-center justify-between">
            <span class="rating" :class="{'active': movie.rating === 1}" @click="setRating(1)">1</span>
            <span class="rating" :class="{'active': movie.rating === 2}" @click="setRating(2)">2</span>
            <span class="rating" :class="{'active': movie.rating === 3}" @click="setRating(3)">3</span>
            <span class="rating" :class="{'active': movie.rating === 4}" @click="setRating(4)">4</span>
            <span class="rating" :class="{'active': movie.rating === 5}" @click="setRating(5)">5</span>
          </div>
          <div class="mt-4 flex items-center justify-between">
            <button class="w-full bg-blue-500 hover:bg-blue-700 text-white text-center font-bold py-2 px-4 rounded-sm focus:outline-hidden focus:shadow-outline" type="submit" :disabled="store.state.loading">
              <span v-if="!isSubmitting">Save</span>
              <i v-else class="fad fa-spinner-third fa-spin" style="color: white;"></i>
            </button>
          </div>
        </form>
        <div v-if="store.state.alert" class="mt-4 shadow-md rounded-sm p-6 text-white text-center">
          {{ store.state.alert.message }}
        </div>
      </div>
      <div class="w-full mt-4 md:mt-0 md:w-1/2 md:ml-3">
        <img class="poster" :src="movie?.posterUrl ?? undefined" />
      </div>
    </div>
  </div>
</template>

<style scoped>
@reference "../../index.css";

.rating {
  @apply w-8 h-8 rounded-xl flex items-center justify-center border bg-white hover:bg-gray-300 cursor-pointer;
}

.rating.active {
  @apply bg-gray-700 text-white;
}

.poster {
  @apply border border-dashed rounded-sm;
  height: 304px;
  object-fit: contain;
  width: 100%;
}
</style>
