import React, { useState, useEffect } from 'react';
import { Briefcase, Plus, Edit, Trash2, CheckCircle, XCircle } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Table, type TableColumn } from '../components/common/Table';
import { FilterBar } from '../components/common/FilterBar';
import { Modal } from '../components/common/Modal';
import { Autocomplete } from '../components/common/Autocomplete';
import { EnumSelect } from '../components/common/EnumSelect';
import { DatePicker } from '../components/common/DatePicker';
import { parseISO, format } from 'date-fns';
import { useAuth } from '../hooks/useAuth';

interface Orgao { id: string; nome: string; abreviatura: string; }
interface Colaborador { id: string; nome: string; cpf: string; }
interface Cargo { id: string; nome: string; cbo: string; }
interface CentroCusto { id: string; descricao: string; }

interface Vinculo {
  id: string;
  colaboradorId: string;
  colaboradorNome: string;
  colaboradorCpf: string;
  orgaoId: string;
  orgaoNome: string;
  orgaoAbreviatura: string;
  matricula: string;
  cargoId: string;
  cargoNome: string;
  regimeJuridicoId: number;
  regimeJuridicoDescricao: string;
  formaIngressoId: number;
  formaIngressoDescricao: string;
  centroCustoId: string;
  centroCustoDescricao: string;
  dataAdmissao: string;
  ativo: boolean;
}

export default function Vinculos() {
  const [vinculos, setVinculos] = useState<Vinculo[]>([]);
  const [loading, setLoading] = useState(true);
  const { hasPermission } = useAuth();
  const [filters, setFilters] = useState({ matricula: '', nomeColaborador: '' });

  const [pagination, setPagination] = useState({ currentPage: 1, totalPages: 1, totalCount: 0, pageSize: 10 });
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingVinculo, setEditingVinculo] = useState<Vinculo | null>(null);

  const [formData, setFormData] = useState({
    colaboradorId: '',
    orgaoId: '',
    matricula: '',
    cargoId: '',
    regimeJuridicoId: 0,
    formaIngressoId: 0,
    centroCustoId: '',
    dataAdmissao: null as Date | null
  });

  const canAdd = hasPermission('Vinculos', 'Criar');
  const canEdit = hasPermission('Vinculos', 'Editar');
  const canDelete = hasPermission('Vinculos', 'Excluir');

  useEffect(() => {
    fetchVinculos();
  }, []);

  const fetchVinculos = async (page = 1, f = filters) => {
    try {
      showLoading('Carregando vínculos...');
      const params: any = { page, pageSize: pagination.pageSize };
      if (f.matricula) params.matricula = f.matricula;
      if (f.nomeColaborador) params.nomeColaborador = f.nomeColaborador;

      const response = await api.get('/vinculos', { params });
      setVinculos(response.data.items);
      setPagination(prev => ({ ...prev, currentPage: response.data.page, totalPages: response.data.totalPages, totalCount: response.data.totalCount }));
    } catch (error) {
      alertError('Erro ao carregar vínculos');
    } finally {
      closeLoading();
      setLoading(false);
    }
  };

  const handleFilter = () => fetchVinculos(1, filters);
  const handleClearFilters = () => {
    setFilters({ matricula: '', nomeColaborador: '' });
    fetchVinculos(1, { matricula: '', nomeColaborador: '' });
  };

  const openNewModal = () => {
    setEditingVinculo(null);
    setFormData({
      colaboradorId: '', orgaoId: '', matricula: '', cargoId: '',
      regimeJuridicoId: 0, formaIngressoId: 0, centroCustoId: '', dataAdmissao: null
    });
    setIsModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      showLoading('Salvando vínculo...');
      if (editingVinculo) {
        await api.put(`/vinculos/${editingVinculo.id}`, formData);
        alertSuccess('Sucesso', 'Vínculo atualizado com sucesso.');
      } else {
        await api.post('/vinculos', formData);
        alertSuccess('Sucesso', 'Vínculo criado com sucesso.');
      }
      setIsModalOpen(false);
      fetchVinculos(pagination.currentPage);
    } catch (error: any) {
      alertError('Atenção', error.response?.data?.message || 'Erro ao salvar vínculo.');
    } finally {
      closeLoading();
    }
  };

  const handleEdit = (v: Vinculo) => {
    setEditingVinculo(v);
    setFormData({
      colaboradorId: v.colaboradorId,
      orgaoId: v.orgaoId,
      matricula: v.matricula,
      cargoId: v.cargoId,
      regimeJuridicoId: v.regimeJuridicoId,
      formaIngressoId: v.formaIngressoId,
      centroCustoId: v.centroCustoId,
      dataAdmissao: v.dataAdmissao ? parseISO(v.dataAdmissao) : null
    });
    setIsModalOpen(true);
  };

  const handleDelete = async (id: string) => {
    const confirmed = await alertDeleteConfirm('Tem certeza que deseja excluir este vínculo?');
    if (confirmed) {
      try {
        await api.delete(`/vinculos/${id}`);
        alertSuccess('Sucesso', 'Vínculo excluído com sucesso.');
        fetchVinculos(pagination.currentPage);
      } catch (error) {
        alertError('Erro ao excluir o vínculo.');
      }
    }
  };

  const handleToggleStatus = async (v: Vinculo) => {
    try {
      await api.put(`/vinculos/${v.id}/toggle-status`);
      alertSuccess('Sucesso', v.ativo ? 'Vínculo inativado.' : 'Vínculo ativado com sucesso.');
      fetchVinculos(pagination.currentPage);
    } catch (error) {
      alertError('Erro ao alterar o status do vínculo.');
    }
  };

  const columns: TableColumn<Vinculo>[] = [
    { header: 'Matrícula', accessor: 'matricula' },
    { header: 'Nome', accessor: 'colaboradorNome' },
    { header: 'Órgão', accessor: 'orgaoNome' },
    { header: 'Cargo', accessor: 'cargoNome' },
    { header: 'Centro de Custos', accessor: 'centroCustoDescricao' },
    { header: 'Admissão', render: (v) => v.dataAdmissao ? format(parseISO(v.dataAdmissao), 'dd/MM/yyyy') : '-' },
    {
      header: 'Status',
      render: (v) => (
        <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${v.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
          {v.ativo ? 'Ativo' : 'Inativo'}
        </span>
      )
    },
    {
      header: 'Ações',
      align: 'right',
      render: (v) => (
        <div className="flex justify-end space-x-3 items-center">
          {canEdit && (
            <>
              <button title="Editar" onClick={() => handleEdit(v)} className="text-indigo-600 hover:text-indigo-900">
                <Edit size={18} />
              </button>
            </>
          )}
          {canDelete && (
            <button title="Excluir" onClick={() => handleDelete(v.id)} className="text-red-600 hover:text-red-900">
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
            <Briefcase className="mr-3 text-indigo-600" size={28} />
            Vínculos Funcionais
          </h1>
          <p className="text-gray-500 mt-1 text-sm font-medium">Gerencie matrículas, cargos e relotações de colaboradores.</p>
        </div>
        {canAdd && (
          <button
            onClick={openNewModal}
            className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-md flex items-center transition-colors font-medium shadow-sm"
          >
            <Plus size={20} className="mr-2" />
            Novo Vínculo
          </button>
        )}
      </div>

      <FilterBar onFilter={handleFilter} onClear={handleClearFilters}>
        <FormInput
          label="Matrícula"
          value={filters.matricula}
          onChange={(e) => setFilters({ ...filters, matricula: e.target.value })}
          placeholder="Buscar por matrícula..."
        />
        <FormInput
          label="Nome do Colaborador"
          value={filters.nomeColaborador}
          onChange={(e) => setFilters({ ...filters, nomeColaborador: e.target.value })}
          placeholder="Buscar por nome..."
        />
      </FilterBar>

      <Table<Vinculo>
        columns={columns}
        data={vinculos}
        isLoading={loading}
        emptyMessage="Nenhum vínculo encontrado."
        pagination={{
          currentPage: pagination.currentPage,
          totalPages: pagination.totalPages,
          totalCount: pagination.totalCount,
          pageSize: pagination.pageSize,
          onPageChange: (page) => fetchVinculos(page)
        }}
      />

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingVinculo ? 'Editar Vínculo' : 'Novo Vínculo'}
        size="lg"
        footer={(
          <>
            <button
              type="submit"
              form="vinculo-form"
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
        <form id="vinculo-form" onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="md:col-span-2">
              <Autocomplete
                label="Colaborador (Pessoa)"
                required
                onSearch={async (term) => {
                  const res = await api.get('/colaboradores', { params: { nome: term } });
                  return res.data.items;
                }}
                displayValue={(c: Colaborador) => `${c.nome} (${c.cpf})`}
                keyValue={(c: Colaborador) => c.id}
                onSelect={(val: any) => setFormData({ ...formData, colaboradorId: val?.id || '' })}
                defaultValue={editingVinculo ? `${editingVinculo.colaboradorNome} (${editingVinculo.colaboradorCpf})` : ''}
                placeholder="Buscar colaborador..."
              />
            </div>

            <FormInput
              label="Matrícula"
              required
              value={formData.matricula}
              onChange={(e) => setFormData({ ...formData, matricula: e.target.value })}
              placeholder="Ex: 541289"
            />

            <div>
              <DatePicker
                label="Data de Admissão"
                required
                selected={formData.dataAdmissao}
                onChange={(date) => setFormData({ ...formData, dataAdmissao: date })}
              />
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="md:col-span-2">
              <Autocomplete
                label="Local / Órgão de Lotação"
                required
                onSearch={async (term) => {
                  const res = await api.get('/orgaos');
                  return res.data.filter((o: Orgao) => o.nome.toLowerCase().includes(term.toLowerCase()) || o.abreviatura.toLowerCase().includes(term.toLowerCase()));
                }}
                displayValue={(o: Orgao) => `${o.nome} (${o.abreviatura})`}
                keyValue={(o: Orgao) => o.id}
                onSelect={(val: any) => setFormData({ ...formData, orgaoId: val?.id || '' })}
                defaultValue={editingVinculo ? `${editingVinculo.orgaoNome} (${editingVinculo.orgaoAbreviatura})` : ''}
                placeholder="Selecione o órgão..."
              />
            </div>

            <div className="md:col-span-2">
              <Autocomplete
                label="Centro de Custo"
                required
                onSearch={async (term) => {
                  const res = await api.get('/centro-custos', { params: { descricao: term } });
                  return res.data;
                }}
                displayValue={(cc: CentroCusto) => cc.descricao}
                keyValue={(cc: CentroCusto) => cc.id}
                onSelect={(val: any) => setFormData({ ...formData, centroCustoId: val?.id || '' })}
                defaultValue={editingVinculo ? editingVinculo.centroCustoDescricao : ''}
                placeholder="Vincule ao Centro de Custo..."
              />
            </div>

            <div className="md:col-span-2">
              <Autocomplete
                label="Cargo Ocupado"
                required
                onSearch={async (term) => {
                  const res = await api.get('/cargos', { params: { nome: term } });
                  return res.data.items;
                }}
                displayValue={(c: Cargo) => c.nome}
                keyValue={(c: Cargo) => c.id}
                onSelect={(val: any) => setFormData({ ...formData, cargoId: val?.id || '' })}
                defaultValue={editingVinculo ? editingVinculo.cargoNome : ''}
                placeholder="Buscar cargo funcional..."
              />
            </div>

            <EnumSelect
              label="Regime Jurídico / Tipo de Vínculo"
              required
              enumType="RegimeJuridico"
              value={formData.regimeJuridicoId}
              onChange={(e) => setFormData({ ...formData, regimeJuridicoId: Number(e.target.value) })}
            />

            <EnumSelect
              label="Forma de Ingresso"
              required
              enumType="FormaIngresso"
              value={formData.formaIngressoId}
              onChange={(e) => setFormData({ ...formData, formaIngressoId: Number(e.target.value) })}
            />
          </div>
        </form>
      </Modal>
    </div>
  );
}
