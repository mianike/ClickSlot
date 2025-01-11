'use client';

import React from 'react';
import Link from 'next/link';

const HomePage: React.FC = () => {
    return (
        <div style={{ textAlign: 'center', marginTop: '50px' }}>
            <h1>Welcome to ClickSlot!</h1>
            <p>Выберите действие:</p>
            <div style={{ marginTop: '20px' }}>
                <Link href="/masters">
                    <button style={{ marginRight: '10px' }}>Список мастеров</button>
                </Link>
                <Link href="/auth">
                    <button>Войти/Зарегистрироваться</button>
                </Link>
            </div>
        </div>
    );
};

export default HomePage;