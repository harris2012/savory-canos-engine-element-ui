using Savory.Canos.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Src
{
    public class ServiceJsEngine
    {
        public string ProjectName { get; set; } = "CanosProject";

        public Dictionary<string, Subject> SubjectMap { get; set; } = new Dictionary<string, Subject>();

        public Dictionary<string, Metadata> MetadataMap { get; set; } = new Dictionary<string, Metadata>();

        public string ApiHost { get; set; } = string.Empty;

        public string TransformText()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("import axios from 'axios';");
            builder.AppendLine();

            builder.AppendLine("//if (process.env.NODE_ENV == 'development') {");
            builder.AppendLine($"    axios.defaults.baseURL = '{(string.IsNullOrEmpty(this.ApiHost) ? "/" : this.ApiHost)}';");
            builder.AppendLine("//}");
            builder.AppendLine();

            builder.AppendLine("const AppService = {");

            if (this.SubjectMap != null && this.SubjectMap.Count > 0)
            {
                foreach (var subject in this.SubjectMap.Values)
                {
                    builder.AppendLine();
                    builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_data: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/data', request),");
                    builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_items: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/items', request),");
                    builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_item: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/item', request),");
                    builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_basic: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/basic', request),");
                    builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_count: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/count', request),");
                    builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_update: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/update', request),");
                    builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_create: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/create', request),");
                    builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_empty: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/empty', request),");

                    if (subject.PropertyMap.Values.Any(v => v == BuiltInProperties.Position))
                    {
                        builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_position: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/position', request),");
                    }
                    if (subject.PropertyMap.Values.Any(v => v == BuiltInProperties.DataStatus))
                    {
                        builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_enable: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/enable', request),");
                        builder.AppendLine($"    {subject.Name.ToLowerCamelCase()}_disable: request => axios.post('api/{subject.Name.ToLowerCaseBreakLine()}/disable', request),");
                    }
                }
            }

            if (this.MetadataMap != null && this.MetadataMap.Count > 0)
            {
                foreach (var metadata in this.MetadataMap.Values)
                {
                    builder.AppendLine();
                    builder.AppendLine($"    {metadata.MetaName.ToLowerCamelCase()}_items: request => axios.post('api/{metadata.MetaName.ToLowerCaseBreakLine()}/items', request),");
                    builder.AppendLine($"    {metadata.MetaName.ToLowerCamelCase()}_selected_item: request => axios.post('api/{metadata.MetaName.ToLowerCaseBreakLine()}/selected-item', request),");
                    builder.AppendLine($"    {metadata.MetaName.ToLowerCamelCase()}_selected_items: request => axios.post('api/{metadata.MetaName.ToLowerCaseBreakLine()}/selected-items', request),");
                }
            }

            builder.AppendLine();
            builder.AppendLine("    user_profile: request => axios.post('api/user/profile', request)");
            builder.AppendLine("}");

            builder.AppendLine();
            builder.AppendLine("export default AppService");

            return builder.ToString();
        }
    }
}
