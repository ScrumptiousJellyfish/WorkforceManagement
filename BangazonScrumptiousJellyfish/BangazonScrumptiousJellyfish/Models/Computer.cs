using System;
using System.Collections.Generic;

namespace Workforce.Models
{
    public class Computer
    {
        public int Id { get; set; }
        public int PurchaseDate { get; set; }
        public DateTime DecommissionDate { get; set; }
        public bool Status { get; set; }

    }

}