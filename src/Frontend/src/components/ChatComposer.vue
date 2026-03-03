<script setup lang="ts">
import { ref } from 'vue'

const model = ref('')
const emit = defineEmits<{
  send: [text: string]
}>()

function handleSend() {
  const text = model.value.trim()
  if (!text) return
  emit('send', text)
  model.value = ''
}
</script>

<template>
  <form class="flex gap-2 border-t border-gray-200 bg-white p-3" @submit.prevent="handleSend">
    <input
      v-model="model"
      type="text"
      placeholder="Type a message…"
      class="flex-1 rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:border-blue-500"
    />
    <button
      type="submit"
      class="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
      :disabled="!model.trim()"
    >
      Send
    </button>
  </form>
</template>
