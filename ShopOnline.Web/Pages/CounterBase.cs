using Microsoft.AspNetCore.Components;

namespace ShopOnline.Web.Pages
{
    public class CounterBase : ComponentBase
    {
       
    public int currentCount = 0;

        public void IncrementCount()
        {
            currentCount++;
        }
    
}
}
