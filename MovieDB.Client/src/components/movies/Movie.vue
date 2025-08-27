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
      <div class="bg-white rounded-sm p-1 m-1">
        <span v-for="i in [1, 2, 3, 4, 5]">
          <i v-if="i <= movie.rating" class="fad fa-fire-alt mr-1" style="color: darkorange;"></i>
          <i v-else class="fad fa-fire-alt mr-1" style="color: black;"></i>
        </span>
      </div>
      <div class="bg-white rounded-sm px-1 m-1 text-sm flex items-center text-black">
        {{ seenAt }}
      </div>
    </div>
  </div>
</template>

<style scoped>
@reference "../../index.css";

.movie {
  @apply relative mb-3 rounded-lg border bg-white hover:shadow-lg transition duration-200 transform hover:-translate-y-1;
  @apply bg-center bg-no-repeat bg-cover;
  @apply w-[120px] md:w-[200px] lg:w-[270px];
  aspect-ratio: 12/19;
}

.movie img {
  @apply rounded-sm;
  position: relative;
}

.overlay {
  @apply hidden md:flex justify-between left-0 absolute w-full;
}
</style>
