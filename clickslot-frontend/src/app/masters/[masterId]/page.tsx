'use client';

import { useState, useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import axiosInstance from '../../api/axiosInstance';

interface Offering {
  id: number;
  name: string;
  price: number;
  duration: string;
}

interface MasterReview {
  id: number;
  clientName: string;
  rating: number;
  comment: string;
}

interface Master {
  id: number;
  name: string;
  email: string;
  phone: string;
  address: string;
  rating: number;
  reviewsCount: number;
  offerings: Offering[];
  masterReviews: MasterReview[];
}

export default function MasterDetailPage() {
  const [master, setMaster] = useState<Master | null>(null);
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const pathname = usePathname();
  const masterId = pathname.split('/').pop();

  useEffect(() => {
    const fetchMaster = async () => {
      if (masterId) {
        setLoading(true);
        try {
          const response = await axiosInstance.get<Master>(`/masters/${masterId}`);
          setMaster(response.data);
        } catch (error: unknown) {
          if (error instanceof Error) {
            console.error('Ошибка при загрузке данных мастера', error.message);
          } else {
            console.error('Неизвестная ошибка');
          }
        } finally {
          setLoading(false);
        }
      }
    };

    fetchMaster();
  }, [masterId]);

  if (loading) return <p className="text-center mt-8">Загрузка...</p>;

  if (!master) return <p className="text-center mt-8">Мастер не найден</p>;

  const handleBooking = (offeringId: number) => {
    router.push(`/bookings/new?masterId=${masterId}&offeringId=${offeringId}`);
  };

  return (
    <div className="flex flex-col items-center justify-center p-4">
      <div className="text-center">
        <h1 className="text-3xl font-bold">{master.name}</h1>
        <h2 className="text-xl font-semibold text-gray-800">{master.email}</h2>
        <h2 className="text-xl font-semibold text-gray-800">{master.phone}</h2>
        <h2 className="text-xl font-semibold text-gray-800">{master.address}</h2>
        <h3 className="mt-4 font-semibold text-wrap text-gray-600">
          Рейтинг: {master.rating} (Отз: {master.reviewsCount})
        </h3>
      </div>

      <h2 className="text-xl font-semibold mt-6">Услуги</h2>
      {/* Кнопка назад */}
      <div className="w-full text-left px-10">
        <button
          type="button"
          onClick={() => router.back()}
          className="text-sm text-gray-600 hover:text-gray-800 flex items-center">
          <span className="mr-1">&larr;</span> 
          Назад
        </button>
      </div>
      <div className="grid gap-6 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 mt-2 px-10 w-full">
        {master.offerings.length > 0 ? (
          master.offerings.map((offering) => (
            <div
              key={offering.id}
              className="bg-white border border-gray-300 rounded-lg shadow-md p-4 w-full"
            >
              <h3 className="text-lg font-semibold">{offering.name}</h3>
              <p>Стоимость: {offering.price} руб.</p>
              <p>Продолжительность: {offering.duration}</p>
              <button
                onClick={() => handleBooking(offering.id)}
                className="mt-4 px-4 py-2 bg-blue-500 text-white rounded"
              >
                Записаться
              </button>
            </div>
          ))
        ) : (
          <p>У мастера нет услуг</p>
        )}
      </div>

      <h2 className="text-xl font-semibold mt-6">Последние 15 отзывов</h2>
      <div className="grid gap-6 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-3 mt-4 px-10 w-full">
        {master.masterReviews.slice(0, 15).map((review) => (
          <div
            key={review.id}
            className="bg-white border border-gray-300 rounded-lg shadow-md p-4 w-full"
          >
            <h3 className="text-lg font-semibold">{review.clientName}</h3>
            <p className="text-2x font-semibold">Оценка: {review.rating}</p>
            <p>{review.comment}</p>
          </div>
        ))}
      </div>

      <div className="mt-10 px-10 w-full flex justify-center">
        <button
          onClick={() => router.push(`/reviews/new?masterId=${masterId}`)}
          className="bg-green-500 text-white px-6 py-3 rounded-md shadow-md hover:bg-green-600 transition"
        >
          Оставить отзыв
        </button>
      </div>
    </div>
  );
}
