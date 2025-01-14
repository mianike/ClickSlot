'use client';

import { useState, useEffect } from 'react';
import axiosInstance from '../api/axiosInstance'; // Путь к вашему axiosInstance
import Link from 'next/link';
import { useRouter} from 'next/navigation';

interface Master {
  id: number;
  name: string;
  rating: number;
  reviewsCount:number;
  offeringsCount: number;
}

export default function MastersPage() {
  const [masters, setMasters] = useState<Master[]>([]);
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(10); // фиксируем размер страницы
  const router = useRouter();

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
    fetchMasters('', currentPage);
  }, [currentPage]);

  const handleSearch = () => {
    setCurrentPage(1); // При новом поиске сбрасываем страницу на первую
    fetchMasters(search, 1);
  };

  const handleMasterOfferings = (masterId: number) => {
    router.push(`/masters/${masterId}`);
  };

  return (
    <div className="px-4 sm:px-6 lg:px-10 mt-4 mb-4"> {/* Добавлены отступы с боков */}
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
        <p>Мастера с услугами не найдены</p>
      ) : (
        <div className="grid gap-6 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
          {masters.map((master) => (
            <div
              key={master.id}
              className="bg-white border border-gray-300 rounded-lg shadow-md p-4 w-full"
            >
              <h2 className="text-xl font-semibold text-gray-800">{master.name}</h2>
              <h3 className="mt-4 font-semibold text-lg text-gray-600">Услуг: {master.offeringsCount}</h3>
              <h3 className="mt-4 font-semibold text-wrap text-gray-600">Рейтинг: {master.rating} (Отз: {master.reviewsCount})</h3>
              <button
                onClick={() => handleMasterOfferings(master.id)} // Переход на страницу записи
                className="mt-4 px-4 py-2 bg-blue-500 text-white rounded"
              >
                Выбрать
              </button>
            </div>
          ))}
        </div>
      )}

      {/* Пагинация */}
      <div className="flex flex-col items-center p-20 min-h-screen bg-gray-100">{/* Пагинация */}
      <div className="flex gap-4 mt-8">
        <button
          disabled={currentPage === 1 || loading}
          onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
          className={`px-4 py-2 rounded ${
            currentPage === 1 ? "bg-gray-400 cursor-not-allowed" : "bg-blue-500 text-white hover:bg-blue-600"
            }`}
        >
          Предыдущая
        </button>
        <span className="mt-2">Страница: {currentPage}</span>
        <button
          onClick={() => setCurrentPage((prev) => prev + 1)}
          className={`px-4 py-2 rounded ${
            masters.length === pageSize ? "bg-blue-500 text-white hover:bg-blue-600" : "bg-gray-400 cursor-not-allowed"
            }`}
            disabled={masters.length < pageSize || loading}
        >
          Следующая
        </button>
      </div>
      </div>
    </div>
  );
}