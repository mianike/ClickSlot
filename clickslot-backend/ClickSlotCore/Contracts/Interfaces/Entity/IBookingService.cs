﻿using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IBookingService : IService
    {
        Task<BookingDTO?> GetByIdAsync(int id);
        Task<IEnumerable<BookingDTO>> GetAllByMasterIdAsync(int masterId);
        Task<IEnumerable<BookingDTO>> GetAllByMasterIdAsync(int masterId, int page, int pageSize);
        Task<BookingDTO> CreateAsync(BookingDTO bookingDto);
        Task<BookingDTO> UpdateAsync(BookingDTO bookingDto);
        Task<bool> DeleteAsync(int id);
    }
}
