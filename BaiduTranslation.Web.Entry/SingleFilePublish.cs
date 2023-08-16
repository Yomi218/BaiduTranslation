using Furion;
using System.Reflection;

namespace BaiduTranslation.Web.Entry
{
    public class SingleFilePublish : ISingleFilePublish
    {
        public Assembly[] IncludeAssemblies()
        {
            return Array.Empty<Assembly>();
        }

        public string[] IncludeAssemblyNames()
        {
            return new[]
            {
            "BaiduTranslation.Application",
            "BaiduTranslation.Core",
            "BaiduTranslation.Web.Core"
        };
        }
    }
}