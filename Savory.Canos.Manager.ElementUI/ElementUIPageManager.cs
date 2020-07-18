using Savory.Canos.Engine.ElementUI.Src.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Manager.ElementUI
{
    public class ElementUIPageManager : ProjectManager<ElementUIPageParam>
    {
        public override void Generate(GenerateResult generateResult, ElementUIPageParam param)
        {
            var path = new ElementUIPath(param.Prefix);

            if (param.SubjectMap == null || param.SubjectMap.Count == 0)
            {
                return;
            }

            foreach (var item in param.SubjectConfigMap)
            {
                var subject = param.SubjectMap[item.Key];
                var subjectConfig = item.Value;

                generateResult.WriteToFile(Path.Combine(path.SrcPages, $"{subject.Name}ListPage.vue"), new SubjectListPageEngine { Subject = subject }.TransformText());

                if (subjectConfig.WithCreatePage)
                {
                    generateResult.WriteToFile(Path.Combine(path.SrcPages, $"{subject.Name}CreatePage.vue"), new SubjectCreatePageEngine { Subject = subject }.TransformText());
                }
                if (subjectConfig.WithUpdatePage)
                {
                    generateResult.WriteToFile(Path.Combine(path.SrcPages, $"{subject.Name}UpdatePage.vue"), new SubjectUpdatePageEngine { Subject = subject }.TransformText());
                }
                if (subjectConfig.WithItemPage)
                {
                    generateResult.WriteToFile(Path.Combine(path.SrcPages, $"{subject.Name}ItemPage.vue"), new SubjectItemPageEngine { Subject = subject }.TransformText());
                }
            }
        }
    }
}
