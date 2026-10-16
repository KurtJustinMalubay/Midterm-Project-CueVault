(() => {
    const storageKey = "cuevault-theme";

    function readSavedTheme() {
        try {
            const saved = localStorage.getItem(storageKey);
            if (saved === "dark" || saved === "light") return saved;
        } catch {
            // Storage may be unavailable in private browsing; use the system preference.
        }

        return window.matchMedia?.("(prefers-color-scheme: dark)").matches ? "dark" : "light";
    }

    function apply(theme) {
        const normalized = theme === "dark" ? "dark" : "light";
        document.documentElement.dataset.theme = normalized;
        try {
            localStorage.setItem(storageKey, normalized);
        } catch {
            // The theme remains applied for the current page even when storage is unavailable.
        }
        return normalized;
    }

    const initialTheme = readSavedTheme();
    document.documentElement.dataset.theme = initialTheme;
    window.cueVaultTheme = {
        read: () => document.documentElement.dataset.theme || initialTheme,
        apply
    };
})();