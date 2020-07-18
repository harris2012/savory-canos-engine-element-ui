using Savory.Canos.Template;
using Savory.Canos.Template.Vue;
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
using PropTypeConstant = Savory.CodeDom.Vue.PropTypeConstant;

namespace Savory.Canos.Engine.ElementUI.Src.Components
{
    public class SubjectCreateDialogEngine : BaseEngine
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
            var vDialogComponent = new VDialogComponent().MaxWidth(1000).Scrollable().Model("opened").Persistent("formExecuting");

            var vCardComponent = vDialogComponent.AddChild<VCardComponent>();
            vCardComponent.AddChild<VCardTitleComponent>().SetContent("新增");
            vCardComponent.AddChild<VDividerComponent>();

            var subjectCreateComponent = vCardComponent.AddChild<VCardTextComponent>().AddProperty("style", "max-height:500px;")
                .AddChild<Component>($"{this.Subject.Name.ToLowerCamelCase()}CreateComponent");
            subjectCreateComponent.AddProperty(":formExecuting.sync", "formExecuting");
            subjectCreateComponent.AddProperty(":formExecuted.sync", "formExecuted");
            subjectCreateComponent.AddProperty(":formValid.sync", "formValid");
            subjectCreateComponent.AddProperty(":formReseting", "formReseting");

            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    subjectCreateComponent.AddProperty($":{parent.PrimaryPropertyFullName().ToLowerCamelCase()}", $"_{parent.PrimaryPropertyFullName().ToLowerCamelCase()}");
                }
                subjectCreateComponent.VIf(string.Join(" && ", this.Subject.ParentList.Select(v => v.PrimaryPropertyFullName().ToLowerCamelCase())));
            }

            vCardComponent.AddChild<VDividerComponent>();

            var vCardActionsComponent = vCardComponent.AddChild<VCardActionsComponent>();
            vCardActionsComponent.AddChild<VSpacerComponent>();
            {
                var vBtnComponent = vCardActionsComponent.AddChild<VBtnComponent>().AddAttribute("outlined").Color("primary").SetContent("保存");
                vBtnComponent.AddProperty("@click", "start()");
                vBtnComponent.AddProperty(":disabled", "!formValid");
                vBtnComponent.AddProperty(":loading", "formExecuting");
            }

            return vDialogComponent;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //import
            codeScript.AddImport($"{this.Subject.Name}CreateComponent", $"@/components/{this.Subject.Name}CreateComponent");

            //components
            codeScript.AddComponent($"{this.Subject.Name}CreateComponent".ToLowerCaseBreakLine(), $"{this.Subject.Name}CreateComponent");

            //props
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddProp(parent.PrimaryPropertyFullName().ToLowerCamelCase(), parent.PrimaryProperty.ProgramType.ToPropType(), required: true);
                }
            }
            codeScript.AddProp("reload_items", PropTypeConstant.BOOLEAN);
            codeScript.AddProp("open", PropTypeConstant.BOOLEAN);

            //data
            codeScript.AddDataValue("opened", "null");
            codeScript.AddDataValue("formValid", "null");
            codeScript.AddDataValue("formExecuting", "null");
            codeScript.AddDataValue("formExecuted", "null");
            codeScript.AddDataValue("formReseting", "null");

            //computed
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddComputed($"_{parent.PrimaryPropertyFullName().ToLowerCamelCase()}")
                        .StepStatement($"return this.{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }
            codeScript.AddComputed($"_{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}")
                    .StepStatement($"return this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");

            //watch
            {
                var watchOpen = codeScript.AddWatch("open");
                watchOpen.AddParameter("newValue");
                watchOpen.StepStatement("this.opened = newValue;");
            }
            {
                var watchOpened = codeScript.AddWatch("opened");
                watchOpened.AddParameter("newValue");
                watchOpened.StepStatement("this.$emit('update:open', newValue);");
                watchOpened.StepIf("!newValue")
                    .StepStatement("this.formReseting = {};");
            }
            {
                var watchFormExecuted = codeScript.AddWatch("formExecuted");
                watchFormExecuted.AddParameter("newValue");
                watchFormExecuted.StepStatement("//如果表单已经执行成功，则关闭当前窗口");
                watchFormExecuted.StepIf("newValue")
                    .StepStatement("this.$emit('update:reload_items', true);")
                    .StepStatement("this.$emit('update:open', false);");
            }

            //mounted
            codeScript.AddMounted()
                .StepStatement("this.opened = this.open;");

            //methods
            codeScript.AddMethod("start")
                .StepStatement("this.formExecuting = true;");

            return codeScript;
        }
    }
}
