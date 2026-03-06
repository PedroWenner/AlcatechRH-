import { useState, useEffect } from 'react';
import { createPortal } from 'react-dom';
import { Plus, Edit, Save, X, Trash2 } from 'lucide-react';
import api from '../services/api';
import { maskCPF, maskPhone, maskCell, maskCEP } from '../utils/masks';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Pagination } from '../components/common/Pagination';
import { FilterBar } from '../components/common/FilterBar';

interface Cargo {
  id: string;
  nome: string;
}

interface Colaborador {
  id: string;
  nome: string;
  cpf: string;
  rg?: string;
  pis?: string;
  dataNascimento: string;
  telefone?: string;
  celular?: string;
  cep: string;
  logradouro: string;
  numero: string;
  complemento?: string;
  bairro: string;
  cidade: string;
  estado: string;
  cargoId: string;
  cargoNome: string;
}

export default function Employees() {
  const [employees, setEmployees] = useState<Colaborador[]>([]);
  const [cargos, setCargos] = useState<Cargo[]>([]);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    totalPages: 1,
    totalCount: 0,
    pageSize: 10
  });
  const [filters, setFilters] = useState({
    nome: '',
    cpf: '',
    cargoId: ''
  });
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingEmployee, setEditingEmployee] = useState<Colaborador | null>(null);
  const [formData, setFormData] = useState({
    nome: '', cpf: '', rg: '', pis: '', dataNascimento: '',
    telefone: '', celular: '', cep: '', logradouro: '',
    numero: '', complemento: '', bairro: '', cidade: '',
    estado: '', cargoId: ''
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    fetchEmployees();
    fetchCargos();
  }, []);

  const fetchEmployees = async (page = 1, f = filters) => {
    try {
      showLoading('Carregando...');
      const params: any = {
        page,
        pageSize: pagination.pageSize
      };
      if (f.nome) params.nome = f.nome;
      if (f.cpf) params.cpf = f.cpf;
      if (f.cargoId) params.cargoId = f.cargoId;

      console.log(params);
      const response = await api.get('/colaboradores', { params });
      setEmployees(response.data.items);
      setPagination(prev => ({
        ...prev,
        currentPage: response.data.page,
        totalPages: response.data.totalPages,
        totalCount: response.data.totalCount
      }));
    } catch (error) {
      console.error('Erro ao buscar colaboradores', error);
      alertError('Erro ao carregar colaboradores');
    } finally {
      closeLoading();
    }
  };

  const applyFilters = () => fetchEmployees(1);
  const clearFilters = () => {
    const newFilters = { nome: '', cpf: '', cargoId: '' };
    setFilters(newFilters);
    fetchEmployees(1, newFilters);
  };


  const fetchCargos = async () => {
    try {
      const response = await api.get('/cargos/all');
      setCargos(response.data);
    } catch (error) {
      console.error('Erro ao buscar cargos', error);
      alertError('Erro ao carregar cargos');
    }
  };

  const validateCPF = (cpf: string) => {
    const cleanCpf = cpf.replace(/\D/g, '');
    if (cleanCpf.length !== 11) return 'CPF deve ter 11 dígitos';
    // Basic check for repeated numbers (real validation is on backend but let's provide feedback)
    if (/^(\d)\1+$/.test(cleanCpf)) return 'CPF inválido';
    return '';
  };

  const handleBlur = (field: string, value: string) => {
    let error = '';
    if (field === 'cpf') error = validateCPF(value);
    if (!value && ['nome', 'dataNascimento', 'cep', 'logradouro', 'numero', 'bairro', 'cidade', 'estado', 'cargoId'].includes(field)) {
      error = 'Campo obrigatório';
    }

    setErrors(prev => ({ ...prev, [field]: error }));
  };

  const handleCepSearch = async (cep: string) => {
    const cleanCep = cep.replace(/\D/g, '');
    if (cleanCep.length === 8) {
      try {
        showLoading('Buscando CEP...');
        const response = await api.get(`/colaboradores/cep/${cleanCep}`);
        if (response.data) {
          setFormData(prev => ({
            ...prev,
            logradouro: response.data.logradouro,
            bairro: response.data.bairro,
            cidade: response.data.localidade,
            estado: response.data.uf
          }));
          setErrors(prev => ({ ...prev, cep: '', logradouro: '', bairro: '', cidade: '', estado: '' }));
        }
      } catch (error) {
        console.error('CEP não encontrado ou erro na busca');
        setErrors(prev => ({ ...prev, cep: 'CEP não encontrado' }));
      } finally {
        closeLoading();
      }
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // Final check
    const newErrors: Record<string, string> = {};
    if (!formData.nome) newErrors.nome = 'Campo obrigatório';
    const cpfErr = validateCPF(formData.cpf);
    if (cpfErr) newErrors.cpf = cpfErr;

    if (Object.values(newErrors).some(x => x)) {
      setErrors(newErrors);
      alertError('Campos inválidos', 'Por favor, corrija os erros no formulário.');
      return;
    }

    showLoading('Salvando...');
    try {
      if (editingEmployee) {
        await api.put(`/colaboradores/${editingEmployee.id}`, { ...formData, id: editingEmployee.id });
        alertSuccess('Colaborador atualizado');
      } else {
        await api.post('/colaboradores', formData);
        alertSuccess('Colaborador cadastrado');
      }
      setIsModalOpen(false);
      fetchEmployees();
    } catch (error: any) {
      console.error('Erro ao salvar colaborador', error);
      const msg = error.response?.data?.message || 'Verifique os dados (ex: CPF válido).';
      alertError('Erro ao salvar', msg);
    } finally {
      closeLoading();
    }
  };

  const handleDelete = async (id: string) => {
    const confirmed = await alertDeleteConfirm('Excluir Colaborador?', 'Esta ação removerá o colaborador permanentemente.');
    if (confirmed) {
      showLoading('Excluindo...');
      try {
        await api.delete(`/colaboradores/${id}`);
        alertSuccess('Colaborador excluído');
        fetchEmployees();
      } catch (error) {
        console.error('Erro ao excluir colaborador', error);
        alertError('Erro ao excluir colaborador');
      } finally {
        closeLoading();
      }
    }
  };

  const handleEdit = (emp: Colaborador) => {
    setEditingEmployee(emp);
    setFormData({
      nome: emp.nome,
      cpf: emp.cpf,
      rg: emp.rg || '',
      pis: emp.pis || '',
      dataNascimento: emp.dataNascimento.split('T')[0],
      telefone: emp.telefone || '',
      celular: emp.celular || '',
      cep: emp.cep,
      logradouro: emp.logradouro,
      numero: emp.numero,
      complemento: emp.complemento || '',
      bairro: emp.bairro,
      cidade: emp.cidade,
      estado: emp.estado,
      cargoId: emp.cargoId
    });
    setErrors({});
    setIsModalOpen(true);
  };

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold text-gray-900">Gestão de Colaboradores</h1>
        <button
          onClick={() => {
            setEditingEmployee(null);
            setFormData({
              nome: '', cpf: '', rg: '', pis: '', dataNascimento: '',
              telefone: '', celular: '', cep: '', logradouro: '',
              numero: '', complemento: '', bairro: '', cidade: '',
              estado: '', cargoId: ''
            });
            setErrors({});
            setIsModalOpen(true);
          }}
          className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors"
        >
          <Plus size={18} className="mr-2" />
          Novo Colaborador
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
          label="CPF"
          mask={maskCPF}
          value={filters.cpf}
          onChange={e => setFilters({ ...filters, cpf: e.target.value.replace(/\D/g, '') })}
          placeholder="000.000.000-00"
        />
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Cargo</label>
          <select
            value={filters.cargoId}
            onChange={e => setFilters({ ...filters, cargoId: e.target.value })}
            className="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:ring-2 focus:ring-indigo-100 focus:border-indigo-500 outline-none"
          >
            <option value="">Todos os Cargos</option>
            {cargos.map(c => <option key={c.id} value={c.id}>{c.nome}</option>)}
          </select>
        </div>
      </FilterBar>

      <div className="bg-white shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nome / CPF</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Cargo</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Contato</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Ações</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {employees.map((emp) => (
              <tr key={emp.id}>
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="text-sm font-medium text-gray-900">{emp.nome}</div>
                  <div className="text-sm text-gray-500">{maskCPF(emp.cpf)}</div>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{emp.cargoNome}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {emp.celular && <div className="text-xs">Cell: {maskCell(emp.celular)}</div>}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button onClick={() => handleEdit(emp)} className="text-indigo-600 hover:text-indigo-900 mr-4">
                    <Edit size={18} />
                  </button>
                  <button onClick={() => handleDelete(emp.id)} className="text-red-600 hover:text-red-900">
                    <Trash2 size={18} />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <Pagination
          currentPage={pagination.currentPage}
          totalPages={pagination.totalPages}
          onPageChange={(page) => fetchEmployees(page)}
          totalCount={pagination.totalCount}
          pageSize={pagination.pageSize}
        />
      </div>

      {isModalOpen && typeof document !== 'undefined' && createPortal(
        <div className="fixed z-[1000] inset-0 overflow-y-auto">
          <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
            <div className="fixed inset-0 transition-opacity" aria-hidden="true" onClick={() => setIsModalOpen(false)}>
              <div className="absolute inset-0 bg-gray-500 opacity-75"></div>
            </div>
            <div className="inline-block align-bottom bg-white rounded-lg text-left shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-4xl sm:w-full relative z-[1001]">
              <form onSubmit={handleSubmit} className="p-6">
                <div className="flex justify-between items-center mb-6">
                  <h3 className="text-lg font-medium text-gray-900">{editingEmployee ? 'Editar Colaborador' : 'Novo Colaborador'}</h3>
                  <button type="button" onClick={() => setIsModalOpen(false)} className="text-gray-400 hover:text-gray-500">
                    <X size={24} />
                  </button>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-4">
                  <div className="md:col-span-2">
                    <FormInput
                      label="Nome Completo"
                      required
                      value={formData.nome}
                      error={errors.nome}
                      onChange={e => setFormData({ ...formData, nome: e.target.value })}
                      onBlur={e => handleBlur('nome', e.target.value)}
                    />
                  </div>
                  <div>
                    <FormInput
                      label="CPF"
                      required
                      mask={maskCPF}
                      value={formData.cpf}
                      error={errors.cpf}
                      onChange={e => setFormData({ ...formData, cpf: e.target.value.replace(/\D/g, '') })}
                      onBlur={e => handleBlur('cpf', e.target.value)}
                      placeholder="000.000.000-00"
                    />
                  </div>

                  <FormInput
                    label="RG"
                    value={formData.rg}
                    onChange={e => setFormData({ ...formData, rg: e.target.value })}
                  />
                  <FormInput
                    label="PIS"
                    value={formData.pis}
                    onChange={e => setFormData({ ...formData, pis: e.target.value })}
                  />
                  <FormInput
                    label="Data de Nascimento"
                    required
                    type="date"
                    value={formData.dataNascimento}
                    error={errors.dataNascimento}
                    onChange={e => setFormData({ ...formData, dataNascimento: e.target.value })}
                    onBlur={e => handleBlur('dataNascimento', e.target.value)}
                  />

                  <FormInput
                    label="Telefone Fixo"
                    mask={maskPhone}
                    value={formData.telefone}
                    onChange={e => setFormData({ ...formData, telefone: e.target.value.replace(/\D/g, '') })}
                    placeholder="(00) 0000-0000"
                  />
                  <FormInput
                    label="Celular"
                    mask={maskCell}
                    value={formData.celular}
                    onChange={e => setFormData({ ...formData, celular: e.target.value.replace(/\D/g, '') })}
                    placeholder="(00) 0 0000-0000"
                  />

                  <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Cargo</label>
                    <select
                      required
                      value={formData.cargoId}
                      onChange={e => setFormData({ ...formData, cargoId: e.target.value })}
                      onBlur={e => handleBlur('cargoId', e.target.value)}
                      className={`w-full rounded-md border px-3 py-2 transition-all outline-none focus:ring-2 
                        ${errors.cargoId ? "border-red-500 bg-red-50 focus:ring-red-200" : "border-gray-300 focus:ring-indigo-100 focus:border-indigo-500"}
                      `}
                    >
                      <option value="">Selecione...</option>
                      {cargos.map(c => <option key={c.id} value={c.id}>{c.nome}</option>)}
                    </select>
                    {errors.cargoId && <p className="text-sm text-red-600 mt-1">{errors.cargoId}</p>}
                  </div>

                  <div className="md:col-span-3 border-t pt-4 mt-2">
                    <h4 className="text-sm font-bold text-gray-600 mb-2 uppercase tracking-wider">Endereço</h4>
                  </div>

                  <FormInput
                    label="CEP"
                    required
                    mask={maskCEP}
                    value={formData.cep}
                    error={errors.cep}
                    onChange={e => {
                      const val = e.target.value.replace(/\D/g, '');
                      setFormData({ ...formData, cep: val });
                      if (val.length === 8) handleCepSearch(val);
                    }}
                    onBlur={e => handleBlur('cep', e.target.value)}
                    placeholder="00000-000"
                  />
                  <div className="md:col-span-2">
                    <FormInput
                      label="Logradouro"
                      required
                      value={formData.logradouro}
                      error={errors.logradouro}
                      onChange={e => setFormData({ ...formData, logradouro: e.target.value })}
                      onBlur={e => handleBlur('logradouro', e.target.value)}
                    />
                  </div>
                  <FormInput
                    label="Número"
                    required
                    value={formData.numero}
                    error={errors.numero}
                    onChange={e => setFormData({ ...formData, numero: e.target.value })}
                    onBlur={e => handleBlur('numero', e.target.value)}
                  />
                  <FormInput
                    label="Bairro"
                    required
                    value={formData.bairro}
                    error={errors.bairro}
                    onChange={e => setFormData({ ...formData, bairro: e.target.value })}
                    onBlur={e => handleBlur('bairro', e.target.value)}
                  />
                  <FormInput
                    label="Cidade"
                    required
                    value={formData.cidade}
                    error={errors.cidade}
                    onChange={e => setFormData({ ...formData, cidade: e.target.value })}
                    onBlur={e => handleBlur('cidade', e.target.value)}
                  />
                  <FormInput
                    label="Estado (UF)"
                    required
                    maxLength={2}
                    value={formData.estado}
                    error={errors.estado}
                    onChange={e => setFormData({ ...formData, estado: e.target.value.toUpperCase() })}
                    onBlur={e => handleBlur('estado', e.target.value)}
                  />
                  <div className="md:col-span-3">
                    <FormInput
                      label="Complemento"
                      value={formData.complemento}
                      onChange={e => setFormData({ ...formData, complemento: e.target.value })}
                    />
                  </div>
                </div>

                <div className="mt-8 flex justify-end space-x-3">
                  <button type="button" onClick={() => setIsModalOpen(false)} className="px-6 py-2 border rounded-md text-gray-700 hover:bg-gray-50 flex items-center transition-colors">
                    Cancelar
                  </button>
                  <button type="submit" className="px-8 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 flex items-center shadow-md transition-all active:scale-95">
                    <Save size={18} className="mr-2" /> Salvar Colaborador
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>,
        document.body
      )}
    </div>
  );
}
