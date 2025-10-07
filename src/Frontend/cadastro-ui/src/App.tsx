import { useState } from 'react'
import { BrowserRouter, Routes, Route, Link, useNavigate } from 'react-router-dom'
import './App.css'
import Login from './components/Login'
import PessoaList from './components/PessoaList'
import PessoaForm from './components/PessoaForm'

function Home() {
  return (
    <div>
      <h1>Cadastro Pessoas - v2</h1>
      <nav>
        <Link to="/pessoas">Pessoas</Link> | <Link to="/login">Login</Link>
      </nav>
    </div>
  )
}

function AppRoutes() {
  const [editing, setEditing] = useState<unknown | null>(null);
  const navigate = useNavigate();

  return (
    <Routes>
      <Route path="/" element={<Home />} />
  <Route path="/login" element={<Login onLogin={() => { navigate('/pessoas'); }} />} />
      <Route path="/pessoas" element={<PessoaList onEdit={(p) => { setEditing(p); navigate('/pessoas/editar'); }} />} />
      <Route path="/pessoas/novo" element={<PessoaForm onSaved={() => navigate('/pessoas')} />} />
      <Route path="/pessoas/editar" element={<PessoaForm initial={editing} onSaved={() => navigate('/pessoas')} />} />
    </Routes>
  )
}

export default function App() {
  return (
    <BrowserRouter>
      <AppRoutes />
    </BrowserRouter>
  )
}
