using System;
using IR.Infrastructure;
using IR.Infrastructure.Interfaces;
using IR.ResizeModule.Services;
using IR.ResizeModule.ViewModels;
using IR.ResizeModule.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace IR.ResizeModule
{
    public class Module : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public Module(IUnityContainer container, IRegionManager regionManager)
        {
            if(container == null)
                throw new ArgumentNullException("container");

            if(regionManager == null)
                throw new ArgumentNullException("regionManager");

            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            container.RegisterType<IResizeService, ResizeService>(new ContainerControlledLifetimeManager())
                .RegisterType<IResizeViewModel, ResizeViewModel>(new ContainerControlledLifetimeManager());

            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ResizeView))
                .RegisterViewWithRegion(RegionNames.MainToolBarRegion, typeof(ResizeCommandsView));
        }
    }
}
