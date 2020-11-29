using Microsoft.AspNetCore.Http;
using System;

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

        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Today.Year - dateTime.Year;
            if (dateTime.AddYears(age) > DateTime.Today)
                age--;
            return age;

        }
    }
}
