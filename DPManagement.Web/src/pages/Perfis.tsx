import { useState, useEffect } from 'react';
import { Plus, Trash2, Edit, CheckCircle, XCircle } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Table, type TableColumn } from '../components/common/Table';
import { FilterBar } from '../components/common/FilterBar';
import { Modal } from '../components/common/Modal';

interface Perfil {
    id: string;
    nome: string;
    descricao?: string;
    ativo: boolean;
    permissoesIds?: string[];
}

interface PermissaoMatriz {
    modulo: string;
    permissoes: {
        id: string;
        acao: string;
        descricao: string;
        ativo: boolean;
    }[];
}

export default function Perfis() {
    const [perfis, setPerfis] = useState<Perfil[]>([]);
    const [filters, setFilters] = useState({
        nome: ''
    });
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingPerfil, setEditingPerfil] = useState<Perfil | null>(null);
    const [formData, setFormData] = useState({ nome: '', descricao: '' });

    // Access Matrix State
    const [isMatrixOpen, setIsMatrixOpen] = useState(false);
    const [selectedPerfil, setSelectedPerfil] = useState<Perfil | null>(null);
    const [permissoesMatriz, setPermissoesMatriz] = useState<PermissaoMatriz[]>([]);
    const [selectedPermissoesIds, setSelectedPermissoesIds] = useState<Set<string>>(new Set());

    useEffect(() => {
        fetchPerfis();
        fetchMatriz();
    }, []);

    const fetchMatriz = async () => {
        try {
            const response = await api.get('/permissoes/matriz');
            setPermissoesMatriz(response.data);
        } catch (error) {
            console.error('Erro ao buscar matriz de permissões', error);
        }
    };

    const fetchPerfis = async () => {
        try {
            showLoading('Carregando...');
            const response = await api.get('/perfis');
            // Simulated filtering since API currently lists all
            let data = response.data;
            if (filters.nome) {
                data = data.filter((p: Perfil) => p.nome.toLowerCase().includes(filters.nome.toLowerCase()));
            }
            setPerfis(data);
        } catch (error) {
            console.error('Erro ao buscar perfis', error);
            alertError('Erro ao carregar perfis');
        } finally {
            closeLoading();
        }
    };

    const applyFilters = () => fetchPerfis();
    const clearFilters = () => {
        setFilters({ nome: '' });
        // setTimeout to allow state update before fetching
        setTimeout(() => fetchPerfis(), 0);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        showLoading('Salvando perfil...');
        try {
            if (editingPerfil) {
                await api.put(`/perfis/${editingPerfil.id}`, formData);
                alertSuccess('Perfil atualizado');
            } else {
                await api.post('/perfis', formData);
                alertSuccess('Perfil cadastrado');
            }
            setIsModalOpen(false);
            setEditingPerfil(null);
            setFormData({ nome: '', descricao: '' });
            fetchPerfis();
        } catch (error) {
            console.error('Erro ao salvar perfil', error);
            alertError('Erro ao salvar perfil');
        } finally {
            closeLoading();
        }
    };

    const handleEdit = (perfil: Perfil) => {
        setEditingPerfil(perfil);
        setFormData({ nome: perfil.nome, descricao: perfil.descricao || '' });
        setIsModalOpen(true);
    };

    const handleToggleStatus = async (perfil: Perfil) => {
        showLoading(perfil.ativo ? 'Inativando...' : 'Ativando...');
        try {
            await api.patch(`/perfis/${perfil.id}/ativar?ativo=${!perfil.ativo}`);
            alertSuccess(`Perfil ${perfil.ativo ? 'inativado' : 'ativado'} com sucesso`);
            fetchPerfis();
        } catch (error) {
            console.error('Erro ao alterar status', error);
            alertError('Erro ao alterar status');
        } finally {
            closeLoading();
        }
    };

    const handleDelete = async (id: string) => {
        const confirmed = await alertDeleteConfirm('Excluir Perfil?', 'Esta ação removerá o perfil permanentemente.');
        if (confirmed) {
            showLoading('Excluindo...');
            try {
                await api.delete(`/perfis/${id}`);
                alertSuccess('Perfil excluído');
                fetchPerfis();
            } catch (error) {
                console.error('Erro ao excluir perfil', error);
                alertError('Erro ao excluir perfil');
            } finally {
                closeLoading();
            }
        }
    };

    const handleManageAccess = (perfil: Perfil) => {
        setSelectedPerfil(perfil);
        // Pre-select the existing permissions
        setSelectedPermissoesIds(new Set(perfil.permissoesIds || []));
        setIsMatrixOpen(true);
    };

    const handleTogglePermission = (permissaoId: string) => {
        const newSelected = new Set(selectedPermissoesIds);
        if (newSelected.has(permissaoId)) {
            newSelected.delete(permissaoId);
        } else {
            newSelected.add(permissaoId);
        }
        setSelectedPermissoesIds(newSelected);
    };

    const handleSaveMatrix = async () => {
        if (!selectedPerfil) return;
        showLoading('Salvando permissões...');
        try {
            await api.post(`/perfis/${selectedPerfil.id}/permissoes`, Array.from(selectedPermissoesIds));
            alertSuccess('Permissões atualizadas com sucesso');
            setIsMatrixOpen(false);
            fetchPerfis(); // Reload to get updated permissoesIds
        } catch (error) {
            console.error('Erro ao salvar permissões', error);
            alertError('Erro ao atualizar permissões');
        } finally {
            closeLoading();
        }
    };

    const columns: TableColumn<Perfil>[] = [
        { header: 'Nome', accessor: 'nome' },
        { header: 'Descrição', accessor: 'descricao' },
        {
            header: 'Status',
            accessor: 'ativo',
            render: (perfil) => (
                <span
                    className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${perfil.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                        }`}
                >
                    {perfil.ativo ? 'Ativo' : 'Inativo'}
                </span>
            ),
        },
        {
            header: 'Ações',
            align: 'right',
            render: (perfil) => (
                <div className="space-x-4 flex justify-end">
                    <button
                        title={perfil.ativo ? 'Inativar' : 'Ativar'}
                        onClick={() => handleToggleStatus(perfil)}
                        className={`${perfil.ativo ? 'text-red-500 hover:text-red-700' : 'text-green-500 hover:text-green-700'}`}
                    >
                        {perfil.ativo ? <XCircle size={18} /> : <CheckCircle size={18} />}
                    </button>
                    <button
                        title="Gerenciar Acessos"
                        onClick={() => handleManageAccess(perfil)}
                        className="text-amber-600 hover:text-amber-900"
                    >
                        <CheckCircle size={18} />
                    </button>
                    <button title="Editar" onClick={() => handleEdit(perfil)} className="text-indigo-600 hover:text-indigo-900">
                        <Edit size={18} />
                    </button>
                    <button title="Excluir" onClick={() => handleDelete(perfil.id)} className="text-red-600 hover:text-red-900">
                        <Trash2 size={18} />
                    </button>
                </div>
            ),
        },
    ];

    // Helper function to order actions visually
    const getOrderedActions = (permissoes: any[]) => {
        const order = ['Visualizar', 'Criar', 'Editar', 'Excluir'];
        return [...permissoes].sort((a, b) => {
            const indexA = order.indexOf(a.acao);
            const indexB = order.indexOf(b.acao);
            return (indexA === -1 ? 99 : indexA) - (indexB === -1 ? 99 : indexB);
        });
    };

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-2xl font-semibold text-gray-900">Gestão de Perfis</h1>
                <button
                    onClick={() => { setEditingPerfil(null); setFormData({ nome: '', descricao: '' }); setIsModalOpen(true); }}
                    className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors"
                >
                    <Plus size={18} className="mr-2" />
                    Novo Perfil
                </button>
            </div>

            <FilterBar onFilter={applyFilters} onClear={clearFilters}>
                <FormInput
                    label="Nome do Perfil"
                    value={filters.nome}
                    onChange={e => setFilters({ ...filters, nome: e.target.value })}
                    placeholder="Buscar por nome..."
                />
            </FilterBar>

            <Table
                data={perfis}
                columns={columns}
            />

            <Modal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                title={editingPerfil ? 'Editar Perfil' : 'Novo Perfil'}
                size="md"
                footer={(
                    <>
                        <button
                            type="submit"
                            form="perfil-form"
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
                <form id="perfil-form" onSubmit={handleSubmit} className="space-y-4">
                    <FormInput
                        label="Nome"
                        required
                        value={formData.nome}
                        onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
                        placeholder="Ex: Administrador"
                    />
                    <FormInput
                        label="Descrição"
                        value={formData.descricao}
                        onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
                        placeholder="Breve descrição do perfil"
                    />
                </form>
            </Modal>

            <Modal
                isOpen={isMatrixOpen}
                onClose={() => setIsMatrixOpen(false)}
                title={`Matriz de Acessos: ${selectedPerfil?.nome}`}
                size="lg"
                footer={(
                    <>
                        <button
                            type="button"
                            onClick={handleSaveMatrix}
                            className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm"
                        >
                            Salvar Acessos
                        </button>
                        <button
                            type="button"
                            onClick={() => setIsMatrixOpen(false)}
                            className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
                        >
                            Cancelar
                        </button>
                    </>
                )}
            >
                <div className="space-y-6 overflow-x-auto">
                    <table className="min-w-full divide-y divide-gray-200">
                        <thead className="bg-gray-50">
                            <tr>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                    Módulo
                                </th>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                    Permissões CRUD
                                </th>
                            </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                            {permissoesMatriz.map((item) => (
                                <tr key={item.modulo}>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                                        {item.modulo}
                                    </td>
                                    <td className="px-6 py-4 text-sm text-gray-500">
                                        <div className="flex flex-wrap gap-4">
                                            {getOrderedActions(item.permissoes).map(p => (
                                                <label key={p.id} className="flex items-center space-x-2 cursor-pointer">
                                                    <input
                                                        type="checkbox"
                                                        checked={selectedPermissoesIds.has(p.id)}
                                                        onChange={() => handleTogglePermission(p.id)}
                                                        className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                                                    />
                                                    <span>{p.acao}</span>
                                                </label>
                                            ))}
                                        </div>
                                    </td>
                                </tr>
                            ))}
                            {permissoesMatriz.length === 0 && (
                                <tr>
                                    <td colSpan={2} className="px-6 py-4 text-center text-sm text-gray-500">
                                        Nenhuma permissão cadastrada no sistema.
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
            </Modal>
        </div>
    );
}
