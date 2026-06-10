// Aquí configuro Axios para comunicarme con mi API de .NET
// Creo una instancia con la URL base y manejo automático del token JWT
import axios from 'axios';

// Creo la instancia de axios apuntando a mi API que corre en el puerto 5223 del backend
const api = axios.create({
  baseURL: 'http://localhost:5223/api', // Puerto por defecto de mi API .NET
});

// Interceptor: antes de cada petición, agrego el token JWT si existe
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// === Servicios de Autenticación ===
export const authService = {
  // Inicio sesión enviando email y contraseña al endpoint de login
  login: (email, password) => api.post('/auth/login', { email, password }),
};

// === Servicios Públicos (sin autenticación) ===
export const publicService = {
  // Obtengo los eventos activos con filtros opcionales
  getEvents: (params) => api.get('/public/events', { params }),
  // Obtengo el detalle de un evento específico
  getEventById: (id) => api.get(`/public/events/${id}`),
  // Realizo una compra de boletos
  purchase: (data) => api.post('/public/purchase', data),
};

// === Servicios de Admin (requieren JWT con rol Admin) ===
export const adminService = {
  // Obtengo todos los eventos (incluye cancelados)
  getEvents: (params) => api.get('/events', { params }),
  // Obtengo detalle de un evento
  getEventById: (id) => api.get(`/events/${id}`),
  // Creo un nuevo evento
  createEvent: (data) => api.post('/events', data),
  // Actualizo un evento existente
  updateEvent: (id, data) => api.put(`/events/${id}`, data),
  // Cancelo un evento
  cancelEvent: (id) => api.delete(`/events/${id}`),
  // Obtengo las métricas del dashboard de ventas
  getDashboard: () => api.get('/admin/dashboard'),
};

export default api;
