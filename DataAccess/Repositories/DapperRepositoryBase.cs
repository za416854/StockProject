using Dapper;
using Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories;
 

public class DapperRepositoryBase 
{
    /// <summary>
    /// Logger物件
    /// </summary>
    protected readonly ILogger _logger;
    /// <summary>
    /// Dapper Context
    /// </summary>
    protected readonly DapperContext _context;
    /// <summary>
    /// 模組名稱
    /// </summary>
    protected readonly string _modName;

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="context">DapperContext</param>
    /// <param name="modName">模組名稱</param>
    public DapperRepositoryBase(ILogger logger, DapperContext context, string modName)
    {
        this._logger = logger;
        this._context = context;
        this._modName = modName;
    }
    #region 查詢函式

    /// <summary>
    /// 查詢全部資料
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數</param>
    public IEnumerable<T>? QueryAll<T>(string sql, object? parameters = null)
    {
        try
        {
            using (var conn = _context.CreateConnection())
            {
                return conn.Query<T>(sql, parameters);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("QueryAll", ex.Message, sql, parameters);
            return default;
        }
    }

    /// <summary>
    /// 查詢第1筆資料 (若無資料時不會Exception)
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數</param>
    public T? QueryFirstOrDefault<T>(string sql, object? parameters = null)
    {
        try
        {
            using (var conn = _context.CreateConnection())
            {
                return conn.QueryFirstOrDefault<T>(sql, parameters);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("QueryFirstOrDefault", ex.Message, sql, parameters);
            return default;
        }
    }

    /// <summary>
    /// 查詢單筆資料 (若無資料時不會Exception)
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數</param>
    public T? QuerySingleOrDefault<T>(string sql, object? parameters = null)
    {
        try
        {
            using (var conn = _context.CreateConnection())
            {
                return conn.QuerySingleOrDefault<T>(sql, parameters);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("QuerySingleOrDefault", ex.Message, sql, parameters);
            return default;
        }
    }

    /// <summary>
    /// 查詢資料並回傳DataTable物件
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數或資料模型</param>
    public DataTable? QueryToDataTable(string sql, object? parameters = null)
    {
        try
        {
            using (var conn = _context.CreateConnection())
            {
                var dataReader = conn.ExecuteReader(sql, parameters);
                var dataTable = new DataTable();
                dataTable.Load(dataReader);
                return dataTable;
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("QueryToDataTable", ex.Message, sql, parameters);
            return null;
        }
    }

    #endregion

    #region 異動函式

    /// <summary>
    /// 新增資料
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="entity">資料模型</param>
    public int Insert<T>(string sql, T entity)
    {
        return executeCUD<T>("Insert", sql, entity);
    }

    /// <summary>
    /// 修改資料
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="entity">資料模型</param>
    public int Update<T>(string sql, T entity)
    {
        return executeCUD<T>("Update", sql, entity);
    }

    /// <summary>
    /// 刪除資料
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="entity">資料模型</param>
    public int Delete<T>(string sql, T entity)
    {
        return executeCUD<T>("Delete", sql, entity);
    }

    // 執行標準新增/修改/刪除
    private int executeCUD<T>(string funcName, string sql, T entity)
    {
        try
        {
            using (var conn = _context.CreateConnection())
            {
                return conn.Execute(sql, entity);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog<T>(funcName, ex.Message, sql, entity);
            return -1;
        }
    }

    #endregion

    #region 執行函式

    /// <summary>
    /// 執行指令
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數</param>
    public int ExecuteCommand(string sql, DynamicParameters? parameters = null)
    {
        try
        {
            using (var conn = _context.CreateConnection())
            {
                return conn.Execute(sql, parameters);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("ExecuteCommand", ex.Message, sql, parameters);
            return -1;
        }
    }

    /// <summary>
    /// 執行指令
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數</param>
    /// <remark>
    /// 使用範例:
    /// int result = ExecuteCommand(sql, new { TargetUID = targetUID, SourceUID = sourceUID });
    /// </remark>
    public int ExecuteCommand(string sql, object? parameters = null)
    {
        try
        {
            using (var conn = _context.CreateConnection())
            {
                return conn.Execute(sql, parameters);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("ExecuteCommand", ex.Message, sql, parameters);
            return -1;
        }
    }

    /// <summary>
    /// 執行指令並回傳單一值
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數</param>
    public object? ExecuteScaler(string sql, object? parameters = null)
    {
        try
        {
            using (var conn = _context.CreateConnection())
            {
                return conn.ExecuteScalar(sql, parameters);
            }
        }
        catch (Exception ex)
        {
            writeErrorLog("ExecuteScaler", ex.Message, sql, parameters);
            return null;
        }
    }

    #endregion

    #region 工具函式

    /// <summary>
    /// 取得含有參數值的SQL指令字串 
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數</param>
    protected string getSQLCmdWithParamValues(string sql, DynamicParameters? parameters = null)
    {
        string commandText = sql;

        if (parameters != null)
        {
            // 為避免取代字串錯誤，先依字串長度排序
            string[] sortedArray = parameters.ParameterNames.OrderByDescending(x => x.Length).ToArray();

            foreach (string paramName in sortedArray)
            {
                string replaceKey = $"@{paramName}";
                var paramValue = parameters.Get<object>(paramName);
                if (paramValue == null || paramValue == DBNull.Value)
                {
                    commandText = commandText.Replace(replaceKey, "NULL");
                }
                else
                {
                    string valueText = getValueText(paramValue);
                    commandText = commandText.Replace(replaceKey, valueText);
                }
            }
        }

        return commandText;
    }

    /// <summary>
    /// 取得含有參數值的SQL指令字串 
    /// </summary>
    /// <param name="sql">SQL指令</param>
    /// <param name="entity">資料物件</param>
    protected string getSQLCmdWithParamValues<T>(string sql, T entity)
    {
        string commandText = sql;

        if (entity != null)
        {
            // 為避免取代字串錯誤，先依字串長度排序
            Type objectType = entity.GetType();
            PropertyInfo[] sortedArray = objectType.GetProperties().OrderByDescending(x => x.Name.Length).ToArray();

            foreach (PropertyInfo prop in sortedArray)
            {
                string replaceKey = $"@{prop.Name}";
                var paramValue = prop.GetValue(entity);

                if (paramValue == null || paramValue == DBNull.Value)
                {
                    commandText = commandText.Replace(replaceKey, "NULL");
                }
                else
                {
                    string valueText = getValueText(paramValue);
                    commandText = commandText.Replace(replaceKey, valueText);
                }
            }
        }

        return commandText;
    }

    // 依欄位型態取得值字串
    private string getValueText(object paramValue)
    {
        Type valueType = getValueType(paramValue);

        string valueText = "";

        if (valueType == typeof(string))
        {
            valueText = "'" + paramValue.ToString() + "'";
        }
        else if (valueType == typeof(DateTime))
        {
            DateTime dateTimeValue = Convert.ToDateTime(paramValue);
            if (dateTimeValue.TimeOfDay.Minutes == 0)
            {
                valueText = "'" + dateTimeValue.ToString("yyyy/MM/dd") + "'";
            }
            else
            {
                valueText = "'" + dateTimeValue.ToString("yyyy/MM/dd HH:mm:ss") + "'";
            }
        }
        else
        {
            valueText += paramValue.ToString();
        }

        return valueText;
    }

    // 由參數值取得型態
    private Type getValueType(object paramValue)
    {
        switch (Type.GetTypeCode(paramValue.GetType()))
        {
            case TypeCode.String:
                return typeof(string);
            case TypeCode.DateTime:
                return typeof(DateTime);
            default:
                return typeof(int);
        }
    }

    /// <summary>
    /// 寫入偵錯SQL指令 
    /// </summary>
    /// <param name="functionName">函式名稱</param>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數物件</param>
    protected void writeDebugCommand(string functionName, string sql, DynamicParameters? parameters = null)
    {
        string sqlCmd = getSQLCmdWithParamValues(sql, parameters);
        writeDebugLog(functionName, sqlCmd);
    }

    /// <summary>
    /// 寫入偵錯SQL指令 
    /// </summary>
    /// <param name="functionName">函式名稱</param>
    /// <param name="sql">SQL指令</param>
    /// <param name="entity">資料物件</param>
    protected void writeDebugCommand<T>(string functionName, string sql, T entity)
    {
        string sqlCmd = getSQLCmdWithParamValues<T>(sql, entity);
        writeDebugLog(functionName, sqlCmd);
    }

    /// <summary>
    /// 寫入偵錯記錄
    /// </summary>
    /// <param name="functionName">函式名稱</param>
    /// <param name="message">訊息</param>
    protected void writeDebugLog(string functionName, string message)
    {
        string logText = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{_modName}] [{functionName}] {message}\n\r";
        _logger.LogDebug(logText);

        /*
        string jsonText = getLogJsonText("DEBUG", functionName, "Debug", message);
        _logger.LogDebug(jsonText + "\r");
        */
    }

    /// <summary>
    /// 寫入錯誤記錄檔
    /// </summary>
    /// <param name="functionName">函式名稱</param>
    /// <param name="errorMessage">錯誤訊息</param>
    /// <param name="sql">SQL指令</param>
    /// <param name="parameters">參數物件</param>
    protected void writeErrorLog(string functionName, string errorMessage, string sql, DynamicParameters? parameters = null)
    {
        string sqlCmd = getSQLCmdWithParamValues(sql, parameters);
        writeErrorLog(functionName, errorMessage, sqlCmd);
    }

    /// <summary>
    /// 寫入錯誤記錄檔
    /// </summary>
    /// <param name="functionName">函式名稱</param>
    /// <param name="errorMessage">錯誤訊息</param>
    /// <param name="sql">SQL指令</param>
    /// <param name="entity">資料物件</param>
    protected void writeErrorLog<T>(string functionName, string errorMessage, string sql, T entity)
    {
        string sqlCmd = getSQLCmdWithParamValues<T>(sql, entity);
        writeErrorLog(functionName, errorMessage, sqlCmd);
    }

    // 寫入包含SQL指令的錯誤訊息
    private void writeErrorLog(string functionName, string errorMessage, string sqlCmd)
    {
        string logText = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{_modName}] [{functionName}] {errorMessage} {sqlCmd}\n\r";
        _logger.LogError(logText);

        /*
        string jsonText = getLogJsonText("ERROR", functionName, errorMessage, sqlCmd);
        _logger.LogError(jsonText + "\r");
        */
    }

    /// <summary>
    /// 寫入錯誤記錄檔
    /// </summary>
    /// <param name="functionName">函式名稱</param>
    /// <param name="errorMessage">錯誤訊息</param>
    protected void writeErrorLog(string functionName, string errorMessage)
    {
        string logText = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{_modName}] [{functionName}] {errorMessage}\n\r";
        _logger.LogError(logText);

        /*
        string jsonText = getLogJsonText("ERROR", functionName, "ErrorMessage", errorMessage);
        _logger.LogError(jsonText + "\r");
        */
    }

    /// <summary>
    /// 取得Log檔案的JSON文字 
    /// </summary>
    /// <param name="logLevel">記錄等級</param>
    /// <param name="functionName">功能名稱</param>
    /// <param name="content">內容</param>
    /// <param name="message">訊息</param>
    protected string getLogJsonText(string logLevel, string functionName, string content, string message)
    {
        Log4netData logData = new Log4netData
        {
            LogTime = DateTime.Now.ToString("yyyyMMdd_HHmmss"),
            LogLevel = logLevel,
            //Env = log4net.GlobalContext.Properties["Env"].ToString(),
            //HostName = log4net.GlobalContext.Properties["ServerIP"].ToString(),
            //UserID = log4net.ThreadContext.Properties["UserID"].ToString(),
            //SessionID = log4net.ThreadContext.Properties["SessionID"].ToString(),
            //SourceIP = log4net.ThreadContext.Properties["SourceIP"].ToString(),
            Env = string.Empty,
            HostName = string.Empty,
            UserID = string.Empty,
            SessionID = string.Empty,
            SourceIP = string.Empty,
            Module = _modName,
            Function = functionName,
            Content = content,
            Message = message
        };

        return JsonConvert.SerializeObject(logData);
    }

    /// <summary>
    /// 轉換物件為JSON文字
    /// </summary>
    /// <param name="source">來源物件</param>
    protected string convertToJson(object source)
    {
        return JsonConvert.SerializeObject(source);
    }

    /// <summary>
    /// 由JSON文字轉換為物件
    /// </summary>
    /// <param name="jsonText"></param>
    protected T? convertToObject<T>(string jsonText)
    {
        return JsonConvert.DeserializeObject<T>(jsonText);
    }

    #endregion
}

