import React, { useState, useEffect } from 'react';
import { Plus, Trash2, Edit, CheckCircle, XCircle, Shield } from 'lucide-react';
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
    moduloPai: string | null;
    modulosFilhos: {
        modulo: string;
        permissoes: {
            id: string;
            acao: string;
            descricao: string;
            ativo: boolean;
        }[];
    }[];
}

interface Permissao {
    id: string;
    modulo: string;
    moduloPai?: string;
    acao: string;
    descricao?: string;
    ativo: boolean;
}

export default function Perfis() {
    const [activeTab, setActiveTab] = useState<'perfis' | 'permissoes'>('perfis');

    // === PERFIL STATE ===
    const [perfis, setPerfis] = useState<Perfil[]>([]);
    const [filters, setFilters] = useState({ nome: '' });
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingPerfil, setEditingPerfil] = useState<Perfil | null>(null);
    const [formData, setFormData] = useState({ nome: '', descricao: '' });

    // === MATRIX STATE ===
    const [isMatrixOpen, setIsMatrixOpen] = useState(false);
    const [selectedPerfil, setSelectedPerfil] = useState<Perfil | null>(null);
    const [permissoesMatriz, setPermissoesMatriz] = useState<PermissaoMatriz[]>([]);
    const [selectedPermissoesIds, setSelectedPermissoesIds] = useState<Set<string>>(new Set());

    // === PERMISSOES STATE ===
    const [permissoesList, setPermissoesList] = useState<Permissao[]>([]);
    const [permFilters, setPermFilters] = useState({ modulo: '', moduloPai: '', acao: '' });
    const [isPermModalOpen, setIsPermModalOpen] = useState(false);
    const [editingPermissao, setEditingPermissao] = useState<Permissao | null>(null);
    const [permFormData, setPermFormData] = useState({ modulo: '', moduloPai: '', acao: '', descricao: '' });

    useEffect(() => {
        fetchPerfis();
        fetchMatriz();
        fetchPermissoes();
    }, []);

    // --- FETCHES ---
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
            showLoading('Carregando perfis...');
            const response = await api.get('/perfis');
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

    const fetchPermissoes = async () => {
        try {
            showLoading('Carregando permissões...');
            const response = await api.get('/permissoes');
            let data = response.data;
            if (permFilters.modulo) {
                data = data.filter((p: Permissao) => p.modulo.toLowerCase().includes(permFilters.modulo.toLowerCase()));
            }
            if (permFilters.moduloPai) {
                data = data.filter((p: Permissao) => (p.moduloPai || '').toLowerCase().includes(permFilters.moduloPai.toLowerCase()));
            }
            if (permFilters.acao) {
                data = data.filter((p: Permissao) => p.acao.toLowerCase().includes(permFilters.acao.toLowerCase()));
            }
            setPermissoesList(data);
        } catch (error) {
            console.error('Erro ao buscar permissoes', error);
            alertError('Erro ao carregar permissões');
        } finally {
            closeLoading();
        }
    };

    // --- FILTERS ---
    const applyFilters = () => activeTab === 'perfis' ? fetchPerfis() : fetchPermissoes();
    const clearFilters = () => {
        if (activeTab === 'perfis') {
            setFilters({ nome: '' });
            setTimeout(() => fetchPerfis(), 0);
        } else {
            setPermFilters({ modulo: '', moduloPai: '', acao: '' });
            setTimeout(() => fetchPermissoes(), 0);
        }
    };

    // --- PERFIL HANDLERS ---
    const handleSubmitPerfil = async (e: React.FormEvent) => {
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

    const handleEditPerfil = (perfil: Perfil) => {
        setEditingPerfil(perfil);
        setFormData({ nome: perfil.nome, descricao: perfil.descricao || '' });
        setIsModalOpen(true);
    };

    const handleToggleStatusPerfil = async (perfil: Perfil) => {
        showLoading(perfil.ativo ? 'Inativando...' : 'Ativando...');
        try {
            await api.put(`/perfis/${perfil.id}/ativar?ativo=${!perfil.ativo}`);
            alertSuccess(`Perfil ${perfil.ativo ? 'inativado' : 'ativado'} com sucesso`);
            fetchPerfis();
        } catch (error) {
            console.error('Erro ao alterar status', error);
            alertError('Erro ao alterar status');
        } finally {
            closeLoading();
        }
    };

    const handleDeletePerfil = async (id: string) => {
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

    // --- MATRIX HANDLERS ---
    const handleManageAccess = (perfil: Perfil) => {
        setSelectedPerfil(perfil);
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
            fetchPerfis();
        } catch (error) {
            console.error('Erro ao salvar permissões', error);
            alertError('Erro ao atualizar permissões');
        } finally {
            closeLoading();
        }
    };

    // --- PERMISSAO HANDLERS ---
    const handleSubmitPermissao = async (e: React.FormEvent) => {
        e.preventDefault();
        showLoading('Salvando permissão...');
        try {
            if (editingPermissao) {
                await api.put(`/permissoes/${editingPermissao.id}`, permFormData);
                alertSuccess('Permissão atualizada');
            } else {
                await api.post('/permissoes', permFormData);
                alertSuccess('Permissão cadastrada');
            }
            setIsPermModalOpen(false);
            setEditingPermissao(null);
            setPermFormData({ modulo: '', moduloPai: '', acao: '', descricao: '' });
            fetchPermissoes();
            fetchMatriz(); // Refresh matrix dynamically after a generic permissão add/edit
        } catch (error) {
            console.error('Erro ao salvar permissao', error);
            alertError('Erro ao salvar permissão');
        } finally {
            closeLoading();
        }
    };

    const handleEditPermissao = (permissao: Permissao) => {
        setEditingPermissao(permissao);
        setPermFormData({ modulo: permissao.modulo, moduloPai: permissao.moduloPai || '', acao: permissao.acao, descricao: permissao.descricao || '' });
        setIsPermModalOpen(true);
    };

    const handleToggleStatusPermissao = async (permissao: Permissao) => {
        showLoading(permissao.ativo ? 'Inativando...' : 'Ativando...');
        try {
            await api.put(`/permissoes/${permissao.id}/ativar?ativo=${!permissao.ativo}`);
            alertSuccess(`Permissão ${permissao.ativo ? 'inativada' : 'ativada'} com sucesso`);
            fetchPermissoes();
            fetchMatriz();
        } catch (error) {
            console.error('Erro ao alterar status da permissão', error);
            alertError('Erro ao alterar status');
        } finally {
            closeLoading();
        }
    };

    const handleDeletePermissao = async (id: string) => {
        const confirmed = await alertDeleteConfirm('Excluir Permissão?', 'Esta ação removerá permanentemente o registro.');
        if (confirmed) {
            showLoading('Excluindo...');
            try {
                await api.delete(`/permissoes/${id}`);
                alertSuccess('Permissão excluída');
                fetchPermissoes();
                fetchMatriz();
            } catch (error) {
                console.error('Erro ao excluir permissão', error);
                alertError('Erro ao excluir permissão');
            } finally {
                closeLoading();
            }
        }
    };

    // --- COLUMNS ---
    const columnsPerfis: TableColumn<Perfil>[] = [
        { header: 'Nome', accessor: 'nome' },
        { header: 'Descrição', accessor: 'descricao' },
        {
            header: 'Status',
            accessor: 'ativo',
            render: (perfil) => (
                <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${perfil.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
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
                        onClick={() => handleToggleStatusPerfil(perfil)}
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
                    <button title="Editar" onClick={() => handleEditPerfil(perfil)} className="text-indigo-600 hover:text-indigo-900">
                        <Edit size={18} />
                    </button>
                    <button title="Excluir" onClick={() => handleDeletePerfil(perfil.id)} className="text-red-600 hover:text-red-900">
                        <Trash2 size={18} />
                    </button>
                </div>
            ),
        },
    ];

    const columnsPermissoes: TableColumn<Permissao>[] = [
        { header: 'Módulo Pai', render: (p) => p.moduloPai || '-' },
        { header: 'Módulo', accessor: 'modulo' },
        { header: 'Ação', accessor: 'acao' },
        { header: 'Descrição', accessor: 'descricao' },
        {
            header: 'Status',
            accessor: 'ativo',
            render: (permissao) => (
                <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${permissao.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
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
                        onClick={() => handleToggleStatusPermissao(permissao)}
                        className={`${permissao.ativo ? 'text-red-500 hover:text-red-700' : 'text-green-500 hover:text-green-700'}`}
                    >
                        {permissao.ativo ? <XCircle size={18} /> : <CheckCircle size={18} />}
                    </button>
                    <button title="Editar" onClick={() => handleEditPermissao(permissao)} className="text-indigo-600 hover:text-indigo-900">
                        <Edit size={18} />
                    </button>
                    <button title="Excluir" onClick={() => handleDeletePermissao(permissao.id)} className="text-red-600 hover:text-red-900">
                        <Trash2 size={18} />
                    </button>
                </div>
            ),
        },
    ];

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
            <div className="flex justify-between items-center bg-white p-6 rounded-lg shadow-sm border border-gray-200 mb-6">
                <div>
                  <h1 className="text-2xl font-bold text-gray-800 flex items-center gap-2">
                    <Shield className="text-indigo-600" />
                    Perfis & Acessos
                  </h1>
                  <p className="text-gray-500 mt-1">Gerencie os perfis de acesso e as matrizes de permissão.</p>
                </div>
                {activeTab === 'perfis' ? (
                    <button
                        onClick={() => { setEditingPerfil(null); setFormData({ nome: '', descricao: '' }); setIsModalOpen(true); }}
                        className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors shadow-sm"
                    >
                        <Plus size={20} className="mr-2" />
                        Novo Perfil
                    </button>
                ) : (
                    <button
                        onClick={() => { setEditingPermissao(null); setPermFormData({ modulo: '', moduloPai: '', acao: '', descricao: '' }); setIsPermModalOpen(true); }}
                        className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors shadow-sm"
                    >
                        <Plus size={20} className="mr-2" />
                        Nova Permissão
                    </button>
                )}
            </div>

            {/* TABS */}
            <div className="border-b border-gray-200">
                <nav className="-mb-px flex space-x-8" aria-label="Tabs">
                    <button
                        onClick={() => setActiveTab('perfis')}
                        className={`whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm
                            ${activeTab === 'perfis' ? 'border-indigo-500 text-indigo-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'}`}
                    >
                        Gerenciar Perfis
                    </button>
                    <button
                        onClick={() => setActiveTab('permissoes')}
                        className={`whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm
                            ${activeTab === 'permissoes' ? 'border-indigo-500 text-indigo-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'}`}
                    >
                        Cadastros de Permissões
                    </button>
                </nav>
            </div>

            {/* TAB CONTENT: PERFIS */}
            {activeTab === 'perfis' && (
                <div className="space-y-6">
                    <FilterBar onFilter={applyFilters} onClear={clearFilters}>
                        <FormInput
                            label="Nome do Perfil"
                            value={filters.nome}
                            onChange={e => setFilters({ ...filters, nome: e.target.value })}
                            placeholder="Buscar por nome..."
                        />
                    </FilterBar>
                    <Table data={perfis} columns={columnsPerfis} />
                </div>
            )}

            {/* TAB CONTENT: PERMISSOES */}
            {activeTab === 'permissoes' && (
                <div className="space-y-6">
                    <FilterBar onFilter={applyFilters} onClear={clearFilters}>
                        <FormInput
                            label="Módulo Pai"
                            value={permFilters.moduloPai}
                            onChange={e => setPermFilters({ ...permFilters, moduloPai: e.target.value })}
                            placeholder="Buscar por módulo pai..."
                        />
                        <FormInput
                            label="Módulo"
                            value={permFilters.modulo}
                            onChange={e => setPermFilters({ ...permFilters, modulo: e.target.value })}
                            placeholder="Buscar por módulo..."
                        />
                        <FormInput
                            label="Ação"
                            value={permFilters.acao}
                            onChange={e => setPermFilters({ ...permFilters, acao: e.target.value })}
                            placeholder="Buscar por ação..."
                        />
                    </FilterBar>
                    <Table data={permissoesList} columns={columnsPermissoes} />
                </div>
            )}

            {/* MODALS: PERFIS */}
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
                <form id="perfil-form" onSubmit={handleSubmitPerfil} className="space-y-4">
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

            {/* MODALS: PERMISSION MATRIX */}
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
                            {permissoesMatriz.map((grupoPai) => (
                                <React.Fragment key={grupoPai.moduloPai || 'outros'}>
                                    {grupoPai.moduloPai && (
                                        <tr className="bg-gray-50 border-t border-b border-gray-200">
                                            <td colSpan={2} className="px-6 py-2 text-xs font-bold text-gray-700 uppercase tracking-wider bg-gray-100">
                                                {grupoPai.moduloPai}
                                            </td>
                                        </tr>
                                    )}
                                    {grupoPai.modulosFilhos.map((item) => (
                                        <tr key={item.modulo}>
                                            <td className={`px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900 ${grupoPai.moduloPai ? 'pl-10 border-l-[3px] border-indigo-200' : ''}`}>
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
                                                                className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded focus:ring-offset-0"
                                                            />
                                                            <span className="select-none">{p.acao}</span>
                                                        </label>
                                                    ))}
                                                </div>
                                            </td>
                                        </tr>
                                    ))}
                                </React.Fragment>
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

            {/* MODALS: PERMISSOES (CREATE/EDIT) */}
            <Modal
                isOpen={isPermModalOpen}
                onClose={() => setIsPermModalOpen(false)}
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
                            onClick={() => setIsPermModalOpen(false)}
                            className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
                        >
                            Cancelar
                        </button>
                    </>
                )}
            >
                <form id="permissao-form" onSubmit={handleSubmitPermissao} className="space-y-4">
                    <FormInput
                        label="Módulo Pai (Opcional)"
                        value={permFormData.moduloPai}
                        onChange={(e) => setPermFormData({ ...permFormData, moduloPai: e.target.value })}
                        placeholder="Ex: Cadastros, Relatórios..."
                    />
                    <FormInput
                        label="Módulo"
                        required
                        value={permFormData.modulo}
                        onChange={(e) => setPermFormData({ ...permFormData, modulo: e.target.value })}
                        placeholder="Ex: Auditoria"
                    />
                    <FormInput
                        label="Ação"
                        required
                        value={permFormData.acao}
                        onChange={(e) => setPermFormData({ ...permFormData, acao: e.target.value })}
                        placeholder="Ex: Visualizar"
                    />
                    <FormInput
                        label="Descrição"
                        value={permFormData.descricao}
                        onChange={(e) => setPermFormData({ ...permFormData, descricao: e.target.value })}
                        placeholder="Breve descrição da permissão"
                    />
                </form>
            </Modal>
        </div>
    );
}
