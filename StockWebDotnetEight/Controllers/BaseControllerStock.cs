using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Dtos;
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
    /// <summary>
    /// 轉換物件成JSON文字
    /// </summary>
    /// <param name="source">來源物件</param>
    protected string ConvertToJson(object source)
    {
        return this._helper.ConvertToJson(source);
    }

    /// <summary>
    /// 自訂錯誤回覆訊息
    /// </summary>
    /// <param name="statusCode">HTTP狀態碼</param>
    /// <param name="errorCode">錯誤代碼列舉</param>
    /// <param name="errorMessage">錯誤訊息</param>
    protected ObjectResult CommonResponseError(int statusCode, ERROR_CODES errorCode, string errorMessage)
    {
        return StatusCode(statusCode, new CommonResponseDto<string>
        {
            returnCode = ((int)errorCode).ToString(),
            returnMessage = errorMessage
        });
    }

    /// <summary>
    /// 資料庫操作錯誤回覆訊息
    /// </summary>
    protected ObjectResult CommonResponseDBError()
    {
        return CommonResponseError(StatusCodes.Status500InternalServerError, ERROR_CODES.ErrorException, "資料庫操作發生錯誤");
    }

    /// <summary>
    /// 找不到資料回覆訊息
    /// </summary>
    /// <param name="errorMessage">自訂錯誤訊息</param>
    protected ObjectResult CommonResponseNotFound(string? errorMessage = null)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            errorMessage = "找不到資料";
        }

        return CommonResponseError(StatusCodes.Status404NotFound, ERROR_CODES.ErrorNotFound, errorMessage);
    }

    /// <summary>
    /// 共用刪除回覆訊息
    /// </summary>
    /// <param name="selectedItem">勾選筆數</param>
    /// <param name="success">成功筆數</param>
    /// <param name="failed">失敗筆數</param>
    /// <param name="extraMsg">額外訊息</param>
    protected ActionResult<CommonResponseDto<string>> CommonResponseDelete(int selectedItem, int success, int failed, string extraMsg = "")
    {
        string returnMsg = string.Format("勾選{0}筆：{1}筆成功，{2}筆失敗。{3}", selectedItem, success, failed, extraMsg);


        return StatusCode(StatusCodes.Status200OK, new CommonResponseDto<string>()
        {
            returnData = returnMsg
        });
    }
    /// <summary>
    /// 共用回覆訊息
    /// </summary>    
    /// <param name="rtnObject">回覆資料物件</param>
    /// <param name="statusCode">HTTP狀態碼</param>
    protected ActionResult<CommonResponseDto<T>> CommonResponse<T>(T? rtnObject, int statusCode = 200)
    {
        return StatusCode(statusCode, new CommonResponseDto<T>()
        {
            returnData = rtnObject
        });
    }
}
