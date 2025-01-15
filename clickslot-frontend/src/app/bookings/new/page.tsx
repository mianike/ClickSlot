'use client';

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { useSearchParams } from "next/navigation";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import axiosInstance from "../../api/axiosInstance";
import "../../../styles/calendarStyles.css";

interface Schedule {
  id: number;
  date: string;
  startTime: string;
  endTime: string;
}

interface Master {
  id: number;
  name: string;
}

interface Offering {
  id: number;
  name: string;
  price: string;
  duration: string;
}

export default function NewBookingPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const masterId = searchParams.get("masterId");
  const offeringId = searchParams.get("offeringId");

  const [schedules, setSchedules] = useState<Schedule[]>([]);
  const [availableSlots, setAvailableSlots] = useState<Date[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [selectedSlot, setSelectedSlot] = useState<Date | null>(null);
  const [loading, setLoading] = useState(true);
  const [master, setMaster] = useState<Master | null>(null);
  const [offering, setOffering] = useState<Offering | null>(null);

  useEffect(() => {
    if (masterId && offeringId) {
      const fetchMasterAndOffering = async () => {
        try {
          const [masterResponse, offeringResponse] = await Promise.all([
            axiosInstance.get<Master>(`/masters/${masterId}`),
            axiosInstance.get<Offering>(`/offerings/${offeringId}`),
          ]);
          setMaster(masterResponse.data);
          setOffering(offeringResponse.data);
        } catch (error) {
          console.error("Ошибка при загрузке данных мастера или услуги", error);
          alert("Не удалось загрузить данные. Попробуйте позже.");
        } finally {
          setLoading(false);
        }
      };
      fetchMasterAndOffering();
    }
  }, [masterId, offeringId]);

  const fetchSchedules = async () => {
    if (!masterId) return;

    setLoading(true);
    try {
      const response = await axiosInstance.get<Schedule[]>(
        `/schedules/master/${masterId}`
      );
      setSchedules(response.data);
    } catch (error) {
      console.error("Ошибка при загрузке расписания:", error);
      alert("Не удалось загрузить расписание. Попробуйте обновить страницу.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (masterId) {
      fetchSchedules();
    }
  }, [masterId]);

  const fetchAvailableSlots = async (date: Date) => {
    if (!masterId || !offeringId) return;

    setLoading(true);
    try {
      const formattedDate = date.toLocaleDateString("en-CA");
      const response = await axiosInstance.get<string[]>(
        `/masters/${masterId}/offerings/${offeringId}/slots?date=${formattedDate}`
      );
      const slots = response.data.map((slot) => new Date(slot));
      setAvailableSlots(slots);
    } catch (error) {
      console.error("Ошибка при загрузке слотов:", error);
      alert("Не удалось загрузить доступные слоты. Попробуйте снова.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (selectedDate) {
      fetchAvailableSlots(selectedDate);
    }
  }, [selectedDate]);

  const handleDateChange = (value: any) => {
    if (value instanceof Date) {
      setSelectedDate(value);
      setSelectedSlot(null);
    } else if (Array.isArray(value)) {
      setSelectedDate(value[0] || null);
      setSelectedSlot(null);
    } else {
      setSelectedDate(null);
      setSelectedSlot(null);
    }
  };

  const handleBooking = async () => {
    if (!selectedDate || !master || !offering || !selectedSlot) return;
  
    try {
      const startDateTime = new Date(selectedDate);
      startDateTime.setHours(selectedSlot.getHours(), selectedSlot.getMinutes());
  
      const endDateTime = new Date(startDateTime);
      endDateTime.setMinutes(
        startDateTime.getMinutes() + (parseInt(offering.duration.split(":")[0]) * 60 + parseInt(offering.duration.split(":")[1]))
      );
      
      const newBooking = {
        masterId: master.id,
        offeringId: offering.id,
        startTime: startDateTime.toISOString(), // Передаем ISO-строку
        endTime: endDateTime.toISOString(),    // Передаем ISO-строку
      };
  
      await axiosInstance.post("/bookings", newBooking);
      alert("Запись на услугу прошла успешно!");
      router.push("/");
    } catch (error) {
      console.error("Ошибка при бронировании:", error);
      alert("Не удалось забронировать услугу. Попробуйте снова.");
    }
  };
  
 
  const isWorkDay = (date: Date) => {
    const formattedDate = date.toLocaleDateString("en-CA");
    return schedules.some((schedule) => schedule.date === formattedDate);
  };

  if (loading) {
    return <p>Загрузка...</p>;
  }

  return (
    <div className="flex flex-col items-center p-4 min-h-screen bg-gray-100">
      <h1 className="text-3xl font-bold mb-6 text-center">Запись на услугу</h1>

      {master && offering && (
        <>
          <div className="text-center mb-6">
            <h2 className="text-2xl font-semibold">{master.name}</h2>
            <h2 className="text-xl font-semibold">{offering.name}</h2>
            <h3>Стоимость: {offering.price} руб.</h3>
            <h3>Продолжительность {offering.duration} </h3>
          </div>

          <div className="mb-6">
          <Calendar
  onChange={handleDateChange}
  value={selectedDate}
  tileClassName={({ date }) => {
    const formattedDate = date.toLocaleDateString("en-CA");
    const isToday = formattedDate === new Date().toLocaleDateString("en-CA");
    const isWork = isWorkDay(date);
    const isSelected =
      selectedDate &&
      formattedDate === selectedDate.toLocaleDateString("en-CA");

    if (isSelected) {
      return "selected-date";
    }
    if (isToday) {
      return "bg-gray-300 text-black font-bold";
    }
    if (isWork) {
      return "workday";
    }
    return "";
  }}
  className="rounded-lg shadow-md"
/>
          </div>

          {selectedDate && (
            <div className="flex flex-col gap-6 items-center">
              <h2 className="text-xl font-bold mt-4">Доступные слоты</h2>
              <div className="grid grid-cols-8 gap-4">
                {availableSlots.length > 0 ? (
                  availableSlots.map((slot) => (
                    <div
                      key={slot.getTime()}
                      onClick={() => setSelectedSlot(slot)}
                      className={`p-4 rounded-lg shadow-md cursor-pointer ${
                        selectedSlot?.getTime() === slot.getTime()
                          ? "bg-blue-500 text-white"
                          : "bg-gray-200"
                      }`}
                    >
                      <p>
                      {`${slot.toLocaleTimeString([], {
                        hour: "2-digit",
                        minute: "2-digit",
                      })} - ${new Date(
                        slot.getTime() + (parseInt(offering.duration.split(":")[0]) * 60 + parseInt(offering.duration.split(":")[1])) * 60000 // Преобразование минут в миллисекунды
                        ).toLocaleTimeString([], {
                          hour: "2-digit",
                          minute: "2-digit",
                        })}`}
                      </p>
                    </div>
                  ))
                ) : (
                  <p></p>
                )}
              </div>

              {selectedSlot && (
                <button
                  onClick={handleBooking}
                  className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 shadow-md"
                >
                  Забронировать
                </button>
              )}
            </div>
          )}
        </>
      )}
    </div>
  );
}
