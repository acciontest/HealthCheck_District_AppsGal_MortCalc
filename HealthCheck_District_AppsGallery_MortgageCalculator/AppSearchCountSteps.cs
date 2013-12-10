using System;
using TechTalk.SpecFlow;

namespace HealthCheck_District_AppsGallery_MortgageCalculator
{
    [Binding]
    public class AppSearchCountSteps
    {
        [When(@"I search for application at ""(.*)"" with search multiple term ""(.*)""")]
        public void WhenISearchForApplicationAtWithSearchMultipleTerm(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I search for application at ""(.*)"" with search term ""(.*)""")]
        public void WhenISearchForApplicationAtWithSearchTerm(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I see record-count is (.*)")]
        public void ThenISeeRecord_CountIs(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I see primaryapplication\.metainfo\.name contains ""(.*)""")]
        public void ThenISeePrimaryapplication_Metainfo_NameContains(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
