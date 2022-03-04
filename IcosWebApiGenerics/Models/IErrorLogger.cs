namespace IcosWebApiGenerics.Models
{
    public interface IErrorLogger
    {
        void LogErrorMessage(string error);

        /// <summary>
        /// Property to log error message (if any) before API validation
        /// </summary>
        public string ErrorMessage { get; }
    }
}