using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Text;
using System.Reflection;
using Domain.Entities;
using Domain.Dtos;

namespace TaiwanLife.RES.WebAPI.Helpers;

/// <summary>
/// NPOI Excel 工具函式
/// </summary>
public class NPOIHelper 
{   
    #region 公用匯出函式

    /// <summary>
    /// 由資料表轉換為Excel MemoryStream
    /// </summary>
    /// <param name="dataTable">資料表物件</param>
    /// <param name="sheetName">Excel工件表名稱</param>
    /// <param name="columnTitleList">Excel欄位標題清單</param>
    /// <param name="dataNameList">資料庫欄位名稱清單</param>
    /// <param name="currencyColumns">貨幣欄位名稱清單</param>
    /// <param name="integerColumns">整數欄位名稱清單</param>
    public MemoryStream ConvertToExcel(DataTable dataTable, string sheetName, string[] columnTitleList, 
        string[] dataNameList, string[]? currencyColumns = null, string[]? integerColumns = null)
    {
        // 建立Excel 2007檔案
        IWorkbook wb = new XSSFWorkbook();
        ISheet ws = wb.CreateSheet(sheetName);

        // 建立十進位數字欄位型態
        createNumericCellStyle(wb);
        // 建立貨幣欄位型態
        createCurrencyCellStyle(wb);
        // 建立整數欄位型態
        createIntegerCellStyle(wb);

        // 第一行為欄位名稱
        IRow firstRow = ws.CreateRow(0);    
        for (int k = 0; k < columnTitleList.Length; k++)
        {            
            firstRow.CreateCell(k).SetCellValue(columnTitleList[k]);
        }

        for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
        {
            IRow row = ws.CreateRow(rowIndex + 1);
            for (int columnIndex = 0; columnIndex < dataNameList.Length; columnIndex++)
            {
                string dataColumnName = dataNameList[columnIndex];
                setCellValue(dataTable, dataColumnName, rowIndex, columnIndex, row, currencyColumns, integerColumns);
            }
        }

        MemoryStream ms = new MemoryStream();
        wb.Write(ms);

        return ms;
    }

    // 設定Excel Cell的內容值 (數值欄位型態設定為Numeric)
    private void setCellValue(DataTable dataTable, string dataColumnName, int rowIndex, int columnIndex, IRow row, 
        string[]? currencyColumns = null, string[]? integerColumns = null)
    {
        ICell newCell = row.CreateCell(columnIndex);
        string? columnText = "";

        if (dataTable.Columns.Contains(dataColumnName))
        {
            DataColumn? column = dataTable.Columns[dataColumnName];
            if (column != null)
            {                
                object columnValue = dataTable.Rows[rowIndex][dataColumnName];
                if (columnValue != null && columnValue != DBNull.Value)
                {
                    // 2022-12-06 增加格式化字串欄位為整數的功能
                    if (integerColumns != null && integerColumns.Contains(dataColumnName))
                    {
                        newCell.SetCellType(CellType.Numeric);

                        if (!string.IsNullOrEmpty(columnValue.ToString()))
                        {
                            newCell.SetCellValue(Convert.ToDouble(columnValue));
                        }

                        // 格式化整數欄位
                        newCell.CellStyle = ExcelIntegerCellStyle;                            
                        return;
                    }

                    if (column.DataType == typeof(string))
                    {                        
                        columnText = columnValue.ToString();
                    }
                    else if (column.DataType == typeof(DateTime))
                    {                        
                        DateTime dateTimeValue = Convert.ToDateTime(columnValue);
                        if (dateTimeValue.Hour == 0 && dateTimeValue.Minute == 0 && dateTimeValue.Second == 0)
                        {
                            columnText = dateTimeValue.ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            columnText = dateTimeValue.ToString("yyyy/MM/dd HH:mm:ss");
                        }              
                    }
                    else if (column.DataType == typeof(decimal) || column.DataType == typeof(int) 
                        || column.DataType == typeof(short) || column.DataType == typeof(long))
                    {                        
                        newCell.SetCellType(CellType.Numeric);
                        newCell.SetCellValue(Convert.ToDouble(columnValue));
                        
                        if (column.DataType == typeof(decimal) && ExcelNumericCellStyle != null)
                        {
                            if (currencyColumns != null && ExcelCurrencyCellStyle != null && currencyColumns.Contains(dataColumnName))
                            {
                                // 格式化十進位數值欄位為貨幣型態
                                newCell.CellStyle = ExcelCurrencyCellStyle;
                            }
                            else
                            {
                                // 格式化十進位數值欄位為小數點後5位
                                newCell.CellStyle = ExcelNumericCellStyle;
                            }
                        }
                        else
                        {
                            // 格式化整數欄位
                            newCell.CellStyle = ExcelIntegerCellStyle;
                        }

                        return;
                    }
                    else
                    {
                        columnText = columnValue.ToString();
                    }                    
                }
            }
        }

        newCell.SetCellValue(columnText);
    }

    // Excel十進位數字欄位型態
    private ICellStyle? ExcelNumericCellStyle;

    // 建立十進位數字欄位型態
    private void createNumericCellStyle(IWorkbook workBook)
    {
        ExcelNumericCellStyle = workBook.CreateCellStyle();
        ExcelNumericCellStyle.Alignment = HorizontalAlignment.Right;
        ExcelNumericCellStyle.VerticalAlignment = VerticalAlignment.Center;
        ExcelNumericCellStyle.DataFormat = workBook.CreateDataFormat().GetFormat("0.00000");
    }

    // Excel貨幣欄位型態
    private ICellStyle? ExcelCurrencyCellStyle;

    // 建立貨幣欄位型態
    private void createCurrencyCellStyle(IWorkbook workBook)
    {
        ExcelCurrencyCellStyle = workBook.CreateCellStyle();
        ExcelCurrencyCellStyle.Alignment = HorizontalAlignment.Right;
        ExcelCurrencyCellStyle.VerticalAlignment = VerticalAlignment.Center;
        ExcelCurrencyCellStyle.DataFormat = workBook.CreateDataFormat().GetFormat("#,##0");
    }

    // Excel整數欄位型態
    private ICellStyle? ExcelIntegerCellStyle;

    // 建立整數欄位型態
    private void createIntegerCellStyle(IWorkbook workBook)
    {
        ExcelIntegerCellStyle = workBook.CreateCellStyle();
        ExcelIntegerCellStyle.Alignment = HorizontalAlignment.Right;
        ExcelIntegerCellStyle.VerticalAlignment = VerticalAlignment.Center;
        ExcelIntegerCellStyle.DataFormat = workBook.CreateDataFormat().GetFormat("0");
    }

    #endregion

    #region 公用匯入函式

    /// <summary>
    /// 由 Excel 轉換為物件清單(List)
    /// </summary>
    /// <param name="sheetData">Excel工件表 MemoryStream</param>
    /// <param name="columnMappings">物件屬性名稱與Excel欄位索引對照</param>
    /// <param name="skipColumnIndex">可為null的欄位Index</param>
    /// <param name="startRowIndex">起始資料行</param>
    /// <param name="cellemptySetNull">Cell值空白設定為NULL</param>
    /// <param name="sheetIndex">Excel Sheet的索引值</param>
    public async Task<List<T>?> ConvertToList<T>(MemoryStream sheetData, Dictionary<string, int> columnMappings, 
        List<int> skipColumnIndex, int startRowIndex = 1, bool cellemptySetNull = false, int sheetIndex = 0)
    {
        // 取得 PropertyInfo List
        List<PropertyInfo> properties = new List<PropertyInfo>();
        properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        var returnList = new List<T>();

        using (Stream stream = new MemoryStream(sheetData.ToArray()))
        {
            var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(sheetIndex);

            // 2022-10-24 改用LastRowNum判斷最後一行
            for (int rowIndex = startRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
            {                    
                var row = sheet.GetRow(rowIndex);                    
                if (row == null) break;     // 結束匯入作業

                // 2022-11-07 增加檢核是否每個欄位都是空值
                if (isAllCellEmpty(row)) break;     // 結束匯入作業

                var item = Activator.CreateInstance<T>();
                bool isHaveData = false;
                
                foreach (var column in properties)
                {                    
                    // 以屬性欄位名稱取得Excel對應欄位索引
                    int excelColumnIndex = columnMappings.ContainsKey(column.Name) ? columnMappings[column.Name] : -1;
                    if (excelColumnIndex == -1) continue;
                    
                    // 略過不存在的欄位
                    var excelCell = row.GetCell(excelColumnIndex);
                    if (excelCell == null) continue;

                    // 依Excel欄位型態，轉換為對應的型別物件
                    var cellValue = getExcelCellValue(excelCell);

                    // 增加判斷是否有抓到任何資料
                    isHaveData = true;
                    //System.Diagnostics.Debug.Print($"{excelColumnIndex}:{cellValue}");

                    // 檢核是否欄位空白 [忽略不需檢核的欄位]
                    if (!skipColumnIndex.Contains(excelColumnIndex))
                    {
                        if (string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            return null;
                        }
                    }
                                        
                    // 設定物件屬性欄位值
                    Type t = Nullable.GetUnderlyingType(column.PropertyType) ?? column.PropertyType;
                    string typeName = t.Name.ToUpper();

                    // Cell值空白設定為NULL
                    if (cellemptySetNull && (cellValue != null && cellValue.Equals(string.Empty)))
                    {                           
                        if (typeName == "DECIMAL" || typeName.StartsWith("INT"))
                        {
                            cellValue = 0;
                            column.SetValue(item, Convert.ChangeType(cellValue, t));
                        }
                        else
                        {
                            column.SetValue(item, null);
                        }
                    }
                    else
                    {
                        // 數值欄位
                        if (typeName == "DECIMAL" || typeName.StartsWith("INT"))
                        {
                            if (cellValue == null || cellValue.Equals(string.Empty))
                            {
                                cellValue = 0;          // 數值欄位為空值時，改為預設值0
                            }
                        }

                        column.SetValue(item, Convert.ChangeType(cellValue, t));

                        /* 2023-01-16 For Debug
                        try
                        {
                            column.SetValue(item, Convert.ChangeType(cellValue, t));
                        }
                        catch (Exception ex)
                        {
                            string columnName = column.Name;                            
                            string message = ex.Message;
                        }
                        */
                    }                    
                }

                // 排除空行
                if (isHaveData)
                {
                    returnList.Add(item);
                }
            }
        }

        await Task.Delay(1);

        return returnList;
    }

    // 2022-11-07 增加檢核是否每個欄位都是空值
    private bool isAllCellEmpty(IRow row)
    {
        foreach (ICell cell in row.Cells)
        {
            if (!string.IsNullOrWhiteSpace(cell.ToString()))
            {
                return false;
            }
        }

        return true;
    }

    // 依Excel欄位型態，轉換為對應的型別物件
    private object getExcelCellValue(ICell cell)
    {
        switch (cell.CellType)
        {
            // 首先在NPOI中數字和日期都屬於Numeric類型
            // 通過NPOI中自帶的DateUtil.IsCellDateFormatted判斷是否為時間日期類型
            case CellType.Numeric when DateUtil.IsCellDateFormatted(cell):
                return cell.DateCellValue;

            case CellType.Numeric:  // 其他數字類型                
                return cell.NumericCellValue;                
                            
            case CellType.Blank:    // 空數據類型
                return "";
            
            case CellType.Formula:  // 公式類型
                return cell.NumericCellValue;
                                                    
            case CellType.Boolean:  // 布林類型
                return cell.BooleanCellValue;
                                        
            default:    // 其他類型都按字元串類型來處理
                return cell.StringCellValue;
        }        
    }

    /// <summary>
    /// 讀取Excel檔案第1行資料
    /// </summary>
    /// <param name="excelData">Excel檔資料</param>
    public List<string> ReadFirstRow(MemoryStream excelData)
    {
        List<string> returnList = new List<string>();

        using (Stream stream = new MemoryStream(excelData.ToArray()))
        {
            var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            var row = sheet.GetRow(0);                    
            if (row != null)
            {
                for (int columnIndex = 0; columnIndex < row.Cells.Count; columnIndex++)
                {
                    var cellSerialNo = row.GetCell(columnIndex);
                    if (cellSerialNo != null)
                    {
                        var cellValue = getExcelCellValue(cellSerialNo);
                        if (cellValue == null)
                        {
                            returnList.Add("");
                        }
                        else
                        {
                            returnList.Add(cellValue.ToString()!);
                        }
                    }
                }
            }
        }

        return returnList;        
    }    

    // 取得整數欄位值 [忽略錯誤]
    private int getIntegerCellValue(ICell cell)
    {
        try
        {
            return Convert.ToInt32(getExcelCellValue(cell));
        }
        catch
        {
            return 0;
        }
    }

    // 取得日期欄位值 [忽略錯誤]
    private DateTime? getDateTimeCellValue(ICell cell)
    {
        try
        {
            string textDateTime = getExcelCellValue(cell).ToString()!;
            return string.IsNullOrEmpty(textDateTime) ? null : Convert.ToDateTime(textDateTime);
        }
        catch
        {
            return null;
        }
    }

    // 取得文字欄位值 [忽略錯誤]
    private string? getStringCellValue(ICell cell)
    {
        try
        {
            return Convert.ToString(getExcelCellValue(cell));
        }
        catch
        {
            return null;
        }
    }

    // 取得金額欄位值 [忽略錯誤]
    private decimal getDecimalCellValue(ICell cell)
    {
        try
        {
            return Convert.ToDecimal(getExcelCellValue(cell));
        }
        catch
        {
            return 0;
        }
    }

    #endregion
}