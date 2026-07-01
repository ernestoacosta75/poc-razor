// Tema condiviso (colore + icona) per il tipo messaggio.
// Usato sia dalla colonna TYPE di MessagesGrid.razor sia dal badge tipo in MessageDetail.razor.
(function (window) {
    const TYPE_PILL_THEMES = {
        "SAILING": { bg: "#e0f2fe", text: "#0284c7", icon: "detailslayout" },
        "BERTHING": { bg: "#eff6ff", text: "#1e40af", icon: "pinright" },
        "AT SEA": { bg: "#f3e8ff", text: "#6b21a8", icon: "contentlayout" },
        "AT PORT": { bg: "#f3e8ff", text: "#6b21a8", icon: "bi bi-buildings-fill" },
        "COSP": { bg: "#06b6d41A", text: "#00838f", icon: "video" },
        "EOSP": { bg: "#e0f7fa", text: "#00838f", icon: "indeterminatestate" },
        "TRANSIT": { bg: "#e8eaf6", text: "#283593", icon: "optionsoutline" },
        "REFUELING": { bg: "#fff3e0", text: "#ef6c00", icon: "fill" },
        "BUNKER": { bg: "#fff3e0", text: "#ef6c00", icon: "textdocument" },
        "DELAY": { bg: "#e8f5e9", text: "#2e7d32", icon: "bi bi-hourglass" },
        "EGCS": { bg: "#e8f5e9", text: "#2e7d32", icon: "strike" },
        "SLUDGE": { bg: "#f5f5f5", text: "#424242", icon: "trash" },
        "DISCHARGED": { bg: "#f5f5f5", text: "#424242", icon: "trash" },
        "SURVEY": { bg: "#e8f5e9", text: "#2e7d32", icon: "cardcontent" },
        "SHORE POWER": { bg: "#e8f5e9", text: "#2e7d32", icon: "bi bi-plug-fill" }
    };

    const DEFAULT_THEME = { bg: "#e5e7eb", text: "#1f2937", icon: "bi bi-chat-left-text" };

    function resolve(msgType) {
        const normalized = (msgType || "").trim().toUpperCase();
        const key = Object.keys(TYPE_PILL_THEMES).find(k => normalized.includes(k)) || "";
        return TYPE_PILL_THEMES[key] || DEFAULT_THEME;
    }

    function iconHtml(icon, style) {
        style = style || "";
        return icon.startsWith("bi ")
            ? `<i class="${icon}" style="${style}"></i>`
            : `<i class="dx-icon dx-icon-${icon}" style="font-family: 'DXIcons' !important; font-style: normal; ${style}"></i>`;
    }

    window.MessageTypeTheme = { resolve, iconHtml };
})(window);
