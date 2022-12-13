﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DTO
{
    public class OrderDTO: Order
    {
        public int AccountId { get; set; }
        public string CustomerName { get; set; }
        public string StatusName { get; set; }
    }
}