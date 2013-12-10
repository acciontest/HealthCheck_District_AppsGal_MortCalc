using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using AlteryxGalleryAPIWrapper;
using HtmlAgilityPack;
using MortageCalculatorCheck;
using NUnit.Framework;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace HealthCheck_District_AppsGallery
{
    [Binding]
    public class VersionCheckSteps
    {

        private string _theUrl;
        private string _theResponse;
        private string _search;
        private string _name;
        private string alteryxurl;
        private string _sessionid;
        private string _appid;
        private string _userid;
        private string _appName;
        private string jobid;
        private string outputid;
        private string validationId;

        private Client Obj = new Client("https://devgallery.alteryx.com/api/");
        //  private Client Obj =new Client("https://gallery.alteryx.com/api/");
        private RootObject jsString = new RootObject();

        [Given(@"the alteryx service is running at ""(.*)""")]
        public void GivenTheAlteryxServiceIsRunningAt(string p0)
        {

            _theUrl = p0;
        }


        [Given(@"alteryx running at""(.*)""")]
        public void GivenAlteryxRunningAt(string url)
        {
            alteryxurl = url;
        }


        //VersionCheck District
        [When(@"I invoke the GET at ""(.*)""")]
        public void WhenIInvokeTheGETAt(string p0)
        {
            string fullUrl = _theUrl + "/" + p0;

            WebRequest request = WebRequest.Create(fullUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            _theResponse = responseFromServer;
        }

        //Version Check
        [Then(@"I see the version binaryVersions/serviceLayer is ""(.*)"" and binaryVersions/cloud is ""(.*)""")]
        public void CloudIs_(string expectedservicelayerVersion, string expectedcloudVersion)
        {
            var serializer = new JavaScriptSerializer();
            var dict = serializer.Deserialize<Dictionary<string, dynamic>>(_theResponse);
            Console.WriteLine("Version of serviceLayer is: " + dict["binaryVersions"]["serviceLayer"]);
            Assert.AreEqual(expectedservicelayerVersion, dict["binaryVersions"]["serviceLayer"]);
            Console.WriteLine("Version of cloud is: " + dict["binaryVersions"]["cloud"]);
            Assert.AreEqual(expectedcloudVersion, dict["binaryVersions"]["cloud"]);

        }





        //District Check
        [Then(@"I see at least (.*) active districts")]
        public void ThenISeeAtLeastActiveDistricts(int expectedDistrict)
        {
            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ArrayList>(_theResponse);
            int count = json.Count;
            Assert.That(count, Is.AtLeast(expectedDistrict));
        }


        //District Check
        [When(@"I invoke GET at ""(.*)"" for ""(.*)""")]
        public void WhenIInvokeGETAtFor(string p0, string expecteddistrict)
        {
            string fullUrl = _theUrl + "/" + p0;
            WebRequest request = WebRequest.Create(fullUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            _theResponse = responseFromServer;

            var DistrictObj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>(_theResponse);
            int count = DistrictObj.Length;

            int i = 0;
            for (i = 0; i <= count - 1; i++)
            {
                if (DistrictObj[i]["title"] == expecteddistrict)
                {
                    break;
                }
            }

            string id = DistrictObj[i]["id"];
            string apiUrl = fullUrl + "/" + id;


            WebRequest apirequest = WebRequest.Create(apiUrl);
            apirequest.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse apiresponse = (HttpWebResponse)apirequest.GetResponse();
            Stream apidataStream = apiresponse.GetResponseStream();
            StreamReader apireader = new StreamReader(apidataStream);
            string apiresponseFromServer = apireader.ReadToEnd();
            _theResponse = apiresponseFromServer;
        }

        //District Check
        [Then(@"I see that district description contains the text ""(.*)""")]
        public void ThenISeeThatDistrictDescriptionContainsTheText(string expecteddescription)
        {
            var dict = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(_theResponse);
            Console.Write(dict["description"]);
            StringAssert.Contains(expecteddescription, dict["description"]);
        }




        //AppSearchCount
        [When(@"I search for application at ""(.*)"" with search multiple term ""(.*)""")]
        public void WhenISearchForApplicationAtWithSearchMultipleTerm(string apiurl, string searchterm)
        {
            string term = searchterm.TrimStart();
            string[] split = term.Split(new Char[] { ' ', ',', '.', ':', '\t' });
            if (split.Length > 0)
            {
                _search = split[0];
            }
            string Url = _theUrl + "/" + apiurl + "?search=" + _search;
            WebRequest webRequest = System.Net.WebRequest.Create(Url);
            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream);
            string responseFromServer = reader.ReadToEnd();
            _theResponse = responseFromServer;
        }

        //AppSearchRecordCount
        [Then(@"I see record-count is (.*)")]
        public void ThenISeeRecord_CountIs(int expectedcount)
        {
            var dict = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(_theResponse);
            int count = dict["recordCount"];
            Assert.AreEqual(count, expectedcount);

        }

        //AppSearchRecordCount ExpectedName
        [When(@"I search for application at ""(.*)"" with search term ""(.*)""")]
        public void WhenISearchForApplicationAtWithSearchTerm(string apiurl, string searchterm)
        {
            string search = Regex.Replace(searchterm, @"\s+", "+");
            string Url = _theUrl + "/" + apiurl + "?search=" + search;
            WebRequest webRequest = System.Net.WebRequest.Create(Url);
            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream);
            string responseFromServer = reader.ReadToEnd();
            _theResponse = responseFromServer;

        }


        //AppSearchExpectedName
        [Then(@"I see primaryapplication\.metainfo\.name contains ""(.*)""")]
        public void ThenISeePrimaryapplication_Metainfo_NameContains(string expectedname)
        {
            var dict = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(_theResponse);
            int count = dict["recordCount"];
            if (count == 1)
            {
                _name = dict["records"][0]["primaryApplication"]["metaInfo"]["name"];
            }
            else
            {
                int i = 0;
                for (i = 0; i <= count - 1; i++)
                {
                    if (dict["records"][i]["primaryApplication"]["metaInfo"]["name"] == expectedname)
                    {
                        _name = dict["records"][i]["primaryApplication"]["metaInfo"]["name"];
                        break;
                    }

                }
            }
            Assert.AreEqual(expectedname, _name);
        }





        /*MORTAGAGE CALCULATOR*/

        [Given(@"I am logged in using ""(.*)"" and ""(.*)""")]
        public void GivenIAmLoggedInUsingAnd(string user, string password)
        {
            // Authenticate User and Retreive Session ID
            _sessionid = Obj.Authenticate(user, password).sessionId;

        }

        [Given(@"I publish the application ""(.*)""")]
        public void GivenIPublishTheApplication(string p0)
        {
            //Publish the app & get the ID of the app
            string apppath = @"..\..\docs\Mortgage_Calculator.yxzp";
            Action<long> progress = new Action<long>(Console.Write);
            var pubResult = Obj.SendAppAndGetId(apppath, progress);
            _appid = pubResult.id;
            validationId = pubResult.validation.validationId;


        }
        [Given(@"I check if the application is ""(.*)""")]
        public void GivenICheckIfTheApplicationIs(string status)
        {
            // validate a published app can be run 
            // two step process. First, GetValidationStatus which indicates when validation disposition is available. 
            // Second, GetValidation, which gives actual status Valid, UnValid, etc.

            int count = 0;
            String validStatus = "";

            var validationStatus = Obj.GetValidationStatus(validationId);
            validStatus = validationStatus.status;
            System.Threading.Thread.Sleep(1000);
        CheckValidate:

            if (validStatus == "Completed" && count < 5)
            {
                string disposition = validationStatus.disposition;
            }
            else if (count < 5)
            {
                count++;
                var reCheck = Obj.GetValidationStatus(validationId);
                validStatus = reCheck.status;
                goto CheckValidate;
            }

            else
            {
                SystemException ex = new SystemException();
                ex.ToString();
                //Exception e;
                //throw e.Message;
            }

            #region oldCode

            /*     String validStatus = "";
                 try
                 {
                     System.Threading.Thread.Sleep(3000);
                     var validationStatus = Obj.GetValidationStatus(validationId);
                     validStatus = validationStatus.status;
                     string disposition = validationStatus.disposition;

                 }
                 catch (Exception e)
                 {

                     throw e;
                 }
                 */


            //CheckValidate:
            //    if (validStatus != "Completed")
            //    {
            //        var validationStatus = Obj.GetValidationStatus(validationId);
            //        validStatus = validationStatus.status;
            //        string disposition = validationStatus.disposition;
            //        try
            //        {
            //            if (count <= 5)
            //            {
            //                System.Threading.Thread.Sleep(1000);

            //            }
            //        }
            //        catch (Exception e)
            //        {

            //            throw e;
            //        }
            //        count++;
            //    }
            //    else
            //    {
            //        goto CheckValidate;
            //    }
            #endregion
            var finalValidation = Obj.GetValidation(_appid, validationId); // url/api/apps/{APPID}/validation/{VALIDATIONID}/
            var finaldispostion = finalValidation.validation.disposition;
            StringAssert.IsMatch(status, finaldispostion.ToString());
        }



        [When(@"I run mortgage calculator with principle (.*) interest (.*) payments (.*)")]
        public void WhenIRunMortgageCalculatorWithPrincipleInterestPayments(int principle, Decimal interest, int numpayments)

        {


            jsString.appPackage.id = _appid;
            jsString.userId = _userid;
            jsString.appName = _appName;
            string appinterface = Obj.GetAppInterface(_appid);
            dynamic interfaceresp = JsonConvert.DeserializeObject(appinterface);

            //Construct the payload to be posted.
            string header = String.Empty;
            string payatbegin = String.Empty;
            List<Jsonpayload.Question> questionAnsls = new List<Jsonpayload.Question>();
            questionAnsls.Add(new Jsonpayload.Question("IntRate", interest.ToString()));
            questionAnsls.Add(new Jsonpayload.Question("NumPayments", numpayments.ToString()));
            questionAnsls.Add(new Jsonpayload.Question("Payment", "1832.14"));
            questionAnsls.Add(new Jsonpayload.Question("FutureValue", "0"));
            questionAnsls.Add(new Jsonpayload.Question("LoanAmount", principle.ToString()));
            var solve = new List<Jsonpayload.datac>();
            solve.Add(new Jsonpayload.datac() { key = "Payment", value = "true" });
            var payat = new List<Jsonpayload.datac>();
            payat.Add(new Jsonpayload.datac() { key = "0", value = "true" });
            string SolveFor = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(solve);
            string PayAtBegin = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(payat);


            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    Jsonpayload.Question questionAns = new Jsonpayload.Question();
                    questionAns.name = "SolveFor";
                    questionAns.answer = SolveFor;
                    jsString.questions.Add(questionAns);
                }
                else if (i == 1)
                {
                    Jsonpayload.Question questionAns = new Jsonpayload.Question();
                    questionAns.name = "PayAtBegin";
                    questionAns.answer = PayAtBegin;
                    jsString.questions.Add(questionAns);
                }
                else
                {
                    jsString.questions.AddRange(questionAnsls);
                }
            }
            jsString.jobName = "Job Name";

            // Make Call to run app
            var postData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(jsString);
            string postdata = postData.ToString();
            string resjobqueue = Obj.QueueJob(postdata);
            var jobqueue = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(resjobqueue);
            jobid = jobqueue["id"];
            string status = "";
            System.Threading.Thread.Sleep(3000);
            //while (status != "Completed")
            //{
            string jobstatusresp = Obj.GetJobStatus(jobid);
            var statusresp = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(jobstatusresp);
            status = statusresp["status"];
            //}
        }


        [Then(@"I see output (.*)")]
        public void ThenISeeOutput(decimal answer)
        {
            //url + "/apps/jobs/" + jobId + "/output/"
            string getmetadata = Obj.GetOutputMetadata(jobid);
            dynamic metadataresp = JsonConvert.DeserializeObject(getmetadata);

            // outputid = metadataresp[0]["id"];
            int count = metadataresp.Count;
            for (int j = 0; j <= count - 1; j++)
            {
                outputid = metadataresp[j]["id"];
            }

            string getjoboutput = Obj.GetJobOutput(jobid, outputid, "html");
            string htmlresponse = getjoboutput;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlresponse);
            string output = doc.DocumentNode.SelectSingleNode("//span[@class='DefaultNumericText']").InnerHtml;   
            decimal output1= Convert.ToDecimal(output);
            decimal finaloutput= Math.Round(output1, 2);

            #region
            //  string output = doc.DocumentNode.SelectSingleNode("[@id='preview']").InnerHtml;
            //  var output = doc.DocumentNode.SelectSingleNode("//html/body/div[1]/div/div[9]/div[3]/div[2]/div[1]/table/tbody/tr[1]/td/div").InnerHtml;
            //   var output = doc.DocumentNode.SelectSingleNode("html/body/div[1]/div/div[9]/div[3]/div[2]/div[1]/table/tbody/tr[1]/td/div").InnerHtml;
            //string outputFromHtml = "";
            //foreach (var node in doc.DocumentNode.ChildNodes)
            //{
            //    outputFromHtml += node.InnerText;
            //    if (outputFromHtml == "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Final//EN\">")
            //    {
            //        outputFromHtml = "";
            //    }

            //}
            ////string output1 = Regex.Replace(output, @"\t|\n|\r|,", "");



            //string[] splitOutput;

            //splitOutput = outputFromHtml.Split(default(Char[]), StringSplitOptions.RemoveEmptyEntries);

            //string finalOutput = string.Empty;

            //for (int i = 0; i < splitOutput.Length - 1; i++)
            //{
            //    string finalOutputReg = Regex.Replace(splitOutput[i], @"\t|\n|\r|,", "");
            //    if (finalOutputReg == answer.ToString())
            //    {
            //       finalOutput=finalOutputReg;

            //    }
            //    else
            //    {
            //        finalOutput = splitOutput[1];

            //    }
            //}
            ////StringAssert.Contains(answer.ToString(), output3);
            #endregion

            Assert.AreEqual(answer, finaloutput);

            
        }
      [Then(@"Then I delete the application")]
        public void ThenThenIDeleteTheApplication()
        {

            var deleteres = Obj.DeleteApp(_appid);
        }


    }
}
