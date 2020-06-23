using Android.App;
using Newtonsoft.Json;
using SmartNG.DataProfiles;
using SmartNG.DataProfiles.ErrorMessagesProfiles;
using SmartNG.RestAPIClientHandlers.CustomExceptions;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Application = Xamarin.Forms.Application;

namespace SmartNG.RestAPIClientHandlers
{
    public class GetUserServicesApi
    {

        public GetUserServicesApi()
        {

        }

        public async Task<ObservableCollection<GetServicesProfile>> GetUserServices()
        {

            try
            {
                using (var client = new HttpClient())
                {

                    var accesskeyprofile = new AccessKeyProfile();

                    if (Application.Current.Properties.ContainsKey("ApiKey"))
                    {
                        var Apikey = Application.Current.Properties["ApiKey"].ToString();
                        accesskeyprofile.apiKey = Apikey;
                    }

                    else
                    {
                        return null;   // test..
                    }

                    var stringedProfile = await Task.Run(() => JsonConvert.SerializeObject(accesskeyprofile));

                    var httpContent = new StringContent(stringedProfile, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://smartng.azurewebsites.net/api/user/getservices", httpContent))
                    {

                        if (response.IsSuccessStatusCode)
                        {

                            using (HttpContent check = response.Content)
                            {
                                string test = await check.ReadAsStringAsync();

                                var serviceList = JsonConvert.DeserializeObject<ObservableCollection<GetServicesProfile>>(test);

                                serviceList.ForEach((service) =>
                                {
                                    if (service.serviceName.Contains("_"))
                                    {
                                        var num = service.serviceName.LastIndexOf('_');
                                        service.serviceName = service.serviceName.Remove(num);
                                    }

                                });
                                return serviceList;
                            }
                        }

                        else
                        {
                            using (HttpContent check = response.Content)
                            {
                                string test = await check.ReadAsStringAsync();
                                RestApiErrorMessages errorMessages = await Task.Run(() => JsonConvert.DeserializeObject<RestApiErrorMessages>(test));

                                if (errorMessages.Message == "No Services")
                                    throw new SmartNgHttpException("No Services");
                            }


                            return null;
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
                return null;
            }

            catch (ArgumentNullException args)
            {
                Console.WriteLine(args.Message);
                return null;
            }

            catch (Exception args)
            {
                Console.WriteLine(args.Message);
                return null;
            }


        }

    }
}
