using System;
using System.Collections.Generic;

namespace UP2.Models;

public partial class Product
{
    public int Id { get; set; }

    public string ArticleNumber { get; set; } = null!;

    public string? Name { get; set; }

    public string? Unit { get; set; }

    public decimal? Cost { get; set; }

    public int? MaxDiscountAmount { get; set; }

    public int? Production { get; set; }

    public int? Provider { get; set; }

    public int? Kategory { get; set; }

    public decimal? CurrentDiscount { get; set; }

    public int? QuantityInStock { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual Kategory? KategoryNavigation { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Production? ProductionNavigation { get; set; }

    public virtual Provider? ProviderNavigation { get; set; }
}
