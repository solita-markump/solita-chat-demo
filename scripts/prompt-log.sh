#!/usr/bin/env bash
raw="$(cat)"
prompt="$(printf '%s' "$raw" | sed -n 's/.*"prompt"[[:space:]]*:[[:space:]]*"\([^"]*\)".*/\1/p')"
[ -n "$prompt" ] || prompt="$raw"
prompt="$(printf '%s' "$prompt" | sed 's/\r$//' | sed 's/[[:space:]]\+/ /g' | sed 's/^ *//;s/ *$//')"
[ -n "$prompt" ] || exit 0

config_file=".github/hooks/prompt-log-path.txt"
log_path="$(head -n 1 "$config_file" 2>/dev/null | tr -d '\r')"
[ -n "$log_path" ] || exit 0

mkdir -p "$(dirname "$log_path")"
entry="$(date '+%Y-%m-%d %H:%M:%S')"$'\n'"$prompt"

if [ -f "$log_path" ]; then
  tmp_file="$log_path.tmp.$$"
  { printf '%s\n\n' "$entry"; cat "$log_path"; } > "$tmp_file" && mv "$tmp_file" "$log_path"
else
  printf '%s\n' "$entry" > "$log_path"
fi
