using ClickSlotModel.Enums;

namespace ClickSlotWebAPI.Models.Response
{
    public class MasterResponse
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public int OfferingsCount { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
    }
}