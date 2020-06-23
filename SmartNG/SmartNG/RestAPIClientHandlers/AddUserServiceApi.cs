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

namespace SmartNG.RestAPIClientHandlers
{

    public class AddUserServiceApi
    {
        private AddServiceProfile _addService { get; set; }
        private bool _isAddSuccessful { get; set; } = false;

        public AddUserServiceApi(AddServiceProfile addService)
        {
            _addService = addService;
        }

        public async Task<bool> addNewService()
        {

            try
            {
                using (var client = new HttpClient())
                {

                    var stringedProfile = await Task.Run(() => JsonConvert.SerializeObject(_addService));

                    var httpContent = new StringContent(stringedProfile, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://smartng.azurewebsites.net/api/adduserservice", httpContent))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            _isAddSuccessful = true;
                        }

                        else
                        {
                            _isAddSuccessful = false;

                            using (HttpContent check = response.Content)
                            {
                                string test = await check.ReadAsStringAsync();
                                RestApiErrorMessages errorMessages = await Task.Run(() => JsonConvert.DeserializeObject<RestApiErrorMessages>(test));

                                if (errorMessages.Message == "Device not Found!")
                                    throw new SmartNgHttpException("Device not Found!");

                                else if (errorMessages.Message == "ServiceName already exist")
                                    throw new SmartNgHttpException("ServiceName already exist");
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


            return _isAddSuccessful;
        }


    }

}
