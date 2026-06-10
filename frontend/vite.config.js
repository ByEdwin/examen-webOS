// Aquí configuro Vite para mi proyecto React
// Vite es el bundler que uso para desarrollo y producción
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  // Agrego el plugin de React para soporte de JSX
  plugins: [react()],
  // Configuro el servidor de desarrollo
  server: {
    port: 5173,  // Puerto donde corre el frontend
    open: true   // Abre el navegador automáticamente al iniciar
  }
})
