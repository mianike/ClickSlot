'use client';

import { useState, useEffect } from "react";
import axiosInstance from "../api/axiosInstance";
import { useRouter } from "next/navigation";
import "../../styles/calendarStyles.css";

interface Booking {
  id: number;
  clientId: number;
  clientName: string;
  clientPhone: string;
  masterId: number;
  offeringId: number;
  offeringName: string;
  startTime: string;
  endTime: string;
}

interface User {
  id: number;
  name: string;
  email: string;
}

export default function MasterBookingsPage() {
  const router = useRouter();
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState<User | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10;

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      axiosInstance
        .get<User>("/auth/me")
        .then((response) => {
          setUser(response.data);
          setLoading(false);
        })
        .catch((error) => {
          console.error("Ошибка при получении данных пользователя:", error);
          localStorage.removeItem("token");
          setLoading(false);
          alert("Не удалось загрузить данные пользователя. Попробуйте снова.");
        });
    } else {
      setLoading(false);
    }
  }, []);

  const fetchBookings = async (page: number) => {
    if (!user) return;

    setLoading(true);
    try {
      const response = await axiosInstance.get<Booking[]>(`/bookings/master/${user.id}?page=${page}&pageSize=${pageSize}`
      );
      setBookings(response.data);
    } catch (error) {
      console.error("Ошибка при загрузке записей:", error);
      alert("Не удалось загрузить записи. Попробуйте обновить страницу.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (user) {
      fetchBookings(currentPage);
    }
  }, [user, currentPage]);

  const groupBookingsByDate = (bookings: Booking[]) => {
    const groupedBookings: { [key: string]: Booking[] } = {};

    bookings.forEach((booking) => {
      const date = new Date(booking.startTime).toLocaleDateString("en-CA");
      if (!groupedBookings[date]) {
        groupedBookings[date] = [];
      }
      groupedBookings[date].push(booking);
    });

    return groupedBookings;
  };

  const handleDeleteBooking = async (id: number) => {
    try {
      await axiosInstance.delete(`/bookings/${id}`);
      setBookings((prev) => prev.filter((booking) => booking.id !== id));
      alert("Запись удалена успешно!");
    } catch (error: any) {
      console.error("Ошибка при удалении записи:", error);
      alert(
        error.response?.data?.message ||
          "Не удалось удалить запись. Попробуйте снова."
      );
    }
  };

  if (loading) {
    return <p>Загрузка...</p>;
  }

  const groupedBookings = groupBookingsByDate(bookings);

  return (
    <div className="flex flex-col items-center p-20 min-h-screen bg-gray-100">
      <h1 className="text-3xl font-bold mb-6 text-center">Записи мастера</h1>

      {Object.keys(groupedBookings).length === 0 ? (
        <p>Нет записей для отображения.</p>
      ) : (
        Object.keys(groupedBookings).map((date) => (
          <div key={date} className="mb-8 w-full">
            <h2 className="text-2xl font-bold mb-4">
              {new Date(date).toLocaleDateString()}
            </h2>

            <div className="space-y-4">
              {groupedBookings[date].map((booking) => (
                <div
                  key={booking.id}
                  className="border p-4  bg-white rounded shadow-md"
                >
                  <p>
                    <strong>Клиент:</strong> {booking.clientName}  {booking.clientPhone}
                  </p>
                  <p>
                    <strong>Услуга:</strong> {booking.offeringName}
                  </p>
                  <p>
                    <strong>Время:</strong> {new Date(booking.startTime).toLocaleTimeString()} -{" "}
                    {new Date(booking.endTime).toLocaleTimeString()}
                  </p>
                  <div className="flex gap-4 mt-2">
                    
                    <button
                      onClick={() => handleDeleteBooking(booking.id)}
                      className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
                    >
                      Удалить
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        ))
      )}

      {/* Пагинация */}
      <div className="flex gap-4 mt-8">
        <button
          disabled={currentPage === 1}
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
            bookings.length === pageSize ? "bg-blue-500 text-white hover:bg-blue-600" : "bg-gray-400 cursor-not-allowed"
            }`}
          disabled={bookings.length < pageSize || loading}
        >
          Следующая
        </button>
      </div>
    </div>
  );
}