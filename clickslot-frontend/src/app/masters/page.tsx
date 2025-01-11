'use client';

import React, { useEffect, useState } from 'react';
import axiosInstance from '../api/axiosInstance';

type Master = {
    id: string;
    name: string;
};

const MastersPage: React.FC = () => {
    const [masters, setMasters] = useState<Master[]>([]);

    useEffect(() => {
        axiosInstance
            .get<Master[]>('/masters') // Указываем тип данных ответа
            .then((response) => setMasters(response.data))
            .catch((error: unknown) => {
                console.error('Ошибка загрузки мастеров:', error);
            });
    }, []);

    return (
        <div style={{ textAlign: 'center', marginTop: '50px' }}>
            <h1>Список мастеров</h1>
            {masters.length > 0 ? (
                <ul>
                    {masters.map((master) => (
                        <li key={master.id}>{master.name}</li>
                    ))}
                </ul>
            ) : (
                <p>Мастера не найдены.</p>
            )}
        </div>
    );
};

export default MastersPage;