using Savory.Canos.Template;
using Savory.CodeDom;
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

    public abstract class SubjectBaseComponentEngine : BaseEngine
    {
        public Subject Subject { get; set; }

        protected void BuildImports(CodeScript codeScript)
        {
            foreach (var property in this.Subject.PropertyMap.Values)
            {
                if (property.GeneralComponent == null)
                {
                    continue;
                }

                var component = property.GeneralComponent;

                var path = component.PackageName + component.ComponentPath;

                codeScript.AddImport(component.Name, path);
            }
        }

        protected void BuildComponents(CodeScript codeScript)
        {
            foreach (var property in this.Subject.PropertyMap.Values)
            {
                if (property.GeneralComponent == null)
                {
                    continue;
                }

                var component = property.GeneralComponent;

                codeScript.AddComponent(component.Name.ToLowerCamelCase(), component.Name);
            }
        }

        protected static void BuildProperties(ElFormComponent formBuilder, Subject subject)
        {
            foreach (var property in subject.PropertyMap.Values)
            {
                if (property.PropertyFrom == PropertyFromConstant.BuiltIn)
                {
                    continue;
                }
                if (property.GeneralComponent != null)
                {
                    BuildGeneralComponent(formBuilder, property, property.GeneralComponent);
                    continue;
                }
                switch (property.InputMethod)
                {
                    case InputMethodConstant.CHECKBOX:
                        {
                            BuildCheckbox(formBuilder, property);
                        }
                        break;
                    case InputMethodConstant.SELECT:
                        {
                            BuildSelect(formBuilder, property);
                        }
                        break;
                    case InputMethodConstant.TEXT:
                        {
                            BuildText(formBuilder, property);
                        }
                        break;
                    case InputMethodConstant.NUMBER:
                    default:
                        {
                            BuildNumber(formBuilder, property);
                        }
                        break;
                }
            }
        }

        protected static void BuildGeneralComponent(ElFormComponent formBuilder, Property property, GeneralComponent generalComponent)
        {
            formBuilder.AddChild<Component>(generalComponent.Name)
                .AddProperty(generalComponent.SyncProperty, $"item.{property.Name.ToLowerCamelCase()}");
        }

        protected static void BuildCheckbox(ElFormComponent formBuilder, Property property)
        {
            var builder = formBuilder.AddChild<ElCheckboxComponent>()
                .VModel($"item.{property.Name.ToLowerCamelCase()}")
                .Label(property.DisplayName);

            //if (property.WithRules)
            //{
            //    builder._Rules($"rule.{property.Name.ToLowerCamelCase()}");
            //}

            if (property.Required)
            {
                builder.Required();
            }
        }

        protected static void BuildSelect(ElFormComponent formBuilder, Property property)
        {
            var builder = formBuilder.AddChild<ElSelectComponent>()
                  .VModel($"item.{property.Name.ToLowerCamelCase()}")
                  ._Options($"option.{property.Name.ToLowerCamelCase()}")
                  .Label(property.DisplayName)
                  .Required();

            if (!string.IsNullOrEmpty(property.SelectMultiple.Value) && !"false".Equals(property.SelectMultiple.Value))
            {
                builder._Multiple(property.SelectMultiple.Value);
            }

            if (!string.IsNullOrEmpty(property.SelectItemValue))
            {
                builder.AddProperty("item-value", property.SelectItemValue);
            }

            if (!string.IsNullOrEmpty(property.SelectItemValue))
            {
                builder.AddProperty("item-text", property.SelectItemText);
            }

            //if (property.WithRules)
            //{
            //    builder.Rules($"rule.{property.Name.ToLowerCamelCase()}");
            //}

            if (property.Required)
            {
                builder.Required();
            }
        }

        protected static void BuildNumber(ElFormComponent formBuilder, Property property)
        {
            var builder = formBuilder.AddChild<VTextFieldComponent>()
                .Model($"item.{property.Name.ToLowerCamelCase()}", VModelType.Number)
                .Label(property.DisplayName);

            //if (property.WithRules)
            //{
            //    builder._Rules($"rule.{property.Name.ToLowerCamelCase()}");
            //}

            if (property.Required)
            {
                builder.Required();
            }
        }

        protected static void BuildText(ElFormComponent formBuilder, Property property)
        {
            var builder = formBuilder.AddChild<VTextFieldComponent>()
             .Model($"item.{property.Name.ToLowerCamelCase()}")
             .Label(property.DisplayName);

            if (property.TextMaxLength != null && !string.IsNullOrEmpty(property.TextMaxLength.Value))
            {
                builder._Counter(property.TextMaxLength.Value);
            }

            //if (property.WithRules)
            //{
            //    builder._Rules($"rule.{property.Name.ToLowerCamelCase()}");
            //}

            if (property.Required)
            {
                builder.Required();
            }
        }

        protected void BuildProps(CodeScript codeScript)
        {
            if (this.Subject.ParentList == null || this.Subject.ParentList.Count == 0)
            {
                return;
            }

            foreach (var parent in this.Subject.ParentList)
            {
                codeScript.AddProp(parent.PrimaryPropertyFullName().ToLowerCamelCase(), new VueProp { Type = "String", Required = true });
            }
        }

        protected void BuildComputed(CodeScript codeScript, bool withPrimaryProperty)
        {
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    CodeMethod codeMethod = new CodeMethod();
                    codeMethod.StepStatement($"return this.{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");

                    codeScript.AddComputed($"_{parent.PrimaryPropertyFullName().ToLowerCamelCase()}", codeMethod);
                }
            }

            if (withPrimaryProperty)
            {
                CodeMethod codeMethod = new CodeMethod();
                codeMethod.StepStatement($"return this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");
                codeScript.AddComputed($"_{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}", codeMethod);
            }
        }

        protected static void BuildWatch(CodeScript codeScript, Subject subject)
        {
            if (subject.ParentList == null || subject.ParentList.Count == 0)
            {
                return;
            }

            foreach (var parent in subject.ParentList)
            {
                codeScript.AddWatch($"_{parent.PrimaryPropertyFullName().ToLowerCamelCase()}")
                    .StepStatement($"this.{subject.Name.ToLowerCamelCase()}_empty();");
            }
        }

        public void BuildData(CodeScript codeScript, bool withId)
        {
            codeScript.AddDataValue("formValid", "null");

            DataObject item = new DataObject();
            codeScript.AddDataObject("item", item);
            foreach (var property in this.Subject.PropertyMap.Values)
            {
                if (!withId && property == BuiltInProperties.Id)
                {
                    continue;
                }
                string dataValueDefaultValue = "null";
                switch (property.InputMethod)
                {
                    case InputMethodConstant.SELECT:
                        if (!string.IsNullOrEmpty(property.SelectMultiple.Value) && !"false".Equals(property.SelectMultiple.Value))
                        {
                            dataValueDefaultValue = "[]";
                        }
                        break;
                    default:
                        break;
                }

                VueDataValue dataValue = new VueDataValue();
                dataValue.SetValue(dataValueDefaultValue);
                item.AddDataValue($"{property.Name.ToLowerCamelCase()}", dataValue);

                if (property.Required)
                {
                    dataValue.WithRule("v => !!v", $"'{property.DisplayName} required.'");
                }

                switch (property.InputMethod)
                {
                    case InputMethodConstant.TEXT:
                        {
                            if (property.TextMaxLength != null && !string.IsNullOrEmpty(property.TextMaxLength.Value))
                            {
                                dataValue.WithRule($"v => v != null && v.length <= {property.TextMaxLength.Value}", $"'{property.DisplayName} length > ' + " + property.TextMaxLength.Value);
                            }
                        }
                        break;
                    case InputMethodConstant.SELECT:
                        {
                            dataValue.WithOptions(new DataValueOption());
                        }
                        break;
                    case InputMethodConstant.NUMBER:
                    default:
                        break;
                }
            }

            var option = ToOption(item);
            if (option != null)
            {
                codeScript.AddDataObject("option", option);
            }

            var rule = ToRule(item);
            if (rule != null)
            {
                codeScript.AddDataObject("rule", rule);
            }
        }

        protected static DataObject ToOption(DataObject itemObject)
        {
            DataObject optionObject = new DataObject();

            var hasValue = false;
            foreach (var item in itemObject.DataValueMap)
            {
                var itemValue = item.Value;
                if (!(itemValue is VueDataValue))
                {
                    continue;
                }
                var vueDataValue = itemValue as VueDataValue;
                if (vueDataValue.Options == null)
                {
                    continue;
                }

                optionObject.AddDataValue(item.Key, "[]");
                hasValue = true;
            }

            return hasValue ? optionObject : null;
        }

        protected static DataObject ToRule(DataObject itemObject)
        {
            DataObject ruleObject = new DataObject();

            var hasValue = false;
            foreach (var item in itemObject.DataValueMap)
            {
                var itemValue = item.Value;
                if (!(itemValue is VueDataValue))
                {
                    continue;
                }
                var vueDataValue = itemValue as VueDataValue;
                if (vueDataValue.RuleList == null || vueDataValue.RuleList.Count == 0)
                {
                    continue;
                }

                //ruleObject.AddDataArray(item.Key, itemValue.RuleList.Select(v => (DataValue)v.Expression).ToList());
                hasValue = true;
            }

            return hasValue ? ruleObject : null;
        }

        protected CodeMethod BuildMethod_SubjectEmpty()
        {
            CodeMethod codeMethod = new CodeMethod();

            codeMethod.StepStatement("var request = {};");

            codeMethod.StepStatement($"this.appService.{this.Subject.Name.ToLowerCamelCase()}_empty(request).then(this.{this.Subject.Name.ToLowerCamelCase()}_empty_callback);");

            return codeMethod;
        }

        protected CodeMethod BuildMethod_SubjectEmptyCallback(bool withBasic)
        {
            CodeMethod codeMethod = new CodeMethod();

            codeMethod.AddParameter("response");

            codeMethod.StepStatement("var responseData = response.data;");
            codeMethod.StepIf("responseData.status != 1")
                .StepStatement("return;");

            foreach (var property in this.Subject.PropertyMap.Values)
            {
                switch (property.InputMethod)
                {
                    case InputMethodConstant.SELECT:
                        {
                            codeMethod.StepStatement($"this.option.{property.Name.ToLowerCamelCase()} = responseData.{property.Name.ToLowerCamelCase()};");
                        }
                        break;
                    default:
                        break;
                }
            }

            if (withBasic)
            {
                codeMethod.StepStatement($"this.{this.Subject.Name.ToLowerCamelCase()}_basic();");
            }

            return codeMethod;
        }
    }
}
