using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GraduateDesignBk.Startup))]
namespace GraduateDesignBk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
