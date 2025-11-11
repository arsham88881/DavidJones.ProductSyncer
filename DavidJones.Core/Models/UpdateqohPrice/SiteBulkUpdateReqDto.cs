using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.UpdateQohPrice;

public class SiteBulkUpdateReqDto<T>
{
    public List<T> variants { get; set; } = [];
}
