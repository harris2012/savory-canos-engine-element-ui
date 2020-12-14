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
    public class SubjectItemDialogEngine : BaseEngine
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
            var vDialogComponent = new VDialogComponent().MaxWidth(1000).Scrollable().Model("opened");

            var vCardComponent = vDialogComponent.AddChild<VCardComponent>();
            vCardComponent.AddChild<VCardTitleComponent>().SetContent("查看");
            vCardComponent.AddChild<VDividerComponent>();

            var subjectItemComponent = vCardComponent.AddChild<VCardTextComponent>().AddProperty("style", "max-height:500px;")
                .AddChild<Component>($"{this.Subject.Name.ToLowerCamelCase()}ItemComponent");

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
                subjectItemComponent.AddProperty($":{item}", $"_{item}");
            }
            subjectItemComponent.VIf(string.Join(" && ", items));

            return vDialogComponent;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //import
            codeScript.AddImport($"{this.Subject.Name}ItemComponent", $"@/components/{this.Subject.Name}ItemComponent");

            //components
            codeScript.AddComponent($"{this.Subject.Name}ItemComponent".ToLowerCaseBreakLine(), $"{this.Subject.Name}ItemComponent");

            //props
            if (this.Subject.ParentList != null & this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddProp(parent.PrimaryPropertyFullName().ToLowerCamelCase(), parent.PrimaryProperty.ProgramType.ToPropType(), required: true);
                }
            }
            codeScript.AddProp(this.Subject.PrimaryProperty.Name.ToLowerCamelCase(), this.Subject.PrimaryProperty.ProgramType.ToPropType(), required: true);
            codeScript.AddProp("open", PropTypeConstant.BOOLEAN);

            //data
            codeScript.AddDataValue("opened", "null");

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

            //mounted
            codeScript.AddMounted()
                .StepStatement("this.opened = this.open;");

            return codeScript;
        }
    }
}
