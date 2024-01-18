using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories;
/// <summary>
/// 共用功能介面
/// </summary>
public interface ICommonRepository
{

}

/// <summary>
/// 共用功能資料來源庫
/// </summary>
public class CommonRepository : DapperRepositoryBase, ICommonRepository
{
    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="context">DapperContext</param>
    public CommonRepository(ILogger<CommonRepository> logger, DapperContext context)
        : base(logger, context, nameof(CommonRepository)) { }


}

