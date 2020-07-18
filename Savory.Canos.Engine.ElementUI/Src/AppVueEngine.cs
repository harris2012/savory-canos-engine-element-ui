using Savory.CodeDom.Tag;
using Savory.CodeDom.Tag.Vue.ElementUI;
using Savory.CodeDom.Vue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Src
{
    public class AppVueEngine : BaseEngine
    {
        protected override VueCodeFile PrepareCodeFile()
        {
            VueCodeFile vueCodeFile = new VueCodeFile();

            vueCodeFile.Component = PrepareComponent();
            vueCodeFile.CodeScript = PrepareCodeScript();

            return vueCodeFile;
        }

        private Component PrepareComponent()
        {
            var vApp = new VAppComponent();

            vApp.AddChild<Component>("MainMenu").AddProperty(":drawer", "drawer");
            vApp.AddChild<Component>("MainHeader").AddProperty(":drawer.sync", "drawer");
            vApp.AddChild<VContentComponent>().AddChild<VContainerComponent>().AddAttribute("fluid").AddChild<RouterViewComponent>();

            return vApp;
        }

        private CodeScript PrepareCodeScript()
        {
            CodeScript codeScript = new CodeScript();

            //import
            codeScript.AddImport("MainMenu", "@/components/MainMenu");
            codeScript.AddImport("MainHeader", "@/components/MainHeader");

            //components
            codeScript.AddComponent("main-menu", "MainMenu");
            codeScript.AddComponent("main-header", "MainHeader");

            //data
            codeScript.AddDataValue("drawer", true);

            return codeScript;
        }
    }
}
