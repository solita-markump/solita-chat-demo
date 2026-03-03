<script setup lang="ts">
import { ref } from 'vue'
import { mockMessages, type ChatMessage } from '@/mock-data'
import ChatBubble from '@/components/ChatBubble.vue'
import ChatComposer from '@/components/ChatComposer.vue'

const messages = ref<ChatMessage[]>([...mockMessages])
let nextId = messages.value.length + 1

function handleSend(text: string) {
  messages.value.push({ id: nextId++, author: 'You', text, isMine: true })
}
</script>

<template>
  <div class="flex h-full flex-col bg-white">
    <!-- Header -->
    <header class="flex items-center gap-2 border-b border-gray-200 bg-blue-600 px-4 py-3">
      <h1 class="text-base font-semibold text-white">Solita Chat</h1>
    </header>

    <!-- Message list -->
    <main class="flex flex-1 flex-col gap-2 overflow-y-auto p-3">
      <ChatBubble v-for="msg in messages" :key="msg.id" :message="msg" />
    </main>

    <!-- Composer -->
    <ChatComposer @send="handleSend" />
  </div>
</template>
