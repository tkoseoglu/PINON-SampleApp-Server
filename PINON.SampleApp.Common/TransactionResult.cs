namespace PINON.SampleApp.Common
{
    public class TransactionResult
    {
        public TransactionResult()
        {
            HasError = false;
        }

        public string Message { get; set; } = string.Empty;

        public bool HasError { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsAuthorized { get; set; }

        public int? Id { get; set; } = 0;
    }
}