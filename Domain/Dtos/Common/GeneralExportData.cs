using System.Data;

namespace Domain.Dtos;

/// <summary>
/// 通用匯出Excel資料
/// </summary>
public class GeneralExportData
{
    /// <summary>
    /// 來源資料表物件
    /// </summary>
    public DataTable DataSource { get; set; } = null!;

    /// <summary>
    /// 工作表標題
    /// </summary>
    public string SheetTitle { get; set; } = null!;

    /// <summary>
    /// 匯出欄位標題清單
    /// </summary>
    public string[] ColumnTitleList { get; set; } = null!;

    /// <summary>
    /// 匯出資料欄位清單
    /// </summary>
    public string[] DataNameList { get; set; } = null!;

    /// <summary>
    /// 貨幣欄位名稱清單 [整數,千分位]
    /// </summary>
    public string[]? CurrencyColumns { get; set; }

    /// <summary>
    /// 字串格式化為整數欄位名稱清單
    /// </summary>
    public string[]? IntegerColumns { get; set; }
}