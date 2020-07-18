using Savory.Canos.Engine.ElementUI.Src.Router;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Manager.ElementUI
{
    public class ElementUIRouterManager : ProjectManager<ElementUIRouterParam>
    {
        public override void Generate(GenerateResult generateResult, ElementUIRouterParam param)
        {
            var path = new ElementUIPath(param.Prefix);

            generateResult.WriteToFile(Path.Combine(path.SrcRouter, $"index.js"), new IndexJsEngine
            {
                SubjectMap = param.SubjectMap
            }.TransformText());
        }
    }
}
