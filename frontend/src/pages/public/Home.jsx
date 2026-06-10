// Aquí creo la página principal del portal público
// Muestro los eventos activos en tarjetas con búsqueda y filtros por fecha
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { publicService } from '../../api/api.js';

export default function Home() {
  // Estado para guardar los eventos que obtengo del API
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  // Estados para los filtros de búsqueda
  const [search, setSearch] = useState('');
  const [fromDate, setFromDate] = useState('');
  const [toDate, setToDate] = useState('');
  const navigate = useNavigate();

  // Cargo los eventos al montar el componente y cuando cambian los filtros
  useEffect(() => {
    loadEvents();
  }, []);

  // Función que obtiene los eventos del API con los filtros aplicados
  const loadEvents = async () => {
    setLoading(true);
    try {
      const params = {};
      if (search) params.search = search;
      if (fromDate) params.from = fromDate;
      if (toDate) params.to = toDate;
      const res = await publicService.getEvents(params);
      setEvents(res.data);
    } catch (err) {
      console.error('Error cargando eventos:', err);
    } finally {
      setLoading(false);
    }
  };

  // Busco cuando el usuario presiona Enter o hace clic en buscar
  const handleSearch = (e) => {
    e.preventDefault();
    loadEvents();
  };

  // Formateo la fecha para mostrarla de forma legible
  const formatDate = (dateStr) => {
    return new Date(dateStr).toLocaleDateString('es-MX', {
      weekday: 'long', year: 'numeric', month: 'long', day: 'numeric',
      hour: '2-digit', minute: '2-digit'
    });
  };

  // Formateo el precio en pesos mexicanos
  const formatPrice = (price) => {
    return new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' }).format(price);
  };

  return (
    <>
      {/* Sección hero con título atractivo */}
      <div className="hero">
        <h1>Descubre Eventos Increíbles</h1>
        <p>Encuentra y compra boletos para los mejores conciertos, conferencias y espectáculos</p>
      </div>

      <div className="container">
        {/* Barra de búsqueda con filtros de fecha */}
        <form onSubmit={handleSearch} className="search-bar">
          <input
            type="text"
            className="form-control"
            placeholder="🔍 Buscar eventos por nombre, lugar..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            id="search-input"
          />
          <input type="date" className="form-control" style={{maxWidth:'180px'}}
            value={fromDate} onChange={(e) => setFromDate(e.target.value)} />
          <input type="date" className="form-control" style={{maxWidth:'180px'}}
            value={toDate} onChange={(e) => setToDate(e.target.value)} />
          <button type="submit" className="btn btn-primary">Buscar</button>
        </form>

        {/* Muestro spinner mientras carga, o la grid de eventos */}
        {loading ? (
          <div className="loading"><div className="spinner"></div></div>
        ) : events.length === 0 ? (
          <div className="loading">No se encontraron eventos</div>
        ) : (
          <div className="card-grid">
            {events.map((event) => (
              <div key={event.id} className="card event-card"
                onClick={() => navigate(`/evento/${event.id}`)} id={`event-${event.id}`}>
                <div className="event-date">📅 {formatDate(event.date)}</div>
                <h3>{event.name}</h3>
                <div className="event-location">📍 {event.location}</div>
                <p className="event-description">{event.description}</p>
                <div className="zone-badges">
                  {event.ticketZones?.map((zone) => (
                    <span key={zone.id}
                      className={`zone-badge ${zone.type.toLowerCase()}`}>
                      {zone.type}: {formatPrice(zone.price)}
                    </span>
                  ))}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </>
  );
}
