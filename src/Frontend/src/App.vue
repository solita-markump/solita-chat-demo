<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { ChatMessage } from '@/mock-data'
import ChatBubble from '@/components/ChatBubble.vue'
import ChatComposer from '@/components/ChatComposer.vue'
import { getMessages, sendMessage, ROOM_ID, AUTHOR_NAME } from '@/api/chat-api'

const messages = ref<ChatMessage[]>([])
const loadError = ref<string | null>(null)
const sendError = ref<string | null>(null)
const isLoading = ref(false)

onMounted(async () => {
  isLoading.value = true
  loadError.value = null
  try {
    const data = await getMessages(ROOM_ID)
    messages.value = data.items.map((item) => ({
      id: item.id,
      author: item.authorName,
      text: item.text,
      isMine: item.authorName === AUTHOR_NAME,
    }))
  } catch {
    loadError.value = 'Could not load messages. Is the backend running?'
  } finally {
    isLoading.value = false
  }
})

async function handleSend(text: string) {
  sendError.value = null
  try {
    const item = await sendMessage(ROOM_ID, AUTHOR_NAME, text)
    messages.value.push({
      id: item.id,
      author: item.authorName,
      text: item.text,
      isMine: item.authorName === AUTHOR_NAME,
    })
  } catch {
    sendError.value = 'Could not send message. Please try again.'
  }
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
      <p v-if="isLoading" class="text-center text-xs text-gray-400">Loading…</p>
      <p v-else-if="loadError" class="text-center text-xs text-red-500">{{ loadError }}</p>
      <template v-else>
        <ChatBubble v-for="msg in messages" :key="msg.id" :message="msg" />
      </template>
      <p v-if="sendError" class="text-center text-xs text-red-500">{{ sendError }}</p>
    </main>

    <!-- Composer -->
    <ChatComposer @send="handleSend" />
  </div>
</template>
