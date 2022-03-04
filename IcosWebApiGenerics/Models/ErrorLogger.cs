using System.Text;

namespace IcosWebApiGenerics.Models
{
    public class ErrorLogger : IErrorLogger
    {
        private StringBuilder errorSb;

        public ErrorLogger()
        {
            errorSb = new StringBuilder();
        }
        public void LogErrorMessage(string error)
        {
            errorSb.Append(error);
        }

        /// <summary>
        /// Property to log error message (if any) before API validation
        /// </summary>
        public string ErrorMessage { get { return errorSb.ToString(); } }
    }
}
