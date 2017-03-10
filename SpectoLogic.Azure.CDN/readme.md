# SpectoLogic.Azure.CDN
This libray makes it easy to create links to protected Azure Verizon CDN content.
This is achieved by creating encrypted tokens.

This library supports .NET Standard 1.4 and can be used with an ASP.NET Core application.

## Howto setup and use

### 1. Setup Azure CDN (Verizon Premium)

* <a href="https://docs.microsoft.com/en-us/azure/cdn/cdn-create-new-endpoint">Getting started with Azure CDN</a>
* <a href="https://docs.microsoft.com/en-us/azure/cdn/cdn-token-auth">Securing Azure CDN assets with token authentication</a>

### 2. Create a ASP.NET Core Application
Required Nugget Packages:
*  Microsoft.Extensions.Configuration (1.1.1)
*  Microsoft.Extensions.Configuration.Json (1.1.1)
*  Microsoft.AspNetCore.HttpOverrides (1.1.1)

Required References:
*  SpectoLogic.Azure.CDN.dll (Nugget Package planned)

Add using statement to file startup.cs
```c#
using SpectoLogic.Azure.CDN;
```

Add following code to the method:
 
*public void ConfigureServices(IServiceCollection services)*

```c#
// Required services for CDNMedia</code>
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.Configure<MediaConfig>(Configuration.GetSection(MediaConfig.MediaConfigSectionName));
services.AddScoped<IMediaUrlProvider,CDNMedia>();
```


Add following code to the method: 

*public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)*

```c#
// We want to forward headers to ensure we get the right remote
// IP address in case we use restricted IP Address
app.UseForwardedHeaders();
```

### 3. Create Configuration
BaseUrl contains the base URL to you Azure CDN Node and Key is the secret key you use to validate and create the tokens.
You can then create multiple named policies. You might create policies with different expiring time, block content to certain
areas of the world,...

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
"mediaConfig": {	
	"BaseUrl": "https://cdn.contoso.com/media",
	"Key": "12345",
	"Policies": [
		{ 
			"Name": "restrictGeo",
			"DeniedCountries": "UK,FR"
		}
	]
}
```


### 4. Use URL Creator
#### Inside a Razor Page
Use page dependency injection and create tokenized Urls:

Add this on the top of the cshtml-File:
```
@using SpectoLogic.Azure.CDN;
@inject IMediaUrlProvider CDNMedia
```

And within the cshtml-File use this to create a CDN-Image f.e.:
```c#
<img src="@(CDNMedia.Url("/images/sandcastle.png"))" />;
```

or use a named policy:
```c#
<img src="@(CDNMedia.Url("/images/sandcastle.png","myPolicy"))" />
```

#### Inside a Controller
Use dependency injection to inject IMediaUrlProvider and create tokenized Urls by calling it's Url Method.

```c#
public HomeController(IMediaUrlProvider cdnmedia)
{
	string tokenCDNUrl = cdnmedia.Url("/images/cat.png", "myPolicy");
}
```

## Licence

Licensed under the Apache License. This library uses BouncyCastle
Crypto Library which is goverend by an adapted MIT X11 License (see attached License file)

* Token Encryption - (c) by Verizon 2016
* BouncyCastle - (c) by The Legion of the Bouncy Castle Inc. 
* Rest of the library - (c) by SpectoLogic e.U.

