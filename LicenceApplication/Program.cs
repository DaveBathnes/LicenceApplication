using System;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace LicenceApplication
{
    class Program
    {
        /// <summary>
        /// The main entry point of the application. Takes in user input for the licence application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Application Configuration
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string questionExistingLicence = configuration["question_existing_licence"];
            string questionLicenceTerm = configuration["question_licence_term"];
            string questionDVLAHeld = configuration["question_dvla_held"];
            string questionDateOfBirth = configuration["question_date_of_birth"];

            // Inputs to eligibility check. Licence Type, Term, 
            LicenceType type = LicenceType.New;
            int term = 1;
            bool dvla = false;
            DateTime dob;

            // Licence Type Input
            ConsoleKey renewal_response;
            do
            {
                Console.Write(questionExistingLicence);
                renewal_response = Console.ReadKey(false).Key;
                if (renewal_response != ConsoleKey.Enter) Console.WriteLine();
            } while (renewal_response != ConsoleKey.Y && renewal_response != ConsoleKey.N);

            // Renewal Term Input
            if (renewal_response == ConsoleKey.Y)
            {
                ConsoleKeyInfo termResponse;
                do
                {
                    Console.Write(questionLicenceTerm);
                    termResponse = Console.ReadKey(false);
                    if (termResponse.Key != ConsoleKey.Enter) Console.WriteLine();
                } while (termResponse.Key != ConsoleKey.D1 && termResponse.Key != ConsoleKey.D2 && termResponse.Key != ConsoleKey.D3);
                term = (int)char.GetNumericValue(termResponse.KeyChar);
            }

            // DVLA Licence Input
            ConsoleKey dvla_held;
            do
            {
                Console.Write(questionDVLAHeld);
                dvla_held = Console.ReadKey(false).Key;
                if (dvla_held != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (dvla_held != ConsoleKey.Y && dvla_held != ConsoleKey.N);
            dvla = (dvla_held == ConsoleKey.Y ? true : false);

            // DOB Input
            string dob_response;
            string[] formats = { "dd/MM/yyyy" };
            do
            {
                Console.Write(questionDateOfBirth);
                dob_response = Console.ReadLine();
            } while (!DateTime.TryParseExact(dob_response, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dob));

            var response = LicenceApplication.CheckLicenceEligibility(type, dvla, term, dob);

            // Print out the response - different formatting if success or rejection
            if (response.success)
            {
                Console.Write("\n");
                Console.Write("------------------------------------------------\n");
                Console.Write("--------- Eligible for Taxi Licence ------------\n");
                Console.Write("------------------------------------------------\n");
                Console.Write("\n");
                Console.Write("Costs: \n");

                for (int i = 0; i < response.costs.Count; i++)
                {
                    Console.Write(response.costs[i] + "\n");
                }
            } else
            {
                Console.Write("\n");
                Console.Write("------------------------------------------------\n");
                Console.Write("--------- NOT Eligible for Taxi Licence --------\n");
                Console.Write("------------------------------------------------\n");
                Console.Write("\n");
                Console.Write("Reasons: \n");

                for (int i = 0; i < response.messages.Count; i++)
                {
                    Console.Write(response.messages[i] + "\n");
                }
            }

            // Keep application alive 
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
