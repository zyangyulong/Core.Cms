using Core.CMS.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using Core.CMS.Core.CodeGenerator;
using Core.CMS.Core.Models;
using Xunit;

namespace Core.CMS.Test
{
    /// <summary>
    /// zyl
    /// 2018.12.12
    /// 测试代码生成器
    /// 暂时只实现了SqlServer的实体代码生成
    /// </summary>
    public class GeneratorTest
    {
        [Fact]
        public void GeneratorModelForSqlServer()
        {
            var serviceProvider = BuildServiceForSqlServer();
            var codeGenerator = serviceProvider.GetRequiredService<CodeGenerator>();
            codeGenerator.GenerateModelCodesFromDatabase();
            Assert.Equal("SqlServer", DatabaseType.SqlServer.ToString());
            Assert.Equal(0, 0);
        }

        /// <summary>
        /// 构造依赖注入容器，然后传入参数
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildServiceForSqlServer()
        {
            var services = new ServiceCollection();

            services.Configure<CodeGenerateOption>(options =>
            {
                options.ConnectionString = "Data Source=.;Initial Catalog=CzarCms;User ID=sa;Password=123456;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                options.DbType = DatabaseType.SqlServer.ToString();//数据库类型是SqlServer,其他数据类型参照枚举DatabaseType
                options.Author = "张玉龙";//作者名称
                options.OutputPath = @"E:\Git仓库集合\VSCMS\Core.Cms\NETCoreCMS\Core.CMS.Models";//实体模型输出路径，为空则默认为当前程序运行的路径
                options.ModelsNamespace = "Core.CMS.Models";//实体命名空间
            });
            services.AddSingleton<CodeGenerator>();//注入Model代码生成器
            return services.BuildServiceProvider(); //构建服务提供程序
        }
    }
}
