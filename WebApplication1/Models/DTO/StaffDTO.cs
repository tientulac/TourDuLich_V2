using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DTO
{
    public class StaffDTO: Staff
    {
        public Account Account { get; set; }
        public string UserName { get; set; }
        public string GenderName { get; set; }
    }
}