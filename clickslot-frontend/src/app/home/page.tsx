'use client';

import React, { useEffect, useState } from 'react';
import axiosInstance from '../api/axiosInstance';
import Link from 'next/link';

interface User {
  id: number;
  name: string;
  email: string;
}

const HomePage: React.FC = () => {
  const [user, setUser] = useState<{ name: string } | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

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

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50">
      <h1 className="text-4xl font-bold text-gray-800 mb-6">
        Добро пожаловать на ClickSlot!
      </h1>  
      <p className="text-lg text-gray-600 mb-10">ClickSlot - удобный сервис бронирования услуг</p>

      <div className="flex space-x-4 mb-6">        

        {loading ? (
          <p>Загрузка...</p>
        ) : user ? (
            <div className="flex space-x-6">
          <Link
          href="/masters"
          className="px-6 py-3 bg-blue-500 text-white rounded-lg shadow hover:bg-blue-600 transition">
          Список мастеров
        </Link>
          <Link
            href="/auth/account"
            className="px-6 py-3 bg-blue-500 text-white rounded-lg shadow hover:bg-blue-600 transition">
            Личный кабинет
            </Link>
          </div>
        ) : (
          // Если пользователь не авторизован
          <Link
            href="/auth"
            className="px-6 py-3 bg-green-500 text-white rounded-lg shadow hover:bg-green-600 transition">
            Войти/Зарегистрироваться
          </Link>
        )}
      </div>
    </div>
  );
};

export default HomePage;