namespace ClickSlotWebAPI.Models.Response
{
    public class AppUserResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public IEnumerable<OfferingResponse> Offerings { get; set; }
    }
}
