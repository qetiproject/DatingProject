using Microsoft.AspNetCore.Http;

namespace DatingApp.Api.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse reponse, string message)
        {
            reponse.Headers.Add("Application-Error", message);
            reponse.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            reponse.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
