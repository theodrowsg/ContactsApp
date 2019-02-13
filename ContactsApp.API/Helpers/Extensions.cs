using System;
using Microsoft.AspNetCore.Http;

namespace ContactsApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message){
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
        }

        public static int CalculateAge(this DateTime dateOfBirth){

            int age = DateTime.Today.Year - dateOfBirth.Year;

            if(dateOfBirth.AddYears(age) > DateTime.Today){
                return age--;
            }
            else return age;
        }
        
    }
}