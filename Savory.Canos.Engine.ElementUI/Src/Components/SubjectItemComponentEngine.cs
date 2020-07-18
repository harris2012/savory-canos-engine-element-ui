using Savory.Canos.Template;
using Savory.Canos.Template.Vue;
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
using PropTypeConstant = Savory.CodeDom.Vue.PropTypeConstant;

namespace Savory.Canos.Engine.ElementUI.Src.Components
{
    public class SubjectItemComponentEngine : SubjectBaseComponentEngine
    {
        public Dictionary<string, Metadata> MetadataMap { get; set; }

        protected override VueCodeFile PrepareCodeFile()
        {
            VueCodeFile codeFile = new VueCodeFile();

            codeFile.Component = PrepareComponent();
            codeFile.CodeScript = PrepareCodeScript();

            return codeFile;
        }

        private Component PrepareComponent()
        {
            var divBuilder = new Component().AddCssClass("pa-3").AddCssClass("white").VIf("item != null");

            BuildProperties(divBuilder, this.Subject, this.MetadataMap);

            return divBuilder;
        }

        private static void BuildProperties(Component divBuilder, Subject subject, Dictionary<string, Metadata> metadataMap)
        {
            foreach (var property in subject.PropertyMap.Values)
            {
                switch (property.PropertyFrom)
                {
                    case PropertyFromConstant.Basic:
                    case PropertyFromConstant.BuiltIn:
                        {
                            BuildText(divBuilder, property);
                        }
                        break;
                    case PropertyFromConstant.Metadata:
                        {
                            BuildMetadata(divBuilder, property, metadataMap);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void BuildText(Component formBuilder, Property property)
        {
            var vColComponent = formBuilder.AddChild<VRowComponent>().NoGutters().AddChild<VColComponent>();

            vColComponent.AddChild<Component>("h4").SetContent(property.DisplayName);
            vColComponent.AddChild<PComponent>().SetContent($"{{{{item.{property.Name.ToLowerCamelCase()}}}}}");
        }

        private static void BuildMetadata(Component formBuilder, Property property, Dictionary<string, Metadata> metadataMap)
        {
            var vColComponent = formBuilder.AddChild<VRowComponent>().NoGutters().AddChild<VColComponent>();

            var metadata = metadataMap[property.MetadataName];

            vColComponent.AddChild<Component>("h4").SetContent(property.DisplayName);
            if (property.IsMultiMeta)
            {
                var divComponent = vColComponent.AddChild<Component>().VIf($"item.{property.Name.ToLowerCamelCase()} != null");

                var vChipComponent = divComponent.AddChild<PComponent>().AddChild<VChipComponent>().Label().AddCssClass("mr-3")
                    .VForIndex("v", $"item.{property.Name.ToLowerCamelCase()}", "index")
                    .SetContent($"{{{{v.{metadata.TextName.ToLowerCamelCase()}}}}}");

                var pComponent = vColComponent.AddChild<PComponent>().SetContent("无").VIf($"item.{property.Name.ToLowerCamelCase()} == null");
            }
            else
            {
                var vChipComponent = vColComponent.AddChild<PComponent>().AddChild<VChipComponent>()
                    .Label()
                    .AddCssClass("mr-3")
                    .SetContent($"{{{{item.{property.Name.ToLowerCamelCase()}.{metadata.TextName.ToLowerCamelCase()}}}}}")
                    .VIf($"item.{property.Name.ToLowerCamelCase()} != null");

                var pComponent = vColComponent.AddChild<PComponent>().SetContent("无").VIf($"item.{property.Name.ToLowerCamelCase()} == null");
            }
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //props
            if (this.Subject.ParentList != null & this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddProp(parent.PrimaryPropertyFullName().ToLowerCamelCase(), PropTypeConstant.STRING, required: true);
                }
            }
            codeScript.AddProp(this.Subject.PrimaryProperty.Name.ToLowerCamelCase(), this.Subject.PrimaryProperty.ProgramType.ToPropType(), required: true);

            //data
            {
                var item = codeScript.AddDataObject("item");
                foreach (var property in this.Subject.PropertyMap.Values)
                {
                    string dataValueDefaultValue = "null";
                    switch (property.InputMethod)
                    {
                        case InputMethodConstant.SELECT:
                            if (!string.IsNullOrEmpty(property.SelectMultiple.Value) && !"false".Equals(property.SelectMultiple.Value, StringComparison.OrdinalIgnoreCase))
                            {
                                dataValueDefaultValue = "[]";
                            }
                            break;
                        default:
                            break;
                    }

                    DataValue dataValue = dataValueDefaultValue;
                    item.AddDataValue($"{property.Name.ToLowerCamelCase()}", dataValue);
                }
            }

            //computed
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddComputed($"{parent.PrimaryPropertyFullName().ToLowerCamelCase()}")
                        .StepStatement($"return this.{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }
            codeScript.AddComputed($"_{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}")
                .StepStatement($"return this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");

            //watch
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddWatch($"_{parent.PrimaryPropertyFullName().ToLowerCamelCase()}")
                        .StepStatement($"this.{this.Subject.Name.ToLowerCamelCase()}_item();");
                }
            }
            codeScript.AddWatch($"_{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}")
                .StepStatement($"this.{this.Subject.Name.ToLowerCamelCase()}_item();");

            //mounted
            codeScript.AddMounted()
                .StepStatement($"this.{this.Subject.Name.ToLowerCamelCase()}_item();");

            //methods
            {
                var subject_item = codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_item");
                {
                    List<string> items = new List<string>();
                    if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                    {
                        foreach (var parent in this.Subject.ParentList)
                        {
                            items.Add(parent.PrimaryPropertyFullName().ToLowerCamelCase());
                        }
                    }
                    items.Add(this.Subject.PrimaryProperty.Name.ToLowerCamelCase());
                    var condition = string.Join(" || ", items.Select(v => $"!this._{v}"));
                    subject_item.StepIf(condition)
                        .StepStatement("return;");

                    subject_item.StepStatement("var request = {};");
                    if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                    {
                        foreach (var parent in this.Subject.ParentList)
                        {
                            subject_item.StepStatement($"request.{parent.PrimaryPropertyFullName().ToLowerCamelCase()} = this._{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                        }
                    }
                    subject_item.StepStatement($"request.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = this._{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");
                    subject_item.StepStatement($"this.appService.{this.Subject.Name.ToLowerCamelCase()}_item(request).then(this.{this.Subject.Name.ToLowerCamelCase()}_item_callback);");
                }
            }
            {
                var subject_item_callback = codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_item_callback");
                subject_item_callback.AddParameter("response");

                subject_item_callback.StepStatement("var responseData = response.data;");

                subject_item_callback.StepIf("responseData.status != 1")
                    .StepStatement("return;");

                subject_item_callback.StepStatement("this.item = responseData.item;");
            }

            return codeScript;
        }
    }
}
