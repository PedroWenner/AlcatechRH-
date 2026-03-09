import { useState, useEffect } from 'react';
import { Plus, CheckCircle, XCircle, FolderTree, Shield, Edit } from 'lucide-react';
import api from '../services/api';
import { FilterBar } from '../components/common/FilterBar';
import { FormInput } from '../components/common/FormInput';
import { Autocomplete } from '../components/common/Autocomplete';
import { Modal } from '../components/common/Modal';
import { Table } from '../components/common/Table';
import type { TableColumn } from '../components/common/Table';
import { alertSuccess, alertError } from '../services/alertService';
import { useAuth } from '../hooks/useAuth';

interface CentroCusto {
  id: string;
  descricao: string;
  orgaoId: string;
  orgaoNome: string;
  ativo: boolean;
}

interface Orgao {
  id: string;
  nome: string;
  abreviatura: string;
}

export default function CentroCustos() {
  const { hasPermission } = useAuth();
  const [centros, setCentros] = useState<CentroCusto[]>([]);
  const [orgaos, setOrgaos] = useState<Orgao[]>([]);
  const [loading, setLoading] = useState(true);
  const [filters, setFilters] = useState({ descricao: '' });
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCentro, setEditingCentro] = useState<CentroCusto | null>(null);
  const [formData, setFormData] = useState({ descricao: '', orgaoId: '' });
  const [selectedOrgaoNome, setSelectedOrgaoNome] = useState('');

  const checkPerm = (acao: string) => hasPermission('CentroCustos', acao);

  const fetchCentros = async () => {
    try {
      setLoading(true);
      const params = new URLSearchParams();
      if (filters.descricao) params.append('descricao', filters.descricao);
      
      const response = await api.get('/centro-custos', { params });
      setCentros(response.data);
    } catch (error) {
      alertError('Erro ao carregar Centros de Custos');
    } finally {
      setLoading(false);
    }
  };

  const fetchOrgaos = async () => {
    try {
      const response = await api.get('/orgaos');
      setOrgaos(response.data);
    } catch (error) {
      alertError('Erro ao carregar órgãos para o select');
    }
  };

  useEffect(() => {
    if (checkPerm('Visualizar')) {
      fetchCentros();
      fetchOrgaos();
    }
  }, []);

  const handleFilter = () => fetchCentros();
  
  const handleClearFilters = () => {
    setFilters({ descricao: '' });
    setTimeout(fetchCentros, 0);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.descricao || !formData.orgaoId) {
        alertError('A descrição e o vínculo com Órgão são obrigatórios.');
        return;
    }
    try {
      if (editingCentro) {
        await api.put(`/centro-custos/${editingCentro.id}`, formData);
        alertSuccess('Centro de Custo atualizado com sucesso');
      } else {
        await api.post('/centro-custos', formData);
        alertSuccess('Centro de Custo criado com sucesso');
      }
      setIsModalOpen(false);
      resetForm();
      fetchCentros();
    } catch (error) {
      alertError('Erro ao salvar Centro de Custo');
    }
  };

  const handleToggleStatus = async (id: string, currentStatus: boolean) => {
    try {
      await api.put(`/centro-custos/${id}/ativar?ativo=${!currentStatus}`);
      alertSuccess(`Centro de custo ${currentStatus ? 'inativado' : 'ativado'} com sucesso`);
      fetchCentros();
    } catch (error) {
      alertError(`Erro ao ${currentStatus ? 'inativar' : 'ativar'} centro de custo`);
    }
  };

  const resetForm = () => {
    setEditingCentro(null);
    setFormData({ descricao: '', orgaoId: '' });
    setSelectedOrgaoNome('');
  };

  const openNewModal = () => {
    resetForm();
    setIsModalOpen(true);
  };

  const openEditModal = (centro: CentroCusto) => {
    setEditingCentro(centro);
    setFormData({ descricao: centro.descricao, orgaoId: centro.orgaoId });
    setSelectedOrgaoNome(centro.orgaoNome);
    setIsModalOpen(true);
  };

  // Removed unused orgaoOptions

  const columns: TableColumn<CentroCusto>[] = [
    { header: 'Descrição', accessor: 'descricao' },
    { header: 'Órgão Vinculado', accessor: 'orgaoNome' },
    {
      header: 'Status',
      render: (centro: CentroCusto) => (
        <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
          centro.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
        }`}>
          {centro.ativo ? 'Ativo' : 'Inativo'}
        </span>
      )
    },
    {
      header: 'Ações',
      align: 'right',
      render: (centro: CentroCusto) => (
        <div className="flex space-x-4 justify-end">
          {checkPerm('Editar') && (
            <button
              onClick={() => openEditModal(centro)}
              className="text-indigo-600 hover:text-indigo-900"
              title="Editar"
            >
              <Edit size={18} />
            </button>
          )}
          {checkPerm('Editar') && (
            <button
              onClick={() => handleToggleStatus(centro.id, centro.ativo)}
              className={`${centro.ativo ? 'text-red-600 hover:text-red-900' : 'text-green-600 hover:text-green-900'}`}
              title={centro.ativo ? 'Inativar' : 'Ativar'}
            >
              {centro.ativo ? <XCircle size={18} /> : <CheckCircle size={18} />}
            </button>
          )}
        </div>
      )
    }
  ];

  if (!checkPerm('Visualizar')) {
    return (
      <div className="flex items-center justify-center h-full">
        <div className="text-center">
          <Shield size={48} className="mx-auto text-gray-400 mb-4" />
          <h2 className="text-2xl font-semibold text-gray-900">Acesso Restrito</h2>
          <p className="mt-2 text-gray-600">Você não tem permissão para acessar esta página.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center bg-white p-6 rounded-lg shadow-sm border border-gray-200 mb-6">
        <div>
          <h1 className="text-2xl font-bold text-gray-800 flex items-center gap-2">
            <FolderTree className="text-indigo-600" />
            Centros de Custos
          </h1>
          <p className="text-gray-500 mt-1">Gerencie as partições orçamentárias e financeiras da organização.</p>
        </div>
        {checkPerm('Criar') && (
          <button
            onClick={openNewModal}
            className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors shadow-sm"
          >
            <Plus size={20} className="mr-2" />
            Novo Centro de Custo
          </button>
        )}
      </div>

      <FilterBar onFilter={handleFilter} onClear={handleClearFilters}>
        <FormInput
          label="Descrição"
          value={filters.descricao}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) => setFilters({ ...filters, descricao: e.target.value })}
          placeholder="Buscar por descrição..."
        />
      </FilterBar>

      <Table<CentroCusto>
        columns={columns}
        data={centros}
        isLoading={loading}
        emptyMessage="Nenhum centro de custo encontrado."
      />

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingCentro ? 'Editar Centro de Custo' : 'Novo Centro de Custo'}
      >
        <form onSubmit={handleSubmit} className="space-y-4">
          <FormInput
            label="Descrição"
            required
            value={formData.descricao}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setFormData({ ...formData, descricao: e.target.value })}
            placeholder="Ex: Fundo da Saúde..."
          />
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Vincular a Órgão <span className="text-red-500">*</span>
            </label>
            <Autocomplete
              label=""
              onSearch={async (term: string) => {
                  return orgaos.filter((o: Orgao) => 
                    o.nome.toLowerCase().includes(term.toLowerCase()) || 
                    o.abreviatura.toLowerCase().includes(term.toLowerCase())
                  );
              }}
              displayValue={(item: Orgao) => `${item.nome} (${item.abreviatura})`}
              keyValue={(item: Orgao) => item.id}
              defaultValue={selectedOrgaoNome}
              onSelect={(suggestion: Orgao | null) => {
                if (suggestion) {
                  setSelectedOrgaoNome(`${suggestion.nome} (${suggestion.abreviatura})`);
                  setFormData({ ...formData, orgaoId: suggestion.id });
                } else {
                  setSelectedOrgaoNome('');
                  setFormData({ ...formData, orgaoId: '' });
                }
              }}
              placeholder="Digite o nome do Órgão..."
            />
          </div>
          <div className="flex justify-end space-x-3 mt-6">
            <button
              type="button"
              onClick={() => setIsModalOpen(false)}
              className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
            >
              Cancelar
            </button>
            <button
              type="submit"
              className="px-4 py-2 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
            >
              Salvar
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
