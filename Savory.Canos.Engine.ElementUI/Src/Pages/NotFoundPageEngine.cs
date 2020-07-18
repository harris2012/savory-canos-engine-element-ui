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

namespace Savory.Canos.Engine.ElementUI.Src.Pages
{
    public class NotFoundPageEngine : BaseEngine
    {
        protected override VueCodeFile PrepareCodeFile()
        {
            VueCodeFile codeFile = new VueCodeFile();

            codeFile.Component = new PComponent();
            codeFile.Component.SetContent("404");

            return codeFile;
        }
    }
}
