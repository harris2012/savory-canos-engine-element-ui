using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Src
{
    public class MainJsEngine
    {
        public string ProjectName { get; set; } = "CanosProject";

        public string TransformText()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("import Vue from 'vue'");
            builder.AppendLine("import App from './App.vue'");
            builder.AppendLine("import './registerServiceWorker'");
            builder.AppendLine("import router from './router'");
            builder.AppendLine();

            builder.AppendLine("import vuetify from './plugins/vuetify';");
            builder.AppendLine();

            builder.AppendLine("import './styles/vuetify-extended.css';");
            builder.AppendLine();

            builder.AppendLine("import AppService from './service';");
            builder.AppendLine();

            builder.AppendLine("Vue.config.productionTip = false;");
            builder.AppendLine("Vue.prototype.appService = AppService;");
            builder.AppendLine();

            builder.AppendLine("new Vue({");
            builder.AppendLine("    router,");
            builder.AppendLine("    vuetify,");
            builder.AppendLine("    render: h => h(App)");
            builder.AppendLine("}).$mount('#app')");

            return builder.ToString();
        }
    }
}
