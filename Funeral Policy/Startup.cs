using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Funeral_Policy.Startup))]
namespace Funeral_Policy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
