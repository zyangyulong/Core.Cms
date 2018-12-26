namespace Core.CMS.Core.Options
{
    /// <summary>
    /// 配置基类
    /// </summary>
    public class DbOpion
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }
    }
}