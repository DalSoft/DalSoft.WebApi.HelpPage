# DalSoft.WebApi.HelpPage

Want to understand the background of why this exists? [Head over my blog post on using Web API Help pages with Owin](http://www.dalsoft.co.uk/blog/index.php/2016/01/13/introducing-dalsoft-webapi-helppage-help-pages-for-asp-net-web-api-working-on-owin-self-host/).

## Getting Started

The first thing you need to to is create a [OWIN backed Web API project](http://www.dalsoft.co.uk/blog/index.php/2015/07/21/professional-webapi-part-1-webapi-bootstrap-without-any-bloat-using-owin/). Or just fork https://github.com/DalSoft/DalSoft.WebApi.Bootstrap.SystemWeb

Next install the DalSoft.WebApi.HelpPage NuGet package:
```dos
PM> Install-Package DalSoft.WebApi.HelpPage
```
Now we need to tell the project to generate the XML documentation file, to do this right click on your Web API project and select properties, click on the 'Build' tab and then tick 'XML documentation file:' and the take rest of the defaults as is.

![alt tag](http://www.dalsoft.co.uk/blog/wp-content/uploads/2015/11/xml-documentation1.png)

Add XML documentation to your Web API controller actions (just type type 3 forward slashes directly above your method to get the XML documentation comment template).

Now in Startup.cs add the following line of code:
```cs
app.UseWebApiHelpPage(config);
```

> Where app is IAppBuilder and config is HttpConfiguration

Run your project and in the browser add /help to your Web API host (localhost/help for example) to get the generated help. 

## How does DalSoft.WebApi.HelpPage work?

Installing the DalSoft.WebApi.HelpPage NuGet package copies Razor templates to Help\DalSoft.WebApi.HelpPage.Views with their Build action set to 'Content' and Copy To Output Directory set to 'Copy if newer'. So the templates will be copied to your bin folder with your XML comments file. DalSoft.WebApi.HelpPage.dll is essentially OWIN Middleware that uses the XmlDocumentationProvider and RazorEngine to render the help pages it's as simple as that. Calling app.UseWebApiHelpPage(config) just configures the middleware.

## What's with all the compiler warnings?

Once you add XML documentation to a project (by ticking the box in the project), you will get compiler warnings for any public visible methods you probably don't want this. Fortunately it is is easy suppress this, to do this right click on your Web API project and select properties, click on the 'Build' tab and and type 1591 in the text box 'Suppress warnings'.

## Configuration
All the configuration you need to do is done using app.UseWebApiHelpPage(config) extension method.

If you want your help pages to use a different route (for example /help-pages):
```cs
app.UseWebApiHelpPage(config, "help-pages");
```

If you want your help pages templates to live in a different folder (see Help Page template customization): 
```cs
var viewsPhysicalPath = HelpPageMiddlewareExtensions.GetBinPath() + "\\MyViews";
app.UseWebApiHelpPage(config, viewsPhysicalPath:viewsPhysicalPath);
```
> Note relative paths are not supported

If you want your XML documentation to live in a different folder (the default is in the bin folder): 
```cs
var xmlDocPhysicalPath = HelpPageMiddlewareExtensions.GetBinPath() + "\\myxmldocs";
app.UseWebApiHelpPage(config, xmlDocumentationPhysicalPath:xmlDocPhysicalPath);
```
> Note relative paths are not supported

If you want to extend the templates model with your own viewbag (see Help Page template customization): 
```cs
app.UseWebApiHelpPage(config, viewBag: new { Environment = "dev" });
```
> All the Razor template model types are dynamic, so you can just access the viewbag in the usual way

### Customizing the help page templates
The whole of this this project was to make customizing the help page templates as easy possible. And it doesn't get any easier open the .cshtml Help\DalSoft.WebApi.HelpPage.Views and make the changes you want, add any branding css, front-end frameworks go nuts. 

There is one problem with the above, when you update the NuGet package the .cshtml templates will get overwritten. DalSoft.WebApi.HelpPage solves this issue in two ways:

The first is the package doesn't overwrite the templates blindly, instead you get prompted, of course if you say yes to the prompt then they will get overwritten. 

The second approach which is better for advanced or heavy customization is to copy the templates to another folder for example myhelp, and use viewsPhysicalPath parameter when calling UseWebApiHelpPage:
```cs
var viewsPhysicalPath = HelpPageMiddlewareExtensions.GetBinPath() + "\\myhelp";
app.UseWebApiHelpPage(config, viewsPhysicalPath:viewsPhysicalPath);
```
> **You must make sure you set Build action set to 'Content' and Copy To Output Directory to 'Copy if newer' for all the .cshtml templates and any assets CSS, JS etc**

You can safety delete the folder DalSoft.WebApi.HelpPage. Now when you update the NuGet package the DalSoft.WebApi.HelpPage folder is recreated with the updated code. By reviewing the templates in the DalSoft.WebApi.HelpPage folder you can check out any new features or breaking changes (breaking changes I will try to limit) and change your custom templates. When you are happy delete DalSoft.WebApi.HelpPage and repeat this process for future updates.

Lastly in your custom templates you might want to add your own properties to the view bag to further customize the templates. This is achieved using the viewBag parameter parameter.
```cs
app.UseWebApiHelpPage(config, viewBag: new { Greeting = "Hello, Darran" });
```

The view bag is dynamic so all you need to do to use the new property in the Razor template is:
```cs
@ViewBag.Greeting 
```
You can use all the normal Razor syntax if statements etc.

> **Html helpers are not supported because they are MVC specific**
