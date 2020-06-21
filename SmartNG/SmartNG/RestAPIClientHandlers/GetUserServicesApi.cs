using Newtonsoft.Json;
using SmartNG.DataProfiles;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var accesskeyprofile = new AccessKeyProfile();
                    if (Application.Current.Properties.ContainsKey("ApiKey"))
                    {
                        var Apikey = Application.Current.Properties["ApiKey"].ToString();
                        accesskeyprofile.apiKey = Apikey;
                    }

                    else
                    {
                        return null;
                    }


                    var stringedProfile = await Task.Run(() => JsonConvert.SerializeObject(accesskeyprofile));

                    var httpContent = new StringContent(stringedProfile, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://smartng.azurewebsites.net/api/user/getservices", httpContent))
                    {

                        try
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
