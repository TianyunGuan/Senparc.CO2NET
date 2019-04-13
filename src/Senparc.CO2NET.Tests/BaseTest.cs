using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Senparc.CO2NET.RegisterServices;

namespace Senparc.CO2NET.Tests
{
    //[TestClass]
    public class BaseTest
    {
        protected static IRegisterService registerService;
        protected static SenparcSetting _senparcSetting;

        public BaseTest()
        {
            RegisterServiceCollection();

            RegisterServiceStart();
        }

        /// <summary>
        /// ע�� IServiceCollection �� MemoryCache
        /// </summary>
        public static void RegisterServiceCollection()
        {
            var serviceCollection = new ServiceCollection();
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json", false, false);
            var config = configBuilder.Build();

            _senparcSetting = new SenparcSetting() { IsDebug = true };
            config.GetSection("SenparcSetting").Bind(_senparcSetting);

            serviceCollection.AddSenparcGlobalServices(config);
            serviceCollection.AddMemoryCache();//ʹ���ڴ滺��
        }

        /// <summary>
        /// ע�� RegisterService.Start()
        /// </summary>
        public static void RegisterServiceStart(bool autoScanExtensionCacheStrategies = false)
        {
            //ע��
            var mockEnv = new Mock<IHostingEnvironment>();
            mockEnv.Setup(z => z.ContentRootPath).Returns(() => UnitTestHelper.RootPath);

            registerService = RegisterService.Start(mockEnv.Object, _senparcSetting)
                .UseSenparcGlobal(autoScanExtensionCacheStrategies);
        }
    }
}
