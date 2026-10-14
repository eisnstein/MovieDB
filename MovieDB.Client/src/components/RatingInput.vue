<script setup lang="ts">
import { useId } from 'vue'

const rating = defineModel<number>({ required: true })
const labelId = useId()
</script>
<template>
  <div>
    <span :id="labelId" class="block text-gray-700 text-sm mb-1">Rating</span>
    <div class="flex items-center justify-between" role="group" :aria-labelledby="labelId">
      <button
        v-for="value in [1, 2, 3, 4, 5]"
        :key="value"
        type="button"
        class="rating"
        :class="{ 'active': rating === value }"
        :aria-pressed="rating === value"
        @click="rating = value"
      >
        {{ value }}
      </button>
    </div>
  </div>
</template>

<style scoped>
@reference "../index.css";

.rating {
  @apply w-8 h-8 rounded-xl flex items-center justify-center border bg-white hover:bg-gray-300 cursor-pointer;
}

.rating.active {
  @apply bg-gray-700 text-white;
}
</style>
