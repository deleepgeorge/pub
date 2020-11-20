using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GraphClientTestApp
{

    class Program
    {
        static  void Main(string[] args)
        {
            try
            {
                RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static async Task RunAsync()
        {

            AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile(@"appsettings.json");


            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                      .WithClientSecret(config.ClientSecret)
                      .WithAuthority(new Uri(config.Authority))
                      .Build();

            
            string[] scopes = new string[] { $"{config.ApiUrl}.default" };
            
            AuthenticationResult result = null;      
      
            try
            {
                result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            }
            catch(MsalUiRequiredException ex)
            {
                Console.WriteLine($"Insufficient permissions {ex.Message}");
                return;
            }
            catch (MsalServiceException ex)when (ex.Message.Contains("AADSTS70011"))
            {
                Console.WriteLine($"Invalid scope: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
          

            var vv = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            DelegateAuthenticationProvider ds = new DelegateAuthenticationProvider(async (res) => { res.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken); });          
            GraphServiceClient _graphClient = new GraphServiceClient(ds);

            


            // REPORTS - https://github.com/microsoftgraph/msgraph-sdk-dotnet/issues/534 - workaround
            var vreportReq =  _graphClient.Reports.GetOneDriveActivityFileCounts("D180").Request().GetHttpRequestMessage();
            var repResponse = await _graphClient.HttpProvider.SendAsync(vreportReq);
            string report = string.Empty;
            if(repResponse.IsSuccessStatusCode)
            {
                report = await repResponse.Content.ReadAsStringAsync();
                Console.WriteLine(report);
            }
            

            //OneDrive

            var v = _graphClient.Users["ushjdhisfi"].Drive.Request().GetAsync();
            await v.ContinueWith(usr =>
            {
                Console.WriteLine(usr.Result.WebUrl);
            });

            //User.Read
            var users = _graphClient.Users.Request().Select(r=>new { r.DisplayName, r.UserPrincipalName,r.AccountEnabled,r.Id,r.Drive}).GetAsync();

            // user drive https://docs.microsoft.com/en-us/graph/api/drive-get?view=graph-rest-1.0&tabs=http
            await users.ContinueWith(res =>
             {
                 res.Result.ToList<User>().ForEach( r =>
                 {
                     Console.WriteLine($"Display Name: {r.DisplayName} - principal name: {r.UserPrincipalName} - Account enabled ? {r.AccountEnabled}");
                     _graphClient.Users[r.Id].Drive.Request().GetAsync().ContinueWith(d => { Console.WriteLine(d.Result.WebUrl); }).Wait();                     
                     _graphClient.Users[r.Id].Drive.Root.Children.Request().GetAsync().ContinueWith(res=> {
                         res.Result.ToList<DriveItem>().ForEach(itm => {
                             Console.WriteLine($"Name: {itm.Name} size: {itm.Size} {itm.File} ");
                         });
                     });                     
                 });
                 Console.WriteLine("sdfd");
             });        

           

            if (result != null)
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);                
                var apiUri = "https://graph.microsoft.com/v1.0/users";
                // Call the web API.
                HttpResponseMessage response = await httpClient.GetAsync(apiUri);
                var str = await response.Content.ReadAsStringAsync();
                Console.WriteLine(str);

                apiUri = "https://graph.microsoft.com/v1.0/users/delta?$select=displayName,givenName,surname";

                response = await httpClient.GetAsync(apiUri);
                str = await response.Content.ReadAsStringAsync();
                Console.WriteLine(str);             

            }
        }

        /// Display the result of the Web API call
        /// </summary>
        /// <param name="result">Object to display</param>
        private static void Display(JObject result)
        {
            foreach (JProperty child in result.Properties().Where(p => !p.Name.StartsWith("@")))
            {
                Console.WriteLine($"{child.Name} = {child.Value}");
            }
        }

    }
}
