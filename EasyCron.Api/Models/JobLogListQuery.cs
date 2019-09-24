namespace EasyCron.Api.Models
{
    public class JobLogListQuery
    {
        public string JobId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Keywords { get; set; }
        public bool? IsSuccess { get; set; }
    }
}
