using ClickSlotDAL.Entities.Base;

namespace ClickSlotDAL.Entities
{
    public class Review : EntityBase<int>
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int MasterId { get; set; }
        public Master Master { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}