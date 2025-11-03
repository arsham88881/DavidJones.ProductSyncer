using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Models.DapperDriver;

public class ErrorManager
{
    public string? ErrorMessage { get; set; }
    public int? ErrorNumber { get; set; }
    public ErrorManager()
    {
        ErrorNumber = 0;
        ErrorMessage = "عملیات با موفقیت انجام شد.";
    }
    public ErrorManager(int errorNumber, string errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorNumber = errorNumber;
    }
    ///// <summary>
    ///// true = (you have error) false = (not error you safe)
    ///// </summary>
    //public bool CheckError(bool isExceptionHandledManual = false)
    //{
    //    if (ErrorNumber is null || ErrorNumber == 0)
    //        return false;
    //    else if (ErrorNumber >= 50210 && ErrorNumber <= 50299) //errors you returnd mangually logical
    //        return true;
    //    else if (ErrorNumber == 2601 || ErrorNumber == 2627 || ErrorNumber == 547 || ErrorNumber >= 51000) //errors validations
    //    {
    //        if (!isExceptionHandledManual) throw new CentralException(ErrorMessage);
    //        return true;
    //    }
    //    else if (ErrorNumber < 50000) //you have unhandled sql error
    //    {
    //        if (!isExceptionHandledManual) throw new DatabaseException(ErrorMessage);
    //        return true;
    //    }
    //    return false;
    //}
}