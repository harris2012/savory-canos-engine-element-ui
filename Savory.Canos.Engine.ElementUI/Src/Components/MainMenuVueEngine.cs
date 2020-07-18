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
    public class MainMenuVueEngine : BaseEngine
    {
        public Dictionary<string, Subject> SubjectMap { get; set; }

        protected override VueCodeFile PrepareCodeFile()
        {
            VueCodeFile vueCodeFile = new VueCodeFile();

            vueCodeFile.Component = PrepareComponent();
            vueCodeFile.CodeScript = PrepareCodeScript();
            //vueCodeFile.CodeStyle = PrepareCodeStyle();

            return vueCodeFile;
        }

        private Component PrepareComponent()
        {
            var vNavigationDrawer = new VNavigationDrawerComponent().AddProperty(":value", "drawer").AddProperty(":clipped", "$vuetify.breakpoint.lgAndUp").AddAttribute("app");

            var vList = vNavigationDrawer.AddChild<VListComponent>().AddAttribute("dense");

            var vListItemGroup = vList.AddChild<VListItemGroupComponent>();

            var div = vListItemGroup.AddChild<Component>().VForIndex("item", "items", "i");

            {
                var vListItem = div.AddChild<VListItemComponent>().AddProperty(":to", "{name: item.route}").AddProperty("exact-active-class", "router-link-exact-active");
                vListItem.AddChild<VListItemIconComponent>().AddCssClass("mr-4")
                    .AddChild<VIconComponent>().AddProperty("v-text", "item.icon");
                vListItem.AddChild<VListItemContentComponent>()
                    .AddChild<VListItemTitleComponent>().SetContent("{{ item.text }}");
            }

            {
                var vListItem = div.AddChild<VListItemComponent>().AddProperty(":to", "{name: child.route}")
                    .AddProperty("exact-active-class", "router-link-exact-active")
                    .VForIndex("child", "item.children", "j");
                vListItem.AddChild<VListItemIconComponent>().AddCssClass("mr-4")
                    .AddChild<VIconComponent>().SetContent("{{ child.icon }}");
                vListItem.AddChild<VListItemContentComponent>().AddCssClass("ml-4")
                    .AddChild<VListItemTitleComponent>().SetContent("{{ child.text }}");
            }

            return vNavigationDrawer;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //props
            codeScript.AddProp("drawer", "null");

            //data
            codeScript.AddDataArray("items").AddRange(BuildMenuItemList());

            return codeScript;
        }

        private List<DataObject> BuildMenuItemList()
        {
            if (this.SubjectMap == null || this.SubjectMap.Count == 0)
            {
                return null;
            }

            List<DataObject> dataObjectList = new List<DataObject>();
            foreach (var subject in this.SubjectMap.Values)
            {
                if (subject.ParentList != null && subject.ParentList.Count > 0)
                {
                    continue;
                }

                DataObject dataObject = new DataObject()
                    .AddDataValue("icon", "'mdi-apps'")
                    .AddDataValue("text", $"'{subject.DisplayName}'")
                    .AddDataValue("route", $"'{subject.Name.ToLowerCaseBreakLine()}-list-page'");
                dataObjectList.Add(dataObject);
            }

            return dataObjectList;
        }

        //private CodeStyle PrepareCodeStyle()
        //{
        //    CodeStyle codeStyle = new CodeStyle();

        //    return codeStyle;
        //}

        /*
<!--<style>
    .theme--light.v-list-item.v-list-item--active {
        color: rgba(0, 0, 0, 0.87) !important;
    }

        .theme--light.v-list-item.v-list-item--active .v-icon {
            color: rgba(0, 0, 0, 0.54) !important;
        }

    .theme--light.v-list-item:before {
        background-color: inherit !important;
    }

    .theme--light.v-list-item:hover:before {
        background-color: currentColor !important;
    }

    .theme--light.router-link-exact-active {
        color: inherit !important;
    }

        .theme--light.router-link-exact-active:before {
            background-color: currentColor !important;
            color: rgba(0, 0, 0, 0.87) !important;
        }

        .theme--light.router-link-exact-active .v-icon {
            color: rgba(0, 0, 0, 0.87) !important;
        }
</style>-->
*/
    }
}
