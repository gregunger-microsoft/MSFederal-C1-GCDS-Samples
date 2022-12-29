using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace DotnetFramework472Sample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
