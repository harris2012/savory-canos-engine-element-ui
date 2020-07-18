using Savory.Canos.Engine.ElementUI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Manager.ElementUI
{
    public class ElementUIResourceManager : ProjectManager<ElementUIResourceParam>
    {
        public override void Generate(GenerateResult generateResult, ElementUIResourceParam param)
        {
            generateResult.WriteToFile(ElementUIResourcePaths._browserslistrc, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._browserslistrc));
            generateResult.WriteToFile(ElementUIResourcePaths._eslintrc_js, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._eslintrc_js));
            generateResult.WriteToFile(ElementUIResourcePaths._gitignore, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._gitignore));
            generateResult.WriteToFile(ElementUIResourcePaths._babel_config_js, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._babel_config_js));
            generateResult.WriteToFile(ElementUIResourcePaths._default_conf, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._default_conf));
            generateResult.WriteToFile(ElementUIResourcePaths._postcss_config_js, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._postcss_config_js));
            generateResult.WriteToFile(ElementUIResourcePaths._readme_md, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._readme_md));
            generateResult.WriteToFile(ElementUIResourcePaths._vue_config_js, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._vue_config_js));
            generateResult.WriteToFile(ElementUIResourcePaths._public_favicon_ico, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_favicon_ico));
            generateResult.WriteToFile(ElementUIResourcePaths._public_robots_txt, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_robots_txt));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_android_chrome_192x192_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_android_chrome_192x192_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_android_chrome_512x512_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_android_chrome_512x512_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_apple_touch_icon_120x120_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_apple_touch_icon_120x120_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_apple_touch_icon_152x152_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_apple_touch_icon_152x152_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_apple_touch_icon_180x180_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_apple_touch_icon_180x180_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_apple_touch_icon_60x60_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_apple_touch_icon_60x60_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_apple_touch_icon_76x76_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_apple_touch_icon_76x76_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_apple_touch_icon_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_apple_touch_icon_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_favicon_16x16_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_favicon_16x16_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_favicon_32x32_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_favicon_32x32_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_msapplication_icon_144x144_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_msapplication_icon_144x144_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_mstile_150x150_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_mstile_150x150_png));
            generateResult.WriteToFile(ElementUIResourcePaths._public_img_icons_safari_pinned_tab_svg, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._public_img_icons_safari_pinned_tab_svg));
            generateResult.WriteToFile(ElementUIResourcePaths._src_registerserviceworker_js, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._src_registerserviceworker_js));
            generateResult.WriteToFile(ElementUIResourcePaths._src_assets_logo_png, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._src_assets_logo_png));
            generateResult.WriteToFile(ElementUIResourcePaths._src_assets_logo_svg, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._src_assets_logo_svg));
            //generateResult.WriteToFile(ElementUIResourcePaths._src_components_helloworld_vue, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._src_components_helloworld_vue));
            generateResult.WriteToFile(ElementUIResourcePaths._src_plugins_vuetify_js, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._src_plugins_vuetify_js));
            generateResult.WriteToFile(ElementUIResourcePaths._src_styles_vuetify_extended_css, ElementUIResources.Instance.GetResource(ElementUIResourceKeys._src_styles_vuetify_extended_css));
        }
    }
}
