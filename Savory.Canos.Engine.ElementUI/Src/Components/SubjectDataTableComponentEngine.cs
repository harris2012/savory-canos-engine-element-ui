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
    public class SubjectDataTableComponentEngine : BaseEngine
    {
        public Subject Subject { get; set; }
        public SubjectConfig SubjectConfig { get; set; }
        public Dictionary<string, Subject> SubjectMap { get; set; }
        public bool UseModal { get; set; }

        protected override VueCodeFile PrepareCodeFile()
        {
            VueCodeFile codeFile = new VueCodeFile();

            codeFile.Component = PrepareComponent();
            codeFile.CodeScript = PrepareCodeScript();

            return codeFile;
        }

        private Component PrepareComponent()
        {
            var divComponent = new Component();

            var dataTableComponent = divComponent.AddChild<VDataTableComponent>()._Headers("headers")._Items("items")._ItemsPerPage(15).ShowSelect();
            if (this.Subject.ParentList != null & this.Subject.ParentList.Count > 0)
            {
                dataTableComponent.VIf(string.Join(" && ", this.Subject.ParentList.Select(v => $"_{v.PrimaryPropertyFullName().ToLowerCamelCase()}")));
            }

            //template v-slot:top
            {
                var vSlotTop = dataTableComponent.AddChild<TemplateBuilder>().VSlot("top");
                var vToolbar = vSlotTop.AddChild<VToolbarComponent>();
                vToolbar.AddAttribute("flat");

                if (this.SubjectConfig.WithCreatePage)
                {
                    var vBtnComponent = vToolbar.AddChild<VBtnComponent>()
                        .Color("primary").AddAttribute("outlined").AddCssClass("mr-2");
                    vBtnComponent.SetContent("新增(页面)");
                }

                if (this.SubjectConfig.WithCreateModal)
                {
                    var vBtnComponent = vToolbar.AddChild<VBtnComponent>()
                        .Color("primary").AddAttribute("outlined").AddCssClass("mr-2");
                    vBtnComponent.AddProperty("@click", $"open_createDialog_{this.Subject.Name.ToLowerCamelCase()}()");
                    vBtnComponent.SetContent("新增(弹框)");
                }
            }

            //template v-slot:item.action
            {
                var vSlotItemAction = dataTableComponent.AddChild<TemplateBuilder>().VSlot("item.action", "{ item }");

                List<string> methodParams = new List<string>();
                methodParams.Add("item");

                //查看(页面)
                if (this.SubjectConfig.WithItemPage)
                {
                    var vbtn = vSlotItemAction.AddChild<VBtnComponent>().AddCssClass("mr-2").AddAttribute("icon");

                    vbtn.AddChild<VIconComponent>().AddAttribute("small").SetContent("mdi-eye");
                }

                //查看(弹框)
                if (this.SubjectConfig.WithItemModal)
                {
                    var vbtn = vSlotItemAction.AddChild<VBtnComponent>().AddCssClass("mr-2").AddAttribute("icon")
                        .AddProperty("@click", $"open_itemDialog_{this.Subject.Name.ToLowerCamelCase()}(item)");

                    vbtn.AddChild<VIconComponent>().AddAttribute("small").SetContent("mdi-eye");
                }

                //编辑(页面)
                if (this.SubjectConfig.WithUpdatePage)
                {
                    var vbtn = vSlotItemAction.AddChild<VBtnComponent>().AddCssClass("mr-2").AddAttribute("icon");

                    vbtn.AddChild<VIconComponent>().AddAttribute("small").SetContent("mdi-pencil");
                }

                //编辑(弹框)
                if (this.SubjectConfig.WithUpdateModal)
                {
                    var vbtn = vSlotItemAction.AddChild<VBtnComponent>().AddCssClass("mr-2").AddAttribute("icon")
                        .AddProperty("@click", $"open_updateDialog_{this.Subject.Name.ToLowerCamelCase()}(item)");

                    vbtn.AddChild<VIconComponent>().AddAttribute("small").SetContent("mdi-pencil");
                }

                //删除
                {
                    List<string> items = new List<string>();
                    if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                    {
                        foreach (var parent in this.Subject.ParentList)
                        {
                            items.Add($"{parent.PrimaryPropertyFullName().ToLowerCamelCase()}: {parent.PrimaryPropertyFullName().ToLowerCamelCase()}");
                        }
                    }
                    items.Add($"{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}: item.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}");
                    var text = string.Join(", ", items);

                    var vbtn = vSlotItemAction.AddChild<VBtnComponent>().AddCssClass("mr-2").AddAttribute("icon")
                        .AddProperty(":to", $"{{ name: '{this.Subject.Name.ToLowerCaseBreakLine()}-item-page', params: {{ {text} }} }}");

                    vbtn.AddChild<VIconComponent>().AddAttribute("small").SetContent("mdi-delete");
                }
            }

            //components
            {
                //新增(弹框)
                if (this.SubjectConfig.WithCreateModal)
                {
                    var subjectCreateDialog = divComponent.AddChild<Component>().SetName($"{this.Subject.Name}CreateDialog");

                    List<string> items = new List<string>();
                    if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                    {
                        foreach (var parent in this.Subject.ParentList)
                        {
                            subjectCreateDialog.AddProperty($":{parent.PrimaryPropertyFullName().ToLowerCamelCase()}", parent.PrimaryPropertyFullName().ToLowerCamelCase());
                            items.Add(parent.PrimaryPropertyFullName().ToLowerCamelCase());
                        }
                    }

                    subjectCreateDialog.AddProperty(":open.sync", $"createDialog_{this.Subject.Name.ToLowerCamelCase()}_opened");

                    subjectCreateDialog.AddProperty(":reload_items.sync", "reload_items");

                    subjectCreateDialog.VIf(string.Join(" && ", items));
                }

                //查看(弹框)
                if (this.SubjectConfig.WithItemModal)
                {
                    var subjectItemDialog = divComponent.AddChild<Component>().SetName($"{this.Subject.Name}ItemDialog");

                    List<string> items = new List<string>();
                    if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                    {
                        foreach (var parent in this.Subject.ParentList)
                        {
                            subjectItemDialog.AddProperty($":{parent.PrimaryPropertyFullName().ToLowerCamelCase()}", parent.PrimaryPropertyFullName().ToLowerCamelCase());
                            items.Add(parent.PrimaryPropertyFullName().ToLowerCamelCase());
                        }
                    }
                    items.Add(this.Subject.PrimaryProperty.Name.ToLowerCamelCase());

                    subjectItemDialog.AddProperty(":open.sync", $"itemDialog_{this.Subject.Name.ToLowerCamelCase()}_opened");

                    subjectItemDialog.AddProperty($":{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}", this.Subject.PrimaryProperty.Name.ToLowerCamelCase());

                    subjectItemDialog.VIf(string.Join(" && ", items));
                }

                //更新(弹框)
                if (this.SubjectConfig.WithUpdateModal)
                {
                    var subjectUpdateDialog = divComponent.AddChild<Component>().SetName($"{this.Subject.Name}UpdateDialog");

                    List<string> items = new List<string>();
                    if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                    {
                        foreach (var parent in this.Subject.ParentList)
                        {
                            subjectUpdateDialog.AddProperty($":{parent.PrimaryPropertyFullName().ToLowerCamelCase()}", parent.PrimaryPropertyFullName().ToLowerCamelCase());
                            items.Add(parent.PrimaryPropertyFullName().ToLowerCamelCase());
                        }
                    }
                    items.Add(this.Subject.PrimaryProperty.Name.ToLowerCamelCase());

                    subjectUpdateDialog.AddProperty(":open.sync", $"updateDialog_{this.Subject.Name.ToLowerCamelCase()}_opened");

                    subjectUpdateDialog.AddProperty(":reload_items.sync", "reload_items");

                    subjectUpdateDialog.AddProperty($":{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()}", this.Subject.PrimaryProperty.Name.ToLowerCamelCase());

                    subjectUpdateDialog.VIf(string.Join(" && ", items));
                }
            }

            return divComponent;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //import
            codeScript.AddImport($"{this.Subject.Name}CreateDialog", $"@/dialogs/{this.Subject.Name}CreateDialog");
            codeScript.AddImport($"{this.Subject.Name}ItemDialog", $"@/dialogs/{this.Subject.Name}ItemDialog");
            codeScript.AddImport($"{this.Subject.Name}UpdateDialog", $"@/dialogs/{this.Subject.Name}UpdateDialog");

            //components
            codeScript.AddComponent($"{this.Subject.Name}CreateDialog".ToLowerCaseBreakLine(), $"{this.Subject.Name}CreateDialog");
            codeScript.AddComponent($"{this.Subject.Name}ItemDialog".ToLowerCaseBreakLine(), $"{this.Subject.Name}ItemDialog");
            codeScript.AddComponent($"{this.Subject.Name}UpdateDialog".ToLowerCaseBreakLine(), $"{this.Subject.Name}UpdateDialog");

            //props
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddProp(parent.PrimaryPropertyFullName().ToLowerCamelCase(), PropTypeConstant.STRING, required: true);
                }
            }

            //data
            codeScript.AddDataValue("headers", "[]");
            codeScript.AddDataValue("items", "[]");
            codeScript.AddDataValue(this.Subject.PrimaryProperty.Name.ToLowerCamelCase(), "null");
            {
                codeScript.AddDataValue("reload_items", false);
                codeScript.AddDataValue($"createDialog_{this.Subject.Name.ToLowerCamelCase()}_opened", false);
                codeScript.AddDataValue($"itemDialog_{this.Subject.Name.ToLowerCamelCase()}_opened", false);
                codeScript.AddDataValue($"updateDialog_{this.Subject.Name.ToLowerCamelCase()}_opened", false);
            }

            //computed
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddComputed($"_{parent.PrimaryPropertyFullName().ToLowerCamelCase()}")
                        .StepStatement($"return this.{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                }
            }

            //watch
            if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
            {
                foreach (var parent in this.Subject.ParentList)
                {
                    codeScript.AddWatch($"_{parent.PrimaryPropertyFullName().ToLowerCamelCase()}")
                        .StepStatement("this.load_data();");
                }
            }
            {
                var subjectCreateDialogOpened = codeScript.AddWatch($"createDialog_{this.Subject.Name.ToLowerCamelCase()}_opened");
                subjectCreateDialogOpened.AddParameter("newValue");
                subjectCreateDialogOpened.StepIf("!newValue && this.reload_items")
                    .StepStatement("this.reload_items = false;")
                    .StepStatement("this.load_data();");
            }
            {
                var subjectItemDialogOpened = codeScript.AddWatch($"itemDialog_{this.Subject.Name.ToLowerCamelCase()}_opened");
                subjectItemDialogOpened.AddParameter("newValue");
                subjectItemDialogOpened.StepIf("!newValue && this.reload_items")
                    .StepStatement("this.reload_items = false;")
                    .StepStatement("this.load_data();");
            }
            {
                var subjectUpdateDialogOpened = codeScript.AddWatch($"updateDialog_{this.Subject.Name.ToLowerCamelCase()}_opened");
                subjectUpdateDialogOpened.AddParameter("newValue");
                subjectUpdateDialogOpened.StepIf("!newValue && this.reload_items")
                    .StepStatement("this.reload_items = false;")
                    .StepStatement("this.load_data();");
            }

            //mounted
            codeScript.AddMounted()
                .StepStatement("this.load_data();");

            //methods
            {
                var loadDataMethod = codeScript.AddMethod("load_data");
                if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                {
                    loadDataMethod.StepIf(string.Join(" || ", this.Subject.ParentList.Select(v => $"!this._{v.PrimaryPropertyFullName().ToLowerCamelCase()}")))
                        .StepStatement("return;");
                }
                loadDataMethod.StepStatement("var request = {};");
                if (this.Subject.ParentList != null && this.Subject.ParentList.Count > 0)
                {
                    foreach (var parent in this.Subject.ParentList)
                    {
                        loadDataMethod.StepStatement($"request.{parent.PrimaryPropertyFullName().ToLowerCamelCase()} = this._{parent.PrimaryPropertyFullName().ToLowerCamelCase()};");
                    }
                }
                loadDataMethod.StepStatement($"this.appService.{this.Subject.Name.ToLowerCamelCase()}_data(request).then(this.{this.Subject.Name.ToLowerCamelCase()}_data_callback);");
            }
            {
                var loadDataMethodCallback = codeScript.AddMethod($"{this.Subject.Name.ToLowerCamelCase()}_data_callback");
                loadDataMethodCallback.AddParameter("response");
                loadDataMethodCallback.StepStatement("this.headers = response.data.headers;");
                loadDataMethodCallback.StepStatement("this.items = response.data.items;");
                loadDataMethodCallback.StepStatement("this.headers.push({ value: 'action', text: '操作' });");
            }
            {
                var open_subject_create_dialog_method = codeScript.AddMethod($"open_createDialog_{this.Subject.Name.ToLowerCamelCase()}");
                open_subject_create_dialog_method.StepStatement($"this.createDialog_{this.Subject.Name.ToLowerCamelCase()}_opened = true;");
            }
            {
                var open_subject_item_dialog_method = codeScript.AddMethod($"open_itemDialog_{this.Subject.Name.ToLowerCamelCase()}");
                open_subject_item_dialog_method.AddParameter("item");
                open_subject_item_dialog_method.StepStatement($"this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = item.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");
                open_subject_item_dialog_method.StepStatement($"this.itemDialog_{this.Subject.Name.ToLowerCamelCase()}_opened = true;");
            }
            {
                var open_subject_update_dialog_method = codeScript.AddMethod($"open_updateDialog_{this.Subject.Name.ToLowerCamelCase()}");
                open_subject_update_dialog_method.AddParameter("item");
                open_subject_update_dialog_method.StepStatement($"this.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()} = item.{this.Subject.PrimaryProperty.Name.ToLowerCamelCase()};");
                open_subject_update_dialog_method.StepStatement($"this.updateDialog_{this.Subject.Name.ToLowerCamelCase()}_opened = true;");
            }

            return codeScript;
        }
    }
}
