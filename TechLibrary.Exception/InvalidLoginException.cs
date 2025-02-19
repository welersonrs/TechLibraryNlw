using System.Net;

namespace TechLibrary.Exception
{
    public class InvalidLoginException : TechLibraryException
    {
        public override List<string> GetErrorMessages() => ["Email and/or invalid password."];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
