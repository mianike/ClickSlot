'use client';

import React, { useState } from 'react';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import axiosInstance from '../../api/axiosInstance';
import { useRouter } from 'next/navigation';

const validationSchema = Yup.object({
  name: Yup.string().required('Имя услуги обязательно'),
  price: Yup.number().required('Цена обязательна').min(0, 'Цена не может быть отрицательной'),
  duration: Yup.number().required('Длительность обязательна').min(1, 'Минимум 1 минута').max(1000),
});

const NewOfferingPage: React.FC = () => {
  const router = useRouter();
  const [error, setError] = useState<string | null>(null);

  const convertMinutesToTime = (minutes: number): string => {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    const seconds = 0; // Допустим, мы не учитываем секунды в этом случае
    return `${String(hours).padStart(2, '0')}:${String(mins).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
  };

  const formik = useFormik({
    initialValues: {
      name: '',
      price: 0,
      duration: 0,
    },
    validationSchema,
    onSubmit: async (values) => {
      try {
        const formattedDuration = convertMinutesToTime(values.duration);
        
        await axiosInstance.post('/offerings', {
          ...values,
          duration: formattedDuration,
        });
        
        router.push('/offerings');
      } catch (err) {
        setError('Ошибка при создании услуги.');
      }
    },
  });

  return (
    <div className="max-w-4xl mx-auto p-6 mt-6 mb-6 bg-white rounded-lg shadow-lg">
      <h1 className="text-3xl font-semibold text-center mb-6">Добавить услугу</h1>
      <form onSubmit={formik.handleSubmit}>
        <div className="mb-4">
          <label htmlFor="name" className="block text-sm font-medium text-gray-700">Услуга</label>
          <input
            id="name"
            type="text"
            {...formik.getFieldProps('name')}
            className="mt-2 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
          />
          {formik.touched.name && formik.errors.name && (
            <div className="text-red-500 text-xs mt-1">{formik.errors.name}</div>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="price" className="block text-sm font-medium text-gray-700">Цена, руб</label>
          <input
            id="price"
            type="number"
            {...formik.getFieldProps('price')}
            className="mt-2 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
          />
          {formik.touched.price && formik.errors.price && (
            <div className="text-red-500 text-xs mt-1">{formik.errors.price}</div>
          )}
        </div>

        <div className="mb-4">
          <label htmlFor="duration" className="block text-sm font-medium text-gray-700">Продолжительность (мин)</label>
          <input
            id="duration"
            type="number"
            {...formik.getFieldProps('duration')}
            className="mt-2 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
          />
          {formik.touched.duration && formik.errors.duration && (
            <div className="text-red-500 text-xs mt-1">{formik.errors.duration}</div>
          )}
        </div>

        {error && <div className="text-red-500 text-center mb-4">{error}</div>}

        <button type="submit" className="w-full bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 transition duration-300 mt-6">
          Создать услугу
        </button>
      </form>
    </div>
  );
};

export default NewOfferingPage;