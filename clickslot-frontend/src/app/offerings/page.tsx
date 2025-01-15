'use client';

import React, { useEffect, useState } from 'react';
import axiosInstance from '../api/axiosInstance';
import Link from 'next/link';
import { useRouter} from 'next/navigation';

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
  const [user, setUser] = useState<{ id: number } | null>(null)
  const router = useRouter();

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      axiosInstance.get<User>('/auth/me')
        .then(response => {
          setUser(response.data);
          setLoading(false);
        })
        .catch(error => {
          console.error('Ошибка при получении данных пользователя:', error);
          localStorage.removeItem('token');
          setLoading(false);
        });
    } else {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    const fetchOfferings = async () => {
      if (user) {
        try {
          const response = await axiosInstance.get<Offering[]>(`/offerings/master/${user.id}`);
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
      setOfferings(offerings.filter(offering => offering.id !== offeringId));
    } catch (err) {
      setError('Ошибка при удалении услуги.');
    }
  };

  const handleOfferingEdit = (offeringId: number) => {
    router.push(`/offerings/edit/${offeringId}`);
  };

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p className="text-red-500">{error}</p>;

  return (
    <div className="px-4 sm:px-6 lg:px-8 mt-4 mb-4">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Мои услуги</h1>
        <Link href="/offerings/new" className="bg-green-500 text-white px-6 py-3 rounded-md shadow-md hover:bg-green-600 transition">
          Добавить услугу
        </Link>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {offerings.map(offering => (
          <div key={offering.id} className="bg-white rounded-lg shadow-lg p-6 flex flex-col">
            <h2 className="text-xl font-semibold mb-2">{offering.name}</h2>
            <p className="text-gray-500 mb-2">Стоимость: {offering.price} руб.</p>
            <p className="text-gray-500 mb-4">Продолжительность: {offering.duration}</p>
            <div className="flex justify-between items-center">
              <button
                    type="button"
                    className="px-6 py-3 bg-blue-500 text-white rounded-lg shadow hover:bg-blue-600 transition"
                    onClick={() => handleOfferingEdit(offering.id)}
                >
                    Редактировать
              </button>
              <button
                    type="button"
                    className="px-6 py-3 bg-red-500 text-white rounded-lg shadow hover:bg-red-600 transition"
                    onClick={() => handleDelete(offering.id)}
                >
                    Удалить
                </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default OfferingsPage;