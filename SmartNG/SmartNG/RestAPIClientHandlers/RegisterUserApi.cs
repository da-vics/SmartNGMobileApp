using Newtonsoft.Json;
using SmartNG.DataProfiles;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartNG.RestAPIClientHandlers
{
    public class RegisterUserApi
    {
        private RegistrationProfile _registerUser { get; set; }
        private bool _isRegSuccessful { get; set; } = false;

        public RegisterUserApi(RegistrationProfile registerUser)
        {
            _registerUser = registerUser;
        }

        public async Task<bool> RegisterUser()
        {


            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(_registerUser));

                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://smartng.azurewebsites.net/api/register", httpContent))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            _isRegSuccessful = true;

                            //using (HttpContent check = response.Content)
                            //{
                            //    string test = await check.ReadAsStringAsync();
                            //}
                        }

                        else
                        {
                            _isRegSuccessful = false;
                        }

                    }
                } ///end of main CLient Http Content

            } ///try block end

            catch (Exception args)
            {
                Console.WriteLine(args.Message);
                return false;
            }


            return _isRegSuccessful;
        }


    }
}
