using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Jwt.Dao.EntityFramework.Implementation;
using Jwt.Dao.EntityFramework.Interfaces;



namespace jwtApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            //IDbContextFactory 
            container.RegisterType<IDbContextFactory, DbContextFactory>();
            container.RegisterType<IDbContextScopeFactory, DbContextScopeFactory>();
           
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}