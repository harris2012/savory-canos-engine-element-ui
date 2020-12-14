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
    public class SubjectUpdateComponentEngine : SubjectBaseComponentEngine
    {
        protected override VueCodeFile PrepareCodeFile()
        {
            VueCodeFile codeFile = new VueCodeFile();

            codeFile.Component = PrepareComponent();
            codeFile.CodeScript = PrepareCodeScript();

            return codeFile;
        }

        private Component PrepareComponent()
        {
            ElFormComponent formComponent = new ElFormComponent();

            formComponent.Model("formValid").Ref("form").AddCssClass("px-3").AddCssClass("white");

            BuildProperties(formComponent, this.Subject);

            return formComponent;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //import
            BuildImports(codeScript);

            //components
            BuildComponents(codeScript);

            //props
            codeScript.AddProp("formExecuting", "Boolean");
            codeScript.AddProp("formExecuted", "Boolean");
            codeScript.AddProp("formReseting", "Object");
            BuildProps(codeScript);
            codeScript.AddProp(this.Subject.PrimaryProperty.Name.ToLowerCamelCase(), this.Subject.PrimaryProperty.ProgramType.ToPropType(), required: true);

            //computed
            BuildComputed(codeScript, true);

            //mounted
            BuildMounted(codeScript);

            //data
            BuildData(codeScript, true);

            //watch
            {
                var formValidMethod = codeScript.AddWatch("formValid");
                {
                    formValidMethod.AddParameter("newValue");
                    formValidMethod.StepStatement("this.$emit('update:formValid', newValue);");
                }

                var formExecutingMethod = codeScript.AddWatch("formExecuting");
                {
                    formExecutingMethod.AddParameter("newValue");
                    formExecutingMethod.StepIf("!newValue")
                        .StepStatement("return;");
                    formExecutingMethod.StepStatement("this.execute();");
                }

                var formResetingMethod = codeScript.AddWatch("formReseting");
                {
                    formResetingMethod.StepStatement("this.item = {};");
                }

                BuildWatch(codeScript, this.Subject);

                var primaryProperty = codeScript.AddWatch($"_{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}");
                primaryProperty.StepStatement($"this.{this.Subject.Name.ToLowerCamelCase()}_empty();");
            }

            //methods
            BuildMethods(codeScript);

            return codeScript;
        }

        private void BuildMounted(CodeScript codeScript)
        {
            CodeMethod codeMethod = new CodeMethod();

            codeMethod.StepCallMethod($"{this.Subject.Name.ToLowerCamelCase()}_empty", target: "this");

            codeScript.AddMounted(codeMethod);
        }

        private void BuildMethods(CodeScript codeScript)
        {
            codeScript.AddMethod("execute", BuildMethod_Execute(this.Subject));

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_empty", BuildMethod_SubjectEmpty());

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_empty_callback", BuildMethod_SubjectEmptyCallback(true));

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_basic", BuildMethod_SubjectBasic());

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_basic_callback", BuildMethod_SubjectBasicCallback());

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_update", BuildMethod_SubjectUpdate());

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_update_callback", BuildMethod_SubjectUpdateCallback());
        }

        private CodeMethod BuildMethod_Execute(Subject subject)
        {
            var codeMethod = new CodeMethod();

            codeMethod.StepCallMethod($"{subject.Name.ToLowerCamelCase()}_update", target: "this");

            return codeMethod;
        }

        private CodeMethod BuildMethod_SubjectBasic()
        {
            CodeMethod codeMethod = new CodeMethod();

            List<string> items = new List<string>();

            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                items.AddRange(this.Subject.ParentList.Select(v => $"!this._{v.PrimaryPropertyFullName().ToLowerCamelCase()}"));
            }
            items.Add($"!this._{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}");
            var condition = string.Join(" || ", items);
            codeMethod.StepIf(condition).StepStatement("return;");

            codeMethod.StepStatement("var request = {};");
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeMethod.StepStatement($"request.{parent.PrimaryPropertyFullName().ToLowerCamelCase()} = this._{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }
            codeMethod.StepStatement($"request.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = this._{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");
            codeMethod.StepStatement($"this.appService.{this.Subject.Name.ToLowerCamelCase()}_basic(request).then(this.{this.Subject.Name.ToLowerCamelCase()}_basic_callback);");

            return codeMethod;
        }

        private CodeMethod BuildMethod_SubjectBasicCallback()
        {
            CodeMethod codeMethod = new CodeMethod();

            codeMethod.AddParameter("response");

            codeMethod.StepStatement("var responseData = response.data;");
            codeMethod.StepIf("responseData.status != 1").StepStatement("return;");
            codeMethod.StepStatement("this.item = responseData.item;");

            return codeMethod;
        }

        private CodeMethod BuildMethod_SubjectUpdate()
        {
            CodeMethod codeMethod = new CodeMethod();

            codeMethod.StepStatement("var request = {};");
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeMethod.StepStatement($"request.{parent.PrimaryPropertyFullName().ToLowerCamelCase()} = this._{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }
            codeMethod.StepStatement($"request.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = this._{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");
            codeMethod.StepStatement("request.item = this.item;");

            codeMethod.StepStatement($"this.appService.{this.Subject.Name.ToLowerCamelCase()}_update(request).then(this.{this.Subject.Name.ToLowerCamelCase()}_update_callback);");

            return codeMethod;
        }

        private CodeMethod BuildMethod_SubjectUpdateCallback()
        {
            var codeMethod = new CodeMethod();

            codeMethod.AddParameter("response");

            codeMethod.StepStatement("this.$emit('update:formExecuting', false);");
            codeMethod.StepStatement("var responseData = response.data;");
            codeMethod.StepIf("responseData.status != 1").StepStatement("return;");
            codeMethod.StepStatement("this.$emit('update:formExecuted', true);");

            return codeMethod;
        }
    }
}
