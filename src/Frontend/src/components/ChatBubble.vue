<script setup lang="ts">
import type { ChatMessage } from '@/mock-data'

defineProps<{
  message: ChatMessage
}>()
</script>

<template>
  <div class="flex" :class="message.isMine ? 'justify-end' : 'justify-start'">
    <div
      class="max-w-[75%] rounded-lg px-3 py-2 text-sm"
      :class="
        message.isMine
          ? 'bg-blue-600 text-white rounded-br-none'
          : 'bg-gray-200 text-gray-900 rounded-bl-none'
      "
    >
      <p v-if="!message.isMine" class="mb-0.5 text-xs font-semibold text-gray-500">
        {{ message.author }}
      </p>
      <p class="m-0">{{ message.text }}</p>
    </div>
  </div>
</template>
