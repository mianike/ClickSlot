'use client';

import React from 'react';
import Link from 'next/link';
import { FaUserPlus, FaSignInAlt } from 'react-icons/fa';

const AuthPage: React.FC = () => {
    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-50">
            <h1 className="text-3xl font-bold text-gray-800 mb-8">Добро пожаловать!</h1>
            <p className="text-lg text-gray-600 mb-6">
                Пожалуйста, выберите действие, чтобы продолжить.
            </p>
            <div className="flex space-x-6">
                <Link
                    href="/auth/login"
                    className="px-6 py-3  bg-blue-500 text-white rounded-lg shadow hover:bg-blue-600 transition flex items-center">
                    <FaSignInAlt className="mr-2" /> {/* Иконка для входа */}
                    Войти
                </Link>

                <Link
                    href="/auth/register"
                    className="px-6 py-3 bg-green-500 text-white rounded-lg shadow hover:bg-green-600 transition flex items-center">
                    <FaUserPlus className="mr-2" /> {/* Иконка для регистрации */}
                    Зарегистрироваться
                </Link>
            </div>
        </div>
    );
};

export default AuthPage;