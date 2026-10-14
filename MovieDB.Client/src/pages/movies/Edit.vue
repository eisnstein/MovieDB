<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query'
import { errorMessage } from '@/api/client'
import { fetchMovie, fetchMoviePoster, updateMovie } from '@/api/movie'
import { movieGenres, TMovie } from '@/types/movie'
import MoviePoster from '@/components/movies/MoviePoster.vue'
import RatingInput from '@/components/RatingInput.vue'

const route = useRoute()
const router = useRouter()
const queryClient = useQueryClient()

// The route only matches numeric ids
const movieId = computed(() => Number(route.params.id))

const { isPending: loading, isError, data } = useQuery({
  queryKey: ['movie', movieId],
  queryFn: () => fetchMovie(movieId.value),
})

const errorMsg = computed(() => isError.value ? 'Failed to fetch movie' : null)

// Editable copy, so typing doesn't change the cached movie
const movie = ref<TMovie | null>(null)

watch(data, (fetchedMovie) => {
  // Don't overwrite the user's edits when the query refetches in the background
  if (!fetchedMovie || movie.value?.id === fetchedMovie.id) {
    return
  }

  movie.value = {
    ...fetchedMovie,
    seenAt: fetchedMovie.seenAt.split('T')[0],
    // Older entries may have OMDb's "N/A" stored; saving cleans it up
    posterUrl: fetchedMovie.posterUrl === 'N/A' ? null : fetchedMovie.posterUrl,
  }
}, { immediate: true })

const { mutate, isPending: isSubmitting, error } = useMutation({
  mutationFn: (updatedMovie: TMovie) => updateMovie(updatedMovie.id, {
    title: updatedMovie.title,
    seenAt: updatedMovie.seenAt,
    imdbIdentifier: updatedMovie.imdbIdentifier,
    genre: updatedMovie.genre,
    rating: updatedMovie.rating,
    posterUrl: updatedMovie.posterUrl ?? null,
  }),
  onSuccess: (_, { id }) => {
    queryClient.invalidateQueries({ queryKey: ['movie', id] })
    queryClient.invalidateQueries({ queryKey: ['movies'] })
    router.push('/movies')
  },
})

const saveError = computed(() => error.value ? errorMessage(error.value, 'Could not save the movie') : null)

function onSubmit() {
  if (!movie.value || isSubmitting.value) {
    return
  }

  mutate(movie.value)
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
            <input type="date" required v-model="movie.seenAt" id="date" placeholder="Seen at" lang="de-DE" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-sm">
          </div>
          <div class="mt-4">
            <input type="text" v-model="movie.imdbIdentifier" id="imdb" placeholder="Imdb Identifier" @blur="fetchPoster" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-md">
          </div>
          <div class="mt-4">
            <select v-model="movie.genre" class="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-xs focus:outline-hidden focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
              <option v-for="option in movieGenres" :value="option.value">{{ option.text }}</option>
            </select>
          </div>
          <RatingInput v-model="movie.rating" class="mt-4" />
          <div class="mt-4 flex items-center justify-between">
            <button class="w-full bg-blue-500 hover:bg-blue-700 text-white text-center font-bold py-2 px-4 rounded-sm focus:outline-hidden focus:shadow-outline" type="submit" :disabled="isSubmitting">
              <span v-if="!isSubmitting">Save</span>
              <i v-else class="fad fa-spinner-third fa-spin" style="color: white;"></i>
            </button>
          </div>
        </form>
        <div v-if="saveError" class="mt-4 rounded-sm p-3 bg-red-100 text-red-700 text-center whitespace-pre-line">
          {{ saveError }}
        </div>
      </div>
      <div class="w-full mt-4 md:mt-0 md:w-1/2 md:ml-3">
        <MoviePoster class="poster" :url="movie?.posterUrl" :alt="movie?.title" />
      </div>
    </div>
  </div>
</template>

<style scoped>
@reference "../../index.css";

.poster {
  @apply border border-dashed rounded-sm;
  height: 304px;
  object-fit: contain;
  width: 100%;
}
</style>
