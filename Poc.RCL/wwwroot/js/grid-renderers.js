const TYPE_PILL_THEMES = {
    "SAILING":    { bg: "#e0f2fe", text: "#0284c7", icon: "bi-water" },
    "BERTHING":   { bg: "#eff6ff", text: "#1e40af", icon: "bi-geo-alt" },
    "AT SEA":     { bg: "#f3e8ff", text: "#6b21a8", icon: "bi-waves" },
    "AT PORT":    { bg: "#f3e8ff", text: "#6b21a8", icon: "bi-building" },
    "COSP":       { bg: "#06b6d41A", text: "#00838f", icon: "bi-play-circle-fill" },
    "EOSP":       { bg: "#e0f7fa", text: "#00838f", icon: "bi-record-circle-fill" },
    "TRANSIT":    { bg: "#e8eaf6", text: "#283593", icon: "bi-arrow-left-right" },
    "REFUELING":  { bg: "#fff3e0", text: "#ef6c00", icon: "bi-fuel-pump-fill" },
    "BUNKER":     { bg: "#fff3e0", text: "#ef6c00", icon: "bi-plus-square" },
    "DELAY":      { bg: "#e8f5e9", text: "#2e7d32", icon: "bi-hourglass-split" },
    "EGCS":       { bg: "#e8f5e9", text: "#2e7d32", icon: "bi-wind" },
    "SLUDGE":     { bg: "#f5f5f5", text: "#424242", icon: "bi-trash" },
    "DISCHARGED": { bg: "#f5f5f5", text: "#424242", icon: "bi-trash" },
    "SURVEY":     { bg: "#e8f5e9", text: "#2e7d32", icon: "bi-clipboard-check" }
};

const CellRenderers = {
    AvatarWithText: (val, data, cfg) => `
        <div style="display:flex; align-items:center; gap:10px; padding:4px 0;">
            <div style="width:38px; height:38px; border-radius:8px; background:${data[cfg.colorFld] || "#6b7280"}; color:#fff; font-weight:700; font-size:12px; display:flex; align-items:center; justify-content:center; flex-shrink:0;">${data[cfg.textFld] || ""}</div>
            <div>
                <div style="font-weight:600; font-size:13px; color:#111827;">${val}</div>
                <div style="font-size:11px; color:#6b7280;">${data[cfg.subFld] || ""}</div>
            </div>
        </div>`,

    Badge: (val, data, cfg) => `
        <span style="background:${cfg.bg}; color:${cfg.txt}; border-radius:4px; padding:3px 8px; font-size:11px; font-weight:600; display:inline-block;">${val}</span>`,

    TypePill: (val) => {
        const key = Object.keys(TYPE_PILL_THEMES).find(k => val.trim().toUpperCase().includes(k)) || "";
        const theme = TYPE_PILL_THEMES[key] || { bg: "#e5e7eb", text: "#1f2937", icon: "bi-chat-left-text" };
        return `
            <span style="background:${theme.bg}; color:${theme.text}; border-radius:20px; padding:4px 14px; font-size:11px; font-weight:700; display:inline-flex; align-items:center; gap:6px; font-family:sans-serif; letter-spacing:0.3px; box-shadow:0 1px 2px rgba(0,0,0,0.02);">
                <i class="bi ${theme.icon}" style="font-size:12px; display:flex; align-items:center;"></i>
                <span>${val.toUpperCase()}</span>
            </span>`;
    },

    ComplexDate: (val, data, cfg) => {
        const dateObj = new Date(val);
        const day   = !isNaN(dateObj) ? String(dateObj.getDate()).padStart(2, '0') : "20";
        const month = !isNaN(dateObj) ? dateObj.toLocaleString('en-US', { month: 'short' }).toUpperCase() : "MAR";
        const year  = !isNaN(dateObj) ? dateObj.getFullYear() : "2026";
        return `
            <div style="display:flex; flex-direction:column; gap:5px; padding:2px 0; font-family:sans-serif;">
                <div style="display:flex; align-items:center; gap:10px;">
                    <span style="font-size:26px; font-weight:700; color:#d9a420; line-height:1; letter-spacing:-0.5px;">${day}</span>
                    <div style="display:flex; flex-direction:column; line-height:1.1; justify-content:center;">
                        <span style="font-size:12px; font-weight:700; color:#4a4a4a; letter-spacing:0.5px;">${month}</span>
                        <span style="font-size:11px; font-weight:500; color:#9b9b9b;">${year}</span>
                    </div>
                    <span style="font-size:12px; font-weight:500; color:#595959; background:#e6e6e6; padding:3px 8px; border-radius:6px; margin-left:2px; font-variant-numeric:tabular-nums;">${data[cfg.timeFld] || "10:30"}</span>
                </div>
                <div style="display:flex; margin-top:2px;">
                    <span style="background:#eaf7ff; color:#32b2f0; border:1px solid #cceeff; border-radius:6px; padding:2px 6px; font-size:11px; font-weight:700; display:inline-flex; align-items:center; gap:5px;">
                        <i class="bi bi-arrow-repeat" style="font-size:12px; font-weight:bold;"></i> ${data[cfg.syncFld] || "20/03 12:15"}
                    </span>
                </div>
            </div>`;
    },

    RouteFlow: (val, data, cfg) => {
        const isTransit = val.toUpperCase().includes("CANAL") || (data.messageTypeDesc && data.messageTypeDesc.toUpperCase().includes("TRANSIT"));
        return `
            <div style="display:flex; align-items:center; gap:8px; font-size:12px; font-weight:700; font-family:sans-serif; padding:6px 0;">
                <div style="display:flex; align-items:center; gap:5px;">
                    ${isTransit
                        ? `<i class="bi bi-arrow-left-right" style="color:#f2b413; font-size:13px; font-weight:bold;"></i>`
                        : `<i class="bi bi-geo-alt-fill" style="color:#f0b822; font-size:14px;"></i>`}
                    <span style="color:${isTransit ? "#6366f1" : "#3b82f6"}; letter-spacing:0.3px;">${val.toUpperCase()}</span>
                </div>
                <i class="bi bi-arrow-right" style="color:#f2b413; font-size:14px; font-weight:bold;"></i>
                <div style="display:flex; align-items:center; gap:5px;">
                    ${data[cfg.routeTypeFld] === "land"
                        ? `<i class="bi bi-triangle-fill" style="color:#f59e0b; font-size:9px;"></i>`
                        : `<i class="bi bi-flag-fill" style="color:#eab308; font-size:13px;"></i>`}
                    <span style="color:#7f7f7f; letter-spacing:0.3px;">${(data[cfg.toPortFld] || "N/A").toUpperCase()}</span>
                </div>
            </div>`;
    }
};
