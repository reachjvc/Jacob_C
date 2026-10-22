// Claude Code status line: ready dot | model | dir | git branch | context % | rate limits | effort
const { execFileSync } = require('child_process');
const path = require('path');
const fs = require('fs');
let raw = '';
process.stdin.on('data', d => (raw += d));
process.stdin.on('end', () => {
  let j = {};
  try { j = JSON.parse(raw); } catch {}
  const cwd = (j.workspace && j.workspace.current_dir) || j.cwd || process.cwd();
  const parts = [];
  // WORKING (red) while a turn runs, IDLING (green) otherwise; state written by hooks, see dot-state.js
  let working = false;
  try {
    const sf = path.join(__dirname, 'dot-state', String(j.session_id || ''));
    working = fs.readFileSync(sf, 'utf8') === 'working';
    // Esc fires no Stop hook: an interrupt marker written after the state file means idle.
    if (working) {
      const fd = fs.openSync(j.transcript_path, 'r'), size = fs.fstatSync(fd).size;
      const buf = Buffer.alloc(Math.min(size, 8192));
      fs.readSync(fd, buf, 0, buf.length, size - buf.length);
      fs.closeSync(fd);
      const hits = [...buf.toString('utf8').matchAll(/"text":"\[Request interrupted by user[^\n]*?"timestamp":"([^"]+)"/g)];
      if (hits.length && Date.parse(hits[hits.length - 1][1]) > fs.statSync(sf).mtimeMs) working = false;
    }
  } catch {}
  const dot = working ? '\x1b[91mWORKING\x1b[0m' : '\x1b[92mIDLING\x1b[0m';
  const model = (j.model && (j.model.display_name || j.model.id)) || 'Claude';
  parts.push(j.effort && j.effort.level ? model + ' (' + j.effort.level + ')' : model);
  parts.push(path.basename(cwd) || cwd);
  try {
    const b = execFileSync('git', ['--no-optional-locks', 'branch', '--show-current'],
      { cwd, stdio: ['ignore', 'pipe', 'ignore'], timeout: 1500 }).toString().trim();
    if (b) parts.push('⎇ ' + b);
  } catch {}
  const ctx = j.context_window && j.context_window.used_percentage;
  if (ctx != null) parts.push('ctx ' + Math.round(ctx) + '%');
  const rl = j.rate_limits || {};
  // resets_at is unix seconds (tolerate ms or ISO strings); null if missing/invalid/past
  const resetDate = v => {
    if (v == null) return null;
    const n = typeof v === 'number' ? v : Date.parse(v) / 1000;
    if (!isFinite(n)) return null;
    const d = new Date((n > 1e12 ? n : n * 1000));
    return d.getTime() > Date.now() ? d : null;
  };
  const left = d => {
    const m = Math.max(0, Math.round((d - Date.now()) / 60000));
    const dd = Math.floor(m / 1440), h = Math.floor((m % 1440) / 60), mm = m % 60;
    return dd ? dd + 'd' + h + 'h' : h ? h + 'h' + String(mm).padStart(2, '0') + 'm' : mm + 'm';
  };
  const hhmm = d => d.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' });
  const day = d => d.toLocaleDateString('en-GB', { weekday: 'short' });
  // 5h: "5h 42% (reset 2h13m, 14:30)"   7d: "7d 18% (reset 3d4h, Fri 09:00)"
  const f = (l, k) => {
    const w = rl[k];
    if (!w || w.used_percentage == null) return null;
    let s = l + ' ' + Math.round(w.used_percentage) + '%';
    const d = resetDate(w.resets_at);
    if (d) s += ' (reset ' + left(d) + ', ' + (k === 'seven_day' ? day(d) + ' ' : '') + hhmm(d) + ')';
    return s;
  };
  const lim = [f('5h', 'five_hour'), f('7d', 'seven_day')].filter(Boolean);
  if (lim.length) parts.push(lim.join(' '));
  process.stdout.write(dot + ' ' + parts.join(' | '));
});
