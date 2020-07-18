using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Public
{
    public class IndexHtmlEngine
    {
        public string Title { get; set; }

        public string TransformText()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<!DOCTYPE html>");
            builder.AppendLine("<html lang=\"en\">");
            builder.AppendLine("  <head>");
            builder.AppendLine("    <meta charset=\"utf-8\">");
            builder.AppendLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            builder.AppendLine("    <meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0\">");
            builder.AppendLine("    <link rel=\"icon\" href=\"<%= BASE_URL %>favicon.ico\">");
            builder.AppendLine($"    <title>{this.Title}</title>");
            builder.AppendLine("    <link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700,900\">");
            builder.AppendLine("    <link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/@mdi/font@latest/css/materialdesignicons.min.css\">");
            builder.AppendLine("  </head>");
            builder.AppendLine("  <body>");
            builder.AppendLine("    <noscript>");
            builder.AppendLine("      <strong>We're sorry but my-project doesn't work properly without JavaScript enabled. Please enable it to continue.</strong>");
            builder.AppendLine("    </noscript>");
            builder.AppendLine("    <div id=\"app\"></div>");
            builder.AppendLine("    <!-- built files will be auto injected -->");
            builder.AppendLine("  </body>");
            builder.AppendLine("</html>");

            return builder.ToString();
        }
    }
}
