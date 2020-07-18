using Savory.Canos.Template;
using Savory.CodeDom.Js;
using Savory.CodeDom.Tag;
using Savory.CodeDom.Tag.Vue;
using Savory.CodeDom.Tag.Vue.ElementUI;
using Savory.CodeDom.Vue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Src.Components
{
    public class SubjectUpdatePageEngine : BaseEngine
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
            var div = new Component();
            {
                var subjectForm = div.AddChild<VRowComponent>().AddChild<VColComponent>().AddCssClass("pt-0")
                    .AddChild<Component>($"{this.Subject.Name}UpdateComponent");

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
                    subjectForm.AddProperty($":{item}", $"{item}");
                }
                subjectForm.VIf(string.Join(" && ", items));

                subjectForm.AddProperty(":formExecuting.sync", "formExecuting");
                subjectForm.AddProperty(":formValid.sync", "formValid");
            }

            {
                var vBtn = div.AddChild<VRowComponent>().AddChild<VColComponent>().AddCssClass("pt-0").AddChild<VBtnComponent>()
                          .AddAttribute("outlined")
                          .Color("primary")
                          .SetContent("Submit");
                vBtn.AddProperty("@click", "start");
                vBtn.AddProperty(":disabled", "!formValid");
                vBtn.AddProperty(":loading", "formExecuting");
            }
            {
                div.AddChild<VSwitchComponent>().Model("formValid").Label("FormValid").Disabled();
                div.AddChild<VSwitchComponent>().Model("formExecuting").Label("FormExecuting").Disabled();
            }

            return div;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //import
            codeScript.AddImport($"{this.Subject.Name}UpdateComponent", $"@/components/{this.Subject.Name}UpdateComponent");

            //components
            codeScript.AddComponent($"{this.Subject.Name}UpdateComponent".ToLowerCaseBreakLine(), $"{this.Subject.Name}UpdateComponent");

            //data
            codeScript.AddDataValue("formValid", "null");
            codeScript.AddDataValue("formExecuting", "null");
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddDataValue(parent.PrimaryPropertyFullName().ToLowerCamelCase(), "null");
                }
            }
            codeScript.AddDataValue(this.Subject.PrimaryProperty.Name.ToLowerCamelCase(), "null");

            //mounted
            var mounted = codeScript.AddMounted();
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    mounted.StepStatement($"this.{parent.PrimaryPropertyFullName().ToLowerCamelCase()} = this.$route.params.{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }
            switch (this.Subject.PrimaryProperty.ProgramType)
            {
                case ProgramTypeConstant.INT:
                case ProgramTypeConstant.BIGINT:
                    mounted.StepStatement($"this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = Number(this.$route.params.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()});");
                    break;
                case ProgramTypeConstant.STRING:
                    mounted.StepStatement($"this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = String(this.$route.params.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()});");
                    break;
                default:
                    mounted.StepStatement($"this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = this.$route.params.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");
                    break;
            }

            //methods
            {
                codeScript.AddMethod("start")
                    .StepStatement("this.formExecuting = true;");
            }

            return codeScript;
        }
    }
}
