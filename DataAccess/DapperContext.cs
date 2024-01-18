using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccess;

/// <summary>
/// Dapper Context
/// </summary>
public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="configuration">設定檔</param>
    /// <param name="twlEncrypyt">加解密物件</param>
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;

        string? encryptString = _configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// 建立資料庫連線
    /// </summary>
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    /// <summary>
    /// 取得連線字串
    /// </summary>
    public string? GetConnectionString()
    {
        return _connectionString;
    }
}
