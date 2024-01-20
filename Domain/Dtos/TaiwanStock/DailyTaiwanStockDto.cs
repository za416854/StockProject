using Domain.Entities;

namespace Domain.Dtos
{
    public class DailyTaiwanStockDto
    {
        /// <summary>
        /// 股票 ID
        /// </summary>
        public int StockID { get; set; }

        /// <summary>
        /// 股票代號
        /// </summary>
        public int StockSymbol { get; set; }

        /// <summary>
        /// 股票名稱
        /// </summary>
        public string? StockName { get; set; }

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
        public string? SpecialNote { get; set; }

    }
}
