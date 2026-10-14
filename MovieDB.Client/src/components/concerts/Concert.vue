<script setup lang="ts">
import { computed } from 'vue'
import { formatDate } from '../../services/formatDate'
import { TConcert, concertGenres } from '../../types/concert'
import RatingDisplay from '../RatingDisplay.vue'

const { concert } = defineProps<{
  concert: TConcert
}>()

const seenAt = computed(() => formatDate(concert.seenAt))

const genre = computed(() => concertGenres.find((g) => g.value === concert.genre)?.text ?? 'Unknown genre')
</script>
<template>
  <div class="concert">
    <div class="text-sm flex items-center text-gray-600">{{ seenAt }} <span class="text-gray-800 font-semibold ml-2">{{ concert.location }}</span></div>
    <div class="py-1 font-bold text-lg text-blue-800">
      {{ concert.title }}
    </div>
    <div>
      <RatingDisplay :rating="concert.rating" />
      <span class="ml-2 text-sm text-gray-800">
        {{ genre }}
      </span>
    </div>
  </div>
</template>

<style scoped>
@reference "../../index.css";

.concert {
  @apply relative rounded-lg border bg-white hover:shadow-lg transition duration-200 transform hover:-translate-y-1 p-3;
}
</style>