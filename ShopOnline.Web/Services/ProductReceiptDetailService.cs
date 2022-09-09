using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services
{
    public class ProductReceiptDetailService : IProductReceiptDetailService
    {
        private readonly HttpClient httpClient;
        public ProductReceiptDetailService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ProductReceiptDetailDto> AddItem(ProductReceiptDetailDto productReceiptDetailDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<ProductReceiptDetailDto>("api/ProductReceiptDetail", productReceiptDetailDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductReceiptDetailDto);
                    }

                    return await response.Content.ReadFromJsonAsync<ProductReceiptDetailDto>();

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

        public async Task<IEnumerable<ProductReceiptDetailDto>> AddItems(IEnumerable<ProductReceiptDetailDto> productReceiptDetailDtos)
        {
            //return new List<ProductReceiptDetailDto>();
            try
            {
                var response = await httpClient.PostAsJsonAsync<IEnumerable<ProductReceiptDetailDto>>("api/ProductReceiptDetail/1",productReceiptDetailDtos);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return new List<ProductReceiptDetailDto>();
                    }

                    return await response.Content.ReadFromJsonAsync< IEnumerable<ProductReceiptDetailDto>>();

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

        public async Task DeleteItem(int id)
        {
            try
            {
                await httpClient.DeleteAsync($"api/ProductReceiptDetail/{id}");

              
                return ;
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public Task<ProductReceiptDetailDto> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductReceiptDetailDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/ProductReceiptDetail");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductReceiptDetailDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductReceiptDetailDto>>();
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

        public async Task<IEnumerable<ProductReceiptDetailDto>> GetItemsByVoucherId(int voucherid)
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/ProductReceiptDetail/0/" + voucherid);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductReceiptDetailDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductReceiptDetailDto>>();
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

        public Task<ProductReceiptDetailDto> UpdateItem(ProductReceiptDetailDto ProductReceiptDetailDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductReceiptDetailDto> UpdateItems(List<ProductReceiptDetailDto> productReceiptDetailDtos)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(productReceiptDetailDtos);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await httpClient.PatchAsync($"api/ProductReceiptDetail", content);

                if (response.IsSuccessStatusCode)
                {
                    return null;
                    // return await response.Content.ReadFromJsonAsync<List<ProductReceiptDetailDto>>();
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
