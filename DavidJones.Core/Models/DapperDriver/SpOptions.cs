using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.DapperDriver;

public record class SpOptions
{
    public SpOptions(string name) => Name = name;
    public SpOptions() { }
    public SpOptions(
        string name
       , bool hasTotalCount = false
       , bool hasOutputJson = false
       , bool hasOutputId = false
       , int timeout = 5000)
    {
        Name = name;
        HasTotalCount = hasTotalCount;
        HasOutputJson = hasOutputJson;
        HasOutputId = hasOutputId;
        Timeout = timeout;
    }
    public string Name { get; set; }
    public bool HasTotalCount { get; set; } = false;
    public bool HasOutputJson { get; set; } = false;
    public bool HasOutputId { get; set; } = false;
    public int? Timeout { get; set; } = 5000;
}
