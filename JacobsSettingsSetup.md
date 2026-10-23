# Jacob's Claude Code Settings Setup

Instructions for an AI agent (e.g. Claude Code) setting this up on a new PC.
Requires **Node.js** on PATH (the status line and hooks are Node scripts).

## What's in this repo

| File | Purpose |
|---|---|
| `claude-setup/statusline.js` | Status line: `WORKING/IDLING \| model (effort) \| dir \| ⎇ branch \| ctx % \| 5h/7d rate limits` |
| `claude-setup/dot-state.js` | Hook helper that records whether a session is working or idle (drives the red/green WORKING/IDLING label) |
| `claude-setup/grey-bypass.json` | Custom theme (dark base) that makes the "bypass permissions on" footer grey |
| `claude-setup/user-settings-snippet.json` | Settings to merge into the user's global `~/.claude/settings.json` |
| `.claude/settings.json` | Project settings: full tool permissions + bypass mode in this repo (works automatically, nothing to install) |

## Steps

1. **Copy the scripts** into the user's Claude folder (`~/.claude/`, on Windows `C:\Users\<name>\.claude\`).
   Both files must sit together in that folder, because `statusline.js` reads the `dot-state/` folder next to itself.
   ```bash
   cp claude-setup/statusline.js claude-setup/dot-state.js ~/.claude/
   mkdir -p ~/.claude/themes && cp claude-setup/grey-bypass.json ~/.claude/themes/
   ```

2. **Merge** `claude-setup/user-settings-snippet.json` into `~/.claude/settings.json`.
   - Create the file if it doesn't exist.
   - If it exists, **merge, don't overwrite**: keep the user's existing keys and append the hook entries to any existing hook arrays.
   - Validate the result is valid JSON.

3. **Restart Claude Code.** The status line should appear at the bottom.

## What the settings do

- `theme: "custom:grey-bypass"`: uses `~/.claude/themes/grey-bypass.json`. Claude Code draws the bypass indicator in the theme's `error` color, so this theme makes `error` grey. Side effect: error text is grey too.
- `statusLine`: runs `statusline.js` every 2 seconds.
- `hooks`: call `dot-state.js working` on prompt submit/tool use and `dot-state.js idle` on stop, so the status line shows WORKING (red) or IDLING (green).
- `permissions.defaultMode: "bypassPermissions"`: Claude runs every tool without asking (globally).
- `skipDangerousModePermissionPrompt: true`: skips the one-time "are you sure?" warning for bypass mode. This only works in the **user** settings file, not in a project's settings.

> ⚠️ Bypass mode lets Claude run any command without asking. Only enable it globally if the user is OK with that.
> If they only want it in this repo, skip the `permissions` and `skipDangerousModePermissionPrompt` keys in step 2. The repo's `.claude/settings.json` already enables bypass mode here (Claude may show the bypass warning once to accept).
