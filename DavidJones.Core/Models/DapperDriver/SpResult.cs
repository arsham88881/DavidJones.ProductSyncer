using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.DapperDriver;

public record class SpResult
{
    public string? TrackingId { get; set; } //شناسه برای پیگیری خطا ها ایونت ها
    public SpResult() => ErrorHandler = new ErrorManager();
    public string? OutputJson { get; set; } = null;
    public long? Id { get; set; } = null;
    public ErrorManager ErrorHandler { get; set; }
    public bool IsSuccess { get; set; } //زمانی قابل استفاده میشود که در spOption بزنی که میخواهی دستی خطا ها رو مدیریت کنی
}
public record class SpResult<OUTPUT>
{
    public string? TrackingId { get; set; } //شناسه برای پیگیری خطا ها ایونت ها
    public SpResult() => ErrorHandler = new ErrorManager();
    public long? TotalCount { get; set; } = null;
    public ErrorManager ErrorHandler { get; set; }
    public OUTPUT? Value { get; set; } = default;
    public bool IsSuccess { get; set; } //زمانی قابل استفاده میشود که در spOption بزنی که میخواهی دستی خطا ها رو مدیریت کنی
}