namespace ClickSlotModel.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public AppUserDTO Client { get; set; }
        public int MasterId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
