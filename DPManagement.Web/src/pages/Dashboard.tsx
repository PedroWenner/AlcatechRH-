import { Users, FileText, DollarSign, Activity } from 'lucide-react';

export default function Dashboard() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-semibold text-gray-900">Dashboard Visual</h1>
      </div>

      {/* Grid de Cards Resumo */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <SummaryCard 
          title="Total Colaboradores" 
          value="142" 
          icon={<Users className="text-blue-500" size={24} />} 
          trend="+5%" 
        />
        <SummaryCard 
          title="Férias Pendentes" 
          value="12" 
          icon={<FileText className="text-orange-500" size={24} />} 
          trend="-2%" 
        />
        <SummaryCard 
          title="Custo da Folha" 
          value="R$ 840K" 
          icon={<DollarSign className="text-green-500" size={24} />} 
          trend="+1.2%" 
        />
        <SummaryCard 
          title="Avisos eSocial" 
          value="3" 
          icon={<Activity className="text-red-500" size={24} />} 
          trend="Alerta" 
        />
      </div>

      {/* Área de Gráficos e Tabelas */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mt-8">
        <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-100 flex flex-col h-80">
          <h3 className="text-lg font-medium text-gray-800 mb-4">Admissões x Demissões (Últimos 12 meses)</h3>
          <div className="flex-1 flex items-center justify-center bg-gray-50 rounded border border-dashed border-gray-200 text-gray-400">
            [Gráfico de Barras]
          </div>
        </div>

        <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-100 flex flex-col h-80">
          <h3 className="text-lg font-medium text-gray-800 mb-4">Próximos Vencimentos de Contrato Temporário</h3>
          <div className="flex-1 overflow-auto border rounded border-gray-200">
             <table className="min-w-full divide-y break-words divide-gray-200 text-sm text-left">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-4 py-2 font-medium text-gray-500">Colaborador</th>
                    <th className="px-4 py-2 font-medium text-gray-500">Cargo</th>
                    <th className="px-4 py-2 font-medium text-gray-500">Vencimento</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  <tr>
                    <td className="px-4 py-2">João Silva</td>
                    <td className="px-4 py-2">Dev Pleno</td>
                    <td className="px-4 py-2 text-red-500 font-medium">15/Nov</td>
                  </tr>
                  <tr>
                    <td className="px-4 py-2">Maria Souza</td>
                    <td className="px-4 py-2">UX Designer</td>
                    <td className="px-4 py-2 text-orange-500">22/Nov</td>
                  </tr>
                </tbody>
             </table>
          </div>
        </div>
      </div>
    </div>
  );
}

function SummaryCard({ title, value, icon, trend }: { title: string, value: string, icon: React.ReactNode, trend: string }) {
  return (
    <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-100 flex items-center">
      <div className="p-3 bg-gray-50 rounded-full mr-4">
        {icon}
      </div>
      <div>
        <p className="text-sm font-medium text-gray-500">{title}</p>
        <div className="flex items-baseline mt-1">
          <p className="text-2xl font-semibold text-gray-900">{value}</p>
          <span className={`ml-2 text-sm font-medium ${trend.startsWith('+') ? 'text-green-600' : trend.startsWith('-') ? 'text-orange-600' : 'text-red-600'}`}>
            {trend}
          </span>
        </div>
      </div>
    </div>
  );
}
