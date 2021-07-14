﻿using System;
using System.Collections.Generic;

#nullable disable

namespace InventoryManagementSystem.Models.EF
{
    public partial class PaymentOrder
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
