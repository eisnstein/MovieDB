<script lang="ts" setup>
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query'
import { movieGenres } from '@/types/movie'
import { errorMessage } from '@/api/client'
import { fetchMovie, deleteMovie } from '@/api/movie'
import { formatDate } from '@/services/formatDate'
import MoviePoster from '@/components/movies/MoviePoster.vue'
import RatingDisplay from '@/components/RatingDisplay.vue'

const route = useRoute()
const router = useRouter()
const queryClient = useQueryClient()

// The route only matches numeric ids
const movieId = computed(() => Number(route.params.id))

const { isPending: loading, isError, data: movie } = useQuery({
  queryKey: ['movie', movieId],
  queryFn: () => fetchMovie(movieId.value),
})

const errorMsg = computed(() => isError.value ? 'Failed to fetch movie' : null)

const { mutate: removeMovie, isPending: deleting, error } = useMutation({
  mutationFn: deleteMovie,
  onSuccess: (_, id) => {
    queryClient.removeQueries({ queryKey: ['movie', id] })
    queryClient.invalidateQueries({ queryKey: ['movies'] })
    router.push('/movies')
  },
})

const deleteError = computed(() => error.value ? errorMessage(error.value, 'Could not delete the movie') : null)

const seenAt = computed(() => formatDate(movie.value?.seenAt))

const genre = computed(() => {
  if (movie.value) {
    return movieGenres.find(g => g.value === movie.value!.genre)?.text ?? null
  }

  return null
})

function handleDelete() {
  if (!movie.value || deleting.value) {
    return
  }

  if (!confirm(`Delete "${movie.value.title}"?`)) {
    return
  }

  removeMovie(movie.value.id)
}
</script>
<template>
  <div class="container mx-auto py-6 px-2 sm:px-4 lg:px-6">
    <div v-if="loading" class="p-6 flex align-items justify-center">
      <i class="fad fa-spinner-third fa-spin fa-2x" style="color: blue;"></i>
    </div>
    <div v-if="errorMsg" class="p-6 flex align-items justify-center">
      <div class="text-red-600 font-bold">{{ errorMsg }}</div>
    </div>
    <div v-if="movie">
      <div class="container max-w-2xl mx-auto flex bg-white shadow-md rounded-sm p-3 md:p-6">
        <div class="w-1/2 flex flex-col justify-between">
          <div>
            <div class="text-sm">{{ seenAt }}</div>
            <div class="mt-1 font-bold">{{ movie.title }}</div>
            <div class="mt-3">
              <RatingDisplay :rating="movie.rating" />
            </div>
            <div class="mt-2"><span class="px-2 py-1 inline-flex items-center rounded-full bg-blue-700 text-xs text-white">{{ genre }}</span></div>
          </div>
          <div class="flex items-center gap-2">
            <button class="px-3 py-1 inline-flex items-center rounded-full bg-red-700 text-white" type="button" :disabled="deleting" @click="handleDelete">
              Delete <i class="ml-2 fad" :class="{'fa-trash': !deleting, 'fa-spinner-third fa-spin': deleting}"></i>
            </button>
            <router-link class="px-3 py-1 inline-flex items-center rounded-full bg-blue-700 text-white" type="button" :to="{ name: 'movies-edit', params: { id: movie.id } }">
              Edit <i class="ml-2 fad fa-edit"></i>
            </router-link>
          </div>
        </div>
        <div class="w-1/2 ml-2">
          <MoviePoster class="rounded-sm" :url="movie.posterUrl" :alt="movie.title" />
        </div>
      </div>
      <div v-if="deleteError" class="container max-w-2xl mx-auto mt-4 rounded-sm p-3 bg-red-100 text-red-700 text-center whitespace-pre-line">
        {{ deleteError }}
      </div>
    </div>
  </div>
</template>
