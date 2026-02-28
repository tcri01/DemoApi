using DemoApi.Services;
using DemoApi.Models;
using Microsoft.Extensions.Configuration;
using Xunit;
using Moq;
using System.Collections.Generic;

namespace DemoApi.Tests
{
    public class AIServiceTests
    {
        private readonly AIService _aiService;
        private readonly IConfiguration _configuration;

        public AIServiceTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"CompanyInfo:Name", "Test Sakura"},
                {"CompanyInfo:Phone", "02-12345678"},
                {"CompanyInfo:Address", "Test Address"},
                {"CompanyInfo:BusinessHours", "Mon-Fri"},
                {"AISettings:ApiKey", "DUMMY_KEY"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _aiService = new AIService(_configuration);
        }

        [Theory]
        [InlineData("請問電話是多少", "02-12345678")]
        [InlineData("地址在哪裡", "Test Address")]
        [InlineData("今天天氣好嗎", "無資料")]
        [InlineData("", "無資料")]
        public async Task GetChatResponseAsync_ShouldReturnCorrectAnswerOrNoData(string question, string expectedPart)
        {
            // Act
            var result = await _aiService.GetChatResponseAsync(question);

            // Assert
            if (expectedPart == "無資料")
            {
                Assert.Equal("無資料", result);
            }
            else
            {
                Assert.Contains(expectedPart, result);
            }
        }
    }
}
