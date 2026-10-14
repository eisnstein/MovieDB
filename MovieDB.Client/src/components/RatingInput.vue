<script setup lang="ts">
const rating = defineModel<number>({ required: true })
</script>
<template>
  <div class="flex items-center justify-between" role="group" aria-label="Rating">
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
