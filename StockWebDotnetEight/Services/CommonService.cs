using Omu.ValueInjecter;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;

namespace StockWebDotnetEight.Services;

/// <summary>
/// 共用服務介面
/// </summary>
public interface ICommonService
{

}
public class CommonService : ServiceBase, ICommonService
{
    private readonly ICommonRepository _commonRepo;

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="logger">記錄物件</param>
    /// <param name="commonRepo">資料物件</param>
    public CommonService(ILogger<CommonService> logger, ICommonRepository commonRepo) : base(logger, nameof(CommonService))
    {
        this._commonRepo = commonRepo;
    }
}