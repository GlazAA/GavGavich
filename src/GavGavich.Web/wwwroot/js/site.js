window.gavgavich = {
  _emergencyMap: null,
  _emergencyMarkers: [],

  initHeaderScroll() {
    const header = document.querySelector('.site-header');
    if (!header) return;

    let lastY = window.scrollY;
    const onScroll = () => {
      const y = window.scrollY;
      const goingDown = y > lastY;
      header.classList.toggle('is-scrolled', y > 8);
      header.classList.toggle('is-compact', y > 48 && goingDown);
      header.classList.add('is-visible');
      lastY = y;
    };

    header.classList.add('is-visible');
    onScroll();
    window.addEventListener('scroll', onScroll, { passive: true });
  },

  scrollToHash() {
    const hash = window.location.hash;
    if (!hash || hash.length < 2) return;
    const el = document.querySelector(hash);
    if (!el) return;
    const header = document.querySelector('.site-header');
    const offset = header ? header.offsetHeight + 12 : 80;
    const top = el.getBoundingClientRect().top + window.scrollY - offset;
    window.scrollTo({ top, behavior: 'smooth' });
    if (document.activeElement && document.activeElement !== document.body) {
      document.activeElement.blur();
    }
  },

  initEmergencyMap(elementId, clinics) {
    const el = document.getElementById(elementId);
    if (!el || typeof L === 'undefined') return;

    if (this._emergencyMap) {
      this._emergencyMap.remove();
      this._emergencyMap = null;
      this._emergencyMarkers = [];
    }

    const map = L.map(el, { scrollWheelZoom: false }).setView([55.75, 37.62], 10);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 18,
      attribution: '&copy; OpenStreetMap'
    }).addTo(map);

    const bounds = [];
    (clinics || []).forEach((c) => {
      const marker = L.marker([c.lat, c.lng]).addTo(map);
      marker.bindPopup(
        `<strong>${c.name}</strong><br>` +
        `<a href="${c.website}" target="_blank" rel="noopener">Сайт клиники</a><br>` +
        `<a href="tel:${(c.phone || '').replace(/\D/g, '')}">${c.phone || ''}</a>`
      );
      marker.on('click', () => {
        window.open(c.website, '_blank', 'noopener');
      });
      this._emergencyMarkers.push(marker);
      bounds.push([c.lat, c.lng]);
    });

    if (bounds.length) {
      map.fitBounds(bounds, { padding: [36, 36] });
    }

    this._emergencyMap = map;
    setTimeout(() => map.invalidateSize(), 120);
  },

  focusEmergencyClinic(lat, lng) {
    if (!this._emergencyMap) return;
    this._emergencyMap.setView([lat, lng], 14, { animate: true });
  }
};

.document.addEventListener('DOMContentLoaded', () => {
  window.gavgavich.initHeaderScroll();
  window.gavgavich.scrollToHash();
});

window.addEventListener('hashchange', () => window.gavgavich.scrollToHash());
