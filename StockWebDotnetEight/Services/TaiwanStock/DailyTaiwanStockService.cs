using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;

namespace StockWebDotnetEight.Services.TaiwanStock;
public interface IDailyTaiwanStockService
{
    Task<HttpResponseMessage?> QueryLastDayStockAsync();
}
public class DailyTaiwanStockService : ServiceBase, IDailyTaiwanStockService
{
    public readonly IDailyTaiwanStockRepository _dailyTaiwanStockRepo;
    public DailyTaiwanStockService(IDailyTaiwanStockRepository dailyTaiwanStockRepo)
    {
        this._dailyTaiwanStockRepo = dailyTaiwanStockRepo;
    }

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

}

