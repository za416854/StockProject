using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockWebDotnetEight.Helpers;

namespace StockWebDotnetEight.Controllers;

public class BaseControllerStock : ControllerBase
{
    private readonly IHttpContextAccessor _accessor;
    private readonly UtilityHelper _helper;

    public BaseControllerStock(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        _helper = new UtilityHelper();
    }


}
