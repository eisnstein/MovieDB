<script setup lang="ts">
import { computed } from 'vue'
import { formatDate } from '@/services/formatDate'
import { TTheater, theaterGenres } from '@/types/theater'
import RatingDisplay from '@/components/RatingDisplay.vue'

const { theater } = defineProps<{
  theater: TTheater
}>()

const seenAt = computed(() => formatDate(theater.seenAt))

const genre = computed(() => theaterGenres.find((g) => g.value === theater.genre)?.text ?? 'Unknown genre')
</script>
<template>
  <div class="theater">
    <div class="text-sm flex items-center text-gray-600">{{ seenAt }} <span class="text-gray-800 font-semibold ml-2">{{ theater.location }}</span></div>
    <div class="py-1 font-bold text-lg text-blue-800">
      {{ theater.title }}
    </div>
    <div>
      <RatingDisplay :rating="theater.rating" />
      <span class="ml-2 text-sm text-gray-800">
        {{ genre }}
      </span>
    </div>
  </div>
</template>

<style scoped>
@reference "../../index.css";

.theater {
  @apply relative rounded-lg border bg-white hover:shadow-lg transition duration-200 transform hover:-translate-y-1 p-3;
}
</style>