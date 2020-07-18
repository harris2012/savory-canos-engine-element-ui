using Savory.Canos.Engine.ElementUI.Src.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Manager.ElementUI
{
    public class ElementUIDialogManager : ProjectManager<ElementUIDialogParam>
    {
        public override void Generate(GenerateResult generateResult, ElementUIDialogParam param)
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

                if (subjectConfig.WithCreateModal)
                {
                    generateResult.WriteToFile(Path.Combine(path.SrcDialogs, $"{subject.Name}CreateDialog.vue"), new SubjectCreateDialogEngine
                    {
                        Subject = subject
                    }.TransformText());
                }
                if (subjectConfig.WithUpdateModal)
                {
                    generateResult.WriteToFile(Path.Combine(path.SrcDialogs, $"{subject.Name}UpdateDialog.vue"), new SubjectUpdateDialogEngine
                    {
                        Subject = subject
                    }.TransformText());
                }
                if (subjectConfig.WithItemModal)
                {
                    generateResult.WriteToFile(Path.Combine(path.SrcDialogs, $"{subject.Name}ItemDialog.vue"), new SubjectItemDialogEngine
                    {
                        Subject = subject
                    }.TransformText());
                }
            }
        }
    }
}
