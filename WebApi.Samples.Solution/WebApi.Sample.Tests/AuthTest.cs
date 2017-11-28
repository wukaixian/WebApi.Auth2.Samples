using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using Xunit;

namespace WebApi.Sample.Tests
{
    
    public class AuthTest
    {
        private HttpClient _client=new HttpClient();
        
        public AuthTest()
        {
            _client.BaseAddress=new Uri("http://localhost");  
            _client.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Basic",Convert.ToBase64String(Encoding.ASCII.GetBytes("wkx"+":"+"123")));
        }

        [Fact]
        public void OAuth_Password_Test()
        {
            
            var parameters=new Dictionary<string,string>();
            parameters.Add("username","wkx");
            parameters.Add("password","123");
            parameters.Add("grant_type","password");

            var resp=_client.PostAsync("/api/token", new FormUrlEncodedContent(parameters)).Result;

            Assert.Equal(HttpStatusCode.OK,resp.StatusCode);
            Assert.False(string.IsNullOrEmpty(resp.Content.ReadAsStringAsync().Result));
        }

        [Fact]
        public void OAuth_Client_Crdentials_Grant_Test()
        {
            var parameters=new Dictionary<string,string>();
            parameters.Add("grant_type","client_credentials");
            var resp=_client.PostAsync("/api/token", new FormUrlEncodedContent(parameters)).Result;
            //var resp = _client.GetAsync("/api/token?grant_type=client_credentials").Result;
            Assert.Equal(HttpStatusCode.OK,resp.StatusCode);

            Trace.WriteLine(resp.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void OAuth_Client_Refresh_Token_Test()
        {
            var parmeters=new Dictionary<string,string>();
            parmeters.Add("grant_type","client_credentials");
            var resp=_client.PostAsync("/api/token", new FormUrlEncodedContent(parmeters)).Result;

            Assert.Equal(HttpStatusCode.OK,resp.StatusCode);
            Trace.WriteLine(resp.Content.ReadAsStringAsync().Result);

            System.Threading.Thread.Sleep(3000);
            
            parmeters.Clear();
            parmeters.Add("grant_type", "refresh_token");
            var token = JObject.Parse(resp.Content.ReadAsStringAsync().Result).Value<string>("refresh_token");
            parmeters.Add("refresh_token", token);

            resp=_client.PostAsync("/api/token", new FormUrlEncodedContent(parmeters)).Result;

            Assert.Equal(HttpStatusCode.OK,resp.StatusCode);

            Trace.WriteLine(resp.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void Action_Auth_Test()
        {
            var parmeters = new Dictionary<string, string>();
            parmeters.Add("grant_type", "client_credentials");
            var resp = _client.PostAsync("/api/token", new FormUrlEncodedContent(parmeters)).Result;
            var accessToken = JObject.Parse(resp.Content.ReadAsStringAsync().Result).Value<string>("access_token");
            var refreshToken = JObject.Parse(resp.Content.ReadAsStringAsync().Result).Value<string>("refresh_token");
            Thread.Sleep(3000);

            _client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", accessToken);
            resp = _client.GetAsync("/api/values").Result;

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                parmeters.Clear();
                parmeters.Add("grant_type","refresh_token");
                parmeters.Add("refresh_token",refreshToken);
                resp=_client.PostAsync("/api/token", new FormUrlEncodedContent(parmeters)).Result;
                accessToken = JObject.Parse(resp.Content.ReadAsStringAsync().Result).Value<string>("access_token");
            }

            _client.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("bearer",accessToken);
            resp=_client.GetAsync("/api/values").Result;
            Assert.Equal(HttpStatusCode.OK,resp.StatusCode);

            Trace.WriteLine(resp.Content.ReadAsStringAsync().Result);
        }
    }
}
