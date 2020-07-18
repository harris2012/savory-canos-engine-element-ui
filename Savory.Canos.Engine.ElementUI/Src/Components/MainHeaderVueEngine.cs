using Savory.CodeDom.Css;
using Savory.CodeDom.Js;
using Savory.CodeDom.Tag;
using Savory.CodeDom.Tag.Vue.ElementUI;
using Savory.CodeDom.Vue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Src.Components
{
    public class MainHeaderVueEngine : BaseEngine
    {

        protected override VueCodeFile PrepareCodeFile()
        {
            VueCodeFile vueCodeFile = new VueCodeFile();

            vueCodeFile.Component = PrepareComponent();
            vueCodeFile.CodeScript = PrepareCodeScript();
            vueCodeFile.CodeStyle = PrepareCodeStyle();

            return vueCodeFile;
        }

        private Component PrepareComponent()
        {
            var vAppBar = new VAppBarComponent().AddProperty(":clipped-left", "$vuetify.breakpoint.lgAndUp").AddAttribute("app").AddProperty("color", "blue darken-3").AddAttribute("dark");

            vAppBar.AddChild<VAppBarNavIconComponent>().AddProperty("@click.stop", "drawerValue = !drawerValue");
            vAppBar.AddChild<VToolbarTitleComponent>().AddProperty("style", "width: 300px").AddCssClass("pl-0")
                .AddChild<VBtnComponent>().AddProperty(":to", "{ name: 'welcome' }").AddAttribute("text").AddAttribute("x-large").SetContent("Google Contacts");
            vAppBar.AddChild<VTextFieldComponent>().AddAttribute("flat solo-inverted hide-details")
                .AddProperty("prepend-inner-icon", "mdi-magnify")
                .AddProperty("label", "Search")
                .AddProperty("class", "hidden-sm-and-down");
            vAppBar.AddChild<VSpacerComponent>();
            vAppBar.AddChild<VBtnComponent>().AddAttribute("icon").AddChild<VIconComponent>().SetContent("mdi-apps");
            vAppBar.AddChild<VBtnComponent>().AddAttribute("icon").AddChild<VIconComponent>().SetContent("mdi-bell");


            return vAppBar;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //props
            codeScript.AddProp("drawer", PropTypeConstant.BOOLEAN);

            //data
            codeScript.AddDataValue("drawerValue", "null");

            //mounted
            codeScript.AddMounted()
                .StepStatement("this.drawerValue = this.drawer;");

            //watch
            {
                var method = codeScript.AddWatch("drawerValue");
                method.AddParameter("newValue");
                method.StepStatement("this.$emit('update:drawer', newValue);");
            }

            return codeScript;
        }

        private CodeStyle PrepareCodeStyle()
        {
            CodeStyle codeStyle = new CodeStyle();

            {
                var codeCss = codeStyle.AddCss(".v-btn:before");
                codeCss.BackgroundColor = "inherit !important";
            }

            return codeStyle;
        }
    }
}
