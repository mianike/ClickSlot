'use client';

import { useState, useEffect } from 'react';
import axiosInstance from '../api/axiosInstance'; // Путь к вашему axiosInstance

interface Offering {
  name: string;
  price: number;
  duration: string;
}

interface Master {
  id: number;
  name: string;
  address: string;
  offerings: Offering[];
}

export default function MastersPage() {
  const [masters, setMasters] = useState<Master[]>([]);
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10); // фиксируем размер страницы

  const fetchMasters = async (searchQuery: string, pageNumber: number) => {
    setLoading(true);
    try {
      // Используем axios для запроса
      const response = await axiosInstance.get('/masters', {
        params: {
          search: searchQuery,
          page: pageNumber,
          pageSize: pageSize,
        },
      });

      // Приводим тип ответа к Master[]
      setMasters(response.data as Master[]);
    } catch (error) {
      console.error('Failed to fetch masters', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    // Загрузка всех мастеров при первом рендере
    fetchMasters('', page);
  }, [page]);

  const handleSearch = () => {
    setPage(1); // При новом поиске сбрасываем страницу на первую
    fetchMasters(search, 1);
  };

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Мастера</h1>

      <div className="mb-4 flex gap-4">
        <input
          type="text"
          placeholder="Введите имя мастера или услугу"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="border border-gray-300 p-2 rounded w-full"
        />
        <button
          onClick={handleSearch}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          Поиск
        </button>
      </div>

      {loading ? (
        <p>Загрузка...</p>
      ) : masters.length === 0 ? (
        <p>Мастера не найдены</p>
      ) : (
        <div className="grid gap-6 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          {masters.map((master) => (
            <div
              key={master.id}
              className="bg-white border border-gray-300 rounded-lg shadow-md p-4 hover:shadow-lg transition-shadow"
            >
              <h2 className="text-xl font-semibold text-blue-600">{master.name}</h2>
              <h3 className="mt-4 font-semibold text-lg text-gray-600">Услуг: {master.offerings.length}</h3>
            </div>
          ))}
        </div>
      )}

      <div className="flex justify-between items-center mt-4">
        <button
          onClick={() => setPage((prev) => Math.max(prev - 1, 1))}
          disabled={page === 1 || loading}
          className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600 disabled:opacity-50"
        >
          Назад
        </button>
        <span>Страница: {page}</span>
        <button
          onClick={() => setPage((prev) => prev + 1)}
          disabled={masters.length < pageSize || loading}
          className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600 disabled:opacity-50"
        >
          Вперёд
        </button>
      </div>
    </div>
  );
}