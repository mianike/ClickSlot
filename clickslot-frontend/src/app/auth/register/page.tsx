'use client'; 

import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import axiosInstance from '../../api/axiosInstance'; // Ваш axiosInstance
import { FaUserPlus } from 'react-icons/fa';
import { useRouter } from 'next/router';

interface RegisterResponse {
  token: string;
  user: {
    id: number;
    name: string;
    email: string;
  };
}

const schema = yup.object().shape({
  name: yup.string().required('Name is required'),
  email: yup.string().email('Invalid email').required('Email is required'),
  phone: yup.string().required('Phone is required'),
  address: yup.string(),
  password: yup.string().min(6, 'Password must be at least 6 characters').required('Password is required'),
  role: yup.number().required('Role is required'),
});

interface RegisterFormData {
  name: string;
  email: string;
  phone: string;
  address?: string;
  password: string;
  role: number; // 0 - "Client", 1 - "Master"
}

const RegisterPage: React.FC = () => {
  const router = useRouter();
  const { register, handleSubmit, formState: { errors }, setError } = useForm<RegisterFormData>({
    resolver: yupResolver(schema),
  });

  const [serverErrors, setServerErrors] = useState<any>({});

  const onSubmit = async (data: RegisterFormData) => {
    try {
      const response = await axiosInstance.post<RegisterResponse>('/auth/register', data);
      const { token, user } = response.data;
      localStorage.setItem('token', token);  // Сохраняем токен

      // Перенаправление после успешной регистрации
      router.push('/'); // или на главную страницу
      console.log('User registered successfully', response.data);
    } catch (error: any) {
      if (error.response && error.response.data.errors) {
        const validationErrors = error.response.data.errors;
        for (const field in validationErrors) {
          if (validationErrors.hasOwnProperty(field)) {
            setError(field as keyof RegisterFormData, {
              type: 'manual',
              message: validationErrors[field][0],
            });
          }
        }
        setServerErrors(validationErrors); // Записываем ошибки на сервере
      } else {
        setServerErrors({});
        console.error('Unknown error:', error);
      }
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50">
      <h1 className="text-3xl font-bold text-gray-800 mb-8">Регистрация</h1>
      <form onSubmit={handleSubmit(onSubmit)} className="w-full max-w-md space-y-4">
        <div>
          <label htmlFor="name" className="block text-sm font-medium text-gray-700">Имя</label>
          <input
            type="text"
            id="name"
            {...register('name')}
            className="mt-1 p-2 w-full border border-gray-300 rounded"
          />
          {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
        </div>

        <div>
          <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
          <input
            type="email"
            id="email"
            {...register('email')}
            className="mt-1 p-2 w-full border border-gray-300 rounded"
          />
          {errors.email && <p className="text-red-500 text-sm">{errors.email.message}</p>}
        </div>

        <div>
          <label htmlFor="phone" className="block text-sm font-medium text-gray-700">Телефон</label>
          <input
            type="text"
            id="phone"
            {...register('phone')}
            className="mt-1 p-2 w-full border border-gray-300 rounded"
          />
          {errors.phone && <p className="text-red-500 text-sm">{errors.phone.message}</p>}
        </div>

        <div>
          <label htmlFor="password" className="block text-sm font-medium text-gray-700">Пароль</label>
          <input
            type="password"
            id="password"
            {...register('password')}
            className="mt-1 p-2 w-full border border-gray-300 rounded"
          />
          {errors.password && <p className="text-red-500 text-sm">{errors.password.message}</p>}
        </div>

        <div>
          <label htmlFor="role" className="block text-sm font-medium text-gray-700">Роль</label>
          <select
            id="role"
            {...register('role')}
            className="mt-1 p-2 w-full border border-gray-300 rounded"
          >
            <option value="0">Клиент</option>
            <option value="1">Мастер</option>
          </select>
          {errors.role && <p className="text-red-500 text-sm">{errors.role.message}</p>}
        </div>

        <button
          type="submit"
          className="mt-4 w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600"
        >
          Зарегистрироваться
        </button>
      </form>
    </div>
  );
};

export default RegisterPage;
