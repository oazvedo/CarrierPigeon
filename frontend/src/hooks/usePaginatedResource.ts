import { useCallback, useEffect, useState } from "react";
import { apiClient } from "../api/client";
import type { PaginatedResult } from "../api/types";

const PAGE_SIZE = 10;

export function usePaginatedResource<T extends { id: string }>(basePath: string) {
  const [items, setItems] = useState<T[]>([]);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [reloadToken, setReloadToken] = useState(0);

  const reload = useCallback(() => setReloadToken((token) => token + 1), []);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    setError(null);

    apiClient
      .get<PaginatedResult<T>>(`${basePath}?page=${page}&pageSize=${PAGE_SIZE}`)
      .then((result) => {
        if (cancelled) return;
        setItems(result.items);
        setTotalCount(result.totalCount);
      })
      .catch((err: Error) => {
        if (!cancelled) setError(err.message);
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => {
      cancelled = true;
    };
  }, [basePath, page, reloadToken]);

  const create = useCallback(
    async (body: unknown) => {
      await apiClient.post(basePath, body);
      setPage(1);
      reload();
    },
    [basePath, reload],
  );

  const update = useCallback(
    async (id: string, body: unknown) => {
      await apiClient.put(`${basePath}/${id}`, body);
      reload();
    },
    [basePath, reload],
  );

  const remove = useCallback(
    async (id: string) => {
      await apiClient.delete(`${basePath}/${id}`);
      reload();
    },
    [basePath, reload],
  );

  return {
    items,
    page,
    setPage,
    pageSize: PAGE_SIZE,
    totalCount,
    loading,
    error,
    create,
    update,
    remove,
  };
}
