using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.UpdateProducts;

public class SiteUpdateProductResDto
{
    public SiteUpdateProductResResultDto? result { get; set; }
    public string? error { get; set; }
    public int? error_code { get; set; }
    public int? status { get; set; }
}


public class SiteUpdateProductResResultDto
{
    public SiteUpdateProductResResultProductDto? product { get; set; }
}

public class SiteUpdateProductResResultProductDto
{
    public int? id { get; set; }
}