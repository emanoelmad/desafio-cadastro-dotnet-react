import { useEffect, useState } from 'react';
import api from '../services/api';

type PessoaV2 = { id: number; nome: string; cpf: string; endereco: string };

export default function PessoaList({ onEdit }: { onEdit: (p: PessoaV2) => void }) {
  const [pessoas, setPessoas] = useState<PessoaV2[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  async function load() {
    setLoading(true);
    try {
      const data = await api.fetchPessoasV2();
      setPessoas(data as PessoaV2[]);
    } catch {
      setError('Erro ao buscar pessoas');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => { load(); }, []);

  async function remove(id: number) {
    if (!confirm('Confirmar exclusão?')) return;
    try {
      await api.deletePessoaV2(id);
      load();
    } catch {
      alert('Falha ao excluir');
    }
  }

  return (
    <div>
      <h2>Pessoas</h2>
      {error && <div style={{ color: 'red' }}>{error}</div>}
      <div style={{ margin: '8px 0' }}>
        <button onClick={() => window.location.href = '/pessoas/novo'}>Novo</button>
      </div>
      {loading ? <div>Carregando...</div> : (
      <table>
        <thead>
          <tr><th>Id</th><th>Nome</th><th>CPF</th><th>Endereco</th><th>Ações</th></tr>
        </thead>
        <tbody>
          {pessoas.map(p => (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.nome}</td>
              <td>{p.cpf}</td>
              <td>{p.endereco}</td>
              <td>
                <button onClick={() => onEdit(p)}>Editar</button>
                <button onClick={() => remove(p.id)}>Excluir</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      )}
    </div>
  );
}
