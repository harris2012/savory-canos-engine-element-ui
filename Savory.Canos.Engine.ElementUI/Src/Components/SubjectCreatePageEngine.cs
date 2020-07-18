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
    public class SubjectCreatePageEngine : BaseEngine
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
            Component component = new Component();

            var div = new Component();
            {
                var subjectForm = div.AddChild<VRowComponent>().AddChild<VColComponent>().AddCssClass("pt-0").AddChild<Component>($"{this.Subject.Name}CreateComponent");
                if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                {
                    foreach (var parent in this.Subject.ParentList)
                    {
                        subjectForm.AddProperty($":{parent.PrimaryPropertyFullName().ToLowerCamelCase()}", $"{parent.PrimaryPropertyFullName().ToLowerCamelCase()}");
                    }
                    subjectForm.VIf(string.Join(" && ", this.Subject.ParentList.Select(v => v.PrimaryPropertyFullName().ToLowerCamelCase())));
                }
                subjectForm.AddProperty(":formExecuting.sync", "formExecuting");
                subjectForm.AddProperty(":formExecuted.sync", "formExecuted");
                subjectForm.AddProperty(":formValid.sync", "formValid");
            }
            {
                var vBtn = div.AddChild<VRowComponent>()
                    .AddChild<VColComponent>().AddCssClass("pt-0")
                    .AddChild<VBtnComponent>()
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
            codeScript.AddImport($"{this.Subject.Name}CreateComponent", $"@/components/{this.Subject.Name}CreateComponent");

            //component
            codeScript.AddComponent($"{this.Subject.Name}CreateComponent".ToLowerCaseBreakLine(), $"{this.Subject.Name}CreateComponent");

            //data
            codeScript.AddDataValue("formValid", "null");
            codeScript.AddDataValue("formExecuting", "null");
            codeScript.AddDataValue("formExecuted", "null");
            if (this.Subject.ParentList != null & this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddDataValue(parent.PrimaryPropertyFullName().ToLowerCamelCase(), "null");
                }
            }

            //watch
            {
                var watchFormExecuted = codeScript.AddWatch("formExecuted");
                watchFormExecuted.AddParameter("newValue");
                watchFormExecuted.StepIf("newValue")
                    .StepStatement($"this.$router.push({{ name: '{this.Subject.Name.ToLowerCaseBreakLine()}-list-page' }})");
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

            //methods
            {
                codeScript.AddMethod("start")
                    .StepStatement("this.formExecuting = true;");
            }

            return codeScript;
        }
    }
}
