using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LicenceApplication
{
    /// <summary>
    /// The Licence Types
    /// </summary>
    enum LicenceType { New, Renewal };

    /// <summary>
    /// Our Main Licence Application Class. Holds Methods for a Licence Application
    /// </summary>
    class LicenceApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="licenceType"></param>
        /// <param name="dvla"></param>
        /// <param name="licenceTerm"></param>
        /// <param name="dateOfBirth"></param>
        /// <returns></returns>
        public static LicenceEligibilityResponse CheckLicenceEligibility(LicenceType licenceType, bool dvla, int licenceTerm, DateTime dateOfBirth)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            // Get some initial configuration settings
            int minimumAge = int.Parse(configuration["minimum_age"]);

            LicenceEligibilityResponse response = new LicenceEligibilityResponse();
            var messages = new List<string>();
            var costs = new List<string>();
            double total = 0.00;
            double discount = 0.00;
            response.success = true;

            // 1. First validate the date of birth
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear) age = age - 1;
            if (age <= minimumAge)
            {
                response.success = false;
                messages.Add(String.Format("Licence applicants must be {0} years or older.", minimumAge.ToString()));
            }

            // 2. Then validate the DVLA licence
            if (!dvla)
            {
                response.success = false;
                messages.Add(String.Format("Licence applicants must hold a full DVLA licence."));
            }

            // 





            // Apply any discount
            total = total - (total * discount);



            response.messages = messages;
            response.costs = costs;
            response.total = total;
            return response;
        }
    }

    /// <summary>
    /// Class to hold a response object returned by a licence eligibility request.
    /// </summary>
    class LicenceEligibilityResponse
    {
        public bool success { get; set; }
        public List<string> messages { get; set; }
        public List<string> costs { get; set; }
        public double total { get; set; }
    }

}
