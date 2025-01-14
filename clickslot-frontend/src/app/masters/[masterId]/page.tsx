'use client';

import { useState, useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation'; // Для получения параметров маршрута
import axiosInstance from '../../api/axiosInstance'; // Путь к вашему axiosInstance

interface Offering {
  id: number;
  name: string;
  price: number;
  duration: string;
}

interface Master {
  id: number;
  name: string;
  email: string;
  phone: string;
  address: string;
  offerings: Offering[];
}

export default function MasterDetailPage() {
  const [master, setMaster] = useState<Master | null>(null);
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const pathname = usePathname(); // Получаем текущий путь, чтобы извлечь параметр
  const masterId = pathname.split('/').pop(); // Извлекаем masterId из URL

  useEffect(() => {
    const fetchMaster = async () => {
      if (masterId) {
        setLoading(true);
        try {
          const response = await axiosInstance.get<Master>(`/masters/${masterId}`);
          setMaster(response.data); // Устанавливаем данные мастера
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

  // Обработчик перехода на страницу бронирования
  const handleBooking = (offeringId: number) => {
    router.push(`/bookings/new?masterId=${masterId}&offeringId=${offeringId}`);
  };

  return (
    <div className="flex flex-col items-center justify-center p-4">
      <div className="text-center">
        <h1 className="text-3xl font-bold">{master.name}</h1>
        <p>{master.email}</p>
        <p>{master.phone}</p>
        <p>{master.address}</p>
      </div>

      <h2 className="text-xl font-semibold mt-6">Услуги</h2>
      <div className="flex flex-col gap-4 items-center mt-4 w-full max-w-lg">
        {master.offerings.length > 0 ? (
          master.offerings.map((offering) => (
            <div
              key={offering.id}
              className="bg-white border border-gray-300 rounded-lg shadow-md p-4 w-full"
            >
              <h3 className="text-lg font-semibold">{offering.name}</h3>
              <p>{offering.price} руб.</p>
              <p>{offering.duration}</p>
              <button
                onClick={() => handleBooking(offering.id)} // Переход на страницу записи
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
    </div>
  );
}