using System.Net;
using Newtonsoft.Json;
using System.Text;
using Domain;

namespace StockWebDotnetEight.Helpers;
/// <summary>
/// 常用工具函式
/// </summary>
public class UtilityHelper
{
    /// <summary>
    /// 取得主機端IPv4位置
    /// </summary>
    public string? GetHostIPv4Address()
    {
        string hostName = Dns.GetHostName();
        IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
        foreach (IPAddress ipAddress in ipHostEntry.AddressList)
        {
            var ipv4Address = ipAddress.ToString();
            if (!ipv4Address.Contains(":"))
            {
                return ipv4Address;
            }
        }

        return null;
    }

    /// <summary>
    /// 取得Log檔案的JSON文字 
    /// </summary>
    /// <param name="logLevel">記錄等級</param>
    /// <param name="modName">模組名稱</param>
    /// <param name="functionName">函式名稱</param>
    /// <param name="content">內容</param>
    /// <param name="message">訊息</param>
    public string GetLogJsonText(string logLevel, string modName, string functionName, string content, string message)
    {
        Log4netData logData = new Log4netData
        {
            LogTime = DateTime.Now.ToString("yyyyMMdd_HHmmss"),
            LogLevel = logLevel,
            Env = getLog4netProperty("Env"),
            HostName = getLog4netProperty("ServerIP"),
            UserID = getLog4netProperty("UserID"),
            SessionID = getLog4netProperty("SessionID"),
            SourceIP = getLog4netProperty("SourceIP"),
            Module = modName,
            Function = functionName,
            Content = content,
            Message = message
        };

        return JsonConvert.SerializeObject(logData);
    }

    // 避免發生NullReferenceException
    private string getLog4netProperty(string propName)
    {
        string propValue = string.Empty;

        if (propValue != null)
        {
            var propString = propValue.ToString();
            if (propString != null)
            {
                return propString;
            }
        }

        return "UNKNOWN";
    }

    /// <summary>
    /// 轉換物件為JSON文字
    /// </summary>
    /// <param name="source">來源物件</param>
    public string ConvertToJson(object source)
    {
        return JsonConvert.SerializeObject(source);
    }

    /// <summary>
    /// 由JSON文字轉換為物件
    /// </summary>
    /// <param name="jsonText">來源物件</param>
    public T? DeserializeFromJSON<T>(string jsonText)
    {
        return JsonConvert.DeserializeObject<T>(jsonText);
    }

    /// <summary>
    /// 以GET方式呼叫WebAPI
    /// </summary>
    /// <param name="webAPIURL">WebAPI網址</param>
    /// <param name="contentObject">傳送物件</param>
    public (string, string) HttpGetJSON(string webAPIURL, object contentObject)
    {
        // 將物件轉換為JSON文字
        string? jsonText = null;

        if (contentObject is string)
        {
            jsonText = contentObject.ToString();
        }
        else
        {
            jsonText = ConvertToJson(contentObject);
        }

        if (string.IsNullOrWhiteSpace(jsonText))
        {
            return ("", "轉換物件為JSON文字失敗!");
        }

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, webAPIURL);
                httpRequest.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

                var response = client.Send(httpRequest);
                if (response != null)
                {
                    using (Stream responseStream = response.Content.ReadAsStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        return (reader.ReadToEnd(), "");
                    }
                }
            }

            return ("", "");
        }
        catch (Exception ex)
        {
            return ("", ex.Message);
        }
    }

    /*
    /// <summary>
    /// 以POST方式呼叫WebAPI
    /// </summary>
    /// <param name="webAPIURL">WebAPI網址</param>
    /// <param name="contentObject">傳送物件</param>
    public string HttpPostJSON(string webAPIURL, object contentObject)
    {
        // 將物件轉換為JSON文字
        string jsonText = ConvertToJson(contentObject);

        // 以同步的方法呼叫非同步函式
        using (HttpClient client = new HttpClient())
        {
            var postTask = client.PostAsync(webAPIURL, new StringContent(jsonText, Encoding.UTF8, "application/json"));
            var response = postTask.Result;     // 等待非同步返回
            if (response != null)
            {
                var readTask = response.Content.ReadAsStringAsync();
                return readTask.Result;         // 等待非同步返回
            }
        }

        return "";
    }
    */

    #region 檔案系統

    /// <summary>
    /// 連結路徑
    /// </summary>
    /// <param name="path1">上層路徑</param>
    /// <param name="path2">第一層子目錄</param>
    public string JoinPath(string path1, string path2)
    {
        return Path.Combine(path1, path2);
    }

    /// <summary>
    /// 連結路徑
    /// </summary>
    /// <param name="path1">上層路徑</param>
    /// <param name="path2">第一層子目錄</param>
    /// <param name="path3">第二層子目錄</param>
    public string JoinPath(string path1, string path2, string path3)
    {
        return Path.Combine(path1, path2, path3);
    }

    /// <summary>
    /// 檢查並自動建立目錄
    /// </summary>
    /// <param name="folderPath">目錄位置</param>
    public bool CheckAndCreateFolder(string folderPath)
    {
        if (Directory.Exists(folderPath)) return true;

        Directory.CreateDirectory(folderPath);

        return Directory.Exists(folderPath);
    }

    /// <summary>
    /// 檢查檔案是否存在
    /// </summary>
    /// <param name="filePath">檔案位置</param>
    public bool CheckIsFileExist(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// 刪除檔案
    /// </summary>
    /// <param name="filePath">檔案位置</param>
    public void DeleteFile(string filePath)
    {
        File.Delete(filePath);
    }

    /// <summary>
    /// 上傳子目錄名稱
    /// </summary>
    public const string FOLDER_UPLOAD = "UPLOAD";

    /// <summary>
    /// 下載子目錄名稱
    /// </summary>
    public const string FOLDER_DOWNLOAD = "DOWNLOAD";

    /// <summary>
    /// 取得上下傳暫存檔案路徑
    /// </summary>
    /// <param name="rootFolder">根目錄</param>
    /// <param name="typeFolder">類型(UPLOAD/DOWNLOAD)</param>
    /// <param name="dateFolder">日期(yyyyMMdd)</param>
    /// <param name="fileName">檔案名稱</param>
    public string GetTempFilePath(string rootFolder, string typeFolder, string dateFolder, string? fileName = null)
    {
        // 第一層目錄 上傳/下載
        string targetFolder = JoinPath(rootFolder, typeFolder);

        // 第二層目錄 日期
        targetFolder = JoinPath(targetFolder, dateFolder);

        if (fileName == null)
        {
            return targetFolder;
        }
        else
        {
            CheckAndCreateFolder(targetFolder);
            string targetFile = JoinPath(targetFolder, fileName);
            return targetFile;
        }
    }

    /// <summary>
    /// 寫入文字檔
    /// </summary>
    /// <param name="filePath">檔案位置</param>
    /// <param name="content">檔案內容</param>
    public void WriteAllText(string filePath, string content)
    {
        File.WriteAllText(filePath, content, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// 取得目錄內的子目錄名稱(含路徑)
    /// </summary>
    /// <param name="parentFolder">上層目錄路徑</param>
    public string[]? GetSubFolders(string parentFolder)
    {
        return Directory.GetDirectories(parentFolder);
    }

    /// <summary>
    /// 由路徑取得檔案或目錄名稱
    /// </summary>
    /// <param name="path">檔案或目錄路徑</param>
    public string GetFileName(string path)
    {
        return Path.GetFileName(path);
    }

    /// <summary>
    /// 檢查目錄是否為空的 (子目錄和檔案數都為0)
    /// </summary>
    /// <param name="folderPath">檢查目錄路徑</param>
    public bool FolderIsEmpty(string folderPath)
    {
        return (Directory.GetFiles(folderPath).Length == 0) && (Directory.GetDirectories(folderPath).Length == 0);
    }

    /// <summary>
    /// 檢查目錄是否存在
    /// </summary>
    /// <param name="folderPath">檢查目錄路徑</param>
    public bool CheckIsFolderExist(string folderPath)
    {
        return Directory.Exists(folderPath);
    }

    /// <summary>
    /// 刪除目錄
    /// </summary>
    /// <param name="folderPath">目錄路徑</param>
    /// <param name="isRecurive">是否刪除所有子目錄或及檔案(遞迴刪除)</param>
    public void DeleteFolder(string folderPath, bool isRecurive = true)
    {
        Directory.Delete(folderPath, isRecurive);
    }

    /// <summary>
    /// 取得目錄內的檔案名稱(含路徑)
    /// </summary>
    /// <param name="parentFolder">上層目錄路徑</param>
    /// <param name="searchPattern">篩選條件(預設為*)</param>
    public string[]? GetFolderFiles(string parentFolder, string searchPattern = "*")
    {
        return Directory.GetFiles(parentFolder, searchPattern);
    }

    /// <summary>
    /// 讀取文字檔至行陣列
    /// </summary>
    /// <param name="filePath">檔案位置</param>
    public string[] ReadTextFileLines(string filePath)
    {
        return File.ReadAllLines(filePath);
    }

    /// <summary>
    /// 搬移檔案
    /// </summary>
    /// <param name="source">原始位置</param>
    /// <param name="target">目的位置</param>
    public (bool, string) MoveFile(string source, string target)
    {
        try
        {
            //File.Move(source, target);

            // 2022-10-03 搬移作業改為先複製再刪除
            File.Copy(source, target);
            File.Delete(source);

            return (true, "");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    #endregion
}
