using Savory.Canos.Template;
using Savory.CodeDom.Js;
using Savory.CodeDom.Js.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Src.Router
{
    public class IndexJsEngine
    {
        //public List<VueRoute> RouteList { get; set; }

        public Dictionary<string, Subject> SubjectMap { get; set; }

        public string TransformText()
        {
            var codeFile = PrepareCodeFile();

            JsCodeEngine generator = new JsCodeEngine();

            StringBuilder builder = new StringBuilder();

            generator.GenerateFile(codeFile, new StringWriter(builder));

            return builder.ToString();
        }

        private JsCodeFile PrepareCodeFile()
        {
            JsCodeFile codeFile = new JsCodeFile();

            codeFile.StepStatement("import Vue from 'vue'");
            codeFile.StepStatement("import VueRouter from 'vue-router'");

            codeFile.StepEmpty();
            codeFile.StepStatement("Vue.use(VueRouter)");

            codeFile.StepEmpty();
            if (this.SubjectMap != null && this.SubjectMap.Count > 0)
            {
                foreach (var subject in this.SubjectMap.Values)
                {
                    if (subject.ParentList != null && subject.ParentList.Count > 0)
                    {
                        continue;
                    }
                    codeFile.StepStatement($"import {subject.Name}ListPage from '../pages/{subject.Name}ListPage.vue'");
                }
            }

            var dataArray = codeFile.StepAssignDataArray("const routes").AddDataArray();
            if (this.SubjectMap != null && this.SubjectMap.Count > 0)
            {
                foreach (var subject in this.SubjectMap.Values)
                {
                    if (subject.ParentList != null && subject.ParentList.Count > 0)
                    {
                        continue;
                    }

                    DataObject dataObject = new DataObject();
                    dataObject.AddDataValue("path", $"'/{subject.Name.ToLowerCaseBreakLine()}'");
                    dataObject.AddDataValue("name", $"'{subject.Name.ToLowerCaseBreakLine()}-list-page'");
                    dataObject.AddDataValue("component", $"{subject.Name}ListPage");

                    dataArray.AddDataObject(dataObject);
                }
            }

            codeFile.StepEmpty();
            {
                codeFile.StepStatement("const router = new VueRouter({");
                codeFile.StepStatement("    mode: 'history',");
                codeFile.StepStatement("    base: process.env.BASE_URL,");
                codeFile.StepStatement("    routes");
                codeFile.StepStatement("})");
            }

            codeFile.StepEmpty();
            codeFile.StepStatement("export default router");

            return codeFile;
        }
    }
}
