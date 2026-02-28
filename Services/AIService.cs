using DemoApi.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DemoApi.Services
{
    public interface IAIService
    {
        Task<string> SuggestNotesAsync(string memberName, string bio);
        Task<string> CompleteDescriptionAsync(string currentText);
        /// <summary>
        /// 獲取 AI 客服針對訪問者問題的回答。
        /// 僅基於固定資料回答，無關問題回傳「無資料」。
        /// </summary>
        Task<string> GetChatResponseAsync(string question);
    }

    public class AIService : IAIService
    {
        private readonly CompanyInfo _companyInfo;

        public AIService(IConfiguration configuration)
        {
            // 將設定檔中的公司資訊對應至模型
            _companyInfo = configuration.GetSection("CompanyInfo").Get<CompanyInfo>() ?? new CompanyInfo();
        }

        public async Task<string> SuggestNotesAsync(string memberName, string bio)
        {
            // TODO: Hook up to OpenAI / Claude / Gemini API
            await Task.Delay(500); 
            return $"AI Suggestion for {memberName}: 基於該會員的背景「{bio}」，建議關注其在技術社群的活躍度。";
        }

        public async Task<string> CompleteDescriptionAsync(string currentText)
        {
            // TODO: Hook up to OpenAI / Claude / Gemini API
            await Task.Delay(500); 
            return currentText + " [AI 補全內容：該會員展現了卓越的團隊協作能力，且具備深厚的技術背景，是一位值得長期合作的專業人士。]";
        }

        public async Task<string> GetChatResponseAsync(string question)
        {
            // 系統提示詞：限制 AI 只能基於固定資料回答
            string companyJson = JsonSerializer.Serialize(_companyInfo);
            string systemPrompt = $"你是一個櫻花升降機的客服 AI。請僅使用以下資料回答問題。如果資料中沒有提到答案，請回答『無資料』，且不要解釋。固定資料如下：\n{companyJson}";

            // TODO: 串接實際 API (如 OpenAI Chat Completion)
            // 這裡模擬 AI 查詢固定資料的關鍵字邏輯以符合「無資料」需求
            await Task.Delay(500);

            if (string.IsNullOrWhiteSpace(question)) return "無資料";

            // 簡易模擬 AI 邏輯：判斷問題是否包含特定關鍵字，若有則回答對應欄位，否則回傳無資料
            question = question.ToLower();
            if (question.Contains("電話") || question.Contains("聯繫")) return $"公司電話為：{_companyInfo.Phone}";
            if (question.Contains("地址") || question.Contains("在哪")) return $"公司地址位於：{_companyInfo.Address}";
            if (question.Contains("時間") || question.Contains("營業")) return $"營業時間：{_companyInfo.BusinessHours}";
            if (question.Contains("email") || question.Contains("信箱")) return $"電子郵件：{_companyInfo.Email}";
            if (question.Contains("你是誰") || question.Contains("公司名字")) return $"我們是：{_companyInfo.Name}";
            if (question.Contains("介紹") || question.Contains("做什麼")) return _companyInfo.OtherDetails;

            return "無資料";
        }
    }
}
