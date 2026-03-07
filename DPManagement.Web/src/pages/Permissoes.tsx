import { useState, useEffect } from 'react';
import { Plus, Trash2, Edit, CheckCircle, XCircle } from 'lucide-react';
import api from '../services/api';
import { FormInput } from '../components/common/FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../services/alertService';
import { Table, type TableColumn } from '../components/common/Table';
import { FilterBar } from '../components/common/FilterBar';
import { Modal } from '../components/common/Modal';

interface Permissao {
    id: string;
    modulo: string;
    acao: string;
    descricao?: string;
    ativo: boolean;
}

export default function Permissoes() {
    const [permissoes, setPermissoes] = useState<Permissao[]>([]);
    const [filters, setFilters] = useState({
        modulo: '',
        acao: ''
    });
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingPermissao, setEditingPermissao] = useState<Permissao | null>(null);
    const [formData, setFormData] = useState({ modulo: '', acao: '', descricao: '' });

    useEffect(() => {
        fetchPermissoes();
    }, []);

    const fetchPermissoes = async () => {
        try {
            showLoading('Carregando...');
            const response = await api.get('/permissoes');
            let data = response.data;

            if (filters.modulo) {
                data = data.filter((p: Permissao) => p.modulo.toLowerCase().includes(filters.modulo.toLowerCase()));
            }
            if (filters.acao) {
                data = data.filter((p: Permissao) => p.acao.toLowerCase().includes(filters.acao.toLowerCase()));
            }

            setPermissoes(data);
        } catch (error) {
            console.error('Erro ao buscar permissões', error);
            alertError('Erro ao carregar permissões');
        } finally {
            closeLoading();
        }
    };

    const applyFilters = () => fetchPermissoes();
    const clearFilters = () => {
        setFilters({ modulo: '', acao: '' });
        // setTimeout to allow state update before fetching
        setTimeout(() => fetchPermissoes(), 0);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        showLoading('Salvando permissão...');
        try {
            if (editingPermissao) {
                await api.put(`/permissoes/${editingPermissao.id}`, formData);
                alertSuccess('Permissão atualizada');
            } else {
                await api.post('/permissoes', formData);
                alertSuccess('Permissão cadastrada');
            }
            setIsModalOpen(false);
            setEditingPermissao(null);
            setFormData({ modulo: '', acao: '', descricao: '' });
            fetchPermissoes();
        } catch (error) {
            console.error('Erro ao salvar permissão', error);
            alertError('Erro ao salvar permissão');
        } finally {
            closeLoading();
        }
    };

    const handleEdit = (permissao: Permissao) => {
        setEditingPermissao(permissao);
        setFormData({ modulo: permissao.modulo, acao: permissao.acao, descricao: permissao.descricao || '' });
        setIsModalOpen(true);
    };

    const handleToggleStatus = async (permissao: Permissao) => {
        showLoading(permissao.ativo ? 'Inativando...' : 'Ativando...');
        try {
            await api.patch(`/permissoes/${permissao.id}/ativar?ativo=${!permissao.ativo}`);
            alertSuccess(`Permissão ${permissao.ativo ? 'inativada' : 'ativada'} com sucesso`);
            fetchPermissoes();
        } catch (error) {
            console.error('Erro ao alterar status', error);
            alertError('Erro ao alterar status');
        } finally {
            closeLoading();
        }
    };

    const handleDelete = async (id: string) => {
        const confirmed = await alertDeleteConfirm('Excluir Permissão?', 'Esta ação removerá a permissão permanentemente.');
        if (confirmed) {
            showLoading('Excluindo...');
            try {
                await api.delete(`/permissoes/${id}`);
                alertSuccess('Permissão excluída');
                fetchPermissoes();
            } catch (error) {
                console.error('Erro ao excluir permissão', error);
                alertError('Erro ao excluir permissão');
            } finally {
                closeLoading();
            }
        }
    };

    const columns: TableColumn<Permissao>[] = [
        { header: 'Módulo', accessor: 'modulo' },
        { header: 'Ação', accessor: 'acao' },
        { header: 'Descrição', accessor: 'descricao' },
        {
            header: 'Status',
            accessor: 'ativo',
            render: (permissao) => (
                <span
                    className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${permissao.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                        }`}
                >
                    {permissao.ativo ? 'Ativo' : 'Inativo'}
                </span>
            ),
        },
        {
            header: 'Ações',
            align: 'right',
            render: (permissao) => (
                <div className="space-x-4 flex justify-end">
                    <button
                        title={permissao.ativo ? 'Inativar' : 'Ativar'}
                        onClick={() => handleToggleStatus(permissao)}
                        className={`${permissao.ativo ? 'text-red-500 hover:text-red-700' : 'text-green-500 hover:text-green-700'}`}
                    >
                        {permissao.ativo ? <XCircle size={18} /> : <CheckCircle size={18} />}
                    </button>
                    <button title="Editar" onClick={() => handleEdit(permissao)} className="text-indigo-600 hover:text-indigo-900">
                        <Edit size={18} />
                    </button>
                    <button title="Excluir" onClick={() => handleDelete(permissao.id)} className="text-red-600 hover:text-red-900">
                        <Trash2 size={18} />
                    </button>
                </div>
            ),
        },
    ];

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-2xl font-semibold text-gray-900">Gestão de Permissões</h1>
                <button
                    onClick={() => { setEditingPermissao(null); setFormData({ modulo: '', acao: '', descricao: '' }); setIsModalOpen(true); }}
                    className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors"
                >
                    <Plus size={18} className="mr-2" />
                    Nova Permissão
                </button>
            </div>

            <FilterBar onFilter={applyFilters} onClear={clearFilters}>
                <FormInput
                    label="Módulo"
                    value={filters.modulo}
                    onChange={e => setFilters({ ...filters, modulo: e.target.value })}
                    placeholder="Ex: Folha"
                />
                <FormInput
                    label="Ação"
                    value={filters.acao}
                    onChange={e => setFilters({ ...filters, acao: e.target.value })}
                    placeholder="Ex: Editar"
                />
            </FilterBar>

            <Table
                data={permissoes}
                columns={columns}
            />

            <Modal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                title={editingPermissao ? 'Editar Permissão' : 'Nova Permissão'}
                size="md"
                footer={(
                    <>
                        <button
                            type="submit"
                            form="permissao-form"
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
                <form id="permissao-form" onSubmit={handleSubmit} className="space-y-4">
                    <FormInput
                        label="Módulo"
                        required
                        value={formData.modulo}
                        onChange={(e) => setFormData({ ...formData, modulo: e.target.value })}
                        placeholder="Ex: Funcionarios"
                    />
                    <FormInput
                        label="Ação"
                        required
                        value={formData.acao}
                        onChange={(e) => setFormData({ ...formData, acao: e.target.value })}
                        placeholder="Ex: Criar"
                    />
                    <FormInput
                        label="Descrição"
                        value={formData.descricao}
                        onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
                        placeholder="Descreva o propósito da permissão"
                    />
                </form>
            </Modal>
        </div>
    );
}
