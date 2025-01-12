'use client';

import React from 'react';
import Link from 'next/link';
import Image from 'next/image'; // Для использования изображения

const Header: React.FC = () => {
  return (
    <header className="bg-gray-100 shadow-md text-gray-900 font-sans py-4">
      <div className="max-w-7xl mx-auto flex items-center justify-between">
        {/* Логотип */}
        <div className="flex items-center">
            <Link href="/" className="ml-3 text-2xl font-bold flex items-center">
                <Image
                    src="/logo.svg" // Путь к вашему логотипу
                    alt="Logo"
                    width={40} // Ширина логотипа
                    height={40} // Высота логотипа
                    className="mr-2" // Отступ между логотипом и текстом
                />
                <span>ClickSlot</span>
            </Link>
      </div>

        {/* Навигация */}
        
        {/* <nav>
          <Link href="/masters" className="px-4 text-lg hover:text-gray-900 font-sans">
            Мастера
          </Link>
          <Link href="/auth" className="px-4 text-lg hover:text-gray-900 font-sans">
            Войти/Зарегистрироваться
          </Link>
        </nav> */}
        
      </div>
    </header>
  );
};

export default Header;