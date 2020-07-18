using Savory.Canos.Template;
using Savory.CodeDom.Js;
using Savory.CodeDom.Tag;
using Savory.CodeDom.Tag.Html;
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
    public class SubjectSimpleTableComponentEngine : BaseEngine
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
            var vSimpleTableComponent = new VSimpleTableComponent();

            var templateComponent = vSimpleTableComponent.AddChild<TemplateBuilder>().VSlot("default");

            //thead
            {
                var thead = templateComponent.AddChild<TheadComponent>();
                var tr = thead.AddChild<TrComponent>();
                foreach (var property in this.Subject.PropertyMap.Values)
                {
                    tr.AddChild<ThComponent>().SetContent(property.DisplayName).AddCssClass("text-left");
                }
            }

            //tbody
            {
                var tbody = templateComponent.AddChild<TbodyComponent>();
                var tr = tbody.AddChild<TrComponent>().VFor("item", "items", "id");
                foreach (var property in this.Subject.PropertyMap.Values)
                {
                    tr.AddChild<TdComponent>().SetContent($"{{{{ item.{property.Name.ToLowerCamelCase()} }}}}");
                }
            }

            return vSimpleTableComponent;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //data
            codeScript.AddDataValue("items", "null");

            //mounted
            codeScript.AddMounted()
                .StepStatement("this.load_items();");

            //methods
            {
                var load_items = codeScript.AddMethod("load_items");
                load_items.StepStatement("var request = {};");
                load_items.StepStatement($"this.appService.{this.Subject.Name.ToLowerCamelCase()}_items(request).then(this.{this.Subject.Name.ToLowerCamelCase()}_items_callback);");
            }
            {
                var load_items_callback = codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_items_callback");
                load_items_callback.AddParameter("response");
                load_items_callback.StepStatement("this.items = response.data.items;");
            }

            return codeScript;
        }
    }
}
