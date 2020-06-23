using Newtonsoft.Json;
using SmartNG.DataProfiles;
using SmartNG.DataProfiles.ErrorMessagesProfiles;
using SmartNG.RestAPIClientHandlers.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartNG.RestAPIClientHandlers
{
    public class LoginUserApi
    {

        private LoginProfile _LoginProfile { get; set; }
        private bool _isLogSuccessful { get; set; } = false;


        public LoginUserApi(LoginProfile loginProfile)
        {
            _LoginProfile = loginProfile;
        }

        public async Task<bool> LoginUser()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    var stringedProfile = await Task.Run(() => JsonConvert.SerializeObject(_LoginProfile));

                    var httpContent = new StringContent(stringedProfile, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://smartng.azurewebsites.net/api/login", httpContent))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            _isLogSuccessful = true;

                            using (HttpContent check = response.Content)
                            {
                                string test = await check.ReadAsStringAsync();
                                ApiKeyProfile accessKey = await Task.Run(() => JsonConvert.DeserializeObject<ApiKeyProfile>(test));
                                Application.Current.Properties["ApiKey"] = accessKey.Key;
                                await Application.Current.SavePropertiesAsync();
                            }
                        }

                        else
                        {
                            _isLogSuccessful = false;

                            using (HttpContent check = response.Content)
                            {
                                string test = await check.ReadAsStringAsync();
                                RestApiErrorMessages errorMessages = await Task.Run(() => JsonConvert.DeserializeObject<RestApiErrorMessages>(test));

                                if (errorMessages.Message == "User Not Found")
                                    throw new SmartNgHttpException("User Not Found");
                            }
                        }

                    }
                } ///end of main CLient Http Content

            } ///try block end

            catch (SmartNgHttpException)
            {
                throw;
            }


            catch (HttpRequestException args)
            {
                Console.WriteLine(args.Message);
                return false;
            }

            catch (ArgumentNullException args)
            {
                Console.WriteLine(args.Message);
                return false;
            }

            catch (Exception args)
            {
                Console.WriteLine(args.Message);
                return false;
            }


            return _isLogSuccessful;
        }

    } ///LoginUserApi End
}
