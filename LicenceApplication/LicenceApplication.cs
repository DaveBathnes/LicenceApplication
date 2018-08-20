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
        /// Checks for licence eligibility given a number of driver inputs
        /// </summary>
        /// <param name="licenceType">The Licence Type, reference to the LicenceType enum</param>
        /// <param name="dvla">Boolean as to whether the driver has a full DVLA licence</param>
        /// <param name="licenceTerm">The number of years for the renewal</param>
        /// <param name="dateOfBirth">The date of birth of the driver</param>
        /// <returns></returns>
        public static LicenceEligibilityResponse CheckLicenceEligibility(LicenceType licenceType, bool dvla, int licenceTerm, DateTime dateOfBirth)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            // Get some initial configuration settings
            int minimumAge = int.Parse(configuration["minimum_age"]);
            double newLicenceCost = double.Parse(configuration["new_licence_fee"]);
            double licenceAnnualFee = double.Parse(configuration["licence_annual_fee"]);
            double oneTermDiscount = double.Parse(configuration["one_term_discount"]);
            double twoTermDiscount = double.Parse(configuration["two_term_discount"]);
            double threeTermDiscount = double.Parse(configuration["three_term_discount"]);

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

            // 3. Calculate the cost of the licence
            double licenceCost = licenceAnnualFee * licenceTerm;
            total = total + licenceCost;
            costs.Add(String.Format("£{0} for {1} year taxi licence charged at {2} per year.", licenceCost.ToString(), licenceTerm.ToString(), licenceAnnualFee));

            // 4. If a new licence add on that cost
            if (licenceType == LicenceType.New)
            {
                total = total + newLicenceCost;
                costs.Add(String.Format("£{0} for new licence application", newLicenceCost));
            }

            // 4. Apply any discount
            if (licenceTerm == 1 && oneTermDiscount > 0.00)
            {
                discount = (total * oneTermDiscount);
                total = total - discount;
                costs.Add(String.Format("£{0} discount for one year licence.", discount));
            }
            if (licenceTerm == 2 && twoTermDiscount > 0.00)
            {
                discount = (total * twoTermDiscount);
                total = total - discount;
                costs.Add(String.Format("£{0} discount for two year licence.", discount));
            }
            if (licenceTerm == 3 && threeTermDiscount > 0.00)
            {
                discount = (total * threeTermDiscount);
                total = total - discount;
                costs.Add(String.Format("£{0} discount for three year licence.", discount));
            }

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
