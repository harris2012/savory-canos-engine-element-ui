using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.Canos.Manager.ElementUI
{
    /// <summary>
    /// ElementUIParam
    /// </summary>
    public sealed class ElementUIParam : ParamBase
    {
        /// <summary>
        /// 路径前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// api路径
        /// </summary>
        public string ApiHost { get; set; }
    }
}
