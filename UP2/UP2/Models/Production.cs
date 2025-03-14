﻿using System;
using System.Collections.Generic;

namespace UP2.Models;

public partial class Production
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
