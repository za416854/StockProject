using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using StockWebDotnetEight.Services;
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
    /// <summary>
    /// 例子
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = nameof(TaiwanStockPost))] 
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
            var resBody = queryResult.Content.ReadAsStringAsync();
            return Ok(resBody);
        }
        catch (Exception ex)
        {
            // 寫一筆LOG進去DB

            throw;
        }
    }
    /// <summary>
    /// 新增股票資料
    /// </summary>
    /// <param name="model">資料模型</param>
    /// <response code="200">新增成功</response>
	/// <response code="406">相同股票資料已存在</response>
    [HttpPost(Name = nameof(InsertDailyStock))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public ActionResult<CommonResponseDto<DailyTaiwanStockEnt>> InsertDailyStock(DailyTaiwanStockDto model)
    {
        // 檢查是否資料重複
        //int queryCount = _accountReceiptSvc.IsBankAccountExist(model);

        //if (queryCount > 0)
        //{
        //    return CommonResponseError(StatusCodes.Status406NotAcceptable, ERROR_CODES.ErrorKeyDuplicate, "相同股票資料已存在");
        //}

        int tmrKey = _dailyTaiwanStockSvc.InsertDailyStock(model);

        if (tmrKey == 0)
        {
            //this.WriteUserAuditLog(this._commonSvc, ACTION_TYPES.Insert, $"TMR_Key={tmrKey}");

            // 取得新增後的資料
            var newItem = _dailyTaiwanStockSvc.GetInsertStockByKey(model.StockSymbol);

            return CommonResponse(newItem);
        }
        else
        {
            return CommonResponseDBError();
        }
    }
}
