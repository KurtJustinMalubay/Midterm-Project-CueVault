# CueVault

**CueVault** is a billiards e-commerce store management app built with **Blazor Server** and **Tailwind CSS v4** on **.NET 10**.  
It mirrors the structure of the DevLogs reference app but is rethemed around a billiards brand — dark green felt palette, cue & ball iconography, and store-focused pages.

> No database is used. All data lives in memory while the app runs and resets on restart.

---

## Pages

| Route | Page | Description |
|---|---|---|
| `/login` | Login | Sign in with username/email + password |
| `/register` | Register | Create an account (PBKDF2 hashed password) |
| `/` | Dashboard | Stats, recent orders, low-stock alerts, quick actions |
| `/products` | Products | List, filter, and add billiards products |
| `/orders` | Orders | View orders, update status inline |
| `/reviews` | Reviews | Customer review list with star ratings |
| `/settings` | Settings | Edit profile, toggle preferences, delete account |

---

## Architecture

```
CueVault/
├── Components/
│   ├── App.razor              # HTML shell
│   ├── Routes.razor           # Client-side router
│   ├── _Imports.razor         # Global using statements
│   ├── Layout/
│   │   ├── MainLayout.razor   # Authenticated sidebar layout
│   │   └── AuthLayout.razor   # Split-screen login/register layout
│   ├── Pages/
│   │   ├── Home.razor         # Dashboard
│   │   ├── Products.razor     # Product catalog + add
│   │   ├── Orders.razor       # Order management
│   │   ├── Reviews.razor      # Customer reviews
│   │   ├── Login.razor        # Login form
│   │   ├── Register.razor     # Registration form
│   │   └── Settings.razor     # Account & preferences
│   └── Shared/
│       ├── AddProductModal.razor  # Modal to add a new product
│       ├── SearchModal.razor      # Global product search modal
│       └── UnderDevModal.razor    # "Under development" placeholder modal
├── Services/
│   ├── AuthService.cs         # In-memory auth with PBKDF2 password hashing
│   ├── StoreService.cs        # In-memory product/order/review store (singleton)
│   └── UiState.cs             # Scoped service that controls which modal is open
├── input.css                  # Tailwind v4 theme + base styles (source)
├── wwwroot/css/app.css        # Compiled Tailwind output (auto-generated)
├── Program.cs                 # Service registration + app pipeline
├── CueVault.csproj            # Project file with Tailwind build target
├── HOW_TO_RUN.md              # Setup & run instructions
└── README.md                  # This file
```

---

## Services explained simply

| Service | Lifetime | What it does |
|---|---|---|
| `AuthService` | Scoped | Holds the currently logged-in user. Handles register/login/update. |
| `StoreService` | Singleton | Holds all products, orders, and reviews. Shared across all users. |
| `UiState` | Scoped | Tracks which modal (if any) is currently open. |

**Scoped** = one instance per browser connection.  
**Singleton** = one instance for the whole app lifetime.

---

## Design

- **Color palette**: dark green (`#0c0f0a`) background, lime accent (`#a3d977`), gold highlights (`#d4a847`), danger red (`#e07060`) — inspired by billiards table felt and cue ball colors.
- **Layout**: left sidebar navigation + sticky top header with search and "Add product" CTA.
- **Modals**: search, add product, and a generic "under development" modal for features not yet built.
- **No JavaScript** beyond what Blazor Server injects (`blazor.web.js`).

---

## Data flow

1. User registers → `AuthService` hashes password and stores account in a static dictionary.
2. User logs in → `AuthService` verifies hash, sets `Current` user.
3. Navigating protected pages → `MainLayout` redirects unauthenticated users to `/login`.
4. Product/order mutations → `StoreService` updates its in-memory bags and fires events.
5. Components subscribe to events via `OnInitialized` / `Dispose` so the UI re-renders automatically.
