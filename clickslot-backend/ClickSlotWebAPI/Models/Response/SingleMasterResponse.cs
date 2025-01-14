using ClickSlotDAL.Entities;
using ClickSlotModel.Enums;

namespace ClickSlotWebAPI.Models.Response
{
    public class SingleMasterResponse
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public IEnumerable<OfferingResponse> Offerings { get; set; }
        public IEnumerable<ReviewResponse> MasterReviews { get; set; }
    }
}