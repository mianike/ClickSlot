'use client';

import React, { useEffect, useState } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import axiosInstance from '../../../api/axiosInstance';
import { useFormik } from 'formik';
import * as Yup from 'yup';

interface OfferingData {
  id: number;
  name: string;
  price: number;
  duration: string; // Длительность будет в формате "hh:mm:ss"
}

const validationSchema = Yup.object({
  name: Yup.string().required('Имя услуги обязательно'),
  price: Yup.number().required('Цена обязательна').min(0, 'Цена не может быть отрицательной'),
  duration: Yup.number().required('Продолжительность обязательна').min(1, 'Минимум 1 минута').max(1000),
});

const EditOfferingPage: React.FC = () => {
  const pathname = usePathname();
  const offeringId = pathname.split('/').pop();
  const [offeringData, setOfferingData] = useState<OfferingData | null>(null);
  const [error, setError] = useState<string | null>(null);
  const router = useRouter();

  const convertTimeToMinutes = (time: string): number => {
    const [hours, minutes, seconds] = time.split(':').map(Number);
    return hours * 60 + minutes + seconds / 60;
  };

  useEffect(() => {
    const fetchOffering = async () => {
      if (offeringId) {
        try {
          const response = await axiosInstance.get<OfferingData>(`/offerings/${offeringId}`);
          setOfferingData(response.data);
        } catch (err) {
          setError('Ошибка при загрузке данных услуги.');
        }
      }
    };

    fetchOffering();
  }, [offeringId]);

  const formik = useFormik({
    initialValues: {
      name: offeringData?.name || '',
      price: offeringData?.price || 0,
      duration: offeringData ? convertTimeToMinutes(offeringData.duration) : 0,
    },
    validationSchema,
    enableReinitialize: true,
    onSubmit: async (values) => {
      try {
        const hours = Math.floor(values.duration / 60);
        const minutes = values.duration % 60;
        const formattedDuration = `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:00`;

        await axiosInstance.put(`/offerings/${offeringId}`, {
          ...values,
          duration: formattedDuration,
        });

        router.push('/offerings');
      } catch (err) {
        setError('Ошибка при обновлении услуги.');
      }
    },
  });

  if (!offeringData) return <p>Загрузка...</p>;
  if (error) return <p className="text-red-500">{error}</p>;

  return (
    <div className="max-w-4xl mx-auto p-6 mt-6 mb-6 bg-white rounded-lg shadow-lg">
      <h1 className="text-3xl font-semibold text-center mb-6">Редактировать услугу</h1>
      <form onSubmit={formik.handleSubmit}>
        {/* Кнопка назад */}
        <div className="w-full text-left mb-2">
          <button
            type="button"
            onClick={() => router.back()}
            className="text-sm text-gray-600 hover:text-gray-800 flex items-center">
            <span className="mr-1">&larr;</span> 
            Назад
          </button>
        </div>
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
          Обновить услугу
        </button>
      </form>
    </div>
  );
};

export default EditOfferingPage;