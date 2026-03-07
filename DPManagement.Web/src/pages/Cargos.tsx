import { useState, useEffect } from 'react';
import { Plus, Trash2, Edit } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Table, type TableColumn } from '../components/common/Table';
import { FilterBar } from '../components/common/FilterBar';
import { Autocomplete } from '../components/common/Autocomplete';
import { Modal } from '../components/common/Modal';

interface Cargo {
  id: string;
  nome: string;
  cbo: string;
}

export default function Cargos() {
  const [cargos, setCargos] = useState<Cargo[]>([]);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    totalPages: 1,
    totalCount: 0,
    pageSize: 10
  });
  const [filters, setFilters] = useState({
    nome: '',
    cbo: ''
  });
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCargo, setEditingCargo] = useState<Cargo | null>(null);
  const [formData, setFormData] = useState({ nome: '', cbo: '' });

  useEffect(() => {
    fetchCargos();
  }, []);

  const fetchCargos = async (page = 1, f = filters) => {
    try {
      showLoading('Carregando...');
      const params: any = {
        page,
        pageSize: pagination.pageSize
      };
      if (f.nome) params.nome = f.nome;
      if (f.cbo) params.cbo = f.cbo;

      const response = await api.get('/cargos', { params });
      setCargos(response.data.items);
      setPagination(prev => ({
        ...prev,
        currentPage: response.data.page,
        totalPages: response.data.totalPages,
        totalCount: response.data.totalCount
      }));
    } catch (error) {
      console.error('Erro ao buscar cargos', error);
      alertError('Erro ao carregar cargos');
    } finally {
      closeLoading();
    }
  };

  const applyFilters = () => fetchCargos(1);
  const clearFilters = () => {
    const newFilters = { nome: '', cbo: '' };
    setFilters(newFilters);
    fetchCargos(1, newFilters);
  };



  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    showLoading('Salvando cargo...');
    try {
      if (editingCargo) {
        await api.put(`/cargos/${editingCargo.id}`, { ...formData, id: editingCargo.id });
        alertSuccess('Cargo atualizado');
      } else {
        await api.post('/cargos', formData);
        alertSuccess('Cargo cadastrado');
      }
      setIsModalOpen(false);
      setEditingCargo(null);
      setFormData({ nome: '', cbo: '' });
      fetchCargos();
    } catch (error) {
      console.error('Erro ao salvar cargo', error);
      alertError('Erro ao salvar cargo');
    } finally {
      closeLoading();
    }
  };

  const handleEdit = (cargo: Cargo) => {
    setEditingCargo(cargo);
    setFormData({ nome: cargo.nome, cbo: cargo.cbo });
    setIsModalOpen(true);
  };

  const handleDelete = async (id: string) => {
    const confirmed = await alertDeleteConfirm('Excluir Cargo?', 'Esta ação removerá o cargo permanentemente.');
    if (confirmed) {
      showLoading('Excluindo...');
      try {
        await api.delete(`/cargos/${id}`);
        alertSuccess('Cargo excluído');
        fetchCargos();
      } catch (error) {
        console.error('Erro ao excluir cargo', error);
        alertError('Erro ao excluir cargo');
      } finally {
        closeLoading();
      }
    }
  };

  const columns: TableColumn<Cargo>[] = [
    { header: 'Nome', accessor: 'nome' },
    { header: 'CBO', accessor: 'cbo' },
    {
      header: 'Ações',
      align: 'right',
      render: (cargo) => (
        <div className="space-x-4">
          <button onClick={() => handleEdit(cargo)} className="text-indigo-600 hover:text-indigo-900">
            <Edit size={18} />
          </button>
          <button onClick={() => handleDelete(cargo.id)} className="text-red-600 hover:text-red-900">
            <Trash2 size={18} />
          </button>
        </div>
      ),
    },
  ];

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold text-gray-900">Gestão de Cargos</h1>
        <button
          onClick={() => { setEditingCargo(null); setFormData({ nome: '', cbo: '' }); setIsModalOpen(true); }}
          className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors"
        >
          <Plus size={18} className="mr-2" />
          Novo Cargo
        </button>
      </div>

      <FilterBar onFilter={applyFilters} onClear={clearFilters}>
        <FormInput
          label="Nome"
          value={filters.nome}
          onChange={e => setFilters({ ...filters, nome: e.target.value })}
          placeholder="Buscar por nome..."
        />
        <FormInput
          label="CBO"
          value={filters.cbo}
          onChange={e => setFilters({ ...filters, cbo: e.target.value })}
          placeholder="Buscar por CBO..."
        />
      </FilterBar>

      <Table
        data={cargos}
        columns={columns}
        pagination={{
          currentPage: pagination.currentPage,
          totalPages: pagination.totalPages,
          totalCount: pagination.totalCount,
          pageSize: pagination.pageSize,
          onPageChange: (page) => fetchCargos(page),
        }}
      />

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingCargo ? 'Editar Cargo' : 'Novo Cargo'}
        size="lg"
        footer={(
          <>
            <button
              type="submit"
              form="cargo-form"
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
        <form id="cargo-form" onSubmit={handleSubmit}>
          <div className="space-y-4">
            <FormInput
              label="Nome"
              required
              value={formData.nome}
              onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
            />
            <Autocomplete
              label="CBO"
              required
              placeholder="Busque por código ou título..."
              defaultValue={formData.cbo}
              onSearch={async (term) => {
                const response = await api.get(`/cbos?term=${encodeURIComponent(term)}`);
                return response.data;
              }}
              onSelect={(item) => {
                if (item) {
                  setFormData({ ...formData, cbo: item.codigo });
                } else {
                  setFormData({ ...formData, cbo: '' });
                }
              }}
              displayValue={(item) => `${item.codigo} - ${item.titulo}`}
              keyValue={(item) => item.codigo}
            />
          </div>
        </form>
      </Modal>
    </div>
  );
}
