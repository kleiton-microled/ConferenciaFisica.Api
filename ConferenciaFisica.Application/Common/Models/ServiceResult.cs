namespace ConferenciaFisica.Application.Common.Models
{
    public class ServiceResult<T> : IServiceResult<T>
    {
        private bool _status = true;

        public bool Status
        {
            get => string.IsNullOrEmpty(Error) && Result != null && _status;
            set => _status = value;
        }

        public IList<string> Mensagens { get; set; } = new List<string>();
        public string Error { get; set; }
        public T Result { get; set; }

        public static ServiceResult<T> Success(T result, params string[] mensagens)
        {
            return new ServiceResult<T>
            {
                Result = result,
                Mensagens = mensagens?.ToList() ?? new List<string>()
            };
        }

        public static ServiceResult<T> Failure(string errorMessage)
        {
            return new ServiceResult<T>
            {
                Status = false,
                Error = errorMessage
            };
        }
    }
}
