using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BestSellerList
{
    public class Function
    {
        public static readonly HttpClient client = new HttpClient();
        public async Task<ExpandoObject> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string url = "https://api.nytimes.com/svc/books/v3/lists/current/";
            // API key is taken from Key.cs, in order to hide from public github
            string apiKey = new Key().key;

            string listCat = "";
            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            dict.TryGetValue("list", out listCat);
            string call = url + listCat + ".json?api-key=" + apiKey;

            HttpResponseMessage response = await client.GetAsync(call);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            dynamic expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(json);

            return expandoObject;
        }
    }
}
