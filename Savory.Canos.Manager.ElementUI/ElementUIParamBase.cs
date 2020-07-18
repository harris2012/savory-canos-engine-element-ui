using Savory.Canos.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Manager.ElementUI
{
    public abstract class ElementUIParamBase
    {
        public ElementUIParam ElementUIParam { get; set; }

        #region Param 快捷访问

        /// <summary>
        /// param.Project
        /// </summary>
        public Project Project => ElementUIParam.Project;

        /// <summary>
        /// param.SubjectMap
        /// </summary>
        public Dictionary<string, Subject> SubjectMap => ElementUIParam.SubjectMap;

        /// <summary>
        /// param.SubjectConfigMap
        /// </summary>
        public Dictionary<string, SubjectConfig> SubjectConfigMap => ElementUIParam.SubjectConfigMap;

        /// <summary>
        /// param.MetadataMap
        /// </summary>
        public Dictionary<string, Metadata> MetadataMap => ElementUIParam.MetadataMap;

        #endregion


        #region ElementUIParam 快捷访问

        /// <summary>
        /// 路径前缀
        /// </summary>
        public string Prefix => ElementUIParam.Prefix;

        /// <summary>
        /// api地址
        /// </summary>
        public string ApiHost => ElementUIParam.ApiHost;

        #endregion
    }
}
