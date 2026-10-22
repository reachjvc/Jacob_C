// Hook helper: records whether a session is working or idle, for the status line.
// Usage (hook command): node ~/.claude/dot-state.js working|idle   (hook JSON on stdin)
const fs = require('fs');
const path = require('path');
let raw = '';
process.stdin.on('data', d => (raw += d));
process.stdin.on('end', () => {
  try {
    const id = JSON.parse(raw).session_id;
    if (!/^[\w-]+$/.test(id)) return;
    const dir = path.join(__dirname, 'dot-state');
    fs.mkdirSync(dir, { recursive: true });
    fs.writeFileSync(path.join(dir, id), process.argv[2]);
  } catch {}
});
