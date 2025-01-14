'use client'; // Обязательно добавьте это

import React, { useEffect, useState } from 'react';
import axiosInstance from '../../api/axiosInstance';
import Link from 'next/link';
import * as Yup from 'yup';
import { useFormik } from 'formik';
import { useRouter } from 'next/navigation';

interface UpdatedResponse {
  token: string;
  user: {
    id: number;
    name: string;
    email: string;
  };
}

interface User {
  id: number;
  name: string;
  email: string;
  phone: string;
  address: string;
  role: number;
}

const AccountPage: React.FC = () => {
  const router = useRouter();
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(true); // Состояние загрузки
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      // Получаем данные текущего пользователя и его роль
      axiosInstance.get<User>('/auth/me')
        .then(response => {
          setUser(response.data); // Заполняем данные пользователя
          setLoading(false); // Загрузка завершена
        })
        .catch(error => {
          console.error('Ошибка при получении данных пользователя:', error);
          localStorage.removeItem('token'); // Если токен недействителен
          setError('Ошибка при загрузке данных пользователя.');
          setLoading(false); // Загрузка завершена с ошибкой
        });
    } else {
      setLoading(false); // Если токен не найден, сразу завершить загрузку
    }
  }, []);

  // Валидация формы с помощью Yup
  const validationSchema = Yup.object({
    name: Yup.string().required('Имя обязательно'),
    phone: Yup.string().matches(/^\+?\d{10,14}$/, 'Неверный формат номера телефона').required('Телефон обязателен'),
    address: Yup.string(),
  });

  // Инициализация формы с помощью Formik
  const formik = useFormik({
    initialValues: {
      name: user?.name || '',
      phone: user?.phone || '',
      address: user?.address || '',
    },
    validationSchema,
    onSubmit: (values) => {
      if (user) {
        const updatedUser = { ...values, id: user.id, role: user.role, email: user.email };

        axiosInstance.put<UpdatedResponse>('/auth/update', updatedUser)
          .then(response => {
            const newToken = response.data.token; // Получаем новый токен из ответа
            localStorage.setItem('token', newToken); // Обновляем токен в localStorage
            alert('Профиль обновлён');
            setUser({ ...updatedUser, email: user.email }); // Обновляем состояние пользователя с ролью и email
            router.push('/');
            console.log('User updated successfully', response.data);
          })
          .catch(error => {
            console.error('Ошибка при обновлении профиля:', error);
            alert('Ошибка при обновлении профиля');
          });
      }
    },
  });

  // Подставляем данные пользователя в форму после загрузки
  useEffect(() => {
    if (user) {
      // Сравниваем текущие значения формы с данными пользователя, чтобы избежать лишних вызовов
      if (
        formik.values.name !== user.name ||
        formik.values.phone !== user.phone ||
        formik.values.address !== user.address
      ) {
        formik.setValues({
          name: user.name,
          phone: user.phone,
          address: user.address,
        });
      }
    }
  }, [user]); // Указываем только `user` в зависимостях

  if (loading) {
    return <p>Загрузка...</p>;
  }

  if (error) {
    return <p className="text-red-500">{error}</p>;
  }

  const handleDeleteAccount = () => {
    if (user) {
        axiosInstance.delete(`/auth/${user.id}`)
            .then(() => {
                localStorage.removeItem('token');
                alert('Аккаунт удален');
                router.push('/');
            })
            .catch(error => {
                console.error('Ошибка при удалении аккаунта:', error);
                alert('Ошибка при удалении аккаунта');
            });
    }
};

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50">
      <h1 className="text-4xl font-bold text-gray-800 mb-6">Личные данные</h1>

      {user ? (
        <form onSubmit={formik.handleSubmit} className="w-full max-w-lg bg-white p-6 rounded-lg shadow-md">
          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700">Имя</label>
            <input
              type="text"
              name="name"
              value={formik.values.name}
              onChange={formik.handleChange}
              className="w-full p-2 border border-gray-300 rounded-md"
            />
            {formik.errors.name && formik.touched.name && (
              <div className="text-red-500 text-xs">{formik.errors.name}</div>
            )}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700">Телефон</label>
            <input
              type="text"
              name="phone"
              value={formik.values.phone}
              onChange={formik.handleChange}
              className="w-full p-2 border border-gray-300 rounded-md"
            />
            {formik.errors.phone && formik.touched.phone && (
              <div className="text-red-500 text-xs">{formik.errors.phone}</div>
            )}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700">Адрес</label>
            <input
              type="text"
              name="address"
              value={formik.values.address}
              onChange={formik.handleChange}
              className="w-full p-2 border border-gray-300 rounded-md"
            />
            {formik.errors.address && formik.touched.address && (
              <div className="text-red-500 text-xs">{formik.errors.address}</div>
            )}
          </div>

          <div className="flex justify-between mt-6">
            <button
              type="submit"
              className="px-6 py-3 bg-blue-500 text-white rounded-lg shadow hover:bg-blue-600 transition"
            >
              Обновить
            </button>
            <button
                    type="button"
                    className="px-6 py-3 bg-red-500 text-white rounded-lg shadow hover:bg-red-600 transition"
                    onClick={handleDeleteAccount} // Добавляем обработчик на кнопку удаления
                >
                    Удалить аккаунт
                </button>
            <button
              type="button"
              className="px-8 py-3 bg-blue-500 text-white rounded-lg shadow hover:bg-blue-600 transition"
              onClick={() => {
                // Логика выхода
                localStorage.removeItem('token');
                window.location.href = '/';
              }}
            >
              Выход
            </button>
          </div>

          {/* Дополнительные кнопки для мастера */}
          {user.role === 1 && (
            <div className="mt-4 flex justify-between">
              <Link href="/schedule" className="px-3 py-3 bg-green-500 text-white rounded-lg shadow hover:bg-green-600 transition">
                Моё расписание
              </Link>
              <Link href="/bookings" className="px-6 py-3 bg-green-500 text-white rounded-lg shadow hover:bg-green-600 transition">
                Мои записи
              </Link>
              <Link href="/offerings" className="px-6 py-3 bg-green-500 text-white rounded-lg shadow hover:bg-green-600 transition">
                Мои услуги
              </Link>
            </div>
          )}
        </form>
      ) : (
        <p>Пользователь не найден</p>
      )}
    </div>
  );
};

export default AccountPage;