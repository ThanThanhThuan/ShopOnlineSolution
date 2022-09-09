using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly HttpClient httpClient;
        public SupplierService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<SupplierDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Supplier");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<SupplierDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<SupplierDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }

            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }
    }
}
