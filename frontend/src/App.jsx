// Aquí defino las rutas de mi aplicación y el layout principal con la barra de navegación
import { Routes, Route, Link, useLocation, Navigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext.jsx';

// Importo todas las páginas de mi aplicación
import Home from './pages/public/Home.jsx';
import EventDetail from './pages/public/EventDetail.jsx';
import Login from './pages/admin/Login.jsx';
import AdminEvents from './pages/admin/Events.jsx';
import Dashboard from './pages/admin/Dashboard.jsx';

// Componente principal de la aplicación
export default function App() {
  const { isAuthenticated, isAdmin, user, logout } = useAuth();
  const location = useLocation();

  return (
    <>
      {/* Barra de navegación fija en la parte superior */}
      <nav className="navbar">
        <Link to="/" className="navbar-brand">🎫 EventPro</Link>
        <div className="navbar-links">
          {/* Siempre muestro el enlace al portal público */}
          <Link to="/" className={location.pathname === '/' ? 'active' : ''}>Eventos</Link>
          
          {/* Si el usuario es admin, muestro los enlaces del panel administrativo */}
          {isAuthenticated && isAdmin && (
            <>
              <Link to="/admin/events" className={location.pathname === '/admin/events' ? 'active' : ''}>
                Administrar
              </Link>
              <Link to="/admin/dashboard" className={location.pathname === '/admin/dashboard' ? 'active' : ''}>
                Dashboard
              </Link>
            </>
          )}
          
          {/* Muestro login o logout según el estado de autenticación */}
          {isAuthenticated ? (
            <>
              <span style={{ color: 'var(--text-secondary)', fontSize: '0.85rem' }}>
                {user.name}
              </span>
              <button onClick={logout}>Salir</button>
            </>
          ) : (
            <Link to="/admin/login" className={location.pathname === '/admin/login' ? 'active' : ''}>
              Admin
            </Link>
          )}
        </div>
      </nav>

      {/* Defino las rutas de la aplicación */}
      <Routes>
        {/* Rutas públicas: cualquier usuario puede acceder */}
        <Route path="/" element={<Home />} />
        <Route path="/evento/:id" element={<EventDetail />} />
        
        {/* Ruta de login para administradores */}
        <Route path="/admin/login" element={
          isAuthenticated && isAdmin ? <Navigate to="/admin/events" /> : <Login />
        } />
        
        {/* Rutas protegidas: solo admin autenticado puede acceder */}
        <Route path="/admin/events" element={
          isAuthenticated && isAdmin ? <AdminEvents /> : <Navigate to="/admin/login" />
        } />
        <Route path="/admin/dashboard" element={
          isAuthenticated && isAdmin ? <Dashboard /> : <Navigate to="/admin/login" />
        } />
      </Routes>
    </>
  );
}
