# CueVault — How to Run

## Requirements

| Tool | Version |
|---|---|
| .NET SDK | 8.0 or later |
| tailwindcss CLI | Optional for running; required to regenerate CSS |

---

## 1. Get the Tailwind CLI

Download the standalone Tailwind CSS binary for Windows from the [official releases](https://github.com/tailwindlabs/tailwindcss/releases/latest):

- File to download: `tailwindcss-windows-x64.exe`
- Rename it to `tailwindcss.exe`
- Place it in the `CueVault/` project root (same folder as `CueVault.csproj`)

When `tailwindcss.exe` is present in the project root, the build regenerates `wwwroot/css/app.css` from `input.css`. Without it, the build uses the existing generated CSS; install the CLI if you change Tailwind styles and need to regenerate the file.

---

## 2. Run the app

Open a terminal in the `CueVault/` folder, then:

```bash
dotnet run
```

The app will regenerate Tailwind CSS if the CLI is installed, then start the Blazor Server. Open your browser at **https://localhost:7200**. The app targets .NET 8 and supports major-version runtime roll-forward.

---

## 3. First steps in the app

1. Click **"Create an account"** on the login screen.
2. Register with any username, email, and password (min 8 characters).
3. You land on the **Dashboard** — everything is seeded with sample billiards data.
4. Navigate using the sidebar: **Products → Orders → Reviews → Settings**.

---

## 4. Hot-reload (optional)

```bash
dotnet watch
```

Blazor components update live in the browser on save. Note: Tailwind CSS requires a rebuild to pick up new utility classes.
