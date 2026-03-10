import { useState, useEffect } from 'react';
import api from '../services/api';

export interface CatalogoItem {
  id: number;
  nome: string;
}

export function useCatalogos() {
  const [regimesJuridicos, setRegimesJuridicos] = useState<CatalogoItem[]>([]);
  const [formasIngresso, setFormasIngresso] = useState<CatalogoItem[]>([]);
  const [tiposRubrica, setTiposRubrica] = useState<CatalogoItem[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [hasError, setHasError] = useState(false);

  const fetchCatalogos = async () => {
    setIsLoading(true);
    setHasError(false);
    try {
      const [resRegimes, resFormas, resTipos] = await Promise.all([
        api.get<CatalogoItem[]>('/catalogos/regimes-juridicos'),
        api.get<CatalogoItem[]>('/catalogos/formas-ingresso'),
        api.get<CatalogoItem[]>('/catalogos/tipos-rubrica')
      ]);

      setRegimesJuridicos(resRegimes.data);
      setFormasIngresso(resFormas.data);
      setTiposRubrica(resTipos.data);
    } catch (err) {
      setHasError(true);
      console.error('Erro ao carregar catálogos do servidor:', err);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchCatalogos();
  }, []);

  return {
    regimesJuridicos,
    formasIngresso,
    tiposRubrica,
    isLoading,
    hasError,
    refetch: fetchCatalogos
  };
}
