using Business;
using LinkConverterApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace Test
{
    public class ConverterControllerTest
    {
        private readonly ConverterController _apiConverter;
        private readonly RedisCacheService _redisCache;

        public ConverterControllerTest()
        {
            _apiConverter = new ConverterController();
            _redisCache = new RedisCacheService();
        }
        [Fact]
        public void OkResultConverterLink()
        {
            string mockData = "https://www.trendyol.com/casio/saat-p-1925865?boutiqueId=439892&merchantId=105064";
            IActionResult result = _apiConverter.ConvertLink(mockData);
            OkObjectResult objectResponse = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResponse.StatusCode);

        }
        [Fact]
        public void NotFoundConverterLink()
        {
            string mockData = "https://www.trend123.com/casio/saat-p-1925865?boutiqueId=439892&merchantId=105064";
            IActionResult result = _apiConverter.ConvertLink(mockData);
            NotFoundResult objectResponse = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, objectResponse.StatusCode);
        }
        [Fact]
        public void StartWithWebtoDeepResponseConverterLink()
        {
            string mockData = "https://www.trendyol.com/";
            IActionResult result = _apiConverter.ConvertLink(mockData);
            OkObjectResult objectResponse = Assert.IsType<OkObjectResult>(result);
            Assert.StartsWith("ty://?", objectResponse.Value.ToString().Replace("\"", ""));
        }
        [Fact]
        public void StartWithDeeptoWebResponseConverterLink()
        {
            string mockData = "ty://?";
            IActionResult result = _apiConverter.ConvertLink(mockData);
            OkObjectResult objectResponse = Assert.IsType<OkObjectResult>(result);
            Assert.StartsWith("https://www.trendyol.com/", objectResponse.Value.ToString().Replace("\"", ""));
        }

        [Theory]
        [InlineData("https://www.trendyol.com/casio/saat-p-1925866?boutiqueId=439892&merchantId=105064")]
        public void RedisCacheSaveTest(string request)
        {
            IActionResult result = _apiConverter.ConvertLink(request);
            OkObjectResult objectResponse = Assert.IsType<OkObjectResult>(result);
            var cacheValue = _redisCache.Get(request);
            Assert.Equal(objectResponse.Value, cacheValue);

        }
        [Theory]
        [InlineData("https://www.trendyol.com/casio/saat-p-1925865?boutiqueId=439892&merchantId=105064")]
        public void RedisCacheRemoveTest(string request)
        {
            IActionResult result = _apiConverter.ConvertLink(request);
            OkObjectResult objectResponse = Assert.IsType<OkObjectResult>(result);
            _redisCache.Remove(request);
            var cacheValue = _redisCache.Get(request);
            Assert.Null(cacheValue);
        }
    }
}
