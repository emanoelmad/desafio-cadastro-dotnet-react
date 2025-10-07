import { useEffect, useState } from 'react';
import api from '../services/api';

export default function PessoaList({ onEdit }: { onEdit: (p: any) => void }) {
  const [pessoas, setPessoas] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);

  async function load() {
    try {
      const data = await api.fetchPessoasV2();
      setPessoas(data);
    } catch (err: any) {
      setError('Erro ao buscar pessoas');
    }
  }

  useEffect(() => { load(); }, []);

  async function remove(id: number) {
    if (!confirm('Confirmar exclusão?')) return;
    await api.deletePessoaV2(id);
    load();
  }

  return (
    <div>
      <h2>Pessoas</h2>
      {error && <div style={{ color: 'red' }}>{error}</div>}
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
    </div>
  );
}
