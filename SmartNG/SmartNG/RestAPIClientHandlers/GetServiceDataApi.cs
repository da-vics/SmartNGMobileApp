using Newtonsoft.Json;
using SmartNG.DataProfiles;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartNG.RestAPIClientHandlers
{
    public class GetServiceDataApi
    {

        private GetServicesProfile _getServiceData { get; set; }

        public GetServiceDataApi(GetServicesProfile getServiceData)
        {
            _getServiceData = getServiceData;
        }

        public async Task<ServicesDataProfile> GetUserServices()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var getService = new GetServiceDataProfile();
                    if (Application.Current.Properties.ContainsKey("ApiKey"))
                    {
                        var Apikey = Application.Current.Properties["ApiKey"].ToString();
                        getService.apiKey = Apikey;
                        getService.DeviceId = _getServiceData.deviceId;
                    }

                    else
                    {
                        return null;
                    }


                    var stringedProfile = await Task.Run(() => JsonConvert.SerializeObject(getService));

                    var httpContent = new StringContent(stringedProfile, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://smartng.azurewebsites.net/api/servicedata/getdata", httpContent))
                    {
                        try
                        {
                            if (response.IsSuccessStatusCode)
                            {

                                using (HttpContent check = response.Content)
                                {
                                    string test = await check.ReadAsStringAsync();

                                    var serviceResult = JsonConvert.DeserializeObject<ServicesDataProfile>(test);

                                    ///    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

                                    ///DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(serviceResult.dateInserted, cstZone);

                                    return serviceResult;
                                }
                            }

                            else
                            {
                                throw new ArgumentNullException("No Services");  /// test
                            }
                        }

                        catch (ArgumentNullException)
                        {
                            throw;
                        }


                    }
                } ///end of main CLient Http Content

            } ///try block end


            catch (ArgumentNullException)
            {
                throw;
            }

            catch (Exception args)
            {
                Console.WriteLine(args.Message);
                return null;
            }


        }

    }
}
