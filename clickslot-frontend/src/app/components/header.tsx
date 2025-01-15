'use client';

import React from 'react';
import Link from 'next/link';
import Image from 'next/image';

const Header: React.FC = () => {
  return (
    <header className="bg-gray-100 shadow-md text-gray-900 font-sans py-4">
      <div className="max-w-7xl mx-auto flex items-center justify-between">
        {/* Логотип */}
        <div className="flex items-center">
            <Link href="/" className="ml-3 text-2xl font-bold flex items-center">
                <Image
                    src="/logo.svg"
                    alt="Logo"
                    width={40}
                    height={40}
                    className="mr-2"
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