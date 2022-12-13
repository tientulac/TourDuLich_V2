using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DTO
{
    public class TicketDTO: Ticket
    {
        public string TourName { get; set; }
        public int AccountId { get; set; }
    }
}