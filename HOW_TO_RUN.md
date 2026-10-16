# CueVault — How to Run

## Requirements

| Tool | Version |
|---|---|
| .NET SDK | 8.0 or later |
| tailwindcss CLI | standalone binary (`tailwindcss.exe`) in project root |

---

## 1. Get the Tailwind CLI

Download the standalone Tailwind CSS binary for Windows from the [official releases](https://github.com/tailwindlabs/tailwindcss/releases/latest):

- File to download: `tailwindcss-windows-x64.exe`
- Rename it to `tailwindcss.exe`
- Place it in the `CueVault/` project root (same folder as `CueVault.csproj`)

> The `.csproj` build step calls `tailwindcss.exe -i input.css -o wwwroot/css/app.css` automatically before every build.

---

## 2. Run the app

Open a terminal in the `CueVault/` folder, then:

```bash
dotnet run
```

The app will:
1. Build Tailwind CSS → `wwwroot/css/app.css`
2. Start the Blazor Server on `https://localhost:7200`

Open your browser at **https://localhost:7200**.

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
