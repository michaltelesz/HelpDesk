using System;
using Microsoft.Practices.Unity;
using Helpdesk.Domain.Abstract;
using Helpdesk.Domain.Concrete;

namespace Helpdesk.WebUI.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            //Mock<IRequestRepository> mock = new Mock<IRequestRepository>();
            //mock.Setup(m => m.Computers).Returns(new List<Computer>
            //{
            //    new Computer { Name = "Komputer Pierwszy", SerialNo = "abc123xyz", Temporary = false},
            //    new Computer { Name = "Mocny komputer", SerialNo = "1234567", Temporary = false},
            //    new Computer { Name = "Komputer tymczasowy", SerialNo = "temp", Temporary = true}
            //});

            container.RegisterType<IRequestsRepository, EFRequestsRepository>();
        }
    }
}
