// Aquí creo la página de detalle de un evento con el formulario de compra de boletos
// El usuario ve toda la información del evento, selecciona zona, cantidad y compra
import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { publicService } from '../../api/api.js';

export default function EventDetail() {
  const { id } = useParams(); // Obtengo el ID del evento de la URL
  const [event, setEvent] = useState(null);
  const [loading, setLoading] = useState(true);
  // Estados del formulario de compra
  const [selectedZone, setSelectedZone] = useState(null);
  const [quantity, setQuantity] = useState(1);
  const [buyerName, setBuyerName] = useState('');
  const [buyerEmail, setBuyerEmail] = useState('');
  const [purchasing, setPurchasing] = useState(false);
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');

  // Cargo el detalle del evento al montar el componente
  useEffect(() => { loadEvent(); }, [id]);

  const loadEvent = async () => {
    try {
      const res = await publicService.getEventById(id);
      setEvent(res.data);
    } catch (err) {
      setError('No se pudo cargar el evento');
    } finally {
      setLoading(false);
    }
  };

  // Proceso la compra de boletos
  const handlePurchase = async (e) => {
    e.preventDefault();
    if (!selectedZone) { setError('Selecciona una zona'); return; }
    setError('');
    setPurchasing(true);
    try {
      const res = await publicService.purchase({
        eventId: parseInt(id),
        ticketZoneId: selectedZone.id,
        buyerName, buyerEmail, quantity
      });
      setResult(res.data); // Muestro la confirmación de compra
    } catch (err) {
      setError(err.response?.data?.detail || err.response?.data?.title || 'Error al comprar');
    } finally {
      setPurchasing(false);
    }
  };

  const formatDate = (d) => new Date(d).toLocaleDateString('es-MX', {
    weekday:'long', year:'numeric', month:'long', day:'numeric', hour:'2-digit', minute:'2-digit'
  });
  const formatPrice = (p) => new Intl.NumberFormat('es-MX',{style:'currency',currency:'MXN'}).format(p);

  if (loading) return <div className="loading"><div className="spinner"></div></div>;
  if (!event) return <div className="container"><div className="alert alert-error">{error || 'Evento no encontrado'}</div></div>;

  return (
    <div className="container">
      <Link to="/" style={{color:'var(--text-secondary)',fontSize:'0.9rem'}}>← Volver a eventos</Link>

      <div style={{marginTop:'1.5rem',display:'grid',gridTemplateColumns:'1fr 1fr',gap:'2rem'}}>
        {/* Columna izquierda: información del evento */}
        <div>
          <span className={`status-badge ${event.status.toLowerCase()}`}>{event.status === 'Active' ? 'Activo' : 'Cancelado'}</span>
          <h1 style={{fontSize:'2rem',fontWeight:800,margin:'1rem 0 0.5rem'}}>{event.name}</h1>
          <div style={{color:'var(--text-secondary)',marginBottom:'0.5rem'}}>📅 {formatDate(event.date)}</div>
          <div style={{color:'var(--text-secondary)',marginBottom:'1.5rem'}}>📍 {event.location}</div>
          <p style={{color:'var(--text-secondary)',lineHeight:1.7}}>{event.description}</p>
        </div>

        {/* Columna derecha: formulario de compra */}
        <div className="card">
          {result ? (
            // Muestro la confirmación si la compra fue exitosa
            <div className="success-box">
              <h3>✅ {result.message}</h3>
              <p style={{marginTop:'1rem',color:'var(--text-secondary)'}}>
                <strong>{result.quantity}</strong> boleto(s) - Zona <strong>{result.zone}</strong><br/>
                Precio unitario: {formatPrice(result.unitPrice)}<br/>
                <span style={{fontSize:'1.3rem',fontWeight:700,color:'var(--accent-light)'}}>
                  Total: {formatPrice(result.total)}
                </span>
              </p>
              <button className="btn btn-primary" style={{marginTop:'1rem'}}
                onClick={() => { setResult(null); loadEvent(); }}>
                Comprar más boletos
              </button>
            </div>
          ) : (
            // Muestro el formulario de compra
            <form onSubmit={handlePurchase}>
              <h2 style={{marginBottom:'1rem'}}>Comprar Boletos</h2>
              {error && <div className="alert alert-error">{error}</div>}

              {/* Selector de zona de boletaje */}
              <label style={{fontSize:'0.85rem',fontWeight:500,color:'var(--text-secondary)'}}>Selecciona zona:</label>
              <div className="zone-selector">
                {event.ticketZones?.map((zone) => (
                  <div key={zone.id}
                    className={`zone-option ${selectedZone?.id === zone.id ? 'selected' : ''}`}
                    onClick={() => setSelectedZone(zone)}>
                    <div className="zone-info">
                      <h4>{zone.type}</h4>
                      <span>{zone.availableQuantity} disponibles</span>
                    </div>
                    <div className="zone-price">{formatPrice(zone.price)}</div>
                  </div>
                ))}
              </div>

              {/* Cantidad de boletos */}
              <div className="form-group">
                <label>Cantidad:</label>
                <input type="number" className="form-control" min="1" max="10"
                  value={quantity} onChange={(e) => setQuantity(parseInt(e.target.value) || 1)} />
              </div>

              {/* Datos del comprador */}
              <div className="form-group">
                <label>Tu nombre:</label>
                <input type="text" className="form-control" required
                  value={buyerName} onChange={(e) => setBuyerName(e.target.value)}
                  placeholder="Nombre completo" />
              </div>
              <div className="form-group">
                <label>Tu email:</label>
                <input type="email" className="form-control" required
                  value={buyerEmail} onChange={(e) => setBuyerEmail(e.target.value)}
                  placeholder="correo@ejemplo.com" />
              </div>

              {/* Total calculado: Precio × Cantidad */}
              {selectedZone && (
                <div className="purchase-total">
                  <span className="total-label">Total ({quantity} × {formatPrice(selectedZone.price)})</span>
                  <span className="total-value">{formatPrice(selectedZone.price * quantity)}</span>
                </div>
              )}

              <button type="submit" className="btn btn-primary" style={{width:'100%',marginTop:'1rem',justifyContent:'center'}}
                disabled={purchasing || !selectedZone}>
                {purchasing ? 'Procesando...' : '🎫 Comprar Boletos'}
              </button>
            </form>
          )}
        </div>
      </div>
    </div>
  );
}
