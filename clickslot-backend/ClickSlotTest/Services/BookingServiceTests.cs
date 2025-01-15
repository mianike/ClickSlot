using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotCore.Services.Entity;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using ClickSlotTest;
using Microsoft.EntityFrameworkCore;

public class BookingServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBookingService _bookingService;

    public BookingServiceTests()
    {
        _unitOfWork = UoWInitializer.Initialize();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Booking, BookingDTO>().ReverseMap();
            cfg.CreateMap<AppUser, AppUserDTO>().ReverseMap();
            cfg.CreateMap<Offering, OfferingDTO>().ReverseMap();
        });
        _mapper = configuration.CreateMapper();

        _bookingService = new BookingService(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddBookingToDatabase()
    {
        //Arrange

        var bookingDto = new BookingDTO
        {
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            MasterId = 2,
            ClientId = 2,
            OfferingId = 2
        };

        //Act
        var result = await _bookingService.CreateAsync(bookingDto);
        var bookingInDb = await _unitOfWork.GetRepository<Booking>().AsReadOnlyQueryable()
            .FirstOrDefaultAsync(b => b.Id == result.Id);

        //Assert

        // DateTime сравниваем без Tick
        Assert.NotNull(result);
        Assert.NotNull(bookingInDb);
        Assert.Equal(result.StartTime.AddTicks(-result.StartTime.Ticks % TimeSpan.TicksPerSecond),
            bookingInDb.StartTime.AddTicks(-bookingInDb.StartTime.Ticks % TimeSpan.TicksPerSecond));
        Assert.Equal(result.EndTime.AddTicks(-result.EndTime.Ticks % TimeSpan.TicksPerSecond),
            bookingInDb.EndTime.AddTicks(-bookingInDb.EndTime.Ticks % TimeSpan.TicksPerSecond));
        Assert.Equal(result.MasterId, bookingInDb.MasterId);
        Assert.Equal(result.ClientId, bookingInDb.ClientId);
        Assert.Equal(result.OfferingId, bookingInDb.OfferingId);
    }


    [Fact]
    public async Task GetByIdAsyncShouldReturnCorrectBooking()
    {
        //Arrange

        var createdBooking = new Booking
        {
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            MasterId = 1,
            ClientId = 1,
            OfferingId = 1
        };

        _unitOfWork.GetRepository<Booking>().Create(createdBooking);
        await _unitOfWork.SaveChangesAsync();

        //Act
        var result = await _bookingService.GetByIdAsync(createdBooking.Id);

        //Assert

        // DateTime сравниваем без Tick
        Assert.NotNull(result);
        Assert.Equal(createdBooking.StartTime.AddTicks(-createdBooking.StartTime.Ticks % TimeSpan.TicksPerSecond),
            result.StartTime.AddTicks(-result.StartTime.Ticks % TimeSpan.TicksPerSecond));
        Assert.Equal(createdBooking.EndTime.AddTicks(-createdBooking.EndTime.Ticks % TimeSpan.TicksPerSecond),
             result.EndTime.AddTicks(-result.EndTime.Ticks % TimeSpan.TicksPerSecond));
        Assert.Equal(createdBooking.MasterId, result.MasterId);
        Assert.Equal(createdBooking.ClientId, result.ClientId);
        Assert.Equal(createdBooking.OfferingId, result.OfferingId);
    }
}
