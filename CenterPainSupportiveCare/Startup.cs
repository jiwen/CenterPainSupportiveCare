using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CenterPainSupportiveCare.Startup))]
namespace CenterPainSupportiveCare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
