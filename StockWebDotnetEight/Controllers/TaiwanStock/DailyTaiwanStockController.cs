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
    /// �Ҥl
    /// </summary>
    /// <returns></returns>
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
            // �g�@��LOG�i�hDB

            throw;
        }
    }
    /// <summary>
    /// �s�W�Ѳ����
    /// </summary>
    /// <param name="model">��Ƽҫ�</param>
    /// <response code="200">�s�W���\</response>
	/// <response code="406">�ۦP�Ѳ���Ƥw�s�b</response>
    [HttpPost(Name = nameof(InsertDailyStock))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public ActionResult<CommonResponseDto<DailyTaiwanStockEnt>> InsertDailyStock(DailyTaiwanStockDto model)
    {
        // �ˬd�O�_��ƭ���
        //int queryCount = _accountReceiptSvc.IsBankAccountExist(model);

        //if (queryCount > 0)
        //{
        //    return CommonResponseError(StatusCodes.Status406NotAcceptable, ERROR_CODES.ErrorKeyDuplicate, "�ۦP�Ѳ���Ƥw�s�b");
        //}

        int tmrKey = _dailyTaiwanStockSvc.InsertDailyStock(model);

        if (tmrKey == 0)
        {
            //this.WriteUserAuditLog(this._commonSvc, ACTION_TYPES.Insert, $"TMR_Key={tmrKey}");

            // ���o�s�W�᪺���
            var newItem = _dailyTaiwanStockSvc.GetInsertStockByKey(model.StockSymbol);

            return CommonResponse(newItem);
        }
        else
        {
            return CommonResponseDBError();
        }
    }
}
