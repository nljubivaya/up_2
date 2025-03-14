using System;
using System.Collections.Generic;

namespace UP2.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly? DateOrder { get; set; }

    public DateOnly? DateDelivery { get; set; }

    public int? PickUp { get; set; }

    public string? Fio { get; set; }

    public int? Code { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();
}
