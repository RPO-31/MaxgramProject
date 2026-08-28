import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 7001,
    proxy: {
      '/api': {
        target: 'https://localhost:7029',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})