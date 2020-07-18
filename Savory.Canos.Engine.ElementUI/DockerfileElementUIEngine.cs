using System;
using System.Text;

namespace Savory.Canos.Engine.ElementUI
{
    //public class DockerfileElementUIEngine
    //{
    //    /// <summary>
    //    /// vue 编译基础镜像
    //    /// </summary>
    //    private const string VUETIFY_TOOL_CHAIN = "node:10.16.3";

    //    public string TransformText()
    //    {
    //        StringBuilder builder = new StringBuilder();

    //        builder.AppendLine($"FROM {VUETIFY_TOOL_CHAIN} AS VUETIFY_TOOL_CHAIN").AppendLine();
    //        builder.AppendLine("COPY ./ /tmp/").AppendLine();
    //        builder.AppendLine("WORKDIR /tmp").AppendLine();
    //        builder.AppendLine("RUN npm install && npm run build").AppendLine();
    //        builder.AppendLine();

    //        builder.AppendLine("FROM nginx:1.17.5").AppendLine();
    //        builder.AppendLine("MAINTAINER harriszhang@live.cn").AppendLine();
    //        builder.AppendLine("COPY --from=VUETIFY_TOOL_CHAIN /tmp/dist /usr/share/nginx/html").AppendLine();
    //        builder.AppendLine("COPY  default.conf /etc/nginx/conf.d").AppendLine();

    //        return builder.ToString();
    //    }
    //}
}
