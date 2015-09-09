using System.Reflection;
using System.Web.Http;
using Owin;

namespace DalSoft.WebApi.HelpPage
{
    public static class HelpPageMiddlewareExtensions
    {
        public static void UseWebApiHelpPage(this IAppBuilder app, HttpConfiguration httpConfiguration)
        {
            UseWebApiHelpPage(app, httpConfiguration, "help", Assembly.GetCallingAssembly().GetName().Name + ".xml");
        }

        public static void UseWebApiHelpPage(this IAppBuilder app, HttpConfiguration httpConfiguration, string helpPath, string xmlDocumentationPath)
        {
            if (helpPath.StartsWith("/")) helpPath = helpPath.Substring(1, helpPath.Length - 1);
           
            // Configure help page
            httpConfiguration.SetDocumentationProvider(new XmlDocumentationProvider(xmlDocumentationPath));

            app.Map("/" + helpPath, appBuilder => appBuilder.Use<HelpPageMiddleware>(httpConfiguration, "help"));
        }
    }
}
