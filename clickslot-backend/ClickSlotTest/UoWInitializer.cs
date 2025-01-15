using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Services;

namespace ClickSlotTest
{
    internal class UoWInitializer
    {
        public static IUnitOfWork Initialize()
        {
            var dbContext = DbContextInitializer.Initialize("testsettings.json");
            return new UnitOfWork(dbContext);
        }
    }
}
