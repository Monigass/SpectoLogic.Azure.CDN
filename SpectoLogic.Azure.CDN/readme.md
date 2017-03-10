# SpectoLogic.Azure.CDN
This .NET Standard Libray is thought to be used with an ASP.NET Core application.
The library helps to create tokenized links to your content cached in Azure Verizons CDN servers.

## Licence

Licensed under the Apache License (see attached License file)

## HowTo

### 1. Setup Azure CDN (Verizon Premium)

* <a href="https://docs.microsoft.com/en-us/azure/cdn/cdn-create-new-endpoint">Getting started with Azure CDN</a>
* <a href="https://docs.microsoft.com/en-us/azure/cdn/cdn-token-auth">Securing Azure CDN assets with token authentication</a>

### 2. Create a ASP.NET Core Application
Required Nugget Packages:
*  Microsoft.Extensions.Configuration (1.1.1)
*  Microsoft.Extensions.Configuration.Json (1.1.1)
*  Microsoft.AspNetCore.HttpOverrides (1.1.1)

Add using statement to file startup.cs<br/>
<code>
using SpectoLogic.Azure.CDN;
</code>

Add following code to the method: 
<br/>public void ConfigureServices(IServiceCollection services)
<br/>
<code style="color:green">
// Required services for CDNMedia</code>
<code style="color:black">services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();<br/>
services.Configure<MediaConfig>(Configuration.GetSection(MediaConfig.MediaConfigSectionName));<br/>
services.AddScoped<IMediaUrlProvider,CDNMedia>();</code>
<br/>
<br/>
Add following code to the method: 
<br/>public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
<br/>
<code style="color:green">// We want to forward headers to ensure we get the right remote<br/>
// IP address in case we use restricted IP Address</code><br/>
<code style="color:black">app.UseForwardedHeaders();</code>

### 3. Create Configuration
BaseUrl contains the base URL to you Azure CDN Node and Key is the secret key you use to validate and create the tokens.
You can then create multiple named policies. You might create policies with different expiring time, block content to certain
areas of the world,...

Following values are supported:<br/>
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

<code>"mediaConfig": {	<br/>
&nbsp;&nbsp;&nbsp;"BaseUrl": "https://cdn.contoso.com/media",	<br/>
&nbsp;&nbsp;&nbsp;"Key": "12345",	<br/>
&nbsp;&nbsp;&nbsp;"Policies": [ 	<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{ 	<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"Name": "restrictGeo", 	<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"DeniedCountries": "UK,FR" 	<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;} 	<br/>
&nbsp;&nbsp;&nbsp;] 	<br/>
  }</code> 	<br/>


### 4. Use URL Creator
#### Inside a Razor Page
Use page dependency injection and create tokenized Urls:<br/>

Add this on the top of the cshtml-File:<br/>
<code>@using SpectoLogic.Azure.CDN;<br/>
@inject IMediaUrlProvider CDNMedia</code>
<br/>
And within the cshtml-File use this to create a CDN-Image f.e.:<br/>
<code>&lt;img src="@(CDNMedia.Url("/images/sandcastle.png"))" /&gt;</code>
<br/>
or use a named policy:<br/>
<code>&lt;img src="@(CDNMedia.Url("/images/sandcastle.png","myPolicy"))" /&gt;</code>
<br/>

#### Inside a Controller
Use dependency injection to inject IMediaUrlProvider and create tokenized Urls by calling it's Url Method.

<code>public HomeController(IMediaUrlProvider cdnmedia)<br/>
{<br/>
&nbsp;&nbsp;&nbsp;string tokenCDNUrl = cdnmedia.Url("/images/cat.png", "myPolicy");<br/>
}</code><br/>

