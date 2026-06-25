let _handler = null;

export function addKpiCardFilter(dotNetRef) {
    _handler = (e) => {
        if (e.detail?.types) {
            const flat = e.detail.types.flat();
            dotNetRef.invokeMethodAsync("OnKpiCardFilter", flat);
        }
    };
    document.addEventListener("kpi-card-filter", _handler);
}

export function removeKpiCardFilter() {
    if (_handler) {
        document.removeEventListener("kpi-card-filter", _handler);
        _handler = null;
    }
}
