﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace MedSysProject.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal? UnitPrice { get; set; }

    public string License { get; set; }

    public string Ingredient { get; set; }

    public string Description { get; set; }

    public int? UnitsInStock { get; set; }

    public bool? Discontinued { get; set; }

    public string FimagePath { get; set; }

    public int? Likecount { get; set; }
    [JsonIgnore]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    [JsonIgnore]
    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
    [JsonIgnore]
    public virtual ICollection<ProductsClassification> ProductsClassifications { get; set; } = new List<ProductsClassification>();
    [JsonIgnore]
    public virtual ICollection<TrackingList> TrackingLists { get; set; } = new List<TrackingList>();
}