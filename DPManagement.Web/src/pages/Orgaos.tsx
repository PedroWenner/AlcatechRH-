import React, { useState, useEffect } from 'react';
import { Plus, Edit, Trash2, CheckCircle, XCircle, Building2 } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { Table, type TableColumn } from '../components/common/Table';
import { FilterBar } from '../components/common/FilterBar';
import { Modal } from '../components/common/Modal';
import { Autocomplete } from '../components/common/Autocomplete';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { useAuth } from '../hooks/useAuth';

interface Orgao {
  id: string;
  nome: string;
  abreviatura: string;
  nivel: number;
  orgaoPaiId?: string;
  nomeAbreviaturaPai?: string;
  ativo: boolean;
}

export default function Orgaos() {
  const { hasPermission } = useAuth();
  const [data, setData] = useState<Orgao[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingItem, setEditingItem] = useState<Orgao | null>(null);

  const [formData, setFormData] = useState({
    nome: '',
    abreviatura: '',
    nivel: 1,
    orgaoPaiId: ''
  });

  const canEdit = hasPermission('Estrutura', 'Editar');
  const canDelete = hasPermission('Estrutura', 'Excluir');
  const canAdd = hasPermission('Estrutura', 'Cadastrar');

  const [filters, setFilters] = useState({
    nome: '',
    abreviatura: ''
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async (f = filters) => {
    try {
      const params: any = {};
      if (f.nome) params.nome = f.nome;
      if (f.abreviatura) params.abreviatura = f.abreviatura;

      const response = await api.get('/orgaos', { params });
      const resData = response.data;
      if (resData.success) {
        setData(resData.data);
      } else {
        alertError(resData.message || 'Não foi possível carregar as estruturas');
      }
    } catch (error) {
      console.error('Erro ao buscar estruturas:', error);
      alertError('Não foi possível carregar as estruturas');
    }
  };

  const applyFilters = () => fetchData(filters);
  const clearFilters = () => {
    const newFilters = { nome: '', abreviatura: '' };
    setFilters(newFilters);
    fetchData(newFilters);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.nome || !formData.abreviatura) {
      alertError('Nome e Abreviatura são obrigatórios');
      return;
    }

    if (formData.nivel > 1 && !formData.orgaoPaiId) {
      alertError('Para Secretarias e Departamentos, é obrigatório selecionar a estrutura vinculada (Pai).');
      return;
    }

    showLoading('Salvando...');
    try {
      const payload = {
        ...formData,
        orgaoPaiId: formData.nivel === 1 ? null : formData.orgaoPaiId
      };

      let response;
      if (editingItem) {
        response = await api.put(`/orgaos/${editingItem.id}`, payload);
      } else {
        response = await api.post('/orgaos', payload);
      }

      const resData = response.data;
      if (resData.success) {
        alertSuccess(resData.message || (editingItem ? 'Estrutura atualizada com sucesso' : 'Estrutura criada com sucesso'));
        setIsModalOpen(false);
        fetchData();
      } else {
        alertError(resData.message || 'Erro ao salvar a estrutura', resData.errors?.join('\n'));
      }
    } catch (error: any) {
      console.error('Erro ao salvar estrutura:', error);
      const apiError = error.response?.data;
      alertError(apiError?.message || 'Erro ao salvar a estrutura', apiError?.errors?.join('\n'));
    } finally {
      closeLoading();
    }
  };

  const handleEdit = (item: Orgao) => {
    setEditingItem(item);
    setFormData({
      nome: item.nome,
      abreviatura: item.abreviatura,
      nivel: item.nivel,
      orgaoPaiId: item.orgaoPaiId || ''
    });
    setIsModalOpen(true);
  };

  const handleDelete = async (id: string) => {
    const confirmed = await alertDeleteConfirm('Excluir Estrutura?', 'Esta ação não poderá ser desfeita e pode afetar estruturas filhas.');
    if (confirmed) {
      showLoading('Excluindo...');
      try {
        const response = await api.delete(`/orgaos/${id}`);
        const resData = response.data;
        if (resData.success) {
          alertSuccess(resData.message || 'Estrutura excluída');
          fetchData();
        } else {
          alertError(resData.message || 'Erro ao excluir estrutura');
        }
      } catch (error) {
        console.error('Erro ao excluir estrutura:', error);
        alertError('Erro ao excluir estrutura');
      } finally {
        closeLoading();
      }
    }
  };

  const handleToggleStatus = async (item: Orgao) => {
    const novoStatus = !item.ativo;
    showLoading(novoStatus ? 'Ativando...' : 'Inativando...');
    try {
      const response = await api.patch(`/orgaos/${item.id}/status`, novoStatus, {
        headers: { 'Content-Type': 'application/json' }
      });
      const resData = response.data;
      if (resData.success) {
        alertSuccess(resData.message || (novoStatus ? 'Estrutura ativada' : 'Estrutura inativada'));
        fetchData();
      } else {
        alertError(resData.message || 'Erro ao alterar status');
      }
    } catch (error) {
      console.error('Erro ao alterar status:', error);
      alertError('Erro ao alterar status');
    } finally {
      closeLoading();
    }
  };

  const openNewModal = () => {
    setEditingItem(null);
    setFormData({ nome: '', abreviatura: '', nivel: 1, orgaoPaiId: '' });
    setIsModalOpen(true);
  };

  const getNivelLabel = (nivel: number) => {
    switch (nivel) {
      case 1: return <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-blue-100 text-blue-800">Órgão</span>;
      case 2: return <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-purple-100 text-purple-800">Secretaria</span>;
      case 3: return <span className="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-yellow-100 text-yellow-800">Departamento</span>;
      default: return '-';
    }
  };

  const getParentSearchEndpoint = () => {
    if (formData.nivel === 2) return '/orgaos/nivel/1';
    if (formData.nivel === 3) return '/orgaos/nivel/2';
    return '';
  };

  const getParentLabel = () => {
    if (formData.nivel === 2) return 'Vincular ao Órgão';
    if (formData.nivel === 3) return 'Vincular à Secretaria';
    return 'Vinculado a';
  };

  const columns: TableColumn<Orgao>[] = [
    { header: 'Nível', render: (o) => getNivelLabel(o.nivel) },
    { header: 'Nome', accessor: 'nome' },
    { header: 'Abreviação', accessor: 'abreviatura' },
    { header: 'Vinculado a', render: (o) => o.nomeAbreviaturaPai || '-' },
    {
      header: 'Status',
      render: (o) => (
        <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${o.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
          {o.ativo ? 'Ativo' : 'Inativo'}
        </span>
      )
    },
    {
      header: 'Ações',
      align: 'right',
      render: (o) => (
        <div className="flex justify-end space-x-3 items-center">
          {canEdit && (
            <>
              <button title="Editar" onClick={() => handleEdit(o)} className="text-indigo-600 hover:text-indigo-900">
                <Edit size={18} />
              </button>
              <button
                title={o.ativo ? "Inativar" : "Ativar"}
                onClick={() => handleToggleStatus(o)}
                className={o.ativo ? "text-red-500 hover:text-red-700" : "text-green-500 hover:text-green-700"}
              >
                {o.ativo ? <XCircle size={18} /> : <CheckCircle size={18} />}
              </button>
            </>
          )}
          {canDelete && (
            <button title="Excluir" onClick={() => handleDelete(o.id)} className="text-red-600 hover:text-red-900">
              <Trash2 size={18} />
            </button>
          )}
        </div>
      )
    }
  ];

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center bg-white p-6 rounded-lg shadow-sm border border-gray-200">
        <div>
          <h1 className="text-2xl font-bold text-gray-800 flex items-center gap-2">
            <Building2 className="text-indigo-600" />
            Estrutura Organizacional
          </h1>
          <p className="text-gray-500 mt-1">Gerencie órgãos, secretarias e departamentos.</p>
        </div>
        {canAdd && (
          <button
            onClick={openNewModal}
            className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors shadow-sm"
          >
            <Plus size={20} className="mr-2" />
            Nova Estrutura
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
          label="Abreviatura"
          value={filters.abreviatura}
          onChange={e => setFilters({ ...filters, abreviatura: e.target.value })}
          placeholder="Buscar por abreviatura..."
        />
      </FilterBar>

      <div className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
        <Table data={data} columns={columns} />
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingItem ? 'Editar Estrutura' : 'Nova Estrutura'}
        footer={(<>
          <button
            type="submit"
            form="orgao-form"
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
        </>)}
      >
        <form id="orgao-form" onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Nível Hierárquico</label>
            <select
              className="w-full px-3 py-2 border rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 outline-none"
              value={formData.nivel}
              onChange={(e) => setFormData({ ...formData, nivel: Number(e.target.value), orgaoPaiId: '' })}
            >
              <option value={1}>Órgão (Nível 1)</option>
              <option value={2}>Secretaria (Nível 2)</option>
              <option value={3}>Departamento (Nível 3)</option>
            </select>
          </div>

          <FormInput
            label="Nome da Estrutura"
            value={formData.nome}
            onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
            required
            placeholder="Ex: Prefeitura Municipal de Teste"
          />

          <FormInput
            label="Abreviatura / Sigla"
            value={formData.abreviatura}
            onChange={(e) => setFormData({ ...formData, abreviatura: e.target.value })}
            required
            placeholder="Ex: PMT"
          />

          {formData.nivel > 1 && (
            <div className="mt-2">
              <Autocomplete
                label={getParentLabel()}
                onSearch={async (term) => {
                  const endpoint = getParentSearchEndpoint();
                  if (!endpoint) return [];
                  const res = await api.get(endpoint);
                  const items = res.data.success ? res.data.data : res.data; // Fallback for safety
                  return items.filter((o: Orgao) => o.nome.toLowerCase().includes(term.toLowerCase()) || o.abreviatura.toLowerCase().includes(term.toLowerCase()));
                }}
                placeholder={`Buscar e selecionar ${formData.nivel === 2 ? 'órgão' : 'secretaria'}...`}
                onSelect={(item) => {
                  if (item) {
                    setFormData({ ...formData, orgaoPaiId: item.id });
                  } else {
                    setFormData({ ...formData, orgaoPaiId: '' });
                  }
                }}
                displayValue={(item) => `${item.nome} (${item.abreviatura})`}
                keyValue={(item) => item.id}
                defaultValue={editingItem?.orgaoPaiId ? editingItem.nomeAbreviaturaPai : undefined}
              />
            </div>
          )}
        </form>
      </Modal>
    </div>
  );
}
