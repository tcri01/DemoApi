/**
 * AI 客服跳窗邏輯
 */
document.addEventListener("DOMContentLoaded", () => {
    const chatBtn = document.getElementById("chat-widget-btn");
    const chatWindow = document.getElementById("chat-widget-window");
    const chatClose = document.getElementById("chat-close-btn");
    const chatInput = document.getElementById("chat-input");
    const chatSend = document.getElementById("chat-send-btn");
    const chatContent = document.getElementById("chat-content");

    // 切換視窗顯示
    const toggleWindow = () => {
        chatWindow.classList.toggle("active");
        if (chatWindow.classList.contains("active")) {
            chatInput.focus();
        }
    };

    chatBtn.addEventListener("click", toggleWindow);
    chatClose.addEventListener("click", toggleWindow);

    // 新增對話泡泡邏輯
    const addMessage = (text, type = "user") => {
        const msgDiv = document.createElement("div");
        msgDiv.className = `message ${type}`;
        msgDiv.innerText = text;
        chatContent.appendChild(msgDiv);
        chatContent.scrollTop = chatContent.scrollHeight;
    };

    // 發送問題至 API
    const handleSend = async () => {
        const question = chatInput.value.trim();
        if (!question) return;

        addMessage(question, "user");
        chatInput.value = "";

        // 模擬正在輸入
        const loadingMsg = "正在為您查詢...";
        addMessage(loadingMsg, "ai");
        const lastMsg = chatContent.lastElementChild;

        try {
            const response = await fetch("/api/chat/ask", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ question })
            });
            const data = await response.json();
            
            // 更新最後一則對話（取代正在輸入中）
            lastMsg.innerText = data.answer;
        } catch (error) {
            console.error("AI 客服請求失敗:", error);
            lastMsg.innerText = "抱歉，系統發生錯誤，請稍後再試。";
        }
    };

    chatSend.addEventListener("click", handleSend);
    chatInput.addEventListener("keypress", (e) => {
        if (e.key === "Enter") handleSend();
    });

    // 初始歡迎訊息
    setTimeout(() => {
        addMessage("您好！我是櫻花升降機客服，請問有什麼我可以幫您的？您可以詢問本公司的電話、地址或營業時間。", "ai");
    }, 1000);
});
