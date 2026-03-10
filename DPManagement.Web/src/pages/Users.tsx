import { useState, useEffect } from 'react';
import { Plus, Trash2, Edit, UserCog } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Table, type TableColumn } from '../components/common/Table';
import { FilterBar } from '../components/common/FilterBar';
import { Modal } from '../components/common/Modal';
import { useAuth } from '../hooks/useAuth';

interface Perfil {
  id: string;
  nome: string;
}

interface Usuario {
  id: string;
  nome: string;
  email: string;
  perfilId: string;
  perfilNome: string;
  dataCriacao: string;
}

export default function Users() {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [perfis, setPerfis] = useState<Perfil[]>([]);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    totalPages: 1,
    totalCount: 0,
    pageSize: 10
  });

  const { hasPermission } = useAuth();

  const [filters, setFilters] = useState({
    nome: '',
    email: ''
  });

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingUser, setEditingUser] = useState<Usuario | null>(null);

  const [formData, setFormData] = useState({
    nome: '',
    email: '',
    senha: '',
    perfilId: ''
  });

  useEffect(() => {
    fetchUsuarios();
    fetchPerfis();
  }, []);

  const fetchUsuarios = async () => {
    try {
      showLoading('Carregando usuários...');
      const response = await api.get('/usuarios');
      const resData = response.data;

      if (resData.success) {
        let filtered = resData.data;
        if (filters.nome) {
          filtered = filtered.filter((u: Usuario) => u.nome.toLowerCase().includes(filters.nome.toLowerCase()));
        }
        if (filters.email) {
          filtered = filtered.filter((u: Usuario) => u.email.toLowerCase().includes(filters.email.toLowerCase()));
        }

        setUsuarios(filtered);
        setPagination(prev => ({
          ...prev,
          totalCount: filtered.length,
          totalPages: Math.ceil(filtered.length / prev.pageSize)
        }));
      } else {
        alertError(resData.message || 'Erro ao carregar usuários');
      }
    } catch (error) {
      console.error('Erro ao buscar usuários', error);
      alertError('Erro ao carregar usuários');
    } finally {
      closeLoading();
    }
  };

  const fetchPerfis = async () => {
    try {
      const response = await api.get('/perfis');
      const resData = response.data;
      if (resData.success) {
        setPerfis(resData.data);
      }
    } catch (error) {
      console.error('Erro ao buscar perfis', error);
      alertError('Erro ao carregar perfis');
    }
  };

  const applyFilters = () => fetchUsuarios();

  const clearFilters = () => {
    setFilters({ nome: '', email: '' });
    setTimeout(() => fetchUsuarios(), 0);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    showLoading('Salvando usuário...');
    try {
      let response;
      if (editingUser) {
        const payload = { ...formData, id: editingUser.id };
        if (!payload.senha) {
          delete (payload as any).senha;
        }
        response = await api.put(`/usuarios/${editingUser.id}`, payload);
      } else {
        response = await api.post('/usuarios', formData);
      }

      const resData = response.data;
      if (resData.success) {
        alertSuccess(resData.message || (editingUser ? 'Usuário atualizado com sucesso' : 'Usuário cadastrado com sucesso'));
        setIsModalOpen(false);
        setEditingUser(null);
        setFormData({ nome: '', email: '', senha: '', perfilId: '' });
        fetchUsuarios();
      } else {
        alertError(resData.message || 'Erro ao salvar usuário', resData.errors?.join('\n'));
      }
    } catch (error: any) {
      console.error('Erro ao salvar usuário', error);
      const apiError = error.response?.data;
      alertError(apiError?.message || 'Erro ao salvar usuário', apiError?.errors?.join('\n'));
    } finally {
      closeLoading();
    }
  };

  const handleEdit = (user: Usuario) => {
    setEditingUser(user);
    setFormData({
      nome: user.nome,
      email: user.email,
      senha: '',
      perfilId: user.perfilId
    });
    setIsModalOpen(true);
  };

  const handleDelete = async (id: string) => {
    const confirmed = await alertDeleteConfirm('Excluir Usuário?', 'Esta ação removerá o acesso permanentemente.');
    if (confirmed) {
      showLoading('Excluindo...');
      try {
        const response = await api.delete(`/usuarios/${id}`);
        const resData = response.data;
        if (resData.success) {
          alertSuccess(resData.message || 'Usuário excluído');
          fetchUsuarios();
        } else {
          alertError(resData.message || 'Erro ao excluir usuário');
        }
      } catch (error) {
        console.error('Erro ao excluir usuário', error);
        alertError('Erro ao excluir usuário');
      } finally {
        closeLoading();
      }
    }
  };

  const canAdd = hasPermission('Usuarios', 'Criar');
  const canEdit = hasPermission('Usuarios', 'Editar');
  const canDelete = hasPermission('Usuarios', 'Excluir');

  const columns: TableColumn<Usuario>[] = [
    { header: 'Nome', accessor: 'nome' },
    { header: 'E-mail', accessor: 'email' },
    { header: 'Perfil', accessor: 'perfilNome' },
    {
      header: 'Ações',
      align: 'right',
      render: (user) => (
        <div className="space-x-4">
          {canEdit && (
            <button onClick={() => handleEdit(user)} className="text-indigo-600 hover:text-indigo-900">
              <Edit size={18} />
            </button>
          )}
          {canDelete && (
            <button onClick={() => handleDelete(user.id)} className="text-red-600 hover:text-red-900">
              <Trash2 size={18} />
            </button>
          )}
        </div>
      ),
    },
  ];

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center bg-white p-6 rounded-lg shadow-sm border border-gray-200">
        <div>
          <h1 className="text-2xl font-bold text-gray-800 flex items-center gap-2">
            <UserCog className="text-indigo-600" />
            Gestão de Usuários
          </h1>
          <p className="text-gray-500 mt-1">Gerencie as contas de acesso ao sistema e os perfis vinculados.</p>
        </div>
        {canAdd && (
          <button
            onClick={() => { setEditingUser(null); setFormData({ nome: '', email: '', senha: '', perfilId: '' }); setIsModalOpen(true); }}
            className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors shadow-sm"
          >
            <Plus size={20} className="mr-2" />
            Novo Usuário
          </button>
        )}
      </div>

      <FilterBar onFilter={applyFilters} onClear={clearFilters}>
        <FormInput
          label="Nome"
          value={filters.nome}
          onChange={e => setFilters({ ...filters, nome: e.target.value })}
          placeholder="Buscar por nome..."
        />
        <FormInput
          label="E-mail"
          value={filters.email}
          onChange={e => setFilters({ ...filters, email: e.target.value })}
          placeholder="Buscar por e-mail..."
        />
      </FilterBar>

      <Table
        data={usuarios.slice((pagination.currentPage - 1) * pagination.pageSize, pagination.currentPage * pagination.pageSize)}
        columns={columns}
        pagination={{
          currentPage: pagination.currentPage,
          totalPages: pagination.totalPages,
          totalCount: pagination.totalCount,
          pageSize: pagination.pageSize,
          onPageChange: (page) => setPagination({ ...pagination, currentPage: page }),
        }}
      />

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingUser ? 'Editar Usuário' : 'Novo Usuário'}
        size="lg"
        footer={(
          <>
            <button
              type="submit"
              form="usuario-form"
              className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm"
            >
              Salvar
            </button>
            <button
              type="button"
              onClick={() => setIsModalOpen(false)}
              className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
            >
              Cancelar
            </button>
          </>
        )}
      >
        <form id="usuario-form" onSubmit={handleSubmit}>
          <div className="space-y-4">
            <FormInput
              label="Nome Completo"
              required
              value={formData.nome}
              onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
            />
            <FormInput
              label="E-mail"
              type="email"
              required
              value={formData.email}
              onChange={(e) => setFormData({ ...formData, email: e.target.value })}
            />
            <FormInput
              label={editingUser ? "Nova Senha (deixe em branco para ignorar)" : "Senha"}
              type="password"
              required={!editingUser}
              value={formData.senha}
              onChange={(e) => setFormData({ ...formData, senha: e.target.value })}
            />

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Perfil de Acesso</label>
              <select
                required
                value={formData.perfilId}
                onChange={e => setFormData({ ...formData, perfilId: e.target.value })}
                className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:ring-2 focus:ring-indigo-100 focus:border-indigo-500 outline-none"
              >
                <option value="">Selecione um perfil...</option>
                {perfis.map(p => (
                  <option key={p.id} value={p.id}>{p.nome}</option>
                ))}
              </select>
            </div>
          </div>
        </form>
      </Modal>
    </div>
  );
}
