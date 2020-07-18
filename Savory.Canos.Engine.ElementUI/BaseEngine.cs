using Savory.CodeDom.Vue;
using Savory.CodeDom.Vue.Engine;
using System;
using System.IO;
using System.Text;

namespace Savory.Canos.Engine.ElementUI
{
    public abstract class BaseEngine
    {
        public string TransformText()
        {
            var codeFile = PrepareCodeFile();

            VueCodeFileEngine generator = new VueCodeFileEngine();

            StringBuilder builder = new StringBuilder();

            generator.GenerateCodeFile(codeFile, new StringWriter(builder));

            return builder.ToString();
        }

        protected abstract VueCodeFile PrepareCodeFile();
    }
}
