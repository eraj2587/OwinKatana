using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;



namespace Katana
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    class Program
    {
        

        static void Main(string[] args)
        {
            string url = "http://localhost:8080/";

            using (WebApp.Start<StartUp>(url))
            {
                Console.WriteLine("Server started");
                Console.ReadKey();
                Console.WriteLine("Stopping");
            }

        }
    }

    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {

            //builder.UseWelcomePage();

            //builder.Run(x =>
            //{
            //    return x.Response.WriteAsync("Hello world");
            //});

            //app.Use(async (env, next) =>
            //{
            //    foreach (var pair in env.Environment)
            //    {
            //        Console.WriteLine("{0}:{1}",pair.Key,pair.Value);
            //    }

            //    Console.WriteLine("All keys are fetched");
            //     await next();
            //});

            app.Use(async (env, next) =>
            {
                Console.WriteLine("Status code is {0}",env.Response.StatusCode);
                await next();
            });


            ConfigureWebAPI(app);


            app.Use<HelloWorldComponent>();
        }

        void ConfigureWebAPI(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute("DefaultApi","api/{controller}/{id}",new {id=RouteParameter.Optional });
            app.UseWebApi(config);
        }
    }

    public class HelloWorldComponent
    {
        private AppFunc _next;
        public HelloWorldComponent(AppFunc next)
        {
            _next = next;
        }

        public Task Invoke(IDictionary<string, object> environemt)

        {
            var a = environemt["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(a))
            {

                return writer.WriteAsync("Hello");
            }
        }
    }

}
