using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.UpdateVariants;

public class SiteBulkUpdateResDto
{
    public List<object>? success_variants { get; set; }
    public int[]? failed_variant_ids { get; set; }

}
