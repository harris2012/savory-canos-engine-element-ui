using Savory.Canos.Template;
using Savory.CodeDom.Js;
using Savory.CodeDom.Tag;
using Savory.CodeDom.Tag.Vue;
using Savory.CodeDom.Vue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Src.Components
{
    public class SubjectListPageEngine : BaseEngine
    {
        public Subject Subject { get; set; }

        protected override VueCodeFile PrepareCodeFile()
        {
            VueCodeFile codeFile = new VueCodeFile();

            codeFile.Component = PrepareComponent();
            codeFile.CodeScript = PrepareCodeScript();

            return codeFile;
        }

        private Component PrepareComponent()
        {
            var subjectDataTable = new Component().SetName($"{this.Subject.Name}DataTableComponent");

            if (this.Subject.ParentList != null & this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    subjectDataTable.AddProperty($":{parent.PrimaryPropertyFullName().ToLowerCamelCase()}", parent.PrimaryPropertyFullName().ToLowerCamelCase());
                }

                subjectDataTable.VIf(string.Join(" && ", this.Subject.ParentList.Select(v => v.PrimaryPropertyFullName().ToLowerCamelCase())));
            }
            return subjectDataTable;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //import
            codeScript.AddImport($"{this.Subject.Name}DataTableComponent", $"@/components/{this.Subject.Name}DataTableComponent");

            //components
            codeScript.AddComponent($"{this.Subject.Name}DataTableComponent".ToLowerCaseBreakLine(), $"{this.Subject.Name}DataTableComponent");

            //data
            if (this.Subject.ParentList != null & this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddDataValue(parent.PrimaryPropertyFullName().ToLowerCamelCase(), "null");
                }
            }

            //mounted
            if (this.Subject.ParentList != null & this.Subject.ParentList.Count > 0)
            {
                var mountedMethod = codeScript.AddMounted();
                foreach (var parent in this.Subject.ParentList)
                {
                    mountedMethod.StepStatement($"this.{parent.PrimaryPropertyFullName().ToLowerCamelCase()} = this.$route.params.{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }

            return codeScript;
        }
    }
}
