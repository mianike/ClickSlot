"use client";

import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import axiosInstance from "../../api/axiosInstance";
import { useRouter } from "next/navigation";

interface LoginForm {
  email: string;
  password: string;
}

const schema = yup.object().shape({
  email: yup.string().email("Некорректный email").required("Email обязателен"),
  password: yup.string().min(6, 'Пароль должен быть не короче 6 символов').required('Пароль обязателен'),
});

interface LoginResponse {
  token: string;
}

const LoginPage: React.FC = () => {
  const router = useRouter();
  const {
    register,
    handleSubmit,
    formState: { errors },
    setError,
  } = useForm<LoginForm>({
    resolver: yupResolver(schema),
  });

  const [serverMessage, setServerMessage] = useState<string | null>(null);

  const onSubmit = async (data: LoginForm) => {
    try {
      setServerMessage(null);
      const response = await axiosInstance.post<LoginResponse>("/auth/login", data);
      const { token } = response.data;
      localStorage.setItem("token", token);

      router.push("/");
      console.log("Login successful", response.data);
    } catch (error: any) {
      if (error.response) {
        if (error.response.data.message) {
          setServerMessage(error.response.data.message);
        } else {
          setServerMessage("Произошла ошибка при входе. Проверьте данные.");
        }
      } else {
        setServerMessage("Не удалось соединиться с сервером. Проверьте подключение к интернету.");
        console.error("Unknown error:", error);
      }
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50">
      <h1 className="text-3xl font-bold text-gray-800">Вход</h1>
      <form onSubmit={handleSubmit(onSubmit)} className="w-full max-w-md space-y-4">
        {/* Кнопка назад */}
        <div className="w-full text-left">
          <button
            type="button"
            onClick={() => router.back()}
            className="text-sm text-gray-600 hover:text-gray-800 flex items-center">
            <span className="mr-1">&larr;</span> 
            Назад
          </button>
        </div>
        {/* Общая ошибка */}
        {serverMessage && (
          <p className="text-red-500 text-center text-sm">{serverMessage}</p>
        )}

        {/* Поля формы */}
        <div>
          <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
          <input
            type="email"
            id="email"
            {...register("email")}
            className="mt-1 p-2 w-full border border-gray-300 rounded"
          />
          {errors.email && <p className="text-red-500 text-sm">{errors.email.message}</p>}
        </div>

        <div>
          <label htmlFor="password" className="block text-sm font-medium text-gray-700">Пароль</label>
          <input
            type="password"
            id="password"
            {...register("password")}
            className="mt-1 p-2 w-full border border-gray-300 rounded"
          />
          {errors.password && <p className="text-red-500 text-sm">{errors.password.message}</p>}
        </div>

        <button
          type="submit"
          className="mt-4 w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600"
        >
          Войти
        </button>
      </form>
    </div>
  );
};

export default LoginPage;