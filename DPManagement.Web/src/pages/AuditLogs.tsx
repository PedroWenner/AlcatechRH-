import { useState, useEffect } from 'react';
import { History, FileText, User as UserIcon, Info, X } from 'lucide-react';
import api from '../services/api';
import { showLoading, closeLoading, alertError } from '../utils/swal';
import { FilterBar } from '../components/common/FilterBar';
import { Table, type TableColumn } from '../components/common/Table';
import { DatePicker } from '../components/common/DatePicker';
import { parseISO, format } from 'date-fns';

interface AuditLog {
  id: string;
  tableName: string;
  entityId: string;
  action: string;
  oldValues: string;
  newValues: string;
  changedColumns: string;
  userId: string;
  userName: string;
  createdAt: string;
}

export default function AuditLogs() {
  const [logs, setLogs] = useState<AuditLog[]>([]);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pageSize: 10,
    totalPages: 0,
    totalCount: 0
  });

  const [filters, setFilters] = useState({
    dataInicio: '',
    dataFim: '',
    userId: '',
    tableName: '',
    action: ''
  });

  const [selectedLog, setSelectedLog] = useState<AuditLog | null>(null);

  useEffect(() => {
    fetchLogs();
  }, []);

  const fetchLogs = async (page = 1, f = filters) => {
    try {
      showLoading('Carregando logs...');
      const params: any = {
        page,
        pageSize: pagination.pageSize
      };

      if (f.dataInicio) params.dataInicio = f.dataInicio;
      if (f.dataFim) params.dataFim = f.dataFim;
      if (f.userId) params.userId = f.userId;
      if (f.tableName) params.tableName = f.tableName;
      if (f.action) params.action = f.action;

      const response = await api.get('/audit-logs', { params });
      setLogs(response.data.items);
      setPagination(prev => ({
        ...prev,
        currentPage: response.data.page,
        totalPages: response.data.totalPages,
        totalCount: response.data.totalCount
      }));
    } catch (error) {
      console.error('Erro ao buscar logs', error);
      alertError('Erro ao carregar logs de auditoria');
    } finally {
      closeLoading();
    }
  };

  const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFilters(prev => ({ ...prev, [name]: value }));
  };

  const applyFilters = () => {
    fetchLogs(1);
  };

  const clearFilters = () => {
    const emptyFilters = {
      dataInicio: '',
      dataFim: '',
      userId: '',
      tableName: '',
      action: ''
    };
    setFilters(emptyFilters);
    fetchLogs(1, emptyFilters);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString('pt-BR');
  };

  const parseJson = (json: string) => {
    if (!json) return null;
    try {
      return JSON.parse(json);
    } catch {
      return json;
    }
  };

  const columns: TableColumn<AuditLog>[] = [
    {
      header: 'Data / Hora',
      render: (log) => formatDate(log.createdAt),
    },
    {
      header: 'Usuário',
      render: (log) => (
        <div className="flex items-center space-x-2">
          <UserIcon size={16} className="text-gray-400" />
          <span className="text-sm text-gray-700">{log.userName}</span>
        </div>
      ),
    },
    {
      header: 'Tabela',
      render: (log) => (
        <span className="text-xs font-semibold bg-blue-50 text-blue-600 px-2 py-1 rounded-full border border-blue-100 uppercase tracking-tighter">
          {log.tableName}
        </span>
      ),
    },
    {
      header: 'Ação',
      render: (log) => (
        <span
          className={`text-xs font-semibold px-2 py-1 rounded-full border border-opacity-20 uppercase tracking-tighter ${log.action === 'Insert'
              ? 'bg-green-50 text-green-600 border-green-200'
              : log.action === 'Update'
                ? 'bg-amber-50 text-amber-600 border-amber-200'
                : 'bg-red-50 text-red-600 border-red-200'
            }`}
        >
          {log.action === 'Insert' ? 'Inclusão' : log.action === 'Update' ? 'Alteração' : 'Exclusão'}
        </span>
      ),
    },
    {
      header: 'Detalhes',
      align: 'center',
      render: (log) => (
        <button
          onClick={() => setSelectedLog(log)}
          className="p-2 text-indigo-600 hover:bg-indigo-50 rounded-lg transition-all"
        >
          <Info size={20} />
        </button>
      ),
    },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-2">
          <History className="text-indigo-600" size={32} />
          <h1 className="text-2xl font-bold text-gray-800">Logs de Auditoria</h1>
        </div>
      </div>

      <FilterBar onFilter={applyFilters} onClear={clearFilters}>
        <DatePicker
          label="Início"
          selected={filters.dataInicio ? parseISO(filters.dataInicio) : null}
          onChange={(date) => {
            const dateStr = date ? format(date, 'yyyy-MM-dd') : '';
            setFilters(prev => ({ ...prev, dataInicio: dateStr }));
          }}
        />

        <DatePicker
          label="Fim"
          selected={filters.dataFim ? parseISO(filters.dataFim) : null}
          onChange={(date) => {
            const dateStr = date ? format(date, 'yyyy-MM-dd') : '';
            setFilters(prev => ({ ...prev, dataFim: dateStr }));
          }}
        />

        <div className="space-y-1">
          <label className="text-sm font-medium text-gray-700 mb-1">Tabela</label>
          <div className="relative">
            <div className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
              <FileText size={18} />
            </div>
            <select
              name="tableName"
              value={filters.tableName}
              onChange={handleFilterChange}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-indigo-100 focus:border-indigo-500 outline-none transition-all text-sm appearance-none"
            >
              <option value="">Todas</option>
              <option value="Colaboradores">Colaboradores</option>
              <option value="Cargos">Cargos</option>
              <option value="Usuarios">Usuários</option>
            </select>
          </div>
        </div>

        <div className="space-y-1">
          <label className="text-sm font-medium text-gray-700 mb-1">Ação</label>
          <select
            name="action"
            value={filters.action}
            onChange={handleFilterChange}
            className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-indigo-100 focus:border-indigo-500 outline-none transition-all text-sm"
          >
            <option value="">Todas</option>
            <option value="Insert">Inclusão</option>
            <option value="Update">Alteração</option>
            <option value="Delete">Exclusão</option>
          </select>
        </div>
      </FilterBar>

      <Table
        data={logs}
        columns={columns}
        pagination={{
          currentPage: pagination.currentPage,
          totalPages: pagination.totalPages,
          totalCount: pagination.totalCount,
          pageSize: pagination.pageSize,
          onPageChange: (page) => fetchLogs(page),
        }}
        emptyMessage="Nenhum log encontrado para os filtros selecionados."
      />

      {/* Modal de Detalhes */}
      {selectedLog && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-2xl shadow-2xl w-full max-w-4xl max-h-[90vh] overflow-hidden flex flex-col">
            <div className="p-6 border-b border-gray-100 flex items-center justify-between bg-indigo-600 text-white">
              <div className="flex items-center space-x-3">
                <Info size={24} />
                <div>
                  <h2 className="text-xl font-bold">Detalhes da Alteração</h2>
                  <p className="text-xs text-indigo-100 opacity-80">
                    ID: {selectedLog.id} • {formatDate(selectedLog.createdAt)}
                  </p>
                </div>
              </div>
              <button
                onClick={() => setSelectedLog(null)}
                className="p-2 hover:bg-white/20 rounded-xl transition-colors"
              >
                <X size={24} />
              </button>
            </div>

            <div className="p-6 overflow-y-auto space-y-6 flex-1 bg-gray-50">
              <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-100">
                  <span className="text-[10px] font-bold text-gray-400 uppercase tracking-widest block mb-1">Tabela</span>
                  <p className="font-bold text-gray-700">{selectedLog.tableName}</p>
                </div>
                <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-100">
                  <span className="text-[10px] font-bold text-gray-400 uppercase tracking-widest block mb-1">Ação</span>
                  <p className="font-bold text-gray-700">{selectedLog.action}</p>
                </div>
                <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-100">
                  <span className="text-[10px] font-bold text-gray-400 uppercase tracking-widest block mb-1">Usuário</span>
                  <p className="font-bold text-gray-700">{selectedLog.userName}</p>
                </div>
              </div>

              {selectedLog.action === 'Update' && (
                <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-100">
                  <span className="text-[10px] font-bold text-gray-400 uppercase tracking-widest block mb-3">Colunas Alteradas</span>
                  <div className="flex flex-wrap gap-2">
                    {parseJson(selectedLog.changedColumns)?.map((col: string) => (
                      <span key={col} className="bg-indigo-50 text-indigo-600 px-3 py-1 rounded-lg text-xs font-bold border border-indigo-100">
                        {col}
                      </span>
                    ))}
                  </div>
                </div>
              )}

              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <span className="text-xs font-bold text-red-500 uppercase tracking-widest px-2">Valor Anterior</span>
                  <div className="bg-red-50 p-4 rounded-2xl border border-red-100 font-mono text-xs overflow-auto max-h-64 shadow-inner text-red-900 whitespace-pre-wrap">
                    {selectedLog.oldValues ? JSON.stringify(parseJson(selectedLog.oldValues), null, 2) : 'N/A'}
                  </div>
                </div>
                <div className="space-y-2">
                  <span className="text-xs font-bold text-green-500 uppercase tracking-widest px-2">Novo Valor</span>
                  <div className="bg-green-50 p-4 rounded-2xl border border-green-100 font-mono text-xs overflow-auto max-h-64 shadow-inner text-green-900 whitespace-pre-wrap">
                    {selectedLog.newValues ? JSON.stringify(parseJson(selectedLog.newValues), null, 2) : 'N/A'}
                  </div>
                </div>
              </div>
            </div>

            <div className="p-4 bg-white border-t border-gray-100 flex justify-end">
              <button
                onClick={() => setSelectedLog(null)}
                className="px-8 py-3 bg-indigo-600 text-white rounded-xl font-bold hover:bg-indigo-700 transition-all shadow-lg active:scale-95"
              >
                Fechar Detalhes
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
