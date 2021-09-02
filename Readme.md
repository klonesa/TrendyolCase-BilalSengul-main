<p align="center"><a href="https://github.com/DevelopmentHiring/TrendyolCase-BilalSengul" target="_blank"></a></p>

<h1 align="center">Trendyol Link Converter</h1>

<div align="center">
 <strong>
   Convert URLs to deeplinks or deeplinks to URLs.
 </strong>
</div>


Web applications use URLs and mobile applications use deeplinks. Both applications use
links to redirect specific locations inside applications. When you want to redirect across applications, you should convert URLs to deeplinks or deeplinks to URLs.

<br></br>
**Project Trailer:**
<br></br>
[![video](https://miro.medium.com/max/640/1*XthWUtD_NU-VJ7ESA2qX3A.jpeg)](https://www.youtube.com/watch?v=WqCqv8RBuJg&ab_channel=Bilal%C5%9Eeng%C3%BCl)

<br></br>

## Installation


* Build on Docker
```bash
$ Docker build -t linkconverterapi .
```

## Actions



| Action		         | Explanation								      |
| ---------------------- |:----------------------------------------------:|
| `ConvertLink`				 | The process of learning deep link or web url and convert url to deeplink or deeplink to url .
| `ChangeTurkishCharacter`				 | Converts turkish characters to english
| `ContentIdConvert`				 | Content Id converter method
| `SearchQueryConvert`				 | Search Query converter method
| `ButiqueIdAndMerchantIdConvert`				 | Butique Id and Merchant Id  converter method
| `CampaingIdAndMerchantIdConvert`				 | Campaing Id and Merchant Id converter method

<br></br>
<br></br>
## Test

| Action		         | Explanation								      |
| ---------------------- |:----------------------------------------------:|
| `OkResultConverterLink`				 | Test HTTP status code and type for valid data
| `NotFoundConverterLink`				 | Test HTTP status code and type for invalid data
| `StartWithWebtoDeepResponseConverterLink`				 | Start with web url to deep link test
| `StartWithDeeptoWebResponseConverterLink`				 | Start with deep link to web url test
| `RedisCacheSaveTest`				 | Business layer redis save test 
| `RedisCacheRemoveTest`				 | Business layer redis remove test

<br></br>
<br></br>

## Results

**Web URL to Deeplink:**
| Request                 | Response		         |
| ---------------------- |:----------------------------------------------:|
| `https://www.trendyol.com/casio/saat-p-1925865?boutiqueId=439892&merchantId=105064`| `ty://?Page=Product&ContentId=1925865?&CampaignId=439892&MerchantId=105064` |
| `https://www.trendyol.com/casio/erkek-kol-saatip-1925865`	 | `ty://?Page=Product&ContentId=1925865` |
| `https://www.trendyol.com/casio/erkek-kol-saati-p-1925865?boutiqueId=439892`				 | `ty://?Page=Product&ContentId=1925865&CampaignId=439892` |
| `https://www.trendyol.com/casio/erkek-kol-saatip-1925865?merchantId=105064`				 | `ty://?Page=Product&ContentId=1925865&MerchantId=105064` |
| `https://www.trendyol.com/sr?q=elbise`				 | `ty://?Page=Search&Query=elbise` |
| `https://www.trendyol.com/sr?q=%C3%BCt%C3%BC`| `ty://?Page=Search&Query=%C3%BCt%C3%BC` |
| `https://www.trendyol.com/Hesabim/Favoriler`				 | `ty://?Page=Home` |
| `https://www.trendyol.com/Hesabim/#/Siparislerim`				 | `ty://?Page=Home` |

<br></br>
<br></br>
**Deeplink to Web URL:**
| Request                 | Response		         |
| ---------------------- |:----------------------------------------------:|
| `ty://?Page=Product&ContentId=1925865?CampaignId=439892&MerchantId=105064`				 | `https://www.trendyol.com/brand/name-p-1925865?boutiqueId=439892&merchantId=105064` |
| `ty://?Page=Product&ContentId=1925865`				 | `https://www.trendyol.com/brand/name-p-1925865` |
| `ty://?Page=Product&ContentId=1925865&CampaignId=439892`				 | `https://www.trendyol.com/brand/name-p-1925865?boutiqueId=439892` |
| `ty://?Page=Product&ContentId=1925865&MerchantId=105064`				 | `https://www.trendyol.com/brand/namei-p-1925865?merchantId=105064` |
| `ty://?Page=Search&Query=elbise`				 | `https://www.trendyol.com/sr?q=elbise` |
| `ty://?Page=Search&Query=%C3%BCt%C3%BC`				 | `https://www.trendyol.com/sr?=%C3%BCt%C3%BC` |
| `ty://?Page=Favorites`				 | `https://www.trendyol.com` |
| `ty://?Page=Orders`				 | `https://www.trendyol.com` |













