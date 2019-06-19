using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DiscussionForum.Startup))]
namespace DiscussionForum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
