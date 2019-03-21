# VerizonDigital.CDN.TokenGenerator
This libray makes it easy to create links to protected Azure Verizon CDN content.
This is achieved by creating encrypted tokens.

## Howto setup and use

### 1. Setup Azure CDN (Verizon Premium)

* <a href="https://docs.microsoft.com/en-us/azure/cdn/cdn-create-new-endpoint">Getting started with Azure CDN</a>
* <a href="https://docs.microsoft.com/en-us/azure/cdn/cdn-token-auth">Securing Azure CDN assets with token authentication</a>

### 2. Create a ASP.NET Core Application
Required Nugget Packages:
*  Microsoft.Extensions.Configuration
*  Microsoft.Extensions.Configuration.Json
*  Microsoft.AspNetCore.HttpOverrides

Required References:
*  VerizonDigital.CDN.TokenProvider.dll (Nugget Package planned)

Add using statement to file startup.cs
```c#
using VerizonDigital.CDN.TokenGenerator;
```

Add following code to the method:
 
*public void ConfigureServices(IServiceCollection services)*

```c#
// Required services for CDNTokenProvider</code>
services.Configure<MediaConfig>(Configuration.GetSection(MediaConfig.MediaConfigSectionName));
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<ICDNTokenProvider, CDNTokenProvider>();
```


Add following code to the method: 

*public void Configure(IApplicationBuilder app, IHostingEnvironment env)*

```c#
// We want to forward headers to ensure we get the right remote
// IP address in case we use restricted IP Address
app.UseForwardedHeaders();
```

### 3. Create Configuration
'baseImageUrl' contains the base URL to your image and Azure CDN Node, and 'key' is the secret key you use to validate and create the tokens.
You can then create multiple named policies. You might create policies with different expiring time, block content to certain
areas of the world, etc...

Following values are supported:
* Name
* ExpirationTimeSpan
* RestrictIPAddress (None or Request)
* AllowedUrls
* AllowedCountries
* DeniedCountries
* AllowedReferers
* DeniedReferers
* AllowedProtocol
* DeniedProtocol

You can find a description of valid input <a href="https://docs.microsoft.com/en-us/azure/cdn/cdn-token-auth">here</a>.

```json
"baseImageUrl": "https://cdn.contoso.com/media/images/cat.png",
"mediaConfig": {
    "key": "12345",
    "policies": [
      {
        "name": "default",
        "allowedReferers": "localhost"
      }
    ]
  }
}
```


### 4. Use URL Creator
#### Inside a Razor Page
Use page dependency injection and create tokenized Urls:

Add this on the top of the cshtml-File:
```
@using VerizonDigital.CDN.TokenGenerator;
@inject ICDNTokenProvider tokenProvider```

And within the cshtml-File use this to create a CDN-Image f.e.:
```c#
<img src="@($"{configuration["baseImageUrl"]}?{tokenProvider.NewToken()}")" />
```

or use a named policy:
```c#
<img src="@($"{configuration["baseImageUrl"]}?{tokenProvider.NewToken("default")}")" />
```

#### Inside a Controller
Use dependency injection to inject ICDNTokenProvider and create tokenized Urls by calling it's Url Method.

```c#
public HomeController(ICDNTokenProvider tokenProvider)
{
	var tokenCDNUrl = tokenProvider.NewToken("default");
	var url = $"myurl?{tokenCDNUrl}";
}
```

## Licence

Licensed under the Apache License. This library uses BouncyCastle
Crypto Library which is goverend by an adapted MIT X11 License (see attached License file)

* Token Encryption - (c) by Verizon 2016
* BouncyCastle - (c) by The Legion of the Bouncy Castle Inc. 
* Rest of the library - (c) by SpectoLogic e.U.

