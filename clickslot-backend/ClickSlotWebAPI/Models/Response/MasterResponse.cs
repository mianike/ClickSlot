using ClickSlotModel.Enums;

namespace ClickSlotWebAPI.Models.Response
{
    public class MasterResponse
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int OfferingsCount { get; set; }
    }
}