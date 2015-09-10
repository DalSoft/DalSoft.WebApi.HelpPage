using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Owin;

namespace DalSoft.WebApi.HelpPage.Example.SelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            var jsonSerializerSettings = config.Formatters.JsonFormatter.SerializerSettings;

            //Remove unix epoch date handling, in favor of ISO
            jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff" });

            //Remove nulls from payload and save bytes
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            // Remove xml this will make json the default and your life easier (unless you really need to support xml)
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "text/xml"));
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

            // Attribute routing
            config.MapHttpAttributeRoutes();

            // WebApi
            app.UseWebApi(config);

            // WebApi HelpPage
            app.UseWebApiHelpPage(config, "myhelp");
        }
    }

}
