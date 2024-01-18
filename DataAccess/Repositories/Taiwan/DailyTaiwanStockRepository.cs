using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;
//using TaiwanLife.RES.Domain.Entities;
namespace DataAccess.Repositories;

public interface IDailyTaiwanStockRepository
{
    /// <summary>
    /// 取得股市前一天收盤資訊
    /// </summary>
    /// <returns></returns>
    DataTable GetTaiwanStockData();


}
public class DailyTaiwanStockRepository : DapperRepositoryBase, IDailyTaiwanStockRepository
{
    public DailyTaiwanStockRepository(ILogger<DailyTaiwanStockRepository> logger, DapperContext context) : base(logger, context, nameof(DailyTaiwanStockRepository)) 
    {

    }

    public DataTable GetTaiwanStockData()
    {
        return new DataTable();
    }
}
