import { useState } from 'react';
import api from '../services/api';

export default function PessoaForm({ initial, onSaved }: { initial?: any, onSaved: () => void }) {
  const [nome, setNome] = useState(initial?.nome ?? '');
  const [cpf, setCpf] = useState(initial?.cpf ?? '');
  const [dataNascimento, setDataNascimento] = useState(initial?.dataNascimento ?? '');
  const [endereco, setEndereco] = useState(initial?.endereco ?? '');
  const [error, setError] = useState<string | null>(null);

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    try {
      const payload = {
        id: initial?.id,
        nome,
        cpf,
        dataNascimento,
        endereco
      };
      if (initial?.id) {
        await api.updatePessoaV2(initial.id, payload);
      } else {
        await api.createPessoaV2(payload);
      }
      onSaved();
    } catch (err: unknown) {
      setError('Erro ao salvar');
    }
  }

  return (
    <div>
      <h2>{initial?.id ? 'Editar' : 'Criar'} Pessoa (v2)</h2>
      <form onSubmit={submit}>
        <div>
          <label>Nome</label>
          <input value={nome} onChange={e => setNome(e.target.value)} />
        </div>
        <div>
          <label>CPF</label>
          <input value={cpf} onChange={e => setCpf(e.target.value)} />
        </div>
        <div>
          <label>Data Nasc</label>
          <input type="date" value={dataNascimento} onChange={e => setDataNascimento(e.target.value)} />
        </div>
        <div>
          <label>Endereço</label>
          <input value={endereco} onChange={e => setEndereco(e.target.value)} />
        </div>
        <button type="submit">Salvar</button>
      </form>
      {error && <div style={{ color: 'red' }}>{error}</div>}
    </div>
  );
}
