using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkConverterApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private RedisCacheService Service { get; set; }
        public ConverterController()
        {
            Service = new RedisCacheService();
        }

        static private string CampaingIdAndMerchantIdConvert(string saltUrl, string response)
        {
            string detailPageUrl = saltUrl.Substring(saltUrl.Split('&')[0].Length + saltUrl.Split('&')[1].Length);
            bool isCampaingId = detailPageUrl.Contains("CampaignId");
            bool isMerchantId = detailPageUrl.Contains("MerchantId");
            string campaingId = null;
            string merchantId = null;
            if (isCampaingId && isMerchantId)// Daha esnek olmalı
            {
                var pageDetails = detailPageUrl.Split('&');
                campaingId = pageDetails[0].Contains("CampaignId") ? pageDetails[0].Replace("CampaignId=", "") : pageDetails[1].Replace("CampaignId=", "");
                merchantId = pageDetails[1].Contains("MerchantId") ? pageDetails[1].Replace("MerchantId=", "") : pageDetails[0].Replace("MerchantId=", "");
                response += "?boutiqueId=" + campaingId;
                response += "&merchantId=" + merchantId;
            }
            else if (isCampaingId)
            {
                campaingId = detailPageUrl.Replace("CampaignId=", "");
                response += "?boutiqueId=" + campaingId;
            }
            else if (isMerchantId)
            {
                merchantId = detailPageUrl.Replace("merchantId=", "");
                response += "?merchantId" + merchantId;
            }

            return response;

        }
        static private string ButiqueIdAndMerchantIdConvert(string saltUrl, string response)
        {
            string detailPageUrl = saltUrl.Substring(saltUrl.IndexOf("?") + 1);
            bool isBoutiqueId = detailPageUrl.Contains("boutiqueId");
            bool isMerchantId = detailPageUrl.Contains("merchantId");
            string boutiqueId = null;
            string merchantId = null;
            if (isBoutiqueId && isMerchantId)// Parametre olarak eklenmeli(ok)
            {
                var pageDetails = detailPageUrl.Split('&');
                boutiqueId = pageDetails[0].Contains("boutiqueId") ? pageDetails[0].Replace("boutiqueId=", "") : pageDetails[1].Replace("boutiqueId=", "");
                merchantId = pageDetails[1].Contains("merchantId") ? pageDetails[1].Replace("merchantId=", "") : pageDetails[0].Replace("merchantId=", "");
                response += "&CampaignId=" + boutiqueId;
                response += "&MerchantId=" + merchantId;
            }
            else if (isBoutiqueId)
            {
                boutiqueId = detailPageUrl.Replace("boutiqueId=", "");
                response += "&CampaignId=" + boutiqueId;
            }
            else if (isMerchantId)
            {
                merchantId = detailPageUrl.Replace("merchantId=", "");
                response += "&MerchantId" + merchantId;

            }

            return response;
        }
        static private string SearchQueryConvert(string saltUrl, string response)
        {
            if (saltUrl.StartsWith("sr"))
            {
                response += "Page=Search";
                string query = saltUrl.Substring(saltUrl.IndexOf("sr") + 2).Replace("?q=", "");
                response += "&Query=" + ChangeTurkishCharacter(query);

            }
            else
            {
                response += "sr";
                string query = saltUrl.Split('&')[1].Replace("Query=", "");
                response += "?q=" + ChangeTurkishCharacter(query);
            }

            return response;
        }
        static private string ContentIdConvert(string saltUrl, string response)
        {
            if (saltUrl.Contains("-p-"))
            {
                response += "Page=Product";
                string contentId = saltUrl.Substring(saltUrl.IndexOf("-p-") + 3).Split('?').First();
                response += "&ContentId=" + contentId;
            }
            else
            {
                response += "brand/name-p-";
                string contentId = saltUrl.Split('&')[1].Replace("ContentId=", "");
                response += contentId;
            }

            return response;
        }
        static private string ChangeTurkishCharacter(string text)
        {
            text = text.Replace("İ", "I");
            text = text.Replace("ı", "i");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ö", "O");
            text = text.Replace("ö", "o");
            text = text.Replace("Ü", "U");
            text = text.Replace("ü", "u");
            text = text.Replace("Ş", "S");
            text = text.Replace("ş", "s");
            text = text.Replace("Ç", "C");
            text = text.Replace("ç", "c");
            return text;
        }

        // POST api/values
        [HttpPost]
        public IActionResult ConvertLink([FromBody] string request)
        {
            var cacheData = Service.Get(request);
            string saltUrl, response = "";
            if (request.Contains("https://www.trendyol.com/"))
            {
                saltUrl = request.Replace("https://www.trendyol.com/", "");
                response = "ty://?";

                if (cacheData != null)
                {
                    return Ok(cacheData);
                }
                else if (saltUrl.Contains("-p-"))
                {
                    response = ContentIdConvert(saltUrl, response);
                    if (saltUrl.Contains("?")) response = ButiqueIdAndMerchantIdConvert(saltUrl, response);

                    Service.Set(request, response);

                }
                else if (saltUrl.StartsWith("sr"))
                {
                    response = SearchQueryConvert(saltUrl, response);

                    Service.Set(request, response);

                }
                else
                {
                    response += "Page=Home";

                    Service.Set(request, response);
                }
            }
            else if (request.Contains("ty://?"))
            {
                saltUrl = request.Replace("ty://?", "");
                response = "https://www.trendyol.com/";
                if (cacheData != null)
                {
                    return Ok(cacheData);
                }
                else if (saltUrl.Contains("Page=Product"))
                {
                    response = ContentIdConvert(saltUrl, response);
                    if (saltUrl.Split('&').Length > 2) response = CampaingIdAndMerchantIdConvert(saltUrl, response);

                    Service.Set(request, response);

                }
                else if (saltUrl.Contains("Page=Search"))
                {
                    response = SearchQueryConvert(saltUrl, response);

                    Service.Set(request, response);

                }
                else
                {
                    Service.Set(request, response);
                }
            }
            else
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
