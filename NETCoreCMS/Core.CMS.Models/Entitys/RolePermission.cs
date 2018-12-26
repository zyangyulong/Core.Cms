/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限表                                                    
*│　作    者：张玉龙                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2018-12-23 17:47:18                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: Core.CMS.Models                                  
*│　类    名：RolePermission                                     
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
	/// 角色权限表
	/// </summary>
	[Table("RolePermission")]
	public class RolePermission
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Int32 Id {get;set;}

		/// <summary>
		/// 角色主键
		/// </summary>
		[Required]
		public Int32 RoleId {get;set;}

		/// <summary>
		/// 菜单主键
		/// </summary>
		[Required]
		public Int32 MenuId {get;set;}

		/// <summary>
		/// 操作类型（功能权限）
		/// </summary>
		public String Permission {get;set;}


	}
}
