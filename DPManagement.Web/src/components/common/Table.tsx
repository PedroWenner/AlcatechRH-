import React from 'react';
import { Pagination } from './Pagination';

export interface TableColumn<T> {
    header: string;
    accessor?: keyof T;
    render?: (item: T) => React.ReactNode;
    align?: 'left' | 'center' | 'right';
    className?: string;
}

interface TableProps<T> {
    data: T[];
    columns: TableColumn<T>[];
    pagination?: {
        currentPage: number;
        totalPages: number;
        totalCount: number;
        pageSize: number;
        onPageChange: (page: number) => void;
    };
    emptyMessage?: string;
    isLoading?: boolean;
}

export function Table<T>({
    data,
    columns,
    pagination,
    emptyMessage = 'Nenhum dado encontrado.',
    isLoading = false,
}: TableProps<T>) {
    return (
        <div className="bg-white shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
            <div className="overflow-x-auto">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            {columns.map((column, index) => (
                                <th
                                    key={index}
                                    scope="col"
                                    className={`px-6 py-3 text-xs font-medium text-gray-500 uppercase tracking-wider ${column.align === 'center' ? 'text-center' : column.align === 'right' ? 'text-right' : 'text-left'
                                        } ${column.className || ''}`}
                                >
                                    {column.header}
                                </th>
                            ))}
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                        {isLoading ? (
                            <tr>
                                <td colSpan={columns.length} className="px-6 py-10 text-center text-sm text-gray-500 italic">
                                    Carregando...
                                </td>
                            </tr>
                        ) : data.length > 0 ? (
                            data.map((item, rowIndex) => (
                                <tr key={rowIndex} className="hover:bg-gray-50 transition-colors">
                                    {columns.map((column, colIndex) => (
                                        <td
                                            key={colIndex}
                                            className={`px-6 py-4 whitespace-nowrap text-sm ${column.align === 'center' ? 'text-center' : column.align === 'right' ? 'text-right' : 'text-left'
                                                } ${column.className || ''}`}
                                        >
                                            {column.render
                                                ? column.render(item)
                                                : column.accessor
                                                    ? (item[column.accessor] as React.ReactNode)
                                                    : null}
                                        </td>
                                    ))}
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan={columns.length} className="px-6 py-10 text-center text-sm text-gray-500 italic">
                                    {emptyMessage}
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
            {pagination && (
                <Pagination
                    currentPage={pagination.currentPage}
                    totalPages={pagination.totalPages}
                    onPageChange={pagination.onPageChange}
                    totalCount={pagination.totalCount}
                    pageSize={pagination.pageSize}
                />
            )}
        </div>
    );
}
