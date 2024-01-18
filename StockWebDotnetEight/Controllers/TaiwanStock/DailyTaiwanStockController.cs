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
            // �b�o�̥i�H����@��²�檺�޿�Ϊ�^���եΪ��ƾ�
            var testData = new { message = "Test GET request successful." };

            return Ok(testData);
        }
        catch (Exception ex)
        {
            // �O���ҥ~�A�ήھڹ�ڻݨD�i��B�z
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
