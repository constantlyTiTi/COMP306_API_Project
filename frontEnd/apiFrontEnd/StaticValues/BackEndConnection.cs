﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiFrontEnd.StaticValues
{
    public class BackEndConnection
    {
        public static readonly string Authentication = "Authentication";
        public static readonly string BaseUrl = @"https://localhost:44378/api/";

        public static readonly string loginUrl = @"home/login";
        public static readonly string registerUrl = @"home/Register";
        public static readonly string logoutUrl = @"home/logout";

        public static readonly string mainWindow_allItem = "item/all-item";
        public static readonly string mainWindow_items = "item/items";
    }
}
