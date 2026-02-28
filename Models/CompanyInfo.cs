namespace DemoApi.Models
{
    /// <summary>
    /// 公司固定資料的資料模型，用於 AI 客服查詢基礎。
    /// </summary>
    public class CompanyInfo
    {
        public string Name { get; set; } = string.Empty; // 公司名稱
        public string Phone { get; set; } = string.Empty; // 公司電話
        public string Address { get; set; } = string.Empty; // 公司地址
        public string Email { get; set; } = string.Empty; // 電子郵件
        public string BusinessHours { get; set; } = string.Empty; // 營業時間
        public string OtherDetails { get; set; } = string.Empty; // 其他細節
    }
}
