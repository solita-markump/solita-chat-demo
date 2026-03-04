export const BACKEND_BASE_URL = 'http://localhost:5027';
export const ROOM_ID = 'general';
export const AUTHOR_NAME = 'You';
export const PAGE_SIZE = 50;
// --- API helpers ---
export async function getMessages(roomId, pageSize = PAGE_SIZE) {
    const url = new URL('/api/messages', BACKEND_BASE_URL);
    url.searchParams.set('roomId', roomId);
    url.searchParams.set('pageSize', String(pageSize));
    const response = await fetch(url.toString());
    if (!response.ok) {
        throw new Error(`Failed to load messages: ${response.status}`);
    }
    return response.json();
}
export async function sendMessage(roomId, authorName, text) {
    const response = await fetch(`${BACKEND_BASE_URL}/api/messages`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ roomId, authorName, text }),
    });
    if (!response.ok) {
        throw new Error(`Failed to send message: ${response.status}`);
    }
    return response.json();
}
//# sourceMappingURL=chat-api.js.map