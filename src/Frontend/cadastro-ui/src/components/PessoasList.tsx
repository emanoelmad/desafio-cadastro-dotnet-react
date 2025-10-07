import { useEffect, useState } from 'react'
import api from '../services/api'
import { useNavigate } from 'react-router-dom'

export default function PessoasList() {
  const [pessoas, setPessoas] = useState<any[]>([])
  const navigate = useNavigate()

  async function load() {
    try {
      const res: any = await api.getPessoas()
      setPessoas(res)
    } catch (err: any) {
      alert('Erro ao carregar: ' + err.message)
    }
  }

  useEffect(() => { load() }, [])

  return (
    <div>
      <h2>Pessoas</h2>
      <button onClick={() => navigate('/pessoas/novo')}>Novo</button>
      <ul>
        {pessoas.map(p => (
          <li key={p.id}>
            {p.nome} — {p.cpf} — {p.endereco || ''}
            <button onClick={() => navigate(`/pessoas/editar/${p.id}`)}>Editar</button>
            <button onClick={async () => { await api.deletePessoa(p.id); load() }}>Excluir</button>
          </li>
        ))}
      </ul>
    </div>
  )
}
