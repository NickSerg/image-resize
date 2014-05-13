using System;
using IR.Infrastructure.Interfaces;
using IR.ResizeModule.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace IR.ResizeModule
{
    public class Module : IModule
    {
        private readonly IUnityContainer container;

        public Module(IUnityContainer container)
        {
            if(container == null)
                throw new ArgumentNullException("container");

            this.container = container;
        }

        public void Initialize()
        {
            container.RegisterType<IResizeService, ResizeService>(new ContainerControlledLifetimeManager());
        }
    }
}
