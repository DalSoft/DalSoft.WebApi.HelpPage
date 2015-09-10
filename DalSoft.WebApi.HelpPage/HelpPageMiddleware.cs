using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Owin;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace DalSoft.WebApi.HelpPage
{
    public class HelpPageMiddleware : OwinMiddleware
    {
        private readonly HttpConfiguration _configuration;
        private readonly string _helpRoute;
        private readonly string _viewsPhysicalPath;
        private readonly IDictionary<string, object> _viewBag;

        public HelpPageMiddleware(OwinMiddleware next, HttpConfiguration configuration, string helpRoute, string viewsPhysicalPath, IDictionary<string, object> viewBag) : base(next)
        {            
            _configuration = configuration;
            _helpRoute = helpRoute;
            _viewsPhysicalPath = viewsPhysicalPath;
            _viewBag = viewBag;

            //TODO: validate Ctor parms

            var service = RazorEngineService.Create(new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { }),
                TemplateManager = new ResolvePathTemplateManager(new[] {  _viewsPhysicalPath, _viewsPhysicalPath+"\\DisplayTemplates" })
            });
            
            Engine.Razor = service;
        }
        
        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Uri.Segments.Last().ToLower().Replace("/", string.Empty) == "helppage_css") // Use path instead of .css to avoid SystemWeb hosted users having to change web.config https://serverfault.com/questions/657874/handler-mapping-on-iis-owin-nancy-for-a-csv-file/657880#657880
            {
                context.Response.ContentType = "text/css";
                await context.Response.WriteAsync(File.ReadAllText(Path.Combine(_viewsPhysicalPath, "helppage.css")));
            }
            
            if (context.Request.Uri.Segments.Last().ToLower().Replace("/", string.Empty) == _helpRoute)
            {
                var view = Engine.Razor.RunCompile("index.cshtml", typeof(ApiDescription), _configuration.Services.GetApiExplorer().ApiDescriptions, new DynamicViewBag(_viewBag));
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(view);
            }
            
            if (context.Request.Uri.Segments.Last().ToLower().Replace("/", string.Empty) == "api")
            {
                var apiId = context.Request.Query["apiId"];
                if (!string.IsNullOrEmpty(apiId))
                {
                    var apiModel = _configuration.GetHelpPageApiModel(apiId);
                    if (apiModel != null)
                    {
                        var view = Engine.Razor.RunCompile("api.cshtml", typeof(ApiDescription), apiModel, new DynamicViewBag(_viewBag));
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(view);
                    }
                }

                context.Response.StatusCode = 404;
                context.Response.ReasonPhrase = "API not found.";
            }

            //await Next.Invoke(context); UseWebApi doesn't Invoke Next() bug when using SystemWeb hosting http://stackoverflow.com/questions/18921215/cant-get-asp-net-web-api-2-help-pages-working-when-using-owin/19057466#19057466
        }
    }
}
