using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Savory.Canos.Engine.ElementUI.Public;
using Savory.Canos.Engine.ElementUI.Resources;
using Savory.Canos.Engine.ElementUI.Src;
using Savory.Canos.Engine.ElementUI.Src.Components;
using Savory.Canos.Engine.ElementUI.Src.Pages;
using Savory.Canos.Engine.ElementUI.Src.Router;
using Savory.CodeDom.Npm;
using Savory.CodeDom.Vue.Engine;

namespace Savory.Canos.Manager.ElementUI
{
    public class ElementUIManager : ProjectManager<ElementUIParam>
    {
        private readonly ElementUIPath path;

        public override void Generate(GenerateResult generateResult, ElementUIParam param)
        {
            var path = new ElementUIPath(param.Prefix);

            generateResult.WriteToFile("package.json", BuildPackageJson(param.Project.Name));

            generateResult.WriteToFile("package-lock.json", BuildPackageLockFile(param.Project.Name));

            generateResult.WriteToFile(Path.Combine(path.Public, "index.html"), new IndexHtmlEngine { Title = param.Project.Title }.TransformText());

            generateResult.WriteToFile(Path.Combine(path.Src, "main.js"), new MainJsEngine { ProjectName = param.Project.Name }.TransformText());

            //generateResult.WriteToFile(Path.Combine(path.SrcRouter, "index.js"), new IndexJsEngine
            //{
            //    RouteList = param.VueRouteList
            //}.TransformText());

            generateResult.WriteToFile(Path.Combine(path.Src, "App.vue"), new AppVueEngine
            {
            }.TransformText());

            generateResult.WriteToFile(Path.Combine(path.Src, "service.js"), new ServiceJsEngine
            {
                ProjectName = param.Project.Name,
                SubjectMap = param.SubjectMap,
                MetadataMap = param.MetadataMap,
                ApiHost = param.ApiHost
            }.TransformText());

            generateResult.WriteToFile(Path.Combine(path.SrcComponents, "MainHeader.vue"), new MainHeaderVueEngine
            {
            }.TransformText());

            generateResult.WriteToFile(Path.Combine(path.SrcComponents, "MainMenu.vue"), new MainMenuVueEngine
            {
                SubjectMap = param.SubjectMap
            }.TransformText());

            generateResult.WriteToFile(Path.Combine(path.SrcPages, "WelcomePage.vue"), new WelcomePageEngine
            {
            }.TransformText());

            generateResult.WriteToFile(Path.Combine(path.SrcPages, "_404Page.vue"), new NotFoundPageEngine
            {
            }.TransformText());
        }

        private string BuildPackageJson(string projectName)
        {
            var content = ElementUIResources.Instance.GetStringResource(ElementUIResourceKeys._package_json_txt);

            var packageFile = JsonConvert.DeserializeObject<PackageFile>(content);
            packageFile.Name = projectName;

            //if (true)
            //{
            //    packageFile.Dependencies.Add("savory-canos-vuetify-component", "1.0.10");
            //}

            return JsonConvert.SerializeObject(packageFile, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private string BuildPackageLockFile(string projectName)
        {
            var content = ElementUIResources.Instance.GetStringResource(ElementUIResourceKeys._package_lock_json_txt);

            var packageFile = JsonConvert.DeserializeObject<PackageLockFile>(content);
            packageFile.Name = projectName;

            //if (true)
            //{
            //    var savoryCanosElementUIComponentContent = ElementUIResources.Instance.GetStringResource(ElementUIResourceKeys._package_lock_json_savory_canos_vuetify_component_txt);
            //    var savoryCanosElementUIComponentPackageLockFile = JsonConvert.DeserializeObject<PackageLockFile>(savoryCanosElementUIComponentContent);
            //    foreach (var item in savoryCanosElementUIComponentPackageLockFile.Dependencies)
            //    {
            //        packageFile.Dependencies.Add(item.Key, item.Value);
            //    }
            //}

            return JsonConvert.SerializeObject(packageFile, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
