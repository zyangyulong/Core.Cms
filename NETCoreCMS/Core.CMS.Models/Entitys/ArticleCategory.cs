/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：文章分类                                                    
*│　作    者：张玉龙                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2018-12-23 17:47:18                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: Core.CMS.Models                                  
*│　类    名：ArticleCategory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.CMS.Models
{
	/// <summary>
	/// 张玉龙
	/// 2018-12-23 17:47:18
	/// 文章分类
	/// </summary>
	[Table("ArticleCategory")]
	public class ArticleCategory
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Int32 Id {get;set;}

		/// <summary>
		/// 分类标题
		/// </summary>
		[Required]
		public String Title {get;set;}

		/// <summary>
		/// 父分类ID
		/// </summary>
		[Required]
		public Int32 ParentId {get;set;}

		/// <summary>
		/// 类别ID列表(逗号分隔开)
		/// </summary>
		public String ClassList {get;set;}

		/// <summary>
		/// 类别深度
		/// </summary>
		public Int32? ClassLayer {get;set;}

		/// <summary>
		/// 排序
		/// </summary>
		[Required]
		public Int32 Sort {get;set;}

		/// <summary>
		/// 分类图标
		/// </summary>
		public String ImageUrl {get;set;}

		/// <summary>
		/// 分类SEO标题
		/// </summary>
		public String SeoTitle {get;set;}

		/// <summary>
		/// 分类SEO关键字
		/// </summary>
		public String SeoKeywords {get;set;}

		/// <summary>
		/// 分类SEO描述
		/// </summary>
		public String SeoDescription {get;set;}

		/// <summary>
		/// 是否删除
		/// </summary>
		[Required]
		public Boolean IsDeleted {get;set;}


	}
}
