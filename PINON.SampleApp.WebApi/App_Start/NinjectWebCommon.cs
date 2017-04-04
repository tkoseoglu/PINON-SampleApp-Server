[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PINON.SampleApp.WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PINON.SampleApp.WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace PINON.SampleApp.WebApi.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using PINON.SampleApp.Data.Contracts;
    using PINON.SampleApp.Data;
    using PINON.SampleApp.Data.Contracts.Repos;
    using PINON.SampleApp.Data.Repos;
    using PINON.SampleApp.Identity.Contracts;
    using PINON.SampleApp.Identity;
    using Microsoft.AspNet.Identity;
    using PINON.SampleApp.Identity.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using TOLGA.Common.Contracts;
    using TOLGA.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAppDbContext>().To<AppDbContext>();
            kernel.Bind<IPatientRepo>().To<PatientRepo>();
            kernel.Bind<IHospitalRepo>().To<HospitalRepo>();
            kernel.Bind<IIdentityManager>().To<IdentityManager>();

            kernel.Bind<ITolgaLogging>().To<TolgaLogging>();
            kernel.Bind<ITolgaUtilities>().To<TolgaUtilities>();
        }
    }
}
