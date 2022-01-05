using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ConfidentialClient
{
	class Program
	{
		static void Main(string[] args)
		{
            var accessToken = GetAccessToken();

            Console.WriteLine(accessToken);
            Console.WriteLine("----------");

            var result = GetData(accessToken);

            Console.WriteLine(result);
        }

        public static string GetData(string accessToken)
		{
            var url = "https://localhost:44342/v1/categories/1";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["Authorization"] = $"Bearer {accessToken}";
            httpRequest.Method = "PUT";
            httpRequest.ContentType = "application/json";
            httpRequest.MediaType = "application/json";
            httpRequest.Accept = "application/json";

            const string updatedImageUrl = "imageUrlTest";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                string json = $"{{\"categoryId\":1,\"name\":\"Category-1\",\"image\":\"${updatedImageUrl}\",\"parentCategoryId\":null}}";

                streamWriter.Write(json);
            }


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        public static string GetAccessToken()
        {
            var tenantId = "b41b72d0-4e9f-4c26-8a69-f949f367c91d";
            var clientId = "b2c13659-9ea4-408f-a02f-5cd94cc5d4f2";

            var authorityUri = $"https://login.microsoftonline.com/{tenantId}";

            var redirectUri = "http://localhost";

            var scopes = new List<string> { "api://b2c13659-9ea4-408f-a02f-5cd94cc5d4f2/.default" };


            var clientSecret = "VDp7Q~lcXNpQi4.TYQpQC.fAQdHnNa1OsBrv1";

            var confidentialClient = ConfidentialClientApplicationBuilder
                   .Create(clientId)
                   .WithClientSecret(clientSecret)
                   .WithAuthority(new Uri(authorityUri))
                   .WithRedirectUri(redirectUri)
                   .Build();

            var accessTokenRequest = confidentialClient.AcquireTokenForClient(scopes);

            var accessToken = accessTokenRequest.ExecuteAsync().Result.AccessToken;

            return accessToken;
        }
    }
}
