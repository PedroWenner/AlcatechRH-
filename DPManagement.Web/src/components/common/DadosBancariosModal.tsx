import { useState, useEffect } from 'react';
import { Plus, Edit, Trash2, CheckCircle, XCircle } from 'lucide-react';
import api from '../../services/api';
import { FormInput } from './FormInput';
import { alertSuccess, alertError, alertDeleteConfirm, showLoading, closeLoading } from '../../services/alertService';
import { Table, type TableColumn } from './Table';
import { Modal } from './Modal';
import { Autocomplete } from './Autocomplete';

interface DadoBancario {
  id: string;
  colaboradorId: string;
  bancoId?: string;
  codigoBanco: string;
  nomeBanco: string;
  agencia: string;
  digitoAgencia: string;
  conta: string;
  digitoConta: string;
  operacao: string;
  ativo: boolean;
}

interface DadosBancariosModalProps {
  isOpen: boolean;
  onClose: () => void;
  colaboradorId: string | null;
  colaboradorNome: string | null;
}

export function DadosBancariosModal({ isOpen, onClose, colaboradorId, colaboradorNome }: DadosBancariosModalProps) {
  const [dados, setDados] = useState<DadoBancario[]>([]);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingDado, setEditingDado] = useState<DadoBancario | null>(null);
  const [formData, setFormData] = useState({
    bancoId: '', codigoBanco: '', agencia: '', digitoAgencia: '', conta: '', digitoConta: '', operacao: ''
  });

  useEffect(() => {
    if (isOpen && colaboradorId) {
      fetchDados();
    } else {
      setDados([]);
    }
  }, [isOpen, colaboradorId]);

  const fetchDados = async () => {
    try {
      showLoading('Carregando dados bancários...');
      const response = await api.get(`/colaboradores/${colaboradorId}/dados-bancarios`);
      setDados(response.data);
    } catch (error) {
      console.error('Erro ao buscar dados bancários', error);
      alertError('Erro ao carregar dados bancários');
    } finally {
      closeLoading();
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!colaboradorId) return;

    showLoading('Salvando...');
    try {
      if (editingDado) {
        await api.put(`/colaboradores/${colaboradorId}/dados-bancarios/${editingDado.id}`, formData);
        alertSuccess('Dado bancário atualizado');
      } else {
        await api.post(`/colaboradores/${colaboradorId}/dados-bancarios`, formData);
        alertSuccess('Dado bancário cadastrado');
      }
      setIsFormOpen(false);
      fetchDados();
    } catch (error) {
      console.error('Erro ao salvar dado bancário', error);
      alertError('Erro ao salvar dado bancário');
    } finally {
      closeLoading();
    }
  };

  const handleDelete = async (id: string) => {
    if (!colaboradorId) return;
    const confirmed = await alertDeleteConfirm('Excluir Registro?', 'Esta ação removerá a conta permanentemente.');
    if (confirmed) {
      showLoading('Excluindo...');
      try {
        await api.delete(`/colaboradores/${colaboradorId}/dados-bancarios/${id}`);
        alertSuccess('Registro excluído');
        fetchDados();
      } catch (error) {
        console.error('Erro ao excluir conta', error);
        alertError('Erro ao excluir conta');
      } finally {
        closeLoading();
      }
    }
  };

  const handleToggleStatus = async (dado: DadoBancario) => {
    if (!colaboradorId) return;
    const novoStatus = !dado.ativo;
    showLoading(novoStatus ? 'Ativando...' : 'Inativando...');
    try {
      await api.put(`/colaboradores/${colaboradorId}/dados-bancarios/${dado.id}/ativar?ativo=${novoStatus}`);
      alertSuccess(novoStatus ? 'Conta ativada' : 'Conta inativada');
      fetchDados();
    } catch (error) {
      console.error('Erro ao alterar status', error);
      alertError('Erro ao alterar status');
    } finally {
      closeLoading();
    }
  };

  const handleEdit = (dado: DadoBancario) => {
    setEditingDado(dado);
    setFormData({
      bancoId: dado.bancoId || '',
      codigoBanco: dado.codigoBanco,
      agencia: dado.agencia,
      digitoAgencia: dado.digitoAgencia,
      conta: dado.conta,
      digitoConta: dado.digitoConta,
      operacao: dado.operacao
    });
    setIsFormOpen(true);
  };

  const columns: TableColumn<DadoBancario>[] = [
    {
      header: 'Banco',
      render: (d) => `${d.codigoBanco} - ${d.nomeBanco}`
    },
    {
      header: 'Agência',
      render: (d) => `${d.agencia}-${d.digitoAgencia}`
    },
    {
      header: 'Conta / OP',
      render: (d) => `${d.conta}-${d.digitoConta} (OP: ${d.operacao})`
    },
    {
      header: 'Status',
      render: (d) => (
        <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${d.ativo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
          {d.ativo ? 'Ativo' : 'Inativo'}
        </span>
      )
    },
    {
      header: 'Ações',
      align: 'right',
      render: (d) => (
        <div className="space-x-4 flex justify-end items-center">
          <button title="Editar" onClick={() => handleEdit(d)} className="text-indigo-600 hover:text-indigo-900">
            <Edit size={18} />
          </button>
          <button
            title={d.ativo ? "Inativar" : "Ativar"}
            onClick={() => handleToggleStatus(d)}
            className={d.ativo ? "text-red-500 hover:text-red-700" : "text-green-500 hover:text-green-700"}
          >
            {d.ativo ? <XCircle size={18} /> : <CheckCircle size={18} />}
          </button>
          <button title="Excluir" onClick={() => handleDelete(d.id)} className="text-red-600 hover:text-red-900">
            <Trash2 size={18} />
          </button>
        </div>
      ),
    },
  ];

  return (
    <>
      <Modal
        isOpen={isOpen}
        onClose={onClose}
        title={`Dados Bancários: ${colaboradorNome}`}
        size="4xl"
        footer={(
          <button
            type="button"
            onClick={onClose}
            className="w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:w-auto sm:text-sm"
          >
            Fechar
          </button>
        )}
      >
        <div className="space-y-4">
          <div className="flex justify-end mb-4">
            <button
              onClick={() => {
                setEditingDado(null);
                setFormData({ bancoId: '', codigoBanco: '', agencia: '', digitoAgencia: '', conta: '', digitoConta: '', operacao: '' });
                setIsFormOpen(true);
              }}
              className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 transition-colors"
            >
              <Plus size={18} className="mr-2" />
              Adicionar Conta
            </button>
          </div>

          <Table data={dados} columns={columns} />
        </div>
      </Modal>

      {/* Nested Modal for Form */}
      {isFormOpen && (
        <Modal
          isOpen={isFormOpen}
          onClose={() => setIsFormOpen(false)}
          title={editingDado ? 'Editar Conta' : 'Nova Conta Bancária'}
          size="md"
          footer={(
            <>
              <button
                type="submit"
                form="banco-form"
                className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm"
              >
                Salvar
              </button>
              <button
                type="button"
                onClick={() => setIsFormOpen(false)}
                className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
              >
                Cancelar
              </button>
            </>
          )}
        >
          <form id="banco-form" onSubmit={handleSubmit} className="p-1">
            <div className="grid grid-cols-1 gap-y-4">
              <Autocomplete
                label="Código do Banco"
                required
                placeholder="Ex: 001, Brasil..."
                defaultValue={formData.codigoBanco}
                onSearch={async (term) => {
                  const response = await api.get(`/bancos?term=${encodeURIComponent(term)}`);
                  return response.data;
                }}
                onSelect={(item) => {
                  if (item) {
                    setFormData({ ...formData, bancoId: item.id, codigoBanco: item.codigo });
                  } else {
                    setFormData({ ...formData, bancoId: '', codigoBanco: '' });
                  }
                }}
                displayValue={(item) => `${item.codigo} - ${item.titulo}`}
                keyValue={(item) => item.codigo}
              />
              <div className="grid grid-cols-2 gap-4">
                <FormInput
                  label="Agência" required
                  value={formData.agencia} onChange={e => setFormData({ ...formData, agencia: e.target.value.replace(/\D/g, '') })}
                />
                <FormInput
                  label="Dígito da Agência" required
                  value={formData.digitoAgencia} onChange={e => setFormData({ ...formData, digitoAgencia: e.target.value })}
                />
              </div>
              <div className="grid grid-cols-2 gap-4">
                <FormInput
                  label="Conta" required
                  value={formData.conta} onChange={e => setFormData({ ...formData, conta: e.target.value.replace(/\D/g, '') })}
                />
                <FormInput
                  label="Dígito da Conta" required
                  value={formData.digitoConta} onChange={e => setFormData({ ...formData, digitoConta: e.target.value })}
                />
              </div>
              <FormInput
                label="Operação" placeholder="Ex: 001, 013, ou vazio"
                value={formData.operacao} onChange={e => setFormData({ ...formData, operacao: e.target.value })}
              />
            </div>
          </form>
        </Modal>
      )}
    </>
  );
}
