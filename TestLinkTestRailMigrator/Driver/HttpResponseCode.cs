using RestSharp;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestlinkTestRailMigration.Main.Driver
{
    public class HttpResponseCode
    {
        public static int HttpResponseCodeViaGet(string url)
        {
            var client = new RestClient(url);
            //client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var request = new RestRequest(url, Method.Get);
            var response = client.Execute(request);

            return (int)response.StatusCode;
        }

        public static int HttpResponseCodeViaPost(string url)
        {
            var client = new RestClient(url);
            //client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var request = new RestRequest(url, Method.Post);
            var response = client.Execute(request);

            return (int)response.StatusCode;
        }
    }
}
