using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAMBUG.Startup))]
namespace SAMBUG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
