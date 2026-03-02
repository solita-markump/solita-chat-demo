ALTER TABLE messages
    ALTER COLUMN created_at_utc SET DEFAULT NOW();
