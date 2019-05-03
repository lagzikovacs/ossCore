using System;

namespace ossServer.Utils
{
    public static class ExceptionExtension
    {
        public static string InmostMessage(this Exception ex)
        {
            if (ex.InnerException != null)
                return ex.InnerException.InmostMessage();
            return ex.Message;
        }
    }
}
