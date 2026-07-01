const typeConfig = {
    "EOSP":          { color: "#0891b2", bg: "#e0f2fe", icon: "bi bi-stop-circle-fill" },
    "COSP":          { color: "#0ea5e9", bg: "#dbeafe", icon: "bi bi-play-circle-fill" },
    "EOSP Transit":  { color: "#6366f1", bg: "#eef2ff", icon: "bi bi-stop-circle" },
    "COSP Transit":  { color: "#8b5cf6", bg: "#f5f3ff", icon: "bi bi-play-circle" },
    "Sailing":       { color: "#3b82f6", bg: "#eff6ff", icon: "bi bi-moisture" },
    "Berthing":      { color: "#10b981", bg: "#ecfdf5", icon: "bi bi-anchor" },
    "Refueling":     { color: "#f59e0b", bg: "#fffbeb", icon: "bi bi-droplet-fill" },
    "Sludge":        { color: "#78716c", bg: "#f5f5f4", icon: "bi bi-trash" },
    "EGCS":          { color: "#06b6d4", bg: "#ecfeff", icon: "bi bi-wind" }
};

const months = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];

function _fmtSync(iso) {
    if (!iso) return "";
    const d = new Date(iso);
    if (isNaN(d.getTime())) return "";
    const p = n => String(n).padStart(2, "0");
    return `${p(d.getDate())}/${p(d.getMonth() + 1)} ${p(d.getHours())}:${p(d.getMinutes())}`;
}

const cellRenderers = {
    vessel(el, { data: d }) {
        if (!d) return;
        // Supporto sia minuscole che maiuscole dal backend
        const color = d.vesselColor || d.VesselColor || "#ccc";
        const initials = d.vesselInitials || d.VesselInitials || "?";
        const name = d.vesselName || d.VesselName || "Unknown Vessel";
        const email = d.captainEmail || d.CaptainEmail || "";
        const office = d.office || d.Office || "";

        $(el).html(`
            <div style="display:flex;align-items:center;gap:10px;padding:4px 0;">
                <div style="width:38px;height:38px;border-radius:8px;background:${color};
                            color:#fff;font-weight:700;font-size:12px;display:flex;align-items:center;
                            justify-content:center;flex-shrink:0;">${initials}</div>
                <div>
                    <div style="font-weight:600;font-size:13px;color:#111827;">${name}</div>
                    <div style="font-size:11px;color:#6b7280;">${email}</div>
                    <div style="font-size:11px;color:#9ca3af;">${office}</div>
                </div>
            </div>`);
    },

    voyage(el, { value, data: d }) {
        // Se value è vuoto, prova a rimediare estraendolo dall'oggetto riga
        const rawValue = value || (d ? (d.voyageCode || d.VoyageCode) : "") || "-";
        $(el).html(`
            <span style="background:#fef9c3;color:#854d0e;border:1px solid #fde047;
                         border-radius:4px;padding:3px 8px;font-size:12px;font-weight:600;">${rawValue}</span>`);
    },

    type(el, cellInfo) {
        // PREVENZIONE CRASH: Cerca il valore in 'value' o in qualunque variante dell'oggetto riga
        const d = cellInfo.data;
        const rawValue = cellInfo.value || (d ? (d.messageTypeDesc || d.MessageTypeDesc) : "") || "";

        // Cerca la configurazione o assegna un fallback di default
        const cfg = typeConfig[rawValue] ?? { color: "#6b7280", bg: "#f3f4f6", icon: "bi bi-circle" };
        const label = rawValue ? rawValue.toUpperCase() : "UNKNOWN";

        $(el).html(`
            <span style="background:${cfg.bg};color:${cfg.color};border-radius:20px;
                         padding:4px 12px;font-size:12px;font-weight:600;
                         display:inline-flex;align-items:center;gap:5px;">
                <i class="${cfg.icon}"></i>${label}
            </span>`);
    },

    date(el, { data: d }) {
        if (!d) return;
        const rawDate = d.messageDate || d.MessageDate;
        const time = d.messageTime || d.MessageTime || "--:--";
        const sync = d.syncDate || d.SyncDate;

        const dt = new Date(rawDate);
        const isValidDate = !isNaN(dt.getTime());

        const day = isValidDate ? dt.getDate() : "--";
        const monthLabel = isValidDate ? months[dt.getMonth()] : "---";
        const year = isValidDate ? dt.getFullYear() : "----";

        $(el).html(`
            <div style="padding:2px 0;">
                <div style="display:flex;align-items:baseline;gap:6px;">
                    <span style="font-size:22px;font-weight:700;color:#f97316;">${day}</span>
                    <div style="font-size:11px;color:#6b7280;line-height:1.3;">
                        ${monthLabel}<br>${year}
                    </div>
                    <span style="font-size:12px;background:#f3f4f6;color:#374151;
                                 border-radius:4px;padding:2px 6px;">${time}</span>
                </div>
                <div style="display:flex;align-items:center;gap:4px;margin-top:3px;font-size:11px;color:#9ca3af;">
                    <i class="bi bi-arrow-repeat"></i>${sync ? _fmtSync(sync) : ""}
                </div>
            </div>`);
    },

    route(el, { data: d }) {
        if (!d) return;
        const isTransit = d.isTransitRoute || d.IsTransitRoute || false;
        const from = d.fromPort || d.FromPort || "-";
        const to = d.toPort || d.ToPort || "-";

        const arrow = isTransit
            ? `<i class="bi bi-arrow-left-right" style="color:#8b5cf6;"></i>`
            : `<i class="bi bi-arrow-right"      style="color:#9ca3af;"></i>`;

        $(el).html(`
            <div style="display:flex;align-items:center;gap:6px;font-size:12px;font-weight:500;">
                <i class="bi bi-geo-alt-fill" style="color:#f59e0b;font-size:13px;"></i>
                <span style="color:#1f2937;">${from}</span>
                ${arrow}
                <i class="bi bi-flag-fill" style="color:#6b7280;font-size:12px;"></i>
                <span style="color:#1f2937;">${to}</span>
            </div>`);
    },

    status(el) {
        $(el).html(`
            <span style="background:#dcfce7;color:#166534;border-radius:20px;
                         padding:3px 12px;font-size:11px;font-weight:600;">Sent</span>`);
    },

    actions(el) {
        $(el).html(`<span style="color:#9ca3af;font-size:20px;cursor:pointer;letter-spacing:2px;">···</span>`);
    }
};