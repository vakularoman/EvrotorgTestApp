using System;
using EvrotorgApp.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EvrotorgApp
{
    public class CurrencyHttpClient
    {
        private readonly HttpClient _httpClient;

        public CurrencyHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool, IEnumerable<RateViewModel>)> GetRateByDateAsync(DateTime date)
        {
            try
            {
                var response = await _httpClient.GetAsync($"rates?periodicity=0&ondate={date:yyyy-M-d}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    IEnumerable<RateViewModel> result = default;

                    await Task.Run(() =>
                        result = JsonConvert.DeserializeObject<IEnumerable<RateViewModel>>(resultString)).ConfigureAwait(false);

                    return (true, result);
                }
            }
            catch (Exception)
            {
                // Log exception
                return (false, default);
            }

            return (false, default);
        }
    }
}
