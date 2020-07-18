using Savory.Canos.Engine.ElementUI.Src.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Manager.ElementUI
{
    public class ElementUIComponentManager : ProjectManager<ElementUIComponentParam>
    {
        public override void Generate(GenerateResult generateResult, ElementUIComponentParam param)
        {
            var path = new ElementUIPath(param.Prefix);

            if (param.SubjectMap == null || param.SubjectMap.Count == 0)
            {
                return;
            }

            foreach (var subject in param.SubjectMap.Values)
            {
                //components
                generateResult.WriteToFile(Path.Combine(path.SrcComponents, $"{subject.Name}SimpleTableComponent.vue"), new SubjectSimpleTableComponentEngine
                {
                    Subject = subject
                }.TransformText());

                generateResult.WriteToFile(Path.Combine(path.SrcComponents, $"{subject.Name}CreateComponent.vue"), new SubjectCreateComponentEngine
                {
                    Subject = subject
                }.TransformText());

                generateResult.WriteToFile(Path.Combine(path.SrcComponents, $"{subject.Name}UpdateComponent.vue"), new SubjectUpdateComponentEngine
                {
                    Subject = subject
                }.TransformText());

                generateResult.WriteToFile(Path.Combine(path.SrcComponents, $"{subject.Name}ItemComponent.vue"), new SubjectItemComponentEngine
                {
                    Subject = subject,
                    MetadataMap = param.MetadataMap
                }.TransformText());

                //datatable
                {
                    var subjectConfig = param.SubjectConfigMap[subject.Name];

                    generateResult.WriteToFile(Path.Combine(path.SrcComponents, $"{subject.Name}DataTableComponent.vue"), new SubjectDataTableComponentEngine
                    {
                        Subject = subject,
                        SubjectConfig = subjectConfig,
                        SubjectMap = param.SubjectMap,
                        //UseModal = param.UseModal
                    }.TransformText());
                }

            }
        }
    }
}
