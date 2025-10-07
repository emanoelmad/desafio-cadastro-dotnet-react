const API_BASE = import.meta.env.VITE_API_URL ?? '';

export async function login(username: string, password: string) {
  const res = await fetch(`${API_BASE}/api/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password }),
  });
  if (!res.ok) throw new Error('Login failed');
  return res.json();
}

export function getAuthHeader(): Record<string, string> {
  const token = localStorage.getItem('token');
  return token ? { Authorization: `Bearer ${token}` } : {} as Record<string, string>;
}

export async function fetchPessoasV2() {
  const res = await fetch(`${API_BASE}/api/v2/pessoas`, { headers: getAuthHeader() });
  if (!res.ok) throw new Error('Falha ao buscar pessoas');
  return res.json();
}

export async function createPessoaV2(payload: any) {
  const res = await fetch(`${API_BASE}/api/v2/pessoas`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', ...getAuthHeader() },
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error('Falha ao criar pessoa');
  return res.json();
}

export async function updatePessoaV2(id: number, payload: any) {
  const res = await fetch(`${API_BASE}/api/v2/pessoas/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json', ...getAuthHeader() },
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error('Falha ao atualizar pessoa');
}

export async function deletePessoaV2(id: number) {
  const res = await fetch(`${API_BASE}/api/v2/pessoas/${id}`, {
    method: 'DELETE',
    headers: getAuthHeader(),
  });
  if (!res.ok) throw new Error('Falha ao deletar pessoa');
}

export default { login, fetchPessoasV2, createPessoaV2, updatePessoaV2, deletePessoaV2 };
