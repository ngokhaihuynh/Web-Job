using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartupAttribute(typeof(WebJob.Startup))]
namespace WebJob
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
