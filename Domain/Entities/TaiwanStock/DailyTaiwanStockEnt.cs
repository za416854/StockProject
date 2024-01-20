namespace Domain.Entities;
/// <summary>
/// 台股資料
/// </summary>
public class DailyTaiwanStockEnt : BaseEntity
{
    /// <summary>
    /// 股票 ID
    /// </summary>
    public int StockID { get; set; }

    /// <summary>
    /// 股票代號
    /// </summary>
    public string StockSymbol { get; set; } = null!;

    /// <summary>
    /// 股票名稱
    /// </summary>
    public string StockName { get; set; } = null!;

    /// <summary>
    /// 收盤價
    /// </summary>
    public decimal ClosingPrice { get; set; }

    /// <summary>
    /// 漲跌金額
    /// </summary>
    public decimal ChangeAmount { get; set; }

    /// <summary>
    /// 漲跌百分比
    /// </summary>
    public decimal ChangePercentage { get; set; }

    /// <summary>
    /// 特殊註記
    /// </summary>
    public string SpecialNote { get; set; } = null!;

    /// <summary>
    /// 交易日期
    /// </summary>
    public DateTime TradeDate { get; set; }

    /// <summary>
    /// 創建使用者
    /// </summary>
    public string CreateUser { get; set; } = null!;

    /// <summary>
    /// 創建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 更新日期
    /// </summary>
    public DateTime UpdateDate { get; set; }
}