using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;
using Domain.Entities;
using static Dapper.SqlMapper;
using Domain.Dtos;
//using TaiwanLife.RES.Domain.Entities;
namespace DataAccess.Repositories;

public interface IDailyTaiwanStockRepository
{
    /// <summary>
    /// 取得股市前一天收盤資訊
    /// </summary>
    /// <returns></returns>
    DataTable GetTaiwanStockData();
    /// <summary>
    /// 新增股票資訊
    /// </summary>
    /// <returns></returns>
    int InsertDataDailyStock(DailyTaiwanStockDto entity);
    /// <summary>
    /// 取得特定股票資訊
    /// </summary>
    /// <returns></returns>
    DailyTaiwanStockEnt GetDataByKeyTaiwanStock(int stockSymbol);
}
public class DailyTaiwanStockRepository : DapperRepositoryBase, IDailyTaiwanStockRepository
{
    public DailyTaiwanStockRepository(ILogger<DailyTaiwanStockRepository> logger, DapperContext context) : base(logger, context, nameof(DailyTaiwanStockRepository))
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public DataTable GetTaiwanStockData()
    {
        return new DataTable();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int InsertDataDailyStock(DailyTaiwanStockDto entity)
    {
        var sql = @"
                    INSERT INTO TaiwanStock
                    (stockSymbol, stockName, closingPrice, changeAmount, changePercentage, specialNote, TradeDate, CreateUser, CreateDate, UpdateDate)
                    VALUES
                    (@stockSymbol, @stockName, @closingPrice, @changeAmount, @changePercentage, @specialNote, GETDATE(), 'Kris', GETDATE(), GETDATE()); 
                "
        ; 
        writeDebugCommand(nameof(InsertDataDailyStock), sql, entity); 
        return QuerySingleOrDefault<int>(sql, entity);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stockSymbol"></param>
    /// <returns></returns>
    public DailyTaiwanStockEnt GetDataByKeyTaiwanStock(int stockSymbol)
    {
        return null;
    }
}
