namespace OnlineBookStore.DTOs
{
    public abstract class BaseDTO
    {
        public string CreatedByADName { get; set; }
        public DateTime CreateDate { get; set; }
        public string? ModifyByADName { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Enabled { get; set; }
    }
}
