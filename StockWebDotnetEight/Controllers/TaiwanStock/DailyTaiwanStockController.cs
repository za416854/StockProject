using Microsoft.AspNetCore.Mvc;
using StockWebDotnetEight.Services;
using StockWebDotnetEight.Services.TaiwanStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockWebDotnetEight.Controllers.TaiwanStock;
[Route("api/[controller]")]
[ApiController]
public class DailyTaiwanStockController : BaseControllerStock
{
    private readonly IDailyTaiwanStockService _dailyTaiwanStockSvc;
    private readonly ICommonService _commonSvc;

    public DailyTaiwanStockController(IDailyTaiwanStockService dailyTaiwanStockService, ICommonService commonService
        , IHttpContextAccessor accessor) :base(accessor)
    {
        this._dailyTaiwanStockSvc = dailyTaiwanStockService;
        this._commonSvc = commonService;
    }

    [HttpGet]
    [Route(nameof(TestGet))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult TestGet()
    {
        try
        {
            // 在這裡可以執行一些簡單的邏輯或返回測試用的數據
            var testData = new { message = "Test GET request successful." };

            return Ok(testData);
        }
        catch (Exception ex)
        {
            // 記錄例外，或根據實際需求進行處理
            Console.Error.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }

    [HttpPost(Name = nameof(TaiwanStockPost))] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> TaiwanStockPost()
    {
        try
        {
            var queryResult = await _dailyTaiwanStockSvc.QueryLastDayStockAsync();
            if (queryResult == null)
            {
                  return NotFound();
            }
            return Ok(queryResult);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

}
