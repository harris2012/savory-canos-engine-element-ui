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
    public class SubjectCreateComponentEngine : SubjectBaseComponentEngine
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
            codeScript.AddProp("formExecuting", new VueProp { Type = "Boolean" });
            codeScript.AddProp("formExecuted", new VueProp { Type = "Boolean" });
            codeScript.AddProp("formReseting", new VueProp { Type = "Object" });
            BuildProps(codeScript);

            //computed
            BuildComputed(codeScript, false);

            //mounted
            BuildMounted(codeScript);

            //data
            BuildData(codeScript, false);

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

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_empty_callback", BuildMethod_SubjectEmptyCallback(false));

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_create", BuildMethod_SubjectCreate(this.Subject));

            codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_create_callback", BuildMethod_SubjectCreateCallback());
        }

        private static CodeMethod BuildMethod_Execute(Subject subject)
        {
            var codeMethod = new CodeMethod();

            codeMethod.StepCallMethod($"{subject.Name.ToLowerCamelCase()}_create", target: "this");

            return codeMethod;
        }

        private static CodeMethod BuildMethod_SubjectCreate(Subject subject)
        {
            CodeMethod codeMethod = new CodeMethod();

            codeMethod.StepStatement("var request = {};");
            if (subject.ParentList != null && subject.ParentList.Count > 0)
            {
                foreach (var parent in subject.ParentList)
                {
                    codeMethod.StepStatement($"request.{parent.PrimaryPropertyFullName().ToLowerCamelCase()} = this._{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }
            codeMethod.StepStatement("request.item = this.item;");

            codeMethod.StepStatement($"this.appService.{subject.Name.ToLowerCamelCase()}_create(request).then(this.{subject.Name.ToLowerCamelCase()}_create_callback);");

            return codeMethod;
        }

        private static CodeMethod BuildMethod_SubjectCreateCallback()
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
