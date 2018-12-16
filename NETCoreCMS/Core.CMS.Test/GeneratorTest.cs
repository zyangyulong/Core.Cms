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
    /// ���Դ���������
    /// ��ʱֻʵ����SqlServer��ʵ���������
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
        /// ��������ע��������Ȼ�������
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildServiceForSqlServer()
        {
            var services = new ServiceCollection();

            services.Configure<CodeGenerateOption>(options =>
            {
                options.ConnectionString = "Data Source=.;Initial Catalog=CzarCms;User ID=sa;Password=123456;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                options.DbType = DatabaseType.SqlServer.ToString();//���ݿ�������SqlServer,�����������Ͳ���ö��DatabaseType
                options.Author = "������";//��������
                options.OutputPath = @"E:\Git�ֿ⼯��\VSCMS\Core.Cms\NETCoreCMS\Core.CMS.Models";//ʵ��ģ�����·����Ϊ����Ĭ��Ϊ��ǰ�������е�·��
                options.ModelsNamespace = "Core.CMS.Models";//ʵ�������ռ�
            });
            services.AddSingleton<CodeGenerator>();//ע��Model����������
            return services.BuildServiceProvider(); //���������ṩ����
        }
    }
}
