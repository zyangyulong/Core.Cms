﻿using System;

namespace Core.CMS.Core.Options
{
    /// <summary>
    /// 代码生成器配置选项
    /// </summary>
    public class CodeGenerateOption:DbOpion
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 代码生成时间
        /// </summary>
        public string GeneratorTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 实体输出路径
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// 仓储接口输出路径
        /// </summary>
        public string IRepositoryOutputPath { get; set; }

        /// <summary>
        /// 仓储实现输出路径
        /// </summary>
        public string RepositoryOutputPath { get; set; }

        /// <summary>
        /// 实体命名空间
        /// </summary>
        public string ModelsNamespace { get; set; }
        /// <summary>
        /// 仓储接口命名空间
        /// </summary>
        public string IRepositoryNamespace { get; set; }
        /// <summary>
        /// 仓储命名空间
        /// </summary>
        public string RepositoryNamespace { get; set; }
    }
}