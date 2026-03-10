import React, { useState, useEffect } from 'react';
import { BadgeDollarSign, Plus, Edit, Trash2, CheckCircle, XCircle } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Table, type TableColumn } from '../components/common/Table';
import { FilterBar } from '../components/common/FilterBar';
import { Modal } from '../components/common/Modal';
import { EnumSelect } from '../components/common/EnumSelect';
import { useAuth } from '../hooks/useAuth';

interface Rubrica {
  id: string;
  codigo: string;
  descricao: string;
  tipo: number;
  tipoDescricao: string;
  incideIR: boolean;
  incidePrevidencia: boolean;
  ativo: boolean;
}

export default function Rubricas() {
  const [rubricas, setRubricas] = useState<Rubrica[]>([]);
  const [loading, setLoading] = useState(true);
  const { hasPermission } = useAuth();
  const [filtro, setFiltro] = useState('');
  const [pagination, setPagination] = useState({ currentPage: 1, totalPages: 1, totalCount: 0, pageSize: 10 });
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingRubrica, setEditingRubrica] = useState<Rubrica | null>(null);
  const [formData, setFormData] = useState({
    codigo: '',
    descricao: '',
    tipo: 1,
    incideIR: false,
    incidePrevidencia: false
  });

  const canAdd = hasPermission('Rubricas', 'Criar');
  const canEdit = hasPermission('Rubricas', 'Editar');
  const canDelete = hasPermission('Rubricas', 'Excluir');

  useEffect(() => {
    fetchRubricas();
  }, []);

  const fetchRubricas = async (page = 1, f = filtro) => {
    try {
      showLoading('Carregando rubricas...');
      const response = await api.get('/rubricas', {
        params: { page, pageSize: pagination.pageSize, filtro: f }
      });
      setRubricas(response.data.items);
      setPagination(prev => ({
        ...prev,
        currentPage: response.data.page,
        totalPages: response.data.totalPages,
        totalCount: response.data.totalCount
      }));
    } catch (error) {
      alertError('Erro ao carregar rubricas');
    } finally {
      closeLoading();
      setLoading(false);
    }
  };

  const handleFilter = () => fetchRubricas(1, filtro);
  const handleClearFilters = () => {
    setFiltro('');
    fetchRubricas(1, '');
  };

  const openNewModal = () => {
    setEditingRubrica(null);
    setFormData({ codigo: '', descricao: '', tipo: 1, incideIR: false, incidePrevidencia: false });
    setIsModalOpen(true);
  };

  const handleEdit = (r: Rubrica) => {
    setEditingRubrica(r);
    setFormData({
      codigo: r.codigo,
      descricao: r.descricao,
      tipo: r.tipo,
      incideIR: r.incideIR,
      incidePrevidencia: r.incidePrevidencia
    });
    setIsModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      showLoading('Salvando rubrica...');
      if (editingRubrica) {
        await api.put(`/rubricas/${editingRubrica.id}`, formData);
        alertSuccess('Sucesso', 'Rubrica atualizada com sucesso.');
      } else {
        await api.post('/rubricas', formData);
        alertSuccess('Sucesso', 'Rubrica criada com sucesso.');
      }
      setIsModalOpen(false);
      fetchRubricas(pagination.currentPage);
    } catch (error: any) {
      alertError('Erro ao salvar rubrica', error.response?.data?.message);
    } finally {
      closeLoading();
    }
  };

  const handleDelete = async (id: string) => {
    const confirmed = await alertDeleteConfirm('Deseja excluir esta rubrica?');
    if (confirmed) {
      try {
        await api.delete(`/rubricas/${id}`);
        alertSuccess('Sucesso', 'Rubrica excluída.');
        fetchRubricas(pagination.currentPage);
      } catch (error) {
        alertError('Erro ao excluir rubrica');
      }
    }
  };

  const handleToggleStatus = async (r: Rubrica) => {
    try {
      await api.put(`/rubricas/${r.id}/toggle-status`);
      alertSuccess('Sucesso', r.ativo ? 'Rubrica inativada.' : 'Rubrica ativada.');
      fetchRubricas(pagination.currentPage);
    } catch (error) {
      alertError('Erro ao alterar status');
    }
  };

  const columns: TableColumn<Rubrica>[] = [
    { header: 'Código', accessor: 'codigo' },
    { header: 'Descrição', accessor: 'descricao' },
    { header: 'Tipo', accessor: 'tipoDescricao' },
    { 
      header: 'Incidências', 
      render: (r) => (
        <div className="flex gap-2">
          {r.incideIR && <span className="px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded">IR</span>}
          {r.incidePrevidencia && <span className="px-2 py-1 bg-purple-100 text-purple-800 text-xs rounded">Prev</span>}
          {!r.incideIR && !r.incidePrevidencia && <span className="text-gray-400 text-xs">-</span>}
        </div>
      )
    },
    {
      header: 'Status',
      render: (r) => (
        <span className={`px-2 py-1 text-xs rounded-full font-semibold ${r.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
          {r.ativo ? 'Ativo' : 'Inativo'}
        </span>
      )
    },
    {
      header: 'Ações',
      align: 'right',
      render: (r) => (
        <div className="flex justify-end space-x-2">
          {canEdit && (
            <>
              <button onClick={() => handleEdit(r)} className="text-indigo-600 hover:text-indigo-900" title="Editar">
                <Edit size={18} />
              </button>
              <button onClick={() => handleToggleStatus(r)} className={r.ativo ? "text-red-500 hover:text-red-700" : "text-green-500 hover:text-green-700"} title={r.ativo ? "Inativar" : "Ativar"}>
                {r.ativo ? <XCircle size={18} /> : <CheckCircle size={18} />}
              </button>
            </>
          )}
          {canDelete && (
            <button onClick={() => handleDelete(r.id)} className="text-red-600 hover:text-red-900" title="Excluir">
              <Trash2 size={18} />
            </button>
          )}
        </div>
      )
    }
  ];

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center bg-white p-6 rounded-lg shadow-sm border border-gray-100">
        <div>
          <h1 className="text-2xl font-bold flex items-center text-gray-800">
            <BadgeDollarSign className="mr-3 text-indigo-600" size={28} />
            Rubricas / Eventos
          </h1>
          <p className="text-gray-500 mt-1 text-sm font-medium">Cadastre proventos, descontos e suas incidências.</p>
        </div>
        {canAdd && (
          <button onClick={openNewModal} className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-md flex items-center transition-colors font-medium shadow-sm">
            <Plus size={20} className="mr-2" />
            Nova Rubrica
          </button>
        )}
      </div>

      <FilterBar onFilter={handleFilter} onClear={handleClearFilters}>
        <FormInput
          label="Buscar"
          value={filtro}
          onChange={(e) => setFiltro(e.target.value)}
          placeholder="Código ou descrição..."
        />
      </FilterBar>

      <Table<Rubrica>
        columns={columns}
        data={rubricas}
        isLoading={loading}
        emptyMessage="Nenhuma rubrica encontrada."
        pagination={{
            ...pagination,
            onPageChange: (page) => fetchRubricas(page)
        }}
      />

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingRubrica ? 'Editar Rubrica' : 'Nova Rubrica'}
        size="md"
        footer={(
          <>
            <button type="submit" form="rubrica-form" className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm">
              Salvar
            </button>
            <button type="button" onClick={() => setIsModalOpen(false)} className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm">
              Cancelar
            </button>
          </>
        )}
      >
        <form id="rubrica-form" onSubmit={handleSubmit} className="space-y-4">
          <FormInput
            label="Código"
            required
            value={formData.codigo}
            onChange={(e) => setFormData({ ...formData, codigo: e.target.value })}
            placeholder="Ex: 001, 101..."
          />
          <FormInput
            label="Descrição"
            required
            value={formData.descricao}
            onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
            placeholder="Ex: Salário Base, INSS..."
          />
          <EnumSelect
            label="Tipo de Rubrica"
            required
            enumType="TipoRubrica"
            value={formData.tipo}
            onChange={(e) => setFormData({ ...formData, tipo: Number(e.target.value) })}
          />
          <div className="flex gap-6 mt-4">
            <label className="flex items-center space-x-3 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.incideIR}
                onChange={(e) => setFormData({ ...formData, incideIR: e.target.checked })}
                className="h-5 w-5 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
              />
              <span className="text-sm font-medium text-gray-700">Incide IR</span>
            </label>
            <label className="flex items-center space-x-3 cursor-pointer">
              <input
                type="checkbox"
                checked={formData.incidePrevidencia}
                onChange={(e) => setFormData({ ...formData, incidePrevidencia: e.target.checked })}
                className="h-5 w-5 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
              />
              <span className="text-sm font-medium text-gray-700">Incide Previdência</span>
            </label>
          </div>
        </form>
      </Modal>
    </div>
  );
}
