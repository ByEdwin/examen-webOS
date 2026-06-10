// Aquí creo el panel de administración de eventos con CRUD completo
// El admin puede crear, editar, cancelar y buscar eventos desde esta pantalla
import { useState, useEffect } from 'react';
import { adminService } from '../../api/api.js';

export default function AdminEvents() {
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  // Estado del modal de crear/editar
  const [showModal, setShowModal] = useState(false);
  const [editingEvent, setEditingEvent] = useState(null);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  // Formulario con valores por defecto
  const emptyForm = {
    name:'', description:'', date:'', location:'',
    vipPrice:0, vipQuantity:0, preferentePrice:0, preferenteQuantity:0,
    generalPrice:0, generalQuantity:0
  };
  const [form, setForm] = useState(emptyForm);

  useEffect(() => { loadEvents(); }, []);

  // Cargo los eventos del admin (incluye cancelados)
  const loadEvents = async () => {
    setLoading(true);
    try {
      const params = search ? { search } : {};
      const res = await adminService.getEvents(params);
      setEvents(res.data);
    } catch (err) { console.error(err); }
    finally { setLoading(false); }
  };

  // Abro el modal para crear un nuevo evento
  const handleNew = () => {
    setEditingEvent(null);
    setForm(emptyForm);
    setError('');
    setShowModal(true);
  };

  // Abro el modal para editar un evento existente, cargo sus datos
  const handleEdit = (evt) => {
    setEditingEvent(evt);
    const vip = evt.ticketZones?.find(z => z.type === 'VIP');
    const pref = evt.ticketZones?.find(z => z.type === 'Preferente');
    const gen = evt.ticketZones?.find(z => z.type === 'General');
    setForm({
      name: evt.name, description: evt.description,
      date: evt.date?.slice(0,16), location: evt.location,
      vipPrice: vip?.price||0, vipQuantity: vip?.availableQuantity||0,
      preferentePrice: pref?.price||0, preferenteQuantity: pref?.availableQuantity||0,
      generalPrice: gen?.price||0, generalQuantity: gen?.availableQuantity||0,
    });
    setError('');
    setShowModal(true);
  };

  // Guardo el evento (crear o actualizar según el caso)
  const handleSave = async (e) => {
    e.preventDefault();
    setSaving(true);
    setError('');
    try {
      if (editingEvent) {
        await adminService.updateEvent(editingEvent.id, { id: editingEvent.id, ...form });
        setSuccess('Evento actualizado');
      } else {
        await adminService.createEvent(form);
        setSuccess('Evento creado');
      }
      setShowModal(false);
      loadEvents();
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      setError(err.response?.data?.detail || err.response?.data?.title || 'Error al guardar');
    } finally { setSaving(false); }
  };

  // Cancelo un evento (cambio su estado a Cancelado)
  const handleCancel = async (id) => {
    if (!confirm('¿Estás seguro de cancelar este evento?')) return;
    try {
      await adminService.cancelEvent(id);
      setSuccess('Evento cancelado');
      loadEvents();
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      setError(err.response?.data?.detail || 'Error al cancelar');
    }
  };

  const formatDate = (d) => new Date(d).toLocaleDateString('es-MX',{year:'numeric',month:'short',day:'numeric'});
  const formatPrice = (p) => new Intl.NumberFormat('es-MX',{style:'currency',currency:'MXN'}).format(p);
  const updateForm = (field, value) => setForm(prev => ({...prev, [field]: value}));

  return (
    <div className="container">
      <div className="page-header">
        <h1>Administración de Eventos</h1>
        <p>Crea, edita y gestiona todos los eventos del sistema</p>
      </div>

      {success && <div className="alert alert-success">{success}</div>}
      {error && !showModal && <div className="alert alert-error">{error}</div>}

      {/* Barra de búsqueda y botón de crear */}
      <div className="search-bar">
        <input type="text" className="form-control" placeholder="🔍 Buscar eventos..."
          value={search} onChange={(e) => setSearch(e.target.value)}
          onKeyDown={(e) => e.key === 'Enter' && loadEvents()} />
        <button className="btn btn-secondary" onClick={loadEvents}>Buscar</button>
        <button className="btn btn-primary" onClick={handleNew} id="btn-new-event">+ Nuevo Evento</button>
      </div>

      {/* Tabla de eventos */}
      {loading ? <div className="loading"><div className="spinner"></div></div> : (
        <div className="table-container">
          <table>
            <thead>
              <tr><th>Nombre</th><th>Fecha</th><th>Lugar</th><th>Estado</th><th>VIP</th><th>Pref.</th><th>General</th><th>Acciones</th></tr>
            </thead>
            <tbody>
              {events.map(evt => {
                const vip = evt.ticketZones?.find(z => z.type === 'VIP');
                const pref = evt.ticketZones?.find(z => z.type === 'Preferente');
                const gen = evt.ticketZones?.find(z => z.type === 'General');
                return (
                  <tr key={evt.id}>
                    <td style={{fontWeight:600}}>{evt.name}</td>
                    <td>{formatDate(evt.date)}</td>
                    <td>{evt.location}</td>
                    <td><span className={`status-badge ${evt.status.toLowerCase()}`}>
                      {evt.status === 'Active' ? 'Activo' : 'Cancelado'}</span></td>
                    <td>{formatPrice(vip?.price||0)}</td>
                    <td>{formatPrice(pref?.price||0)}</td>
                    <td>{formatPrice(gen?.price||0)}</td>
                    <td>
                      <div className="btn-group">
                        {evt.status === 'Active' && (
                          <>
                            <button className="btn btn-secondary btn-sm" onClick={() => handleEdit(evt)}>Editar</button>
                            <button className="btn btn-danger btn-sm" onClick={() => handleCancel(evt.id)}>Cancelar</button>
                          </>
                        )}
                      </div>
                    </td>
                  </tr>
                );
              })}
              {events.length === 0 && <tr><td colSpan="8" style={{textAlign:'center',padding:'2rem',color:'var(--text-secondary)'}}>No hay eventos</td></tr>}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal de crear/editar evento */}
      {showModal && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal" onClick={(e) => e.stopPropagation()}>
            <h2>{editingEvent ? 'Editar Evento' : 'Nuevo Evento'}</h2>
            {error && <div className="alert alert-error">{error}</div>}
            <form onSubmit={handleSave}>
              <div className="form-group">
                <label>Nombre del evento</label>
                <input className="form-control" required value={form.name} onChange={(e) => updateForm('name',e.target.value)} />
              </div>
              <div className="form-group">
                <label>Descripción</label>
                <textarea className="form-control" required rows="3" value={form.description} onChange={(e) => updateForm('description',e.target.value)} />
              </div>
              <div className="form-row">
                <div className="form-group">
                  <label>Fecha y hora</label>
                  <input type="datetime-local" className="form-control" required value={form.date} onChange={(e) => updateForm('date',e.target.value)} />
                </div>
                <div className="form-group">
                  <label>Lugar</label>
                  <input className="form-control" required value={form.location} onChange={(e) => updateForm('location',e.target.value)} />
                </div>
              </div>
              <h3 style={{fontSize:'1rem',margin:'1rem 0 0.75rem',color:'var(--accent-light)'}}>Zonas de Boletaje</h3>
              <div className="form-row">
                <div className="form-group"><label>Precio VIP</label>
                  <input type="number" className="form-control" min="1" step="0.01" required value={form.vipPrice} onChange={(e) => updateForm('vipPrice',parseFloat(e.target.value))} /></div>
                <div className="form-group"><label>Cantidad VIP</label>
                  <input type="number" className="form-control" min="1" required value={form.vipQuantity} onChange={(e) => updateForm('vipQuantity',parseInt(e.target.value))} /></div>
              </div>
              <div className="form-row">
                <div className="form-group"><label>Precio Preferente</label>
                  <input type="number" className="form-control" min="1" step="0.01" required value={form.preferentePrice} onChange={(e) => updateForm('preferentePrice',parseFloat(e.target.value))} /></div>
                <div className="form-group"><label>Cantidad Preferente</label>
                  <input type="number" className="form-control" min="1" required value={form.preferenteQuantity} onChange={(e) => updateForm('preferenteQuantity',parseInt(e.target.value))} /></div>
              </div>
              <div className="form-row">
                <div className="form-group"><label>Precio General</label>
                  <input type="number" className="form-control" min="1" step="0.01" required value={form.generalPrice} onChange={(e) => updateForm('generalPrice',parseFloat(e.target.value))} /></div>
                <div className="form-group"><label>Cantidad General</label>
                  <input type="number" className="form-control" min="1" required value={form.generalQuantity} onChange={(e) => updateForm('generalQuantity',parseInt(e.target.value))} /></div>
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-secondary" onClick={() => setShowModal(false)}>Cancelar</button>
                <button type="submit" className="btn btn-primary" disabled={saving}>{saving ? 'Guardando...' : 'Guardar'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
