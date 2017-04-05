using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPedidos.Startup))]
namespace WebPedidos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
