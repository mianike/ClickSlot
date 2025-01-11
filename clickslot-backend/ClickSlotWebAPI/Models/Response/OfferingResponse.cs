namespace ClickSlotWebAPI.Models.Response
{
    public class OfferingResponse
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public TimeSpan Duration { get; set; }
    }
}