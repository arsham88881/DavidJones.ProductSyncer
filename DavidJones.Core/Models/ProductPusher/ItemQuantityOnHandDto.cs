using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.ProductPusher;

public class ItemQuantityOnHandDto
{
    public string? ItemBarcode { get; set; }
    public long? ItemId { get; set; }
    public DateTime? LastUpdate { get; set; }
}
