using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Helpers;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.DependencyRevolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //biri senden IProductService isterse ProductManageri ver
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>().SingleInstance();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>().SingleInstance();

            builder.RegisterType<illerManager>().As<IillerService>().SingleInstance();
            builder.RegisterType<EfillerDal>().As<IillerDal>().SingleInstance();

            builder.RegisterType<ilcelerManager>().As<IilcelerService>().SingleInstance();
            builder.RegisterType<EfilcelerDal>().As<IilcelerDal>().SingleInstance();

            builder.RegisterType<SuperCategoryManager>().As<ISuperCategoryService>().SingleInstance();
            builder.RegisterType<EfSuperCategory>().As<ISuperCategoryDal>().SingleInstance();

            builder.RegisterType<CartManager>().As<ICartService>().SingleInstance();
            builder.RegisterType<EfCartDal>().As<ICartDal>().SingleInstance();

            builder.RegisterType<ColorsManager>().As<IColorsService>().SingleInstance();
            builder.RegisterType<EfColorsDal>().As<IColorDal>().SingleInstance();

            builder.RegisterType<ProductDetailsManager>().As<IProductDetailsService>().SingleInstance();
            builder.RegisterType<EfProductDetialsDal>().As<IProductDetailsDal>().SingleInstance();

            builder.RegisterType<ProductStocksManager>().As<IProductStocksService>().SingleInstance();
            builder.RegisterType<EfProductStocksDal>().As<IProductStocksDal>().SingleInstance();

            builder.RegisterType<ProductImageManager>().As<IProductImageService>().SingleInstance();

            builder.RegisterType<EfCartItemDal>().As<ICartItemDal>().SingleInstance();

            builder.RegisterType<SalahTime>().SingleInstance();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

            builder.RegisterType<ProductManager>().As<IProductService>()
            .EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector() // Aspect selector sınıfını ekliyoruz
            }).SingleInstance();

            builder.RegisterType<PerformanceAspect>().AsSelf();
        }
    }
}