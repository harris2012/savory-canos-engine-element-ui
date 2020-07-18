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
    public class SubjectItemPageEngine : BaseEngine
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
            var subjectItem = new Component();
            subjectItem.Name = $"{this.Subject.Name}ItemComponent";

            List<string> items = new List<string>();
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    items.Add(parent.PrimaryPropertyFullName().ToLowerCamelCase());
                }
            }
            items.Add(this.Subject.PrimaryProperty.Name.ToLowerCamelCase());
            foreach (var item in items)
            {
                subjectItem.AddProperty($":{item}", $"{item}");
            }
            subjectItem.VIf(string.Join(" && ", items));

            return subjectItem;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //import
            codeScript.AddImport($"{this.Subject.Name}ItemComponent", $"@/components/{this.Subject.Name}ItemComponent");

            //components
            codeScript.AddComponent($"{this.Subject.Name}ItemComponent".ToLowerCaseBreakLine(), $"{this.Subject.Name}ItemComponent");

            //data
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddDataValue(parent.PrimaryPropertyFullName().ToLowerCamelCase(), "null");
                }
            }
            codeScript.AddDataValue(this.Subject.PrimaryProperty.Name.ToLowerCamelCase(), "null");

            //mounted
            var mountedMethod = codeScript.AddMounted();
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    mountedMethod.StepStatement($"this.{parent.PrimaryPropertyFullName().ToLowerCamelCase()} = this.$route.params.{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }
            switch (this.Subject.PrimaryProperty.ProgramType)
            {
                case ProgramTypeConstant.INT:
                case ProgramTypeConstant.BIGINT:
                    mountedMethod.StepStatement($"this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = Number(this.$route.params.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()});");
                    break;
                case ProgramTypeConstant.STRING:
                    mountedMethod.StepStatement($"this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = String(this.$route.params.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()});");
                    break;
                default:
                    mountedMethod.StepStatement($"this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = this.$route.params.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");
                    break;
            }

            return codeScript;
        }
    }
}
