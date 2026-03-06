import { useState, useEffect } from 'react';
import { createPortal } from 'react-dom';
import { Plus, Trash2, Edit } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Pagination } from '../components/common/Pagination';

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
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCargo, setEditingCargo] = useState<Cargo | null>(null);
  const [formData, setFormData] = useState({ nome: '', cbo: '' });

  useEffect(() => {
    fetchCargos();
  }, []);

  const fetchCargos = async (page = 1) => {
    try {
      showLoading('Carregando...');
      const response = await api.get(`/cargos?page=${page}&pageSize=${pagination.pageSize}`);
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

      <div className="bg-white shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nome</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">CBO</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Ações</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {cargos.map((cargo) => (
              <tr key={cargo.id}>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{cargo.nome}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{cargo.cbo}</td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button onClick={() => handleEdit(cargo)} className="text-indigo-600 hover:text-indigo-900 mr-4">
                    <Edit size={18} />
                  </button>
                  <button onClick={() => handleDelete(cargo.id)} className="text-red-600 hover:text-red-900">
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
          onPageChange={(page) => fetchCargos(page)}
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
            <span className="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>
            <div className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full relative z-[1001]">
              <form onSubmit={handleSubmit}>
                <div className="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                  <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
                    {editingCargo ? 'Editar Cargo' : 'Novo Cargo'}
                  </h3>
                  <div className="space-y-4">
                    <FormInput
                      label="Nome"
                      required
                      value={formData.nome}
                      onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
                    />
                    <FormInput
                      label="CBO"
                      required
                      value={formData.cbo}
                      onChange={(e) => setFormData({ ...formData, cbo: e.target.value })}
                    />
                  </div>
                </div>
                <div className="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
                  <button
                    type="submit"
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
