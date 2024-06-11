using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowRestSharpReqresApi.StepDefinitions
{
    [Binding]
    public class CreateUserStepDefinitions
    {
        private const string BASE_URL = "https://reqres.in";
        private RestClient client;
        private readonly ScenarioContext _scenarioContext;

        public CreateUserStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        private IRestResponse CreateUser(dynamic payload)
        {
            client = new RestClient(BASE_URL);
            var request = new RestRequest("/api/users", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(payload);
            IRestResponse apiResponse = client.Execute(request);
            return apiResponse;
        }

        [Given(@"user name ""([^""]*)"" with job ""([^""]*)""")]
        public void GivenUserNameWithJob(string inputName, string inputJob)
        {
            var requestPayload = new
            {
                name = inputName,
                job = inputJob
            };
            _scenarioContext["payload"] = requestPayload;
        }

        [When(@"request to create user is sent")]
        public void WhenRequestToCreateUserIsSent()
        {
            dynamic jsonPayload = _scenarioContext["payload"];
            Console.WriteLine("Request payload:");
            Console.WriteLine(JsonConvert.SerializeObject(jsonPayload, Formatting.Indented)); // Print request payload
            var response = CreateUser(jsonPayload);

            _scenarioContext["responseStatusCode"] = (int)response.StatusCode;
            dynamic content = JsonConvert.DeserializeObject(response.Content);
            _scenarioContext["jsonResponse"] = content;

            Console.WriteLine("Response payload:");
            Console.WriteLine(content); // Print response payload

        }

        [Then(@"user is successfully created")]
        public void ThenUserIsSuccessfullyCreated()
        {
            dynamic jsonPayload = _scenarioContext["payload"];
            dynamic jsonResponse = _scenarioContext["jsonResponse"];
            Assert.AreEqual(201, _scenarioContext["responseStatusCode"]);
            Assert.AreEqual(jsonPayload.name, jsonResponse.name.ToObject<string>());
            Assert.AreEqual(jsonPayload.job, jsonResponse.job.ToObject<string>());
        }
    }
}
