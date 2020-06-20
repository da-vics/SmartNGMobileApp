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
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
                        }

                    }
                } ///end of main CLient Http Content

            } ///try block end

            catch (Exception args)
            {
                Console.WriteLine(args.Message);
                return false;
            }


            return _isAddSuccessful;
        }


    }

}
