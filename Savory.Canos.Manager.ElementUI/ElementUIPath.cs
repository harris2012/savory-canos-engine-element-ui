using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Manager.ElementUI
{
    public class ElementUIPath
    {
        private readonly string prefix;

        public ElementUIPath(string prefix)
        {
            this.prefix = prefix;
        }

        public string Public => Path.Combine(prefix, "public");

        public string PublicImg => Path.Combine(this.Public, "img");

        public string PublicImgIcons => Path.Combine(this.PublicImg, "icons");

        /// <summary>
        /// src
        /// </summary>
        public string Src => Path.Combine(prefix, "src");

        /// <summary>
        /// src/assets
        /// </summary>
        public string SrcAssets => Path.Combine(this.Src, "assets");

        /// <summary>
        /// src/components
        /// </summary>
        public string SrcComponents => Path.Combine(this.Src, "components");

        /// <summary>
        /// src/plugins
        /// </summary>
        public string SrcPuglins => Path.Combine(this.Src, "plugins");

        /// <summary>
        /// src/router
        /// </summary>
        public string SrcRouter => Path.Combine(this.Src, "router");

        /// <summary>
        /// src/views
        /// </summary>
        public string SrcViews => Path.Combine(this.Src, "views");

        /// <summary>
        /// src/pages
        /// </summary>
        public string SrcPages => Path.Combine(this.Src, "pages");

        /// <summary>
        /// src/dialogs
        /// </summary>
        public string SrcDialogs => Path.Combine(this.Src, "dialogs");

    }
}
