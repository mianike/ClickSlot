'use client';

import React, { useEffect, useState } from 'react';
import axiosInstance from '../api/axiosInstance';
import Link from 'next/link';

interface Offering {
  id: number;
  name: string;
  price: number;
  duration: number;
}

interface User {
  id: number;
  name: string;
  email: string;
}

const OfferingsPage: React.FC = () => {
  const [offerings, setOfferings] = useState<Offering[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [user, setUser] = useState<{ id: number } | null>(null);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      // Получение данных пользователя
      axiosInstance.get<User>('/auth/me') // Эндпоинт, возвращающий информацию о текущем пользователе
        .then(response => {
          setUser(response.data);
          setLoading(false); // Загрузка завершена
        })
        .catch(error => {
          console.error('Ошибка при получении данных пользователя:', error);
          localStorage.removeItem('token'); // Если токен недействителен
          setLoading(false); // Загрузка завершена, но с ошибкой
        });
    } else {
      setLoading(false); // Если токен не найден, сразу завершить загрузку
    }
  }, []);

  useEffect(() => {
    const fetchOfferings = async () => {
      if (user) {
        try {
          const response = await axiosInstance.get<Offering[]>(`/offerings/master/${user.id}`); // Теперь правильная интерполяция
          setOfferings(response.data);
        } catch (err) {
          setError('Ошибка загрузки данных.');
        } finally {
          setLoading(false);
        }
      }
    };

    fetchOfferings();
  }, [user]);

  const handleDelete = async (offeringId: number) => {
    try {
      await axiosInstance.delete(`/offerings/${offeringId}`);
      setOfferings(offerings.filter(offering => offering.id !== offeringId)); // Удаляем услугу из списка
    } catch (err) {
      setError('Ошибка при удалении услуги.');
    }
  };

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p className="text-red-500">{error}</p>;

  return (
    <div className="px-4 sm:px-6 lg:px-8 mt-4 mb-4"> {/* Добавлены отступы с боков */}
      <div className="flex justify-between items-center mb-6"> {/* Центрируем заголовок и кнопку по горизонтали */}
        <h1 className="text-2xl font-bold">Мои услуги</h1>
        <Link href="/offerings/new" className="bg-green-500 text-white px-6 py-3 rounded-md shadow-md hover:bg-green-600 transition">
          Добавить услугу
        </Link>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {offerings.map(offering => (
          <div key={offering.id} className="bg-white rounded-lg shadow-lg p-6 flex flex-col">
            <h2 className="text-xl font-semibold mb-2">{offering.name}</h2>
            <p className="text-gray-500 mb-2">Цена: {offering.price} руб</p>
            <p className="text-gray-500 mb-4">Длительность: {offering.duration} мин</p>
            <div className="flex justify-between items-center">
              <Link href={`/offerings/edit/${offering.id}`} className="text-blue-500 hover:underline">Редактировать</Link>
              <button onClick={() => handleDelete(offering.id)} className="text-red-500 hover:underline">Удалить</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default OfferingsPage;