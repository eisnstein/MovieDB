<script setup lang="ts">
const { rating } = defineProps<{
  rating: number
}>()
</script>
<template>
  <span role="img" :aria-label="`Rating: ${rating} of 5`">
    <i
      v-for="i in [1, 2, 3, 4, 5]"
      :key="i"
      class="fad fa-fire-alt mr-1"
      :style="i <= rating ? { color: 'darkorange' } : undefined"
    ></i>
  </span>
</template>
