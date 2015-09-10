using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Web.Http;
using Owin;

namespace DalSoft.WebApi.HelpPage
{
    public static class HelpPageMiddlewareExtensions
    {
        public static void UseWebApiHelpPage(this IAppBuilder app, HttpConfiguration httpConfiguration, string helpRoute=null, string viewsPhysicalPath=null, string xmlDocumentationPhysicalPath=null, dynamic viewBag=null)
        {
            const string defaultRoute = "help";
            var executingAssemblyPath = GetBinPath();
            var executingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
            
            helpRoute = helpRoute ?? defaultRoute;

            viewsPhysicalPath = viewsPhysicalPath ?? executingAssemblyPath.Replace("file:\\", string.Empty) + "\\Help\\DalSoft.WebApi.HelpPage.Views";
            xmlDocumentationPhysicalPath = xmlDocumentationPhysicalPath ?? executingAssemblyPath + "//" + executingAssemblyName + ".xml";
            IDictionary<string, object> viewBagDictionary = DynamicToDictionary(viewBag);
            
            if (helpRoute.StartsWith("/"))  
                helpRoute = helpRoute.Substring(1, helpRoute.Length - 1);

            viewBagDictionary.Add("HelpRoute", helpRoute);

            // Configure help page
            httpConfiguration.SetDocumentationProvider(new XmlDocumentationProvider(xmlDocumentationPhysicalPath));

            app.Map("/" + helpRoute, appBuilder => appBuilder.Use<HelpPageMiddleware>(httpConfiguration, helpRoute, viewsPhysicalPath, viewBagDictionary));
        }

        public static string GetBinPath()
        {
            var executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) ?? string.Empty;
            return executingAssemblyPath.Replace("file:\\", string.Empty);
        }

        private static IDictionary<string, object> DynamicToDictionary(dynamic o)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();

            if (o == null) return dictionary;
            
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(o.GetType()))  dictionary.Add(property.Name, property.GetValue(o));
            return dictionary;
        }
    }
}
