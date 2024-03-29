﻿using Newtonsoft.Json;
using SmartNG.DataProfiles;
using SmartNG.DataProfiles.ErrorMessagesProfiles;
using SmartNG.RestAPIClientHandlers.CustomExceptions;
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
                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(_registerUser));

                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://smartng.azurewebsites.net/api/register", httpContent))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            _isRegSuccessful = true;
                        }

                        else
                        {
                            _isRegSuccessful = false;


                            using (HttpContent check = response.Content)
                            {
                                string test = await check.ReadAsStringAsync();
                                RestApiErrorMessages errorMessages = await Task.Run(() => JsonConvert.DeserializeObject<RestApiErrorMessages>(test));

                                if (errorMessages.Message.Contains($"a user with this email {_registerUser.Email}"))
                                    throw new SmartNgHttpException("a user with this email {_registerUser.Email}");
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

            return _isRegSuccessful;
        }


    }
}
