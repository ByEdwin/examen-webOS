// Aquí creo el dashboard de ventas para el administrador
// Muestro métricas clave, ventas por zona y las compras más recientes
import { useState, useEffect } from 'react';
import { adminService } from '../../api/api.js';

export default function Dashboard() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);

  // Cargo las métricas del dashboard al montar el componente
  useEffect(() => {
    const load = async () => {
      try {
        const res = await adminService.getDashboard();
        setData(res.data);
      } catch (err) { console.error(err); }
      finally { setLoading(false); }
    };
    load();
  }, []);

  const formatPrice = (p) => new Intl.NumberFormat('es-MX',{style:'currency',currency:'MXN'}).format(p);
  const formatDate = (d) => new Date(d).toLocaleDateString('es-MX',{month:'short',day:'numeric',hour:'2-digit',minute:'2-digit'});

  if (loading) return <div className="loading"><div className="spinner"></div></div>;
  if (!data) return <div className="container"><div className="alert alert-error">Error cargando dashboard</div></div>;

  // Calculo el máximo de ingresos por zona para las barras proporcionales
  const maxRevenue = Math.max(...(data.salesByZone?.map(z => z.revenue) || [1]), 1);

  return (
    <div className="container">
      <div className="page-header">
        <h1>Dashboard de Ventas</h1>
        <p>Resumen general de ventas y métricas del sistema</p>
      </div>

      {/* Tarjetas de métricas principales */}
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-label">💰 Ingresos Totales</div>
          <div className="stat-value">{formatPrice(data.totalRevenue)}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">🎫 Boletos Vendidos</div>
          <div className="stat-value">{data.totalTicketsSold}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">📅 Eventos Activos</div>
          <div className="stat-value">{data.activeEvents}</div>
        </div>
        <div className="stat-card">
          <div className="stat-label">🚫 Eventos Cancelados</div>
          <div className="stat-value">{data.cancelledEvents}</div>
        </div>
      </div>

      <div style={{display:'grid',gridTemplateColumns:'1fr 1fr',gap:'1.5rem'}}>
        {/* Ventas por zona con barras de progreso */}
        <div className="card">
          <h3 style={{marginBottom:'1.25rem',fontWeight:700}}>Ventas por Zona</h3>
          {data.salesByZone?.length > 0 ? (
            <div className="zone-bars">
              {data.salesByZone.map((zone) => (
                <div className="zone-bar" key={zone.zone}>
                  <span className="zone-bar-label">{zone.zone}</span>
                  <div className="zone-bar-track">
                    <div className={`zone-bar-fill ${zone.zone.toLowerCase()}`}
                      style={{width: `${Math.max((zone.revenue / maxRevenue) * 100, 8)}%`}}>
                      {formatPrice(zone.revenue)}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          ) : <p style={{color:'var(--text-secondary)'}}>Sin ventas aún</p>}
        </div>

        {/* Resumen de boletos por zona */}
        <div className="card">
          <h3 style={{marginBottom:'1.25rem',fontWeight:700}}>Boletos por Zona</h3>
          {data.salesByZone?.length > 0 ? (
            <div style={{display:'flex',flexDirection:'column',gap:'1rem'}}>
              {data.salesByZone.map((zone) => (
                <div key={zone.zone} style={{display:'flex',justifyContent:'space-between',alignItems:'center',
                  padding:'0.75rem',background:'var(--bg-glass)',borderRadius:'var(--radius)'}}>
                  <span className={`zone-badge ${zone.zone.toLowerCase()}`}>{zone.zone}</span>
                  <span style={{fontWeight:700}}>{zone.ticketsSold} boletos</span>
                </div>
              ))}
            </div>
          ) : <p style={{color:'var(--text-secondary)'}}>Sin datos</p>}
        </div>
      </div>

      {/* Tabla de compras recientes */}
      <div style={{marginTop:'1.5rem'}}>
        <h3 style={{marginBottom:'1rem',fontWeight:700}}>Últimas Compras</h3>
        <div className="table-container">
          <table>
            <thead>
              <tr><th>Evento</th><th>Zona</th><th>Comprador</th><th>Cantidad</th><th>Total</th><th>Fecha</th></tr>
            </thead>
            <tbody>
              {data.recentPurchases?.map((p) => (
                <tr key={p.id}>
                  <td style={{fontWeight:600}}>{p.eventName}</td>
                  <td><span className={`zone-badge ${p.zone.toLowerCase()}`}>{p.zone}</span></td>
                  <td>{p.buyerName}</td>
                  <td>{p.quantity}</td>
                  <td style={{fontWeight:600}}>{formatPrice(p.total)}</td>
                  <td style={{color:'var(--text-secondary)'}}>{formatDate(p.purchaseDate)}</td>
                </tr>
              ))}
              {(!data.recentPurchases || data.recentPurchases.length === 0) && (
                <tr><td colSpan="6" style={{textAlign:'center',padding:'2rem',color:'var(--text-secondary)'}}>No hay compras registradas</td></tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
