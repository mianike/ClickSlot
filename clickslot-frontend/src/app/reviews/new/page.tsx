'use client';

import { useState } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import { useFormik } from 'formik';
import * as yup from 'yup';
import axiosInstance from '../../api/axiosInstance';
import { FaStar } from 'react-icons/fa';

const validationSchema = yup.object({
  comment: yup
    .string()
    .min(5, 'Комментарий должен содержать не менее 5 символов')
    .max(200, 'Комментарий не должен превышать 200 символов')
    .required('Комментарий обязателен'),
});

export default function NewReviewPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const masterId = searchParams.get('masterId');

  const [rating, setRating] = useState(0);
  const [hover, setHover] = useState(0);

  const formik = useFormik({
    initialValues: {
      comment: '',
    },
    validationSchema,
    onSubmit: async (values) => {
      try {
        await axiosInstance.post('/review', {
          masterId,
          rating,
          comment: values.comment,
        });
        alert('Отзыв успешно отправлен!');
        router.push(`/masters/${masterId}`);
      } catch (error: unknown) {
        console.error('Ошибка при отправке отзыва:', error);
        alert('Не удалось отправить отзыв.');
      }
    },
  });

  return (
    <div className="max-w-lg mx-auto mt-10 p-6 bg-white shadow-md rounded-md">
      <h1 className="text-2xl font-bold mb-4">Оставить отзыв</h1>
      <form onSubmit={formik.handleSubmit}>
        <div className="mb-4">
          <label className="block text-gray-700 font-medium mb-2">Рейтинг</label>
          <div className="flex">
            {[1, 2, 3, 4, 5].map((star) => (
              <FaStar
                key={star}
                className={`cursor-pointer ${
                  star <= (hover || rating) ? 'text-yellow-500' : 'text-gray-300'
                }`}
                size={30}
                onMouseEnter={() => setHover(star)}
                onMouseLeave={() => setHover(0)}
                onClick={() => setRating(star)}
              />
            ))}
          </div>
        </div>
        <div className="mb-4">
          <label className="block text-gray-700 font-medium mb-2">Комментарий</label>
          <textarea
            name="comment"
            value={formik.values.comment}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            rows={4}
            className={`w-full border px-3 py-2 rounded-md ${
              formik.errors.comment && formik.touched.comment ? 'border-red-500' : 'border-gray-300'
            }`}
          ></textarea>
          {formik.errors.comment && formik.touched.comment && (
            <p className="text-red-500 text-sm mt-1">{formik.errors.comment}</p>
          )}
        </div>
        <button
          type="submit"
          disabled={rating === 0 || formik.isSubmitting}
          className="w-full py-2 bg-blue-500 text-white font-semibold rounded-md hover:bg-blue-600 transition"
        >
          Отправить отзыв
        </button>
      </form>
    </div>
  );
}
