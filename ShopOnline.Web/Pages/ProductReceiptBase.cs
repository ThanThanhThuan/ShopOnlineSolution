using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ShopOnline.Web.Pages
{
    public class ProductReceiptBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IProductReceiptService ProductReceiptService { get; set; }

        [Inject]
        public IProductReceiptDetailService ProductReceiptDetailService { get; set; }

        [Inject]
        public ISupplierService SupplierService { get; set; }

        public int currentCount = 0;
        [Parameter]
        public int VoucherId { get; set; } = -1;
        public int IndexOfEditingRow { get; set; } = -1;
        public ProductReceiptDto ProductReceiptDto  { get; set; } = new ProductReceiptDto();
        public ProductReceiptDto OldProductReceiptDto { get; set; } = new ProductReceiptDto();
        public ObservableCollection<ProductReceiptDetailDto> PRDetailList { get; set; } = new ObservableCollection<ProductReceiptDetailDto>() ;
        public  bool PRDetailListRowQtyChanged { get; set; } = false;
        public bool PRDetailListRowEdited { get; set; } = false;
        public ObservableCollection<ProductReceiptDetailDto> OldPRDetailList { get; set; }
        public List<ProductDto> ProductList { get; set; } = new List<ProductDto>();
        public List<SupplierDto> SupplierList { get; set; } = new List<SupplierDto>();
        public ProductReceiptDetailDto RowToAdd { get; set; } = new ProductReceiptDetailDto();
        public string ErrorMessage { get; set; } = "";
        public bool IsRowAdd { get; set; } = true;
        public bool IsVoucherAdd { get; set; } = true;

        public EditContext? editContext;
        //public SupplierDto Supplier { get; set; }

        public decimal VoucherAmount { get; set; } = 0;
  
        protected async override Task OnInitializedAsync()
        {
            IsVoucherAdd = VoucherId == -1 ? true : false;
            var sps = await SupplierService.GetItems();
            SupplierList = sps.ToList();
            var prs = await ProductService.GetItems();
            ProductList = prs.ToList();
            RowToAdd.ProductName = ProductList[0].Name;
            if (IsVoucherAdd)
            {
                PRDetailList = new ObservableCollection<ProductReceiptDetailDto>();
                ProductReceiptDto.VoucherDate = DateTime.Today.Date;
                ProductReceiptDto.SupplierName = SupplierList[0].SupplierName;
            }
            else //edit mode
            {
                PRDetailList = new ObservableCollection<ProductReceiptDetailDto>();
                ProductReceiptDto = await ProductReceiptService.GetItem(VoucherId);
               var pds =  await ProductReceiptDetailService.GetItemsByVoucherId(VoucherId);
                PRDetailList = new ObservableCollection<ProductReceiptDetailDto>(pds);
                OldPRDetailList = new ObservableCollection<ProductReceiptDetailDto>(PRDetailList);
                OldProductReceiptDto =new ProductReceiptDto()
                {
                    Id = ProductReceiptDto.Id,
                    VoucherDate = ProductReceiptDto.VoucherDate,
                    SupplierId = ProductReceiptDto.SupplierId,
                    SupplierName = ProductReceiptDto.SupplierName
                };
            }
           
            PRDetailList.CollectionChanged += PRDetailList_CollectionChanged;


            //RowToAdd.Qty = 1;
            //editContext = new (RowToAdd );
            //editContext = new EditContext(RowToAdd);
            //editContext.OnFieldChanged += EditContext_OnFieldChanged;
            // return base.OnInitializedAsync();
        }
        protected override async Task OnParametersSetAsync()
        {
            //force refresh when change paras
            PRDetailList.CollectionChanged -= PRDetailList_CollectionChanged;
            await OnInitializedAsync();
            PRDetailList.CollectionChanged += PRDetailList_CollectionChanged;
        }
        public async Task HandleOnValidSubmit()
        {
         
                try
                {
                    await ProductReceiptService.UpdateItem(ProductReceiptDto);
                    //ToastService.ShowSuccess("Your recipe has been saved successfully", "Success!");
                }
                catch (Exception)
                {
                    //ToastService.ShowError("Something went wrong while saving your recipe", "Uh oh!");
                }
            
          
        }
        void PRDetailList_CollectionChanged(object sender,System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PRDetailListRowQtyChanged = true;
            return;
            //-----
            //list changed - an item was added.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                //Do what ever you want to do when an item is added here...
                //the new items are available in e.NewItems
                //              ?e.NewItems._item.ProductName
                //'Glossier - Beauty Kit'
                // e.NewItems.
               // var k = (ProductReceiptDetailDto)e.NewItems[0];
//               ?k.ProductName
//'Canon Digital Camera'
                // var k = e.NewItems.Cast<System.Collections.Specialized.SingleItemReadOnlyList>();
                //Console.WriteLine(e.NewItems[0].ToString);
                //Console.WriteLine(e.NewItems[1].ToString);
                //Console.WriteLine(e.NewItems[2].ToString);
                //                When you use the NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction, object) constructor, the changedItem will either be in the NewItems collection(if you specify NotifyCollectionChangedAction.Add) or the OldItems collection(if you specify NotifyCollectionChangedAction.Remove).

                //If you specify NotifyCollectionChangedAction.Reset, the changedItem parameter must be null, otherwise you'll get an ArgumentException.

                //If you specify any other NotifyCollectionChangedAction value, you'll also get an ArgumentException.
            }
        }
        public async Task AddToPRs()
        {
            if (!IsVoucherAdd) //Edit Mode
            {
                ProductReceiptDto.SupplierId = SupplierList.First(x => x.SupplierName == ProductReceiptDto.SupplierName).Id;
                bool NoDetailChange =false;
                bool NoVoucherChange = false;
                List<ProductReceiptDetailDto> liEditedRow = new List<ProductReceiptDetailDto>();
                List<int> liIdRemoved = new List<int>();
                List<int> liIdNew = new List<int>();
                List<ProductReceiptDetailDto> liNewRow = new List<ProductReceiptDetailDto>();
                if (PRDetailListRowQtyChanged)
                {
                    //Added- Removed Rows
                    List<int> liIdOld = new List<int>();
                    foreach (var item in OldPRDetailList)
                    {
                        liIdOld.Add(item.Id);
                    }
                  
                  
                  
                    foreach (var item in PRDetailList)
                    {
                        if (item.Id==0)
                        {
                            item.ProductId = ProductList.First(x => x.Name == item.ProductName).Id;
                            item.ProductReceiptId = VoucherId;
                            liNewRow.Add(item);
                        }
                        liIdNew.Add(item.Id);
                    }
                    foreach (var item in liIdOld)
                    {
                        if (!liIdNew.Contains(item))
                        {
                            liIdRemoved.Add(item);
                        }
                        else
                        {
                            liEditedRow.Add(liNewRow.FirstOrDefault(x => x.Id == item));
                        }
                    }
                   // await Js.InvokeAsync<bool>("confirm", "liNewRow.Count=" + liNewRow.Count + ",liIdRemoved.Count=" + liIdRemoved.Count); // Confirm

                }
                else if (PRDetailListRowEdited)
                {//Only Edit
                    liEditedRow = new List<ProductReceiptDetailDto>(PRDetailList);
                }
                else
                {
                    NoDetailChange = true;
                }
                // await Js.InvokeAsync<bool>("confirm", "PRDetailListRowQtyChanged=" + PRDetailListRowQtyChanged.ToString()); // Confirm
                if (OldProductReceiptDto. Id == ProductReceiptDto.Id &&
                    OldProductReceiptDto.VoucherDate == ProductReceiptDto.VoucherDate &&
                    OldProductReceiptDto.SupplierName == ProductReceiptDto.SupplierName)
                {
                    NoVoucherChange= true;
                }
                //---------
                if (NoDetailChange && NoVoucherChange)
                {
                    await Js.InvokeVoidAsync("alert", "No Change to Update!"); // Confirm
                }
                else //Edit--------------
                {
                    if (!NoVoucherChange)
                    {
                        await ProductReceiptService.UpdateItem(ProductReceiptDto);
                    }
                    if (PRDetailListRowEdited)
                    {
                        await this.ProductReceiptDetailService.UpdateItems(liEditedRow);
                    }
                    foreach (var item in liIdRemoved)
                    {
                        await this.ProductReceiptDetailService.DeleteItem(item);
                    }
                    if (liNewRow.Count>0)
                    {
                        await this.ProductReceiptDetailService.AddItems(liNewRow);
                    }
                   
                    await Js.InvokeVoidAsync("alert", "Updated #" + VoucherId +"!"); // Confirm
                    //string prompted = await JsRuntime.InvokeAsync<string>("prompt", "Take some input:"); // Prompt
                }
                return;
              
            }
          
            //case Add New--------------------------------
            try
            {
                ProductReceiptDto.SupplierId = SupplierList.First(x => x.SupplierName == ProductReceiptDto.SupplierName).Id;
                var cartItemDto = await ProductReceiptService.AddItem(ProductReceiptDto);
                var id = cartItemDto.Id;
                foreach (var item in PRDetailList)
                {
                    item.ProductReceiptId = id;
                    item.ProductId = ProductList.First(x => x.Name==item.ProductName).Id;
             
                    await ProductReceiptDetailService.AddItem(item);
                }

                //Console.Log("id="+cartItemDto.ID);
                bool confirmed = await Js.InvokeAsync<bool>("confirm", "saved "  + cartItemDto.Id+ " with " + PRDetailList.Count + " row(s)"); // Confirm
                ToastService.ShowSuccess("Your Product Receipt has been saved successfully" + cartItemDto.Id, "Success!");
                Reset();
                //if (cartItemDto != null)
                //{
                //    ShoppingCartItems.Add(cartItemDto);
                //  //  await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                //}

                //NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {
                //ErrorMessage = Exception.ToString();
                throw;
                //ToastService.ShowError("Something went wrong while saving your recipe", "Uh oh!");
                //ErrorMessage = "Error Adding!";
                //Log Exception
            }
        }
        public void Reset()
        {
            ProductReceiptDto = new ProductReceiptDto()
            {
                VoucherDate = DateTime.Today.Date,
                SupplierName = SupplierList[0].SupplierName
            };
            PRDetailList = new ObservableCollection<ProductReceiptDetailDto>();
        }
        public void IncrementCount()
        {
            currentCount++;
        }
        public void AddRow()
        {
            // Js.InvokeAsync<bool>("confirm", "SupplierList.Count,Id " + SupplierList.Count +","+ SupplierList.First(x => x.SupplierName == Supplier.SupplierName).Id); // Confirm
            //Console.WriteLine("SP=" + SupplierList.First(x => x.SupplierName == Supplier.SupplierName).Id);
            //  ToastService.ShowSuccess("Added!");
            PRDetailList.Add(new ProductReceiptDetailDto { ProductName = RowToAdd.ProductName, Qty = RowToAdd.Qty, Price = RowToAdd.Price, TotalAmount = RowToAdd.TotalAmount });
            // PRDetailList.Add(RowToAdd);
            VoucherAmount=PRDetailList.Sum(x => x.TotalAmount);
            RowToAdd = new ProductReceiptDetailDto();
            RowToAdd.ProductName = ProductList[0].Name;
        }
        public void DoneEdit()
        {
            RowToAdd. ProductId = ProductList.First(x => x.Name == RowToAdd.ProductName).Id;
            PRDetailList[ IndexOfEditingRow].ProductId = RowToAdd.ProductId;
            VoucherAmount = PRDetailList.Sum(x => x.TotalAmount);
            IsRowAdd = true;
            RowToAdd = new ProductReceiptDetailDto();
            RowToAdd.ProductName = ProductList[0].Name;
            PRDetailListRowEdited = true;
        }
        public void Edit(ProductReceiptDetailDto item) {
            //Console.WriteLine($"Edit item: {item.ProductName}");
            IsRowAdd = false;
            RowToAdd = item;
             IndexOfEditingRow= PRDetailList.IndexOf(item);
    }
        public void Remove(ProductReceiptDetailDto item)
        {
            //{ Console.WriteLine($"Remove item: {item.Name}"); }
            PRDetailList.Remove(item);
            VoucherAmount = PRDetailList.Sum(x => x.TotalAmount);
        }
        public void CalcAmountQ(ChangeEventArgs args)

        {
               //Console.WriteLine($"RowToAdd.Qty: {RowToAdd.Qty}");
            if (args!=null)
            {
                Console.WriteLine($"RowToAdd.Qty0: " + args.Value);

                RowToAdd.TotalAmount = Convert.ToInt32(args.Value) * RowToAdd.Price;
                VoucherAmount = PRDetailList.Sum(x => x.TotalAmount);
            }
           
        }
        public void CalcAmountP(ChangeEventArgs args)

        {

            if (args != null)
            {
                Console.WriteLine($"RowToAdd.Price: " + args.Value);

                RowToAdd.TotalAmount = Convert.ToDecimal(args.Value) * RowToAdd.Qty;
                VoucherAmount = PRDetailList.Sum(x => x.TotalAmount);
            }

        }
        public void QtyChanged(int value)
        {
            RowToAdd.Qty = value;
            RowToAdd.TotalAmount = RowToAdd.Qty * RowToAdd.Price;
        }
        private void EditContext_OnFieldChanged(object sender,
            FieldChangedEventArgs e)
        {
            Console.WriteLine(e.FieldIdentifier.FieldName);

        }
        public void DeleteVoucher()
        {
           List<int> ids = new List<int>() { VoucherId};
           // ProductReceiptService.De
        }
    }
}
