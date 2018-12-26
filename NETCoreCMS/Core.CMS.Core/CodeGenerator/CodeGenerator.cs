using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.CMS.Core.DbHelper;
using Core.CMS.Core.Extensions;
using Core.CMS.Core.Models;
using Core.CMS.Core.Options;
using Dapper;
using Microsoft.Extensions.Options;

namespace Core.CMS.Core.CodeGenerator
{
    /// <summary>
    /// zyl
    /// 2018.12.12
    /// 代码生成器。参考自：Zxw.Framework.NetCore
    /// <remarks>
    /// 根据数据库表以及表对应的列生成对应的数据库实体
    /// </remarks>
    /// </summary>
    public class CodeGenerator
    {
        private readonly string Delimiter = "\\";//分隔符，默认为windows下的\\分隔符

        private static CodeGenerateOption _options;
        public CodeGenerator(IOptions<CodeGenerateOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            _options = options.Value;
            if (_options.ConnectionString.IsNullOrWhiteSpace())
                throw new ArgumentNullException("不指定数据库连接串就生成代码，你想上天吗？");
            if (_options.DbType.IsNullOrWhiteSpace())
                throw new ArgumentNullException("不指定数据库类型就生成代码，你想逆天吗？");
            var path = AppDomain.CurrentDomain.BaseDirectory;//没有时默认的路径
            if (_options.OutputPath.IsNullOrWhiteSpace())
            {
                _options.OutputPath = path;
                _options.IRepositoryOutputPath = path;
                _options.RepositoryNamespace = path;
            }
            var flag = path.IndexOf("/bin");
            if (flag > 0)
                Delimiter = "/";//如果可以取到值，修改分割符
        }

        /// <summary>
        /// 根据数据库连接字符串生成数据库表对应的Model层代码
        /// </summary>
        /// <param name="isCoveredExsited">是否覆盖已存在的同名文件</param>
        public void GenerateModelCodesFromDatabase(bool isCoveredExsited = true)
        {
            //TODO 从数据库获取表列表以及生成实体对象

            DatabaseType dbType = ConnectionFactory.GetDataBaseType(_options.DbType);//使用数据库连接工厂
            List<DbTable> tables = new List<DbTable>();
            using (var dbConnection = ConnectionFactory.CreateConnection(dbType, _options.ConnectionString))
            {
                //获取完整数据库信息包含表和列的信息
                tables = dbConnection.GetCurrentDatabaseTableList(dbType);
            }
            if (tables != null && tables.Any())
            {
                foreach (var table in tables)
                {  
                    GenerateEntity(table, isCoveredExsited);//生成实体
                    if (table.Columns.Any(c => c.IsPrimaryKey))
                    {
                        var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                        GenerateIRepository(table, pkTypeName, isCoveredExsited);//生成仓储接口
                        GenerateRepository(table, pkTypeName, isCoveredExsited);//生成仓储
                    }
                }
            }

           
        }

        #region 生成数据库实体类型
        /// <summary>
        /// 生成数据库实体类型
        /// </summary>
        /// <param name="table"></param>
        /// <param name="isCoveredExsited"></param>
        private void GenerateEntity(DbTable table, bool isCoveredExsited = true)
        {
            var modelPath = _options.OutputPath+"Entitys";
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            var fullPath = modelPath + Delimiter + table.TableName + ".cs";
            if (File.Exists(fullPath) && !isCoveredExsited)
                return;

            var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
            var sb = new StringBuilder();
            foreach (var column in table.Columns)
            {
                var tmp = GenerateEntityProperty(column);
                sb.AppendLine(tmp);
            }
            var content = ReadTemplate("ModelTemplate.txt");
            content = content.Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ModelsNamespace}", _options.ModelsNamespace)
                .Replace("{Author}", _options.Author)
                .Replace("{Comment}", table.TableComment)
                .Replace("{ModelName}", table.TableName)
                .Replace("{ModelProperties}", sb.ToString());
            WriteAndSave(fullPath, content);
        }
        #endregion


        #region  生成IRepository层代码文件

        private void GenerateIRepository(DbTable table, string keyTypeName, bool ifExsitedCovered = true)
        {
            var iRepositoryPath = _options.IRepositoryOutputPath +"IRep"+Delimiter;
            if (!Directory.Exists(iRepositoryPath))
            {
                Directory.CreateDirectory(iRepositoryPath);
            }
            var fullPath = iRepositoryPath  + "I" + table.TableName + "Repository.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("IRepositoryTemplate.txt");//IRepository的模板
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{IRepositoryNamespace}", _options.IRepositoryNamespace)//仓储接口命名空间
                .Replace("{ModelName}", table.TableName)
                .Replace("{KeyTypeName}", keyTypeName);
            WriteAndSave(fullPath, content);//生成
        }
        #endregion

        #region 生成Repository层代码文件
        /// <summary>
        /// 生成Repository层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateRepository(DbTable table, string keyTypeName, bool ifExsitedCovered = true)
        {
            var repositoryPath = _options.RepositoryOutputPath+"Rep"+Delimiter;
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath  + table.TableName + "Repository.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("RepositoryTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{RepositoryNamespace}", _options.RepositoryNamespace)//Repository命名空间
                .Replace("{ModelName}", table.TableName)
                .Replace("{KeyTypeName}", keyTypeName);
            WriteAndSave(fullPath, content);
        }
        #endregion



        #region 生成实体属性
        /// <summary>
        /// 生成实体属性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列</param>
        /// <returns></returns>
        private static string GenerateEntityProperty(DbTableColumn column)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(column.Comment))
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + column.Comment);
                sb.AppendLine("\t\t/// </summary>");
            }
            if (column.IsPrimaryKey)
            {
                sb.AppendLine("\t\t[Key]");
                sb.AppendLine($"\t\tpublic {column.CSharpType} Id " + "{get;set;}");
            }
            else
            {
                if (!column.IsNullable)
                {
                    sb.AppendLine("\t\t[Required]");
                }
                var colType = column.CSharpType;
                if (colType.ToLower() != "string" && colType.ToLower() != "byte[]" && colType.ToLower() != "object" &&
                    column.IsNullable)
                {
                    colType = colType + "?";
                }

                sb.AppendLine($"\t\tpublic {colType} {column.ColName} " + "{get;set;}");
            }

            return sb.ToString();
        }
        #endregion

        
        #region 读模板和写文件
        /// <summary>
        /// 从代码模板中读取内容
        /// </summary>
        /// <param name="templateName">模板名称，应包括文件扩展名称。比如：template.txt</param>
        /// <returns></returns>
        private string ReadTemplate(string templateName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var content = string.Empty;
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.CodeTemplate.{templateName}"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        private static void WriteAndSave(string fileName, string content)
        {
            //实例化一个文件流--->与写入文件相关联
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                //实例化一个StreamWriter-->与fs相关联
                using (var sw = new StreamWriter(fs))
                {
                    //开始写入
                    sw.Write(content);
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
                }
            }
        }
        #endregion
       

    }
}