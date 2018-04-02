namespace ApiModule
{
    using ApiModule.Models;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Threading;
    using System.Web;

    public static class GenderizeApi
    {
        public static bool FirstNameIsFemale(string firstName, bool continueOnError = true)
        {
            bool isFemale = false;
            var baseUrl = new Uri(string.Format("https://api.genderize.io/?name={0}", HttpUtility.UrlEncode(firstName)));

            var client = new RestClient(baseUrl);
            var request = new RestRequest("");

            var resetEvent = new ManualResetEvent(false);
            client.ExecuteAsync(request, (response, asyncHandle) =>
            {
                if (continueOnError && (response.ResponseStatus == ResponseStatus.Error || string.IsNullOrEmpty(response.Content)))
                {
                    resetEvent.Set();
                    return;
                }

                var genderResponse = JsonConvert.DeserializeObject<GenderizeRestResponse>(response.Content);
                isFemale = genderResponse.IsFemale;

                resetEvent.Set();
            });

            resetEvent.WaitOne();

            return isFemale;
        }

        public static bool FirstNameIsMale(string firstName)
        {
            return !FirstNameIsFemale(firstName);
        }
    }
}
