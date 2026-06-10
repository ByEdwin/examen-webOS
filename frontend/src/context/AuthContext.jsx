// Aquí creo el contexto de autenticación para manejar el estado del login en toda la app
// Uso React Context para compartir el usuario y token entre todos los componentes
import { createContext, useContext, useState, useEffect } from 'react';

// Creo el contexto que compartirá los datos de autenticación
const AuthContext = createContext(null);

// Creo el provider que envuelve toda la aplicación
export function AuthProvider({ children }) {
  // Guardo el usuario en el estado, intento recuperarlo del localStorage al inicio
  const [user, setUser] = useState(() => {
    const saved = localStorage.getItem('user');
    return saved ? JSON.parse(saved) : null;
  });

  // Función para iniciar sesión: guardo el token y los datos del usuario
  const login = (userData) => {
    localStorage.setItem('token', userData.token);
    localStorage.setItem('user', JSON.stringify(userData));
    setUser(userData);
  };

  // Función para cerrar sesión: limpio todo del localStorage
  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setUser(null);
  };

  // Verifico si el usuario está autenticado
  const isAuthenticated = !!user;
  // Verifico si el usuario tiene rol de Admin
  const isAdmin = user?.role === 'Admin';

  // Proporciono todas las funciones y datos a los componentes hijos
  return (
    <AuthContext.Provider value={{ user, login, logout, isAuthenticated, isAdmin }}>
      {children}
    </AuthContext.Provider>
  );
}

// Hook personalizado para acceder al contexto de auth desde cualquier componente
export const useAuth = () => useContext(AuthContext);
