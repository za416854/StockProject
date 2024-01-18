namespace Domain.Dtos;

/// <summary>
/// 共用回應訊息 View Model
/// </summary>
public class CommonResponseDto<T>
{
    /// <summary>
    /// 本次交易的 unique ID
    /// </summary>
    //public string transactionId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 訊息代碼
    /// </summary>
    public string returnCode { get; set; } = "0000";

    /// <summary>
    /// 訊息文字
    /// </summary>
    public string returnMessage { get; set; } = "Success";

    /// <summary>
    /// 回傳資料物件
    /// </summary>
    public T? returnData { get; set; }
}
