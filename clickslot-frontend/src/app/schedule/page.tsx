'use client';

import { useState, useEffect } from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css"; // Стили календаря
import axiosInstance from "../api/axiosInstance";
import { useRouter } from 'next/navigation';
import "../../styles/calendarStyles.css";

interface Schedule {
  id: number;
  date: string; // ISO формат даты, например "2025-01-13"
  startTime: string; // "09:00:00"
  endTime: string; // "18:00:00"
}

interface User {
  id: number;
  name: string;
  email: string;
}

export default function SchedulePage() {
  const router = useRouter();
  const [schedules, setSchedules] = useState<Schedule[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState<User | null>(null);
  const [startTime, setStartTime] = useState("09:00");
  const [endTime, setEndTime] = useState("18:00");

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

  const fetchSchedules = async () => {
    if (!user) return;

    setLoading(true);
    try {
      const response = await axiosInstance.get<Schedule[]>(`/schedules/master/${user.id}`);
      setSchedules(response.data);
    } catch (error) {
      console.error("Ошибка при загрузке расписания:", error);
      alert("Не удалось загрузить расписание. Попробуйте обновить страницу.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (user) {
      fetchSchedules();
    }
  }, [user]);

  const handleAddSchedule = async () => {
    if (!selectedDate || !user) return alert("Выберите дату!");

    if (startTime >= endTime) {
      return alert("Время начала должно быть меньше времени окончания!");
    }

    try {
      const newSchedule: Schedule = {
        id: 0,
        date: selectedDate.toLocaleDateString('en-CA'), // Используем формат 'YYYY-MM-DD'
        startTime: `${startTime}:00`,
        endTime: `${endTime}:00`,
      };

      const response = await axiosInstance.post<Schedule>("/schedules", newSchedule);
      setSchedules((prev) => [...prev, response.data]);
      alert("Рабочий день добавлен успешно!");
    } catch (error: any) {
      console.error("Ошибка при добавлении рабочего дня:", error);
      alert(
        error.response?.data?.message ||
          "Не удалось добавить рабочий день. Попробуйте снова."
      );
    }
  };

  const handleDeleteSchedule = async (id: number) => {
    try {
      await axiosInstance.delete(`/schedules/${id}`);
      setSchedules((prev) => prev.filter((schedule) => schedule.id !== id));
      alert("Рабочий день удалён успешно!");
      setSelectedDate(null);
    } catch (error: any) {
      console.error("Ошибка при удалении рабочего дня:", error);
      alert(
        error.response?.data?.message ||
          "Не удалось удалить рабочий день. Попробуйте снова."
      );
    }
  };

  const handleUpdateSchedule = async (id: number) => {
    if (!selectedDate || !user) return alert("Выберите дату!");

    if (startTime >= endTime) {
      return alert("Время начала должно быть меньше времени окончания!");
    }

    try {
      const updatedSchedule = {
        id,
        date: selectedDate.toLocaleDateString("en-CA"),
        startTime: `${startTime}:00`,
        endTime: `${endTime}:00`,
      };

      const response = await axiosInstance.put<Schedule>(
        `/schedules/${id}`,
        updatedSchedule
      );

      setSchedules((prev) =>
        prev.map((schedule) =>
          schedule.id === id ? { ...schedule, ...response.data } : schedule
        )
      );

      alert("Рабочий день обновлён успешно!");
    } catch (error: any) {
      console.error("Ошибка при обновлении рабочего дня:", error);
      alert(
        error.response?.data?.message ||
          "Не удалось обновить рабочий день. Попробуйте снова."
      );
    }
  };

  const isWorkDay = (date: Date) => {
    const formattedDate = date.toLocaleDateString('en-CA'); // Форматируем дату
    return schedules.some((schedule) => schedule.date === formattedDate);
  };

  const handleDateChange = (value: any) => {
    if (value instanceof Date) {
      setSelectedDate(value);
      const schedule = schedules.find(
        (s) => s.date === value.toLocaleDateString("en-CA")
      );
      if (schedule) {
        setStartTime(schedule.startTime.slice(0, 5));
        setEndTime(schedule.endTime.slice(0, 5));
      } else {
        setStartTime("09:00");
        setEndTime("18:00");
      }
    } else if (Array.isArray(value)) {
      setSelectedDate(value[0] || null);
    } else {
      setSelectedDate(null);
    }
  };

  if (loading) {
    return <p>Загрузка...</p>;
  }

  return (
    <div className="flex flex-col items-center p-4 min-h-screen bg-gray-100">
      <h1 className="text-3xl font-bold mb-6 text-center">Моё расписание</h1>

      <div className="mb-6">
        <Calendar
          onChange={handleDateChange}
          value={selectedDate}
          tileClassName={({ date }) => {
            const formattedDate = date.toLocaleDateString('en-CA');
  const isToday = formattedDate === new Date().toLocaleDateString('en-CA');
  const isWork = isWorkDay(date);

  if (isToday) {
    return "bg-gray-300 text-black font-bold"; // Сегодняшний день
  }
  if (isWork) {
    return "workday"; // Добавляем класс для рабочих дней
  }
  return "";
}}
          className="rounded-lg shadow-md"
        />
      </div>

      {selectedDate && (
      <div className="flex flex-col gap-6 items-center">
        <div className="flex gap-4 items-center">
          <label className="font-semibold">Время начала:</label>
          <input
            type="time"
            value={startTime}
            onChange={(e) => setStartTime(e.target.value)}
            className="border border-gray-300 rounded px-2 py-1 shadow-sm"
          />
          <label className="font-semibold">Время окончания:</label>
          <input
            type="time"
            value={endTime}
            onChange={(e) => setEndTime(e.target.value)}
            className="border border-gray-300 rounded px-2 py-1 shadow-sm"
          />
          {selectedDate && !isWorkDay(selectedDate) && (
              <button
                  onClick={handleAddSchedule}
                      className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600 shadow-md"
                        >
                              Добавить рабочий день
                                </button>
          )}
        </div>

        {selectedDate && (
          <div className="text-center">
            <h2 className="text-xl font-bold mt-4">
              Выбранная дата: {selectedDate.toLocaleDateString()}
            </h2>
            {isWorkDay(selectedDate) ? (
              <div className="mt-4">
                <p className="text-green-700 font-semibold">
                  Это рабочий день!
                </p>
                <div className="flex gap-4 justify-center mt-4">
                  <button
                    onClick={() => {
                      const schedule = schedules.find(
                        (s) => s.date === selectedDate.toLocaleDateString("en-CA")
                      );
                      if (schedule) handleUpdateSchedule(schedule.id);
                    }}
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 shadow-md"
                  >
                    Обновить
                  </button>
                  <button
                    onClick={() => {
                      const schedule = schedules.find(
                        (s) => s.date === selectedDate.toLocaleDateString("en-CA")
                      );
                      if (schedule) handleDeleteSchedule(schedule.id);
                    }}
                    className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 shadow-md"
                  >
                    Удалить рабочий день
                  </button>
                </div>
              </div>
            ) : (
              <p className="text-gray-500 mt-4">Этот день не рабочий.</p>
            )}
          </div>
        )}
      </div>)}
    </div>
  );
}