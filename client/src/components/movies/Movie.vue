<script setup lang="ts">
import { computed } from 'vue'
import { TMovie } from '../../types/movie'

const { movie } = defineProps<{
  movie: TMovie
}>()

const defaultUrl = "https://m.media-amazon.com/images/M/MV5BMDljNTQ5ODItZmQwMy00M2ExLTljOTQtZTVjNGE2NTg0NGIxXkEyXkFqcGdeQXVyODkzNTgxMDg@._V1_SX300.jpg"

const seenAt = computed(() => {
  const date = Date.parse(movie.seenAt)
  return new Intl.DateTimeFormat('de-AT', { year: 'numeric', month: 'numeric', day: 'numeric'}).format(date)
})
</script>
<template>
  <div class="movie" :style="{backgroundImage: `url('${movie.posterUrl ?? defaultUrl}')`}">
    <div class="overlay">
      <div class="bg-white rounded p-1 m-1">
        <span v-for="i in [1, 2, 3, 4, 5]">
          <i v-if="i <= movie.rating" class="fad fa-fire-alt mr-1" style="color: darkorange;"></i>
          <i v-else class="fad fa-fire-alt mr-1"></i>
        </span>
      </div>
      <div class="bg-white rounded px-2 m-1 text-sm flex items-center">
        {{ seenAt }}
      </div>
    </div>
  </div>
</template>

<style scoped>
.movie {
  @apply relative mb-3 rounded-lg border bg-white hover:shadow-lg transition duration-200 transform hover:-translate-y-1;
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
  height: 400px;
  width: 270px;
}

.movie img {
  @apply rounded;
  position: relative;
}

.overlay {
  display: flex;
  justify-content: space-between;
  left: 0;
  position: absolute;
  top: 0;
  width: 100%;
}

</style>