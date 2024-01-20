using Domain.Dtos;
using Newtonsoft.Json;
using StockWebDotnetEight.Helpers;
using TaiwanLife.RES.WebAPI.Helpers;

namespace StockWebDotnetEight.Services
{
    public class ServiceBase
    {
        private readonly string _modName;
        private readonly ILogger _logger;

        /// <summary>
        /// 常用工具函式
        /// </summary>
        protected readonly UtilityHelper _helper;
        public ServiceBase(ILogger logger, string modName) 
        {
            _logger = logger;
            _modName = modName;
            this._helper = new UtilityHelper();
        }

        /// <summary>
        /// 寫入偵錯記錄檔 
        /// </summary>
        /// <param name="functionName">函式名稱</param>
        /// <param name="debugMessage">偵錯訊息</param>
        protected void writeDebugLog(string functionName, string debugMessage)
        {
            string logText = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} [{functionName}] {debugMessage}\n\r";
            _logger.LogDebug(logText);
        }

        /// <summary>
        /// 寫入偵錯記錄檔 
        /// </summary>
        /// <param name="functionName">函式名稱</param>
        /// <param name="logObject">偵錯物件</param>
        protected void writeDebugLog(string functionName, object? logObject)
        {
            string message = logObject == null ? "NULL" : JsonConvert.SerializeObject(logObject);
            string logText = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} [{functionName}] {message}\n\r";
            _logger.LogDebug(logText);
        }

        /// <summary>
        /// 寫入訊息記錄檔 
        /// </summary>
        /// <param name="functionName">函式名稱</param>
        /// <param name="returnObject">資料物件</param>
        protected void writeInfoLog(string functionName, object returnObject)
        {
            string message = JsonConvert.SerializeObject(returnObject);
            string jsonText = this._helper.GetLogJsonText("INFO", _modName, functionName, "JSON", message);
            _logger.LogInformation(jsonText + "\r");
        }

        /// <summary>
        /// 寫入錯誤記錄檔 
        /// </summary>
        /// <param name="functionName">函式名稱</param>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <param name="returnObject">資料物件</param>
        protected void writeErrorLog(string functionName, string errorMessage, object returnObject)
        {
            string content = JsonConvert.SerializeObject(returnObject);

            string logText = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} [{functionName}] {errorMessage} {content}\n\r";
            _logger.LogError(logText);

            /*
            string jsonText = this._helper.GetLogJsonText("ERROR", _modName, functionName, content, errorMessage);
            _logger.LogError(jsonText + "\r");
            */
        }

        /// <summary>
        /// 寫入錯誤記錄檔 
        /// </summary>
        /// <param name="moduleName">模組名稱</param>
        /// <param name="functionName">函式名稱</param>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <param name="returnObject">資料物件</param>
        protected void writeErrorLog(string moduleName, string functionName, string errorMessage, object returnObject)
        {
            string content = JsonConvert.SerializeObject(returnObject);

            string logText = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} [{moduleName}][{functionName}] {errorMessage} {content}\n\r";
            _logger.LogError(logText);

            /*
            string jsonText = this._helper.GetLogJsonText("ERROR", _modName, functionName, content, errorMessage);
            _logger.LogError(jsonText + "\r");
            */
        }

        /// <summary>
        /// 匯出成Excel檔
        /// </summary>
        /// <param name="exportData">匯出資料</param>
        protected MemoryStream? ExportToGeneralExcel(GeneralExportData exportData)
        {
            return new NPOIHelper().ConvertToExcel(exportData.DataSource, exportData.SheetTitle, exportData.ColumnTitleList,
                exportData.DataNameList, exportData.CurrencyColumns, exportData.IntegerColumns);
        }

    }
}
