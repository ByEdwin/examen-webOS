// Aquí creo la página de login para el administrador
// Uso JWT para autenticar y guardo el token en el contexto de la app
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../../api/api.js';
import { useAuth } from '../../context/AuthContext.jsx';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  // Proceso el formulario de login
  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      // Envío las credenciales al endpoint de login
      const res = await authService.login(email, password);
      // Guardo la respuesta (token, nombre, email, rol) en el contexto
      login(res.data);
      // Redirijo al panel de administración
      navigate('/admin/events');
    } catch (err) {
      // Muestro el mensaje de error del API
      setError(err.response?.data?.detail || 'Credenciales inválidas');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page">
      <div className="login-card">
        <h1>🔐 Admin</h1>
        <p>Ingresa tus credenciales de administrador</p>

        {error && <div className="alert alert-error">{error}</div>}

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Email</label>
            <input type="email" className="form-control" required id="login-email"
              value={email} onChange={(e) => setEmail(e.target.value)}
              placeholder="admin@miapp.com" />
          </div>
          <div className="form-group">
            <label>Contraseña</label>
            <input type="password" className="form-control" required id="login-password"
              value={password} onChange={(e) => setPassword(e.target.value)}
              placeholder="••••••••" />
          </div>
          <button type="submit" className="btn btn-primary" id="login-submit"
            style={{width:'100%',justifyContent:'center'}} disabled={loading}>
            {loading ? 'Ingresando...' : 'Iniciar Sesión'}
          </button>
        </form>

        <p style={{marginTop:'1.5rem',fontSize:'0.8rem',color:'var(--text-secondary)',textAlign:'center'}}>
          Demo: admin@miapp.com / Admin123!
        </p>
      </div>
    </div>
  );
}
