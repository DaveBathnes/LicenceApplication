using Xunit;
using LicenceApplication;
using System;

namespace LicenceApplication.Tests
{
    public class LicenceApplicationTests
    {
        /// <summary>
        /// Check for an invalid term time (4 years)
        /// </summary>
        [Fact]
        public void CheckLicenceEligibility_InvalidInput()
        {
            Assert.Throws<ArgumentException>(() => LicenceApplication.CheckLicenceEligibility(LicenceType.New, true, 4, DateTime.Now));
        }

        /// <summary>
        /// Check for a succesful application. New Licence.
        /// For this we are not checking costs, but the inputs producing an outcome
        /// </summary>
        [Fact]
        public void CheckLicenceEligibility_SuccesfulApplication()
        {
            var response = LicenceApplication.CheckLicenceEligibility(LicenceType.New, true, 1, DateTime.Parse("13/06/1984"));
            Assert.True(response.success);
        }

        /// <summary>
        /// Check for costs being returned. New Licence.
        /// </summary>
        [Fact]
        public void CheckLicenceEligibility_SuccesfulMessages()
        {
            var response = LicenceApplication.CheckLicenceEligibility(LicenceType.New, true, 1, DateTime.Parse("13/06/1984"));
            Assert.True(response.costs.Count > 0);
        }

        /// <summary>
        /// Check for an unsuccesful application due to no DVLA licence
        /// </summary>
        [Fact]
        public void CheckLicenceEligibility_UnsuccesfulApplication_NoDVLA()
        {
            var response = LicenceApplication.CheckLicenceEligibility(LicenceType.New, false, 2, DateTime.Parse("13/06/1984"));
            Assert.False(response.success);
        }

        /// <summary>
        /// Check for an unsuccesful application due to a new application but 2 year term
        /// </summary>
        [Fact]
        public void CheckLicenceEligibility_UnsuccesfulApplication_NewNonSingleYear()
        {
            var response = LicenceApplication.CheckLicenceEligibility(LicenceType.New, false, 2, DateTime.Parse("13/06/1984"));
            Assert.False(response.success);
        }

        /// <summary>
        /// Check for an unsuccesful application due to a new application but 2 year term. Messages should be returned.
        /// </summary>
        [Fact]
        public void CheckLicenceEligibility_UnsuccesfulMessages_NewNonSingleYear()
        {
            var response = LicenceApplication.CheckLicenceEligibility(LicenceType.New, false, 2, DateTime.Parse("13/06/1984"));
            Assert.True(response.messages.Count > 0);
        }
    }
}
