using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class BaseEntity
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 顯示順序
    /// </summary>
    public int DisplayOrder { get; set; }
    /// <summary>
    /// 建立使用者姓名 (顯示用)
    /// </summary>
    public string? CreateName { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime? CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime? UpdateTime { get; set; } = DateTime.Now;
}

