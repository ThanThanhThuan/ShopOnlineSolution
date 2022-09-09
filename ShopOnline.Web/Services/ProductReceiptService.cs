using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services
{
    public class ProductReceiptService : IProductReceiptService
    {
        private readonly HttpClient httpClient;

        public ProductReceiptService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ProductReceiptDto> AddItem(ProductReceiptDto productReceiptDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<ProductReceiptDto>("api/ProductReceipt", productReceiptDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductReceiptDto);
                    }

                    return await response.Content.ReadFromJsonAsync<ProductReceiptDto>();

                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<int> DeleteItems(List<int> ids)
        {
            //var response = await httpClient.DeleteAsync("api/ProductReceipt", ids);
            //return response.Content;
            return null;
        }

        public Task<ProductReceiptDetailDto> GetDetail(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductReceiptDetailDto>> GetDetails()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductReceiptDto> GetItem(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/ProductReceipt/{id}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductReceiptDto);
                    }

                    return await response.Content.ReadFromJsonAsync<ProductReceiptDto>();
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

        public async Task<IEnumerable<ProductReceiptDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/ProductReceipt");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductReceiptDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductReceiptDto>>();
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

        public async Task<ProductReceiptDto> UpdateItem( ProductReceiptDto productReceiptDto)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(productReceiptDto);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await httpClient.PatchAsync($"api/ProductReceipt/{productReceiptDto.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProductReceiptDto>();
                }
                return null;

            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }
    }
}
