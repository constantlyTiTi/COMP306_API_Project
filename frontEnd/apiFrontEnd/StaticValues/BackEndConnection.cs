using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiFrontEnd.StaticValues
{
    public class BackEndConnection
    {
        public static readonly string BaseUrl = @"https://localhost:44378/api/";

        public static readonly string loginUrl = @"home/login";
        public static readonly string registerUrl = @"home/Register";
        public static readonly string logoutUrl = @"home/logout";

        public static readonly string mainWindow_allItem = @"item/all-item";
        public static readonly string mainWindow_items = @"item/items";
        public static readonly string mainWindow_itemDetail = @"item/";

        public static readonly string OrderWindow_Order_userName = @"order/user-orders/";
        public static readonly string OrderWindow_Order_orderId = @"order/";

        public static readonly string ShoppingCartWindow_allItem = @"shopping_cart/all-items/";
        public static readonly string ShoppingCartWindow_Item = @"shopping_cart/";
        public static readonly string ShoppingCartWindow_Item_update = @"shopping_cart/update-cart/";
        public static readonly string ShoppingCartWindow_PlaceOrder = @"shopping_cart/place-order";
        public static readonly string ShoppingCartWindow_GenerateRandomId = @"shopping_cart/generate-unique_tempor_user_id";
    }
}
