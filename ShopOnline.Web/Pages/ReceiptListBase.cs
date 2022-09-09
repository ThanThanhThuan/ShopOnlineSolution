using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ReceiptListBase : ComponentBase
    {
        [Inject]
        public IProductReceiptDetailService ProductReceiptDetailService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public List<ProductReceiptDetailDto> PRDetailList { get; set; } = new List<ProductReceiptDetailDto>();
        protected override async Task OnInitializedAsync()
        {
            var ie = await ProductReceiptDetailService.GetItems();
            PRDetailList = ie.ToList();
            PRDetailList.Sort((x, y)=>  x.ProductReceiptId.CompareTo(y.ProductReceiptId));
            //return base.OnInitializedAsync();
        }

        public int currentCount = 0;

        public void Edit(int id)
        {
            NavigationManager.NavigateTo("/ProductReceipt/" + id); 
         
        }

    }
}
