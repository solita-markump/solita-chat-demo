export const BACKEND_BASE_URL = 'http://localhost:5027'
export const ROOM_ID = 'general'
export const AUTHOR_NAME = 'You'
export const PAGE_SIZE = 50
export const MESSAGES_POLL_INTERVAL_MS = 5000

// --- Response models ---

export interface GetMessagesItem {
  id: number
  roomId: string
  authorName: string
  text: string
  createdAtUtc: string
}

export interface GetMessagesResponse {
  items: GetMessagesItem[]
  nextBeforeUtc: string | null
}

export interface SendMessageResponse {
  id: number
  roomId: string
  authorName: string
  text: string
  createdAtUtc: string
}

// --- API helpers ---

export async function getMessages(
  roomId: string,
  pageSize: number = PAGE_SIZE,
): Promise<GetMessagesResponse> {
  const url = new URL('/api/messages', BACKEND_BASE_URL)
  url.searchParams.set('roomId', roomId)
  url.searchParams.set('pageSize', String(pageSize))

  const response = await fetch(url.toString())
  if (!response.ok) {
    throw new Error(`Failed to load messages: ${response.status}`)
  }
  return response.json() as Promise<GetMessagesResponse>
}

export async function sendMessage(
  roomId: string,
  authorName: string,
  text: string,
): Promise<SendMessageResponse> {
  const response = await fetch(`${BACKEND_BASE_URL}/api/messages`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ roomId, authorName, text }),
  })
  if (!response.ok) {
    throw new Error(`Failed to send message: ${response.status}`)
  }
  return response.json() as Promise<SendMessageResponse>
}
