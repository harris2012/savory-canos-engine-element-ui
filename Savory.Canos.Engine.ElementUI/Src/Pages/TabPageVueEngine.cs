using Savory.CodeDom.Vue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Engine.ElementUI.Src.Pages
{
    public class TabPageVueEngine
    {
        public List<VuePageTab> TabItemList { get; set; }

        public string TransformText()
        {
            /*
<template>
    <v-tabs>
        <v-tab :to="{name: item.routeName}" v-for="(item, index) in items" :key="index">{{ item.text }}</v-tab>
        <v-tabs-items>
            <router-view />
        </v-tabs-items>
    </v-tabs>
</template>

<script>
    export default {
        data: () => ({
            items: [
<#
    if(this.TabItemList != null && this.TabItemList.Count > 0)
    {
        foreach(var tabItem in this.TabItemList)
        {
#>
                { routeName: '<#=tabItem.PageRouteName??string.Empty#>', text: '<#=tabItem.Text??string.Empty#>' },
<#
        }
    }
#>
            ],
        })
    }
</script>
             */
            return string.Empty;
        }
    }
}
