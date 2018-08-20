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

            // Inputs to eligibility check. Licence Type, Term, 
            LicenceType type = LicenceType.New;
            int term = 1;
            bool dvla = false;
            DateTime dob;

            // Licence Type Input
            ConsoleKey renewal_response;
            do
            {
                Console.Write("Is this a renewal of an existing licence? [y/n] ");
                renewal_response = Console.ReadKey(false).Key;
                if (renewal_response != ConsoleKey.Enter) Console.WriteLine();
            } while (renewal_response != ConsoleKey.Y && renewal_response != ConsoleKey.N);

            // Renewal Term Input
            if (renewal_response == ConsoleKey.Y)
            {
                ConsoleKeyInfo termResponse;
                do
                {
                    Console.Write("How many years would you like to renew for? [1/2/3] ");
                    termResponse = Console.ReadKey(false);
                    if (termResponse.Key != ConsoleKey.Enter) Console.WriteLine();
                } while (termResponse.Key != ConsoleKey.D1 && termResponse.Key != ConsoleKey.D2 && termResponse.Key != ConsoleKey.D3);
                term = (int)char.GetNumericValue(termResponse.KeyChar);
            }

            // DVLA Licence Input
            ConsoleKey dvla_held;
            do
            {
                Console.Write("Do you hold a full DVLA driving licence? [y/n] ");
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
                Console.Write("What is your date of birth? [DD/MM/YYYY] ");
                dob_response = Console.ReadLine();
            } while (!DateTime.TryParseExact(dob_response, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dob));

            var response = LicenceApplication.CheckLicenceEligibility(type, dvla, term, dob);

            // Print out the response - different styles if success or rejection
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
                Console.Write("--------- Not Eligible for Taxi Licence --------\n");
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
