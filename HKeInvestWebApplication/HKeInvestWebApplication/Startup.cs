using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HKeInvestWebApplication.Startup))]
namespace HKeInvestWebApplication
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
