using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Omu.ValueInjecter;
using System.Reflection;
namespace StockWebDotnetEight.Services;
public interface IDailyTaiwanStockService
{
    /// <summary>
    /// 查詢前日股票收盤資料
    /// </summary>
    /// <returns></returns>
    Task<HttpResponseMessage?> QueryLastDayStockAsync();
    /// <summary>
    /// 新增特定股票資訊
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    int InsertDailyStock(DailyTaiwanStockDto entity);
    /// <summary>
    /// 取得特定股票資訊
    /// </summary>
    /// <param name="stockSymbol"></param>
    /// <returns></returns>
    DailyTaiwanStockEnt GetInsertStockByKey(int stockSymbol);

}
public class DailyTaiwanStockService : ServiceBase, IDailyTaiwanStockService
{
    public readonly IDailyTaiwanStockRepository _dailyTaiwanStockRepo;
    private readonly ILogger _logger;
    public DailyTaiwanStockService(ILogger<DailyTaiwanStockService> logger,IDailyTaiwanStockRepository dailyTaiwanStockRepo) 
        : base(logger, nameof(DailyTaiwanStockService))
    {
        this._dailyTaiwanStockRepo = dailyTaiwanStockRepo;
        _logger = logger;
    }
    /// <summary>
    /// 查詢前日股票收盤資料
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage?> QueryLastDayStockAsync()
    {
        try
        {
            string apiUrl = "https:" + "//openapi.twse.com.tw/v1/exchangeReport/MI_INDEX";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {

                    // 在這裡處理回應資料，例如將其轉換為物件或回傳給前端
                    return response;
                }
                else
                {
                    // API 呼叫失敗，返回錯誤訊息或其他處理
                    return null;
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    /// <summary>
    /// 新增特定股票資訊
    /// </summary>
    /// <param name="entity">資料物件</param>
    public int InsertDailyStock(DailyTaiwanStockDto entity)
    {
        // 由ViewModel轉換為DataModel
        var newItem = Mapper.Map<DailyTaiwanStockEnt>(entity);

        int resultKey = _dailyTaiwanStockRepo.InsertDataDailyStock(entity);

        if (resultKey > 0)
        {
            writeInfoLog(nameof(InsertDailyStock), newItem);
        }

        return resultKey;
    }
    /// <summary>
    /// 取得特定股票資訊
    /// </summary>
    /// <param name="stockSymbol"></param>
    /// <returns></returns>
    public DailyTaiwanStockEnt GetInsertStockByKey(int stockSymbol)
    {
        var returnObject = _dailyTaiwanStockRepo.GetDataByKeyTaiwanStock(stockSymbol);

        if (returnObject != null)
        {
            writeInfoLog(nameof(GetInsertStockByKey), returnObject);
        }
        return returnObject;

    }
}

