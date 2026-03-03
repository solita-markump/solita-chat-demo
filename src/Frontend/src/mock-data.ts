export interface ChatMessage {
  id: number
  author: string
  text: string
  isMine: boolean
}

export const mockMessages: ChatMessage[] = [
  { id: 1, author: 'Alice', text: 'Hey! Welcome to the chat 👋', isMine: false },
  { id: 2, author: 'You', text: 'Thanks! Happy to be here.', isMine: true },
  { id: 3, author: 'Alice', text: 'How can I help you today?', isMine: false },
]
