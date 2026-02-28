using Google.GenAI;
using Google.GenAI.Types;
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
        private readonly string _apiKey;

        /// <summary>
        /// 初始化 AIService，從配置中讀取公司資訊與 AI 設定。
        /// </summary>
        /// <param name="configuration">應用程式配置介面</param>
        public AIService(IConfiguration configuration)
        {
            // 將設定檔中的公司資訊對應至模型
            _companyInfo = configuration.GetSection("CompanyInfo").Get<CompanyInfo>() ?? new CompanyInfo();
            // 從設定檔讀取 AI 相關設定
            _apiKey = configuration["AISettings:ApiKey"] ?? string.Empty;
        }

        /// <summary>
        /// 根據會員背景建議追蹤備註。
        /// </summary>
        /// <param name="memberName">會員姓名</param>
        /// <param name="bio">會員簡介</param>
        /// <returns>AI 建議的備註內容</returns>
        public async Task<string> SuggestNotesAsync(string memberName, string bio)
        {
            // TODO: 串接實際 API (目前為模擬回傳)
            await Task.Delay(500); 
            return $"AI Suggestion for {memberName}: 基於該會員的背景「{bio}」，建議關注其在技術社群的活躍度。";
        }

        /// <summary>
        /// 自動補全會員描述內容。
        /// </summary>
        /// <param name="currentText">目前的描述文字</param>
        /// <returns>補全後的描述文字</returns>
        public async Task<string> CompleteDescriptionAsync(string currentText)
        {
            // TODO: 串接實際 API (目前為模擬回傳)
            await Task.Delay(500); 
            return currentText + " [AI 補全內容：該會員展現了卓越的團隊協作能力，且具備深厚的技術背景，是一位值得長期合作的專業人士。]";
        }

        public async Task<string> GetChatResponseAsync(string question)
        {
            // 檢查 API Key 是否存在
            if (string.IsNullOrEmpty(_apiKey))
            {
                return "API 金鑰未配置。";
            }

            try 
            {
                // 1. 初始化官方 Google.GenAI Client (顯式指定為 Gemini Developer API 模式)
                var client = new Client(apiKey: _apiKey);

                // 2. 處理 System Instruction (僅基於固定資料回答)
                string companyJson = JsonSerializer.Serialize(_companyInfo);
                string systemInstruction = $"你是一個櫻花升降機的客服 AI。請僅使用以下資料回答問題。如果資料中沒有提到答案，請直接回答『無資料』，不要多加解釋，當客戶詢問項目為多項時，請掃描『清單』中所有的條目。只要名稱或功能部分匹配，就視為有資料。固定資料如下：\n{companyJson}";

                // 3. 呼叫 API
                var response = await client.Models.GenerateContentAsync("gemini-2.5-flash", question, new GenerateContentConfig
                {
                    SystemInstruction = new() { Parts = [new() { Text = systemInstruction }] },
                    Temperature = 0
                });

                return response.Candidates?[0].Content?.Parts?[0].Text ?? "無資料";
            }
            catch (Exception ex)
            {
                // 發生錯誤時記錄並回傳提示
                return $"AI 處理時發生錯誤: {ex.Message}";
            }
        }
    }
}
