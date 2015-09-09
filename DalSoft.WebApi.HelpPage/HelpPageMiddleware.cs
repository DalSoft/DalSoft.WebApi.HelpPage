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
        private readonly string _helpPath;

        public HelpPageMiddleware(OwinMiddleware next, HttpConfiguration configuration, string helpPath) : base(next)
        {            
            _configuration = configuration;
            _helpPath = helpPath;
            
            var service = RazorEngineService.Create(new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { }),
                TemplateManager = new ResolvePathTemplateManager(new[] { "views", "views/DisplayTemplates" })
            });
            
            Engine.Razor = service;
        }
        
        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Uri.Segments.Last().ToLower().Replace("/", string.Empty) == "helppage.css")
            {
                context.Response.ContentType = "text/css";
                await context.Response.WriteAsync(File.ReadAllText(Path.Combine("views", "helppage.css")));
            }

            if (context.Request.Uri.Segments.Last().ToLower().Replace("/", string.Empty) == _helpPath)
            {
                var view = Engine.Razor.RunCompile("index.cshtml", typeof(ApiDescription), _configuration.Services.GetApiExplorer().ApiDescriptions);
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
                        var view = Engine.Razor.RunCompile("api.cshtml", typeof(ApiDescription), apiModel);
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(view);
                    }
                }

                context.Response.StatusCode = 404;
                context.Response.ReasonPhrase = "API not found.";
            }
            
            await Next.Invoke(context);
        }
    }
}
