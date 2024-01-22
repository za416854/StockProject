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
    /// �ഫ����JSON��r
    /// </summary>
    /// <param name="source">�ӷ�����</param>
    protected string ConvertToJson(object source)
    {
        return this._helper.ConvertToJson(source);
    }

    /// <summary>
    /// �ۭq���~�^�аT��
    /// </summary>
    /// <param name="statusCode">HTTP���A�X</param>
    /// <param name="errorCode">���~�N�X�C�|</param>
    /// <param name="errorMessage">���~�T��</param>
    protected ObjectResult CommonResponseError(int statusCode, ERROR_CODES errorCode, string errorMessage)
    {
        return StatusCode(statusCode, new CommonResponseDto<string>
        {
            returnCode = ((int)errorCode).ToString(),
            returnMessage = errorMessage
        });
    }

    /// <summary>
    /// ��Ʈw�ާ@���~�^�аT��
    /// </summary>
    protected ObjectResult CommonResponseDBError()
    {
        return CommonResponseError(StatusCodes.Status500InternalServerError, ERROR_CODES.ErrorException, "��Ʈw�ާ@�o�Ϳ��~");
    }

    /// <summary>
    /// �䤣���Ʀ^�аT��
    /// </summary>
    /// <param name="errorMessage">�ۭq���~�T��</param>
    protected ObjectResult CommonResponseNotFound(string? errorMessage = null)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            errorMessage = "�䤣����";
        }

        return CommonResponseError(StatusCodes.Status404NotFound, ERROR_CODES.ErrorNotFound, errorMessage);
    }

    /// <summary>
    /// �@�ΧR���^�аT��
    /// </summary>
    /// <param name="selectedItem">�Ŀﵧ��</param>
    /// <param name="success">���\����</param>
    /// <param name="failed">���ѵ���</param>
    /// <param name="extraMsg">�B�~�T��</param>
    protected ActionResult<CommonResponseDto<string>> CommonResponseDelete(int selectedItem, int success, int failed, string extraMsg = "")
    {
        string returnMsg = string.Format("�Ŀ�{0}���G{1}�����\�A{2}�����ѡC{3}", selectedItem, success, failed, extraMsg);


        return StatusCode(StatusCodes.Status200OK, new CommonResponseDto<string>()
        {
            returnData = returnMsg
        });
    }
    /// <summary>
    /// �@�Φ^�аT��
    /// </summary>    
    /// <param name="rtnObject">�^�и�ƪ���</param>
    /// <param name="statusCode">HTTP���A�X</param>
    protected ActionResult<CommonResponseDto<T>> CommonResponse<T>(T? rtnObject, int statusCode = 200)
    {
        return StatusCode(statusCode, new CommonResponseDto<T>()
        {
            returnData = rtnObject
        });
    }
}
