﻿using System;
using System.Collections.Generic;

namespace UP2.Models;

public partial class Orderproduct
{
    public int Id { get; set; }

    public int Orderid { get; set; }

    public string Productarticlenumber { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductarticlenumberNavigation { get; set; } = null!;
}
