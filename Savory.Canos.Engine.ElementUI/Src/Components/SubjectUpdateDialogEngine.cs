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
using PropTypeConstant = Savory.CodeDom.Vue.PropTypeConstant;

namespace Savory.Canos.Engine.ElementUI.Src.Components
{
    public class SubjectUpdateDialogEngine : BaseEngine
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
            vCardComponent.AddChild<VCardTitleComponent>().SetContent("编辑");
            vCardComponent.AddChild<VDividerComponent>();

            var subjectUpdateComponent = vCardComponent.AddChild<VCardTextComponent>().AddProperty("style", "max-height:500px;")
                .AddChild<Component>($"{this.Subject.Name.ToLowerCamelCase()}UpdateComponent");
            subjectUpdateComponent.AddProperty(":formExecuting.sync", "formExecuting");
            subjectUpdateComponent.AddProperty(":formExecuted.sync", "formExecuted");
            subjectUpdateComponent.AddProperty(":formValid.sync", "formValid");
            {
                List<string> items = new List<string>();
                if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                {
                    foreach (var item in this.Subject.ParentList)
                    {
                        items.Add(item.PrimaryPropertyFullName().ToLowerCamelCase());
                    }
                }
                items.Add(this.Subject.PrimaryProperty.Name.ToLowerCamelCase());
                foreach (var item in items)
                {
                    subjectUpdateComponent.AddProperty($":{item}", $"_{item}");
                }
                subjectUpdateComponent.VIf(string.Join(" && ", items));
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

            //imports
            codeScript.AddImport($"{this.Subject.Name}UpdateComponent", $"@/components/{this.Subject.Name}UpdateComponent");

            //components
            codeScript.AddComponent($"{this.Subject.Name}UpdateComponent".ToLowerCaseBreakLine(), $"{this.Subject.Name}UpdateComponent");

            //props
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddProp(parent.PrimaryPropertyFullName().ToLowerCamelCase(), parent.PrimaryProperty.ProgramType.ToPropType(), required: true);
                }
            }
            codeScript.AddProp(this.Subject.PrimaryProperty.Name.ToLowerCamelCase(), this.Subject.PrimaryProperty.ProgramType.ToPropType(), required: true);
            codeScript.AddProp("reload_items", PropTypeConstant.BOOLEAN);
            codeScript.AddProp("open", PropTypeConstant.BOOLEAN);

            //data
            codeScript.AddDataValue("opened", "null");
            codeScript.AddDataValue("formValid", "null");
            codeScript.AddDataValue("formExecuting", "null");
            codeScript.AddDataValue("formExecuted", "null");

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
