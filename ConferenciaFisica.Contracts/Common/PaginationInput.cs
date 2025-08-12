namespace ConferenciaFisica.Contracts.Common
{
    public class PaginationInput
    {
        private int _pageNumber = 1;
        private int _pageSize = 25;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 25 : value > 100 ? 100 : value;
        }

        public int Skip => (PageNumber - 1) * PageSize;
        public int Take => PageSize;
    }
} 