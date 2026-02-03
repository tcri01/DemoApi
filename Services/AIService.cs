namespace DemoApi.Services
{
    public interface IAIService
    {
        Task<string> SuggestNotesAsync(string memberName, string bio);
        Task<string> CompleteDescriptionAsync(string currentText);
    }

    public class AIService : IAIService
    {
        public async Task<string> SuggestNotesAsync(string memberName, string bio)
        {
            // TODO: Hook up to OpenAI / Claude / Gemini API
            // For now, returning a placeholder as requested.
            await Task.Delay(500); // Simulate API call
            return $"AI Suggestion for {memberName}: 基於該會員的背景「{bio}」，建議關注其在技術社群的活躍度。";
        }

        public async Task<string> CompleteDescriptionAsync(string currentText)
        {
            // TODO: Hook up to OpenAI / Claude / Gemini API
            await Task.Delay(500); 
            return currentText + " [AI 補全內容：該會員展現了卓越的團隊協作能力，且具備深厚的技術背景，是一位值得長期合作的專業人士。]";
        }
    }
}
