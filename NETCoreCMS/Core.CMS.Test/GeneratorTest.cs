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
    /// ���Դ���������
    /// ��ʱֻʵ����SqlServer��ʵ���������
    /// </summary>
    public class GeneratorTest
    {
        [Fact]
        public void GeneratorModelForSqlServer()
        {
            var serviceProvider = BuildServiceForSqlServer(); //����ע��
            var codeGenerator = serviceProvider.GetRequiredService<CodeGenerator>();//�������л�ȡʵ��
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
                Title = "���",
                ParentId = 0,
                ClassList = "",
                ClassLayer = 0,
                Sort = 0,
                ImageUrl = "",
                SeoTitle = "��ʵ�SEOTitle",
                SeoKeywords = "��ʵ�SeoKeywords",
                SeoDescription = "��ʵ�SeoDescription",
                IsDeleted = false,
            };
            var categoryId = categoryRepository.Insert(category);
            var list = categoryRepository.GetList();
            Assert.True(1 == list.Count());
            Assert.Equal("���", list.FirstOrDefault().Title);
            Assert.Equal("SQLServer", DatabaseType.SqlServer.ToString(), ignoreCase: true);
            categoryRepository.Delete(categoryId.Value);
            var count = categoryRepository.RecordCount();
            Assert.True(0 == count);
        }



        /// <summary>
        /// ��������ע��������Ȼ�������
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
                options.DbType = DatabaseType.SqlServer.ToString();//���ݿ�������SqlServer,�����������Ͳ���ö��DatabaseType
                options.Author = "������";//��������
                options.OutputPath = path;//ʵ��ģ��������ɵ�·��
                options.IRepositoryOutputPath = path ;//�ִ��ӿڵ����·��
                options.RepositoryOutputPath= path ;//�ִ�ʵ�ֵ����·��
                options.ModelsNamespace = "Core.CMS.Models";//ʵ�������ռ�
                options.IRepositoryNamespace = "Core.CMS.IRepository";//�ִ��ӿ������ռ�
                options.RepositoryNamespace = "Core.CMS.Repository.SqlServer";//�ִ������ռ�
            });
            services.AddSingleton<CodeGenerator>();//ע��Model����������
            services.Configure<DbOpion>("CzarCms", GetConfiguration().GetSection("DbOpion"));
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            var sss=services.BuildServiceProvider();
            return services.BuildServiceProvider(); //���������ṩ����
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
