import React, { useState, useEffect } from 'react';
import { BadgeDollarSign, Plus, Edit, Trash2, CheckCircle, XCircle, Search } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Table, type TableColumn } from '../components/common/Table';
import { FilterBar } from '../components/common/FilterBar';
import { Modal } from '../components/common/Modal';
import { EnumSelect } from '../components/common/EnumSelect';
import { FormSelect } from '../components/common/FormSelect';
import { DatePicker } from '../components/common/DatePicker';
import { Autocomplete } from '../components/common/Autocomplete';
import { useAuth } from '../hooks/useAuth';
import { useFormValidation } from '../hooks/useFormValidation';

interface NaturezaRubrica {
  codigo: string;
  nome: string;
  descricao: string;
}

interface Rubrica {
  id: string;
  codigo: string;
  descricao: string;
  tipo: number;
  tipoDescricao: string;
  rotina: number;
  rotinaDescricao: string;
  incideIR: boolean;
  incidePrevidencia: boolean;
  ativo: boolean;
  // eSocial
  natRubr?: string;
  ideTabRubr?: string;
  codIncCP?: string;
  codIncIRRF?: string;
  codIncFGTS?: string;
  codIncPisPasep?: string;
  iniValid: string;
  fimValid?: string;
}

export default function Rubricas() {
  const [rubricas, setRubricas] = useState<Rubrica[]>([]);
  const [naturezas, setNaturezas] = useState<NaturezaRubrica[]>([]);
  const [loading, setLoading] = useState(true);
  const { hasPermission } = useAuth();
  const [filtro, setFiltro] = useState('');
  const [pagination, setPagination] = useState({ currentPage: 1, totalPages: 1, totalCount: 0, pageSize: 10 });
  const [activeTab, setActiveTab] = useState<'geral' | 'esocial'>('geral');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isNaturezaModalOpen, setIsNaturezaModalOpen] = useState(false);
  const [editingRubrica, setEditingRubrica] = useState<Rubrica | null>(null);
  const [selectedNaturezaLabel, setSelectedNaturezaLabel] = useState('');
  const [formData, setFormData] = useState({
    codigo: '',
    descricao: '',
    tipo: 1,
    rotina: 0,
    incideIR: false,
    incidePrevidencia: false,
    natRubr: '',
    ideTabRubr: 'PADRAO',
    codIncCP: '00',
    codIncIRRF: '00',
    codIncFGTS: '00',
    codIncPisPasep: '00',
    iniValid: new Date().toISOString().substring(0, 7),
    fimValid: ''
  });

  const { errors, validateField, validateAll, getError, clearAllErrors } = useFormValidation({
    rules: {
      codigo: {
        required: 'O código é obrigatório.',
        maxLength: { value: 20, message: 'O código não pode exceder 20 caracteres.' }
      },
      descricao: {
        required: 'A descrição é obrigatória.',
        maxLength: { value: 200, message: 'A descrição não pode exceder 200 caracteres.' }
      },
      natRubr: {
        required: 'A natureza da rubrica (eSocial) é obrigatória.'
      },
      iniValid: {
        required: 'O início de validade é obrigatório.'
      }
    }
  });

  const canAdd = hasPermission('Rubricas', 'Criar');
  const canEdit = hasPermission('Rubricas', 'Editar');
  const canDelete = hasPermission('Rubricas', 'Excluir');

  useEffect(() => {
    fetchRubricas();
    fetchNaturezas();
  }, []);

  const fetchNaturezas = async () => {
    try {
      const response = await api.get('/naturezas-rubricas');
      if (response.data.success) {
        setNaturezas(response.data.data);
      }
    } catch (error) {
      console.error('Erro ao buscar naturezas eSocial:', error);
    }
  };

  const handleNaturezaSearch = async (term: string) => {
    const filtered = naturezas.filter(n =>
      n.codigo.includes(term) ||
      n.nome.toLowerCase().includes(term.toLowerCase())
    );
    return filtered;
  };

  const fetchRubricas = async (page = 1, f = filtro) => {
    try {
      showLoading('Carregando rubricas...');
      const response = await api.get('/rubricas', {
        params: { page, pageSize: pagination.pageSize, filtro: f }
      });

      const resData = response.data;
      if (resData.success) {
        setRubricas(resData.data.items);
        setPagination(prev => ({
          ...prev,
          currentPage: resData.data.page,
          totalPages: resData.data.totalPages,
          totalCount: resData.data.totalCount
        }));
      } else {
        alertError(resData.message || 'Erro ao carregar rubricas');
      }
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
    setSelectedNaturezaLabel('');
    setFormData({
      codigo: '',
      descricao: '',
      tipo: 1,
      rotina: 0,
      incideIR: false,
      incidePrevidencia: false,
      natRubr: '',
      ideTabRubr: 'PADRAO',
      codIncCP: '00',
      codIncIRRF: '00',
      codIncFGTS: '00',
      codIncPisPasep: '00',
      iniValid: new Date().toISOString().substring(0, 7),
      fimValid: ''
    });
    clearAllErrors();
    setActiveTab('geral');
    setIsModalOpen(true);
  };

  const handleEdit = (r: Rubrica) => {
    setEditingRubrica(r);
    const nat = naturezas.find(n => n.codigo === r.natRubr);
    setSelectedNaturezaLabel(nat ? `${nat.codigo} - ${nat.nome}` : r.natRubr || '');
    setFormData({
      codigo: r.codigo,
      descricao: r.descricao,
      tipo: r.tipo,
      rotina: r.rotina,
      incideIR: r.incideIR,
      incidePrevidencia: r.incidePrevidencia,
      natRubr: r.natRubr || '',
      ideTabRubr: r.ideTabRubr || 'PADRAO',
      codIncCP: r.codIncCP || '00',
      codIncIRRF: r.codIncIRRF || '00',
      codIncFGTS: r.codIncFGTS || '00',
      codIncPisPasep: r.codIncPisPasep || '00',
      iniValid: r.iniValid,
      fimValid: r.fimValid || ''
    });
    clearAllErrors();
    setActiveTab('geral');
    setIsModalOpen(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateAll(formData)) {
      if (!formData.codigo || !formData.descricao) {
        setActiveTab('geral');
      } else {
        setActiveTab('esocial');
      }
      return;
    }

    try {
      showLoading('Salvando rubrica...');
      let response;
      if (editingRubrica) {
        response = await api.put(`/rubricas/${editingRubrica.id}`, formData);
      } else {
        response = await api.post('/rubricas', formData);
      }

      const resData = response.data;
      if (resData.success) {
        alertSuccess('Sucesso', resData.message || 'Operação realizada com sucesso.');
        setIsModalOpen(false);
        fetchRubricas(pagination.currentPage);
      } else {
        alertError(resData.message || 'Erro ao salvar rubrica', resData.errors?.join('\n'));
      }
    } catch (error: any) {
      const apiError = error.response?.data;
      alertError(apiError?.message || 'Erro ao salvar rubrica', apiError?.errors?.join('\n'));
    } finally {
      closeLoading();
    }
  };

  const handleDelete = async (id: string) => {
    const confirmed = await alertDeleteConfirm('Deseja excluir esta rubrica?');
    if (confirmed) {
      try {
        const response = await api.delete(`/rubricas/${id}`);
        const resData = response.data;
        if (resData.success) {
          alertSuccess('Sucesso', resData.message || 'Rubrica excluída.');
          fetchRubricas(pagination.currentPage);
        } else {
          alertError(resData.message || 'Erro ao excluir rubrica');
        }
      } catch (error) {
        alertError('Erro ao excluir rubrica');
      }
    }
  };

  const handleToggleStatus = async (r: Rubrica) => {
    try {
      const response = await api.put(`/rubricas/${r.id}/toggle-status`);
      const resData = response.data;
      if (resData.success) {
        alertSuccess('Sucesso', resData.message);
        fetchRubricas(pagination.currentPage);
      } else {
        alertError(resData.message);
      }
    } catch (error) {
      alertError('Erro ao alterar status');
    }
  };

  const columns: TableColumn<Rubrica>[] = [
    { header: 'Código', accessor: 'codigo' },
    { header: 'Descrição', accessor: 'descricao' },
    { header: 'Tipo', accessor: 'tipoDescricao' },
    { header: 'Rotina', accessor: 'rotinaDescricao' },
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
          <p className="text-gray-500 mt-1 text-sm font-medium">Cadastre proventos, descontos e suas incidências (eSocial S-1010).</p>
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
        size="4xl"
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
        <div className="mb-6 border-b border-gray-200">
          <nav className="-mb-px flex space-x-8">
            <button
              onClick={() => setActiveTab('geral')}
              className={`${activeTab === 'geral'
                ? 'border-indigo-500 text-indigo-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                } whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm transition-colors`}
            >
              Geral
            </button>
            <button
              onClick={() => setActiveTab('esocial')}
              className={`${activeTab === 'esocial'
                ? 'border-indigo-500 text-indigo-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                } whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm transition-colors`}
            >
              eSocial (S-1010)
            </button>
          </nav>
        </div>

        <form id="rubrica-form" onSubmit={handleSubmit} className="space-y-4">
          {activeTab === 'geral' ? (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <FormInput
                label="Código"
                value={formData.codigo}
                error={getError('codigo')}
                onChange={(e) => {
                  setFormData({ ...formData, codigo: e.target.value });
                  validateField('codigo', e.target.value);
                }}
                placeholder="Ex: 001, 101..."
              />
              <FormInput
                label="Descrição"
                value={formData.descricao}
                error={getError('descricao')}
                onChange={(e) => {
                  setFormData({ ...formData, descricao: e.target.value });
                  validateField('descricao', e.target.value);
                }}
                placeholder="Ex: Salário Base, INSS..."
              />
              <EnumSelect
                label="Tipo de Rubrica"
                enumType="TipoRubrica"
                value={formData.tipo}
                onChange={(e) => setFormData({ ...formData, tipo: Number(e.target.value) })}
              />
              <EnumSelect
                label="Rotina de Cálculo"
                enumType="RotinaCalculo"
                value={formData.rotina}
                onChange={(e) => setFormData({ ...formData, rotina: Number(e.target.value) })}
              />
              <div className="flex gap-6 mt-4 col-span-2">
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
            </div>
          ) : (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="col-span-2 flex items-end gap-2 mb-4">
                <div className="flex-1">
                  <Autocomplete
                    label="Natureza da Rubrica (Tab. 03)"
                    placeholder="Busque por código ou nome..."
                    className="mb-0"
                    error={getError('natRubr')}
                    defaultValue={selectedNaturezaLabel}
                    onSearch={handleNaturezaSearch}
                    onSelect={(item) => {
                      if (item) {
                        setFormData({ ...formData, natRubr: item.codigo });
                        setSelectedNaturezaLabel(`${item.codigo} - ${item.nome}`);
                        validateField('natRubr', item.codigo);
                      } else {
                        setFormData({ ...formData, natRubr: '' });
                        setSelectedNaturezaLabel('');
                        validateField('natRubr', '');
                      }
                    }}
                    displayValue={(item) => `${item.codigo} - ${item.nome}`}
                    keyValue={(item) => item.codigo}
                  />
                </div>
                <button
                  type="button"
                  onClick={() => setIsNaturezaModalOpen(true)}
                  className="p-2 h-[38px] text-indigo-600 hover:bg-indigo-50 rounded-md border border-indigo-200 transition-colors flex items-center justify-center"
                  title="Ver Tabela 03 completa"
                >
                  <Search size={20} />
                </button>
              </div>

              <FormInput
                label="Identificador da Tabela"
                value={formData.ideTabRubr}
                onChange={(e) => setFormData({ ...formData, ideTabRubr: e.target.value })}
                placeholder="Ex: PADRAO"
              />

              <FormSelect
                label="Incidência Previdência (Tab. 21)"
                value={formData.codIncCP}
                onChange={(e) => setFormData({ ...formData, codIncCP: e.target.value })}
                options={[
                  { value: '00', label: '00 - Não é base de cálculo' },
                  { value: '11', label: '11 - Base de cálculo mensal' },
                  { value: '12', label: '12 - Base de cálculo 13º salário' },
                  { value: '21', label: '21 - Salário-maternidade mensal' },
                  { value: '31', label: '31 - Gratificações (Férias)' },
                  { value: '91', label: '91 - Incidência suspensa (Decisão Judicial)' }
                ]}
              />

              <FormSelect
                label="Incidência IRRF (Tab. 21)"
                value={formData.codIncIRRF}
                onChange={(e) => setFormData({ ...formData, codIncIRRF: e.target.value })}
                options={[
                  { value: '00', label: '00 - Não é base de cálculo' },
                  { value: '11', label: '11 - Remuneração mensal' },
                  { value: '12', label: '12 - 13º salário' },
                  { value: '13', label: '13 - Férias' },
                  { value: '14', label: '14 - PLR' },
                  { value: '15', label: '15 - Rendimentos Recebidos Acumuladamente' },
                  { value: '31', label: '31 - Retenções (Previdência)' },
                  { value: '91', label: '91 - Verba com incidência suspensa' }
                ]}
              />

              <FormSelect
                label="Incidência FGTS (Tab. 21)"
                value={formData.codIncFGTS}
                onChange={(e) => setFormData({ ...formData, codIncFGTS: e.target.value })}
                options={[
                  { value: '00', label: '00 - Não é base de cálculo' },
                  { value: '11', label: '11 - Base de cálculo mensal' },
                  { value: '12', label: '12 - Base de cálculo 13º salário' },
                  { value: '21', label: '21 - Base de cálculo rescisão' },
                  { value: '91', label: '91 - Incidência suspensa' }
                ]}
              />

              <FormSelect
                label="Incidência PIS/PASEP"
                value={formData.codIncPisPasep}
                onChange={(e) => setFormData({ ...formData, codIncPisPasep: e.target.value })}
                options={[
                  { value: '00', label: '00 - Não é base de cálculo' },
                  { value: '11', label: '11 - Base de cálculo mensal' },
                  { value: '91', label: '91 - Incidência suspensa' }
                ]}
              />

              <DatePicker
                label="Início Validade (MM/AAAA)"
                showMonthYearPicker
                dateFormat="MM/yyyy"
                placeholder="mm/aaaa"
                error={getError('iniValid')}
                selected={formData.iniValid ? new Date(Number(formData.iniValid.split('-')[0]), Number(formData.iniValid.split('-')[1]) - 1, 1) : null}
                onChange={(date) => {
                  if (date) {
                    const y = date.getFullYear();
                    const m = String(date.getMonth() + 1).padStart(2, '0');
                    const val = `${y}-${m}`;
                    setFormData({ ...formData, iniValid: val });
                    validateField('iniValid', val);
                  } else {
                    setFormData({ ...formData, iniValid: '' });
                    validateField('iniValid', '');
                  }
                }}
              />

              <DatePicker
                label="Fim Validade (MM/AAAA)"
                showMonthYearPicker
                dateFormat="MM/yyyy"
                placeholder="mm/aaaa"
                selected={formData.fimValid ? new Date(Number(formData.fimValid.split('-')[0]), Number(formData.fimValid.split('-')[1]) - 1, 1) : null}
                onChange={(date) => {
                  if (date) {
                    const y = date.getFullYear();
                    const m = String(date.getMonth() + 1).padStart(2, '0');
                    setFormData({ ...formData, fimValid: `${y}-${m}` });
                  } else {
                    setFormData({ ...formData, fimValid: '' });
                  }
                }}
              />
            </div>
          )}
        </form>
      </Modal>

      {/* Tabela de Naturezas Modal */}
      <Modal
        isOpen={isNaturezaModalOpen}
        onClose={() => setIsNaturezaModalOpen(false)}
        title="Tabela 03 - Natureza das Rubricas da Folha de Pagamento"
        size="4xl"
      >
        <div className="overflow-hidden">
          <Table<NaturezaRubrica>
            columns={[
              { header: 'Código', accessor: 'codigo' },
              { header: 'Nome', accessor: 'nome' },
              { header: 'Descrição', accessor: 'descricao' },
              {
                header: 'Ação',
                render: (n) => (
                  <button
                    onClick={() => {
                      setFormData({ ...formData, natRubr: n.codigo });
                      setSelectedNaturezaLabel(`${n.codigo} - ${n.nome}`);
                      setIsNaturezaModalOpen(false);
                    }}
                    className="text-indigo-600 hover:text-indigo-900 font-medium text-sm"
                  >
                    Selecionar
                  </button>
                )
              }
            ]}
            data={naturezas}
            isLoading={false}
            emptyMessage="Nenhuma natureza encontrada."
          />
        </div>
      </Modal>
    </div>
  );
}
