CREATE TABLE IF NOT EXISTS messages
(
    id UUID PRIMARY KEY,
    room_id TEXT NOT NULL,
    author_name TEXT NOT NULL,
    message_text TEXT NOT NULL,
    created_at_utc TIMESTAMPTZ NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_messages_room_created_at
    ON messages (room_id, created_at_utc DESC, id DESC);
