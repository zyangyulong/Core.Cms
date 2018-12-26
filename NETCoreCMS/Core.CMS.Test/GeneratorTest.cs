using Core.CMS.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using Core.CMS.Core.CodeGenerator;
using Core.CMS.Core.Models;
using Core.CMS.IRepository;
using Core.CMS.Models;
using Core.CMS.Repository.SqlServer;
using Microsoft.Extensions.Configuration;
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
            var serviceProvider = BuildServiceForSqlServer(); //依赖注入
            var codeGenerator = serviceProvider.GetRequiredService<CodeGenerator>();//从容器中获取实例
            codeGenerator.GenerateModelCodesFromDatabase();
            Assert.Equal("SqlServer", DatabaseType.SqlServer.ToString());
            Assert.Equal(0, 0);
        }
        [Fact]
        public void TestBaseFactory()
        {
            IServiceProvider serviceProvider = BuildServiceForSqlServer();
            IArticleCategoryRepository categoryRepository = serviceProvider.GetService<IArticleCategoryRepository>();
            var category = new ArticleCategory
            {
                Title = "随笔",
                ParentId = 0,
                ClassList = "",
                ClassLayer = 0,
                Sort = 0,
                ImageUrl = "",
                SeoTitle = "随笔的SEOTitle",
                SeoKeywords = "随笔的SeoKeywords",
                SeoDescription = "随笔的SeoDescription",
                IsDeleted = false,
            };
            var categoryId = categoryRepository.Insert(category);
            var list = categoryRepository.GetList();
            Assert.True(1 == list.Count());
            Assert.Equal("随笔", list.FirstOrDefault().Title);
            Assert.Equal("SQLServer", DatabaseType.SqlServer.ToString(), ignoreCase: true);
            categoryRepository.Delete(categoryId.Value);
            var count = categoryRepository.RecordCount();
            Assert.True(0 == count);
        }



        /// <summary>
        /// 构造依赖注入容器，然后传入参数
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildServiceForSqlServer()
        {
            var services = new ServiceCollection();

            String basePath1 = AppContext.BaseDirectory;
            string path = basePath1.Substring(0, basePath1.IndexOf("bin", StringComparison.Ordinal));

            services.Configure<CodeGenerateOption>(options =>
            {
                options.ConnectionString = "Data Source=.;Initial Catalog=CzarCms;User ID=sa;Password=123456;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                options.DbType = DatabaseType.SqlServer.ToString();//数据库类型是SqlServer,其他数据类型参照枚举DatabaseType
                options.Author = "张玉龙";//作者名称
                options.OutputPath = path;//实体模板代码生成的路径
                options.IRepositoryOutputPath = path ;//仓储接口的输出路径
                options.RepositoryOutputPath= path ;//仓储实现的输出路径
                options.ModelsNamespace = "Core.CMS.Models";//实体命名空间
                options.IRepositoryNamespace = "Core.CMS.IRepository";//仓储接口命名空间
                options.RepositoryNamespace = "Core.CMS.Repository.SqlServer";//仓储命名空间
            });
            services.AddSingleton<CodeGenerator>();//注入Model代码生成器
            services.Configure<DbOpion>("CzarCms", GetConfiguration().GetSection("DbOpion"));
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            var sss=services.BuildServiceProvider();
            return services.BuildServiceProvider(); //构建服务提供程序
        }
        public IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }

        

    }
}
