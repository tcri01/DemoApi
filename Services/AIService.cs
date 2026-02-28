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
        private readonly string _companyMarkdown; // 儲存從 Markdown 檔案讀取的公司資訊內容
        private readonly string _apiKey;           // AI 服務的 API 金鑰
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env; // 主機環境資訊，用於取得檔案路徑

        /// <summary>
        /// 初始化 AIService，從 Markdown 檔案讀取公司資訊，從配置中讀取 AI 設定。
        /// </summary>
        /// <param name="configuration">應用程式配置介面</param>
        /// <param name="env">主機環境介面</param>
        public AIService(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _env = env;
            // 從配置讀取 API Key
            _apiKey = configuration["AISettings:ApiKey"] ?? string.Empty;

            // 從 Markdown 檔案讀取公司資訊
            var filePath = System.IO.Path.Combine(_env.ContentRootPath, "Resources", "CompanyInfo.md");
            if (System.IO.File.Exists(filePath))
            {
                _companyMarkdown = System.IO.File.ReadAllText(filePath);
            }
            else
            {
                _companyMarkdown = "暫無公司資訊。";
            }
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

        /// <summary>
        /// 獲取 AI 客服針對訪問者問題的回答。
        /// </summary>
        /// <param name="question">訪問者的提問</param>
        /// <returns>AI 的回答或「無資料」</returns>
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

                // 2. 處理 System Instruction (僅基於 Markdown 資料回答)
                string systemInstruction = $"你是一個櫻花升降機的客服 AI。請僅使用以下資料回答問題。如果資料中沒有提到答案，請直接回答『無資料』，不要多加解釋，當客戶詢問項目為多項時，請掃描『清單』中所有的條目。只要名稱或功能部分匹配，就視為有資料。固定資料如下 (Markdown 格式)：\n{_companyMarkdown}";

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
