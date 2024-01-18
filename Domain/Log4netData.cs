using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Log4netData
    {
        /// <summary>
        /// 記錄時間
        /// </summary>
        public string LogTime { get; set; } = null!;

        /// <summary>
        /// ThreadID
        /// </summary>
        public string ThreadID { get; set; } = "";

        /// <summary>
        /// 記錄等級
        /// </summary>
        public string LogLevel { get; set; } = null!;

        /// <summary>
        /// 執行環境
        /// </summary>
        public string? Env { get; set; }

        /// <summary>
        /// 系統代號
        /// </summary>
        public string Sys { get; set; } = "RES";

        /// <summary>
        /// 主機位置
        /// </summary>
        public string? HostName { get; set; }

        /// <summary>
        /// 使用者代號
        /// </summary>
        public string? UserID { get; set; }

        /// <summary>
        /// 連線序號
        /// </summary>
        public string? SessionID { get; set; }

        /// <summary>
        /// 來源位置
        /// </summary>
        public string? SourceIP { get; set; }

        /// <summary>
        /// 模組名稱
        /// </summary>
        public string Module { get; set; } = null!;

        /// <summary>
        /// 功能名稱
        /// </summary>
        public string Function { get; set; } = null!;

        /// <summary>
        /// TransId
        /// </summary>
        public string TransId { get; set; } = "";

        /// <summary>
        /// LogPath
        /// </summary>
        public string LogPath { get; set; } = "";

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = null!;
    }
}
