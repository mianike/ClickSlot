"use client";

import { useState } from "react";
import axiosInstance from "../../api/axiosInstance"; // Путь к вашему axiosInstance

interface LoginForm {
  email: string;
  password: string;
}

interface LoginResponse {
    token: string;
  }

const LoginPage = () => {
  const [form, setForm] = useState<LoginForm>({ email: "", password: "" });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccessMessage(null);
    

    try {
        const response = await axiosInstance.post<LoginResponse>("/auth/login", form);
        const { token } = response.data; // Теперь TypeScript знает, что тут есть token
        localStorage.setItem("token", token); // Сохранение токена в localStorage
        setSuccessMessage("Вы успешно вошли!");
      } catch (err: any) {
        setError("Неверный логин или пароль.");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

  return (
    <div className="max-w-lg mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Вход</h1>
      {successMessage && <p className="text-green-500">{successMessage}</p>}
      {error && <p className="text-red-500">{error}</p>}
      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          type="email"
          name="email"
          value={form.email}
          onChange={handleChange}
          placeholder="Email"
          className="w-full p-2 border border-gray-300 rounded"
          required
        />
        <input
          type="password"
          name="password"
          value={form.password}
          onChange={handleChange}
          placeholder="Пароль"
          className="w-full p-2 border border-gray-300 rounded"
          required
        />
        <button
          type="submit"
          disabled={loading}
          className="w-full p-2 bg-blue-500 text-white rounded hover:bg-blue-600"
        >
          {loading ? "Загрузка..." : "Войти"}
        </button>
      </form>
    </div>
  );
};

export default LoginPage;