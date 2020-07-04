namespace Urban.ng.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 12;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }  
        public string PropertyType { get; set; }
        public int UserId { get; set; }
        public string OrderBy { get; set; }

        
    }
}