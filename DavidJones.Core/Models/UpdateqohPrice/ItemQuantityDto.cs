using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.UpdateQohPrice;

public class ItemQuantityDto
{
    public int? id { get; set; }
    public int? stock { get; set; }
    //public bool is_stock_manager { get; set; } = true;
}

public class SiteUpdateQuatityReqDto
{
    public bool? is_stock_manager { get; set; } = true;
    public int? stock_number {  get; set; }
}