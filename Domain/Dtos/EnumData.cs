namespace Domain.Dtos;

/// <summary>
/// 作業類型列舉
/// </summary>
public enum ACTION_TYPES
{
    /// <summary>
    /// 登入
    /// </summary>
    Login,

    /// <summary>
    /// 登出
    /// </summary>
    Logout,

    /// <summary>
    /// 查詢
    /// </summary>
    Query,

    /// <summary>
    /// 新增
    /// </summary>
    Insert,

    /// <summary>
    /// 修改
    /// </summary>
    Update,

    /// <summary>
    /// 刪除
    /// </summary>
    Delete,

    /// <summary>
    /// 匯出
    /// </summary>
    Export,

    /// <summary>
    /// 匯入
    /// </summary>
    Import,

    /// <summary>
    /// 上傳
    /// </summary>
    Upload,

    /// <summary>
    /// 下載
    /// </summary>
    Download,

    /// <summary>
    /// 執行
    /// </summary>
    RunTask,

    /// <summary>
    /// 送覆核
    /// </summary>
    Submit,

    /// <summary>
    /// 覆核通過
    /// </summary>
    Approve,

    /// <summary>
    /// 覆核退回
    /// </summary>
    Reject,

    /// <summary>
    /// 重送覆核
    /// </summary>
    ReSubmit,

    /// <summary>
    /// 取消編輯(覆核流程)
    /// </summary>
    Cancel
}

/// <summary>
/// 錯誤代碼列舉
/// </summary>
public enum ERROR_CODES
{
    /// <summary>
    /// 1001 使用者帳密驗證失敗
    /// </summary>
    ErrorUserAuthenticate = 1001,

    /// <summary>
    /// 1002 使用者資料不存在
    /// </summary>
    ErrorUserNotExist = 1002,

    /// <summary>
    /// 1003 使用者已停用
    /// </summary>
    ErrorUserNotActive = 1003,

    /// <summary>
    /// 1004 使用者正由其他人代理中
    /// </summary>
    ErrorUserInAgent = 1004,

    /// <summary>
    /// 2001 驗證傳入參數錯誤
    /// </summary>
    ErrorValidation = 2001,    

    /// <summary>
    /// 2002 找不到資料
    /// </summary>
    ErrorNotFound = 2002,    

    /// <summary>
    /// 3001 主索引資料重複
    /// </summary>
    ErrorKeyDuplicate = 3001,

    /// <summary>
    /// 9001 資料庫操作發生錯誤
    /// </summary>
    ErrorDBException = 9001,

    /// <summary>
    /// 9999 例外錯誤
    /// </summary>
    ErrorException = 9999
}
