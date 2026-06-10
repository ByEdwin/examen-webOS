// Aquí es el punto de entrada de mi aplicación React
// Monto el componente App dentro del elemento root del HTML
import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import App from './App.jsx'
import { AuthProvider } from './context/AuthContext.jsx'
import './index.css'

// Envuelvo la app con BrowserRouter para manejar las rutas
// y AuthProvider para manejar la autenticación en toda la app
ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <App />
      </AuthProvider>
    </BrowserRouter>
  </React.StrictMode>
)
