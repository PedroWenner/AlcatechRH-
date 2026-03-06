import Swal from 'sweetalert2';

export const alertSuccess = (title: string, text?: string) => {
  Swal.fire({
    icon: 'success',
    title,
    text,
    timer: 3000,
    timerProgressBar: true,
    showConfirmButton: false,
    toast: true,
    position: 'top-end',
  });
};

export const alertError = (title: string, text?: string) => {
  Swal.fire({
    icon: 'error',
    title,
    text: text || 'Ocorreu um erro ao realizar a operação.',
  });
};

export const alertWarning = (title: string, text?: string) => {
  Swal.fire({
    icon: 'warning',
    title,
    text,
  });
};

export const alertConfirm = async (title: string, text: string, confirmButtonText = 'Sim, continuar') => {
  const result = await Swal.fire({
    title,
    text,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText,
    cancelButtonText: 'Cancelar',
    confirmButtonColor: '#ef4444',
    cancelButtonColor: '#6b7280',
  });

  return result.isConfirmed;
};

export const alertDeleteConfirm = (title = 'Excluir registro?', text = 'Esta ação não poderá ser desfeita.') => {
  return alertConfirm(title, text, 'Sim, excluir');
};

export const showLoading = (title = 'Processando...') => {
  Swal.fire({
    title,
    allowOutsideClick: false,
    didOpen: () => {
      Swal.showLoading();
    },
  });
};

export const closeLoading = () => {
  Swal.close();
};
