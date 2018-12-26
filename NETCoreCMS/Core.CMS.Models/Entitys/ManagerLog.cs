/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：操作日志                                                    
*│　作    者：张玉龙                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2018-12-23 17:47:18                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: Core.CMS.Models                                  
*│　类    名：ManagerLog                                     
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
	/// 操作日志
	/// </summary>
	[Table("ManagerLog")]
	public class ManagerLog
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public Int32 Id {get;set;}

		/// <summary>
		/// 操作类型
		/// </summary>
		public String ActionType {get;set;}

		/// <summary>
		/// 主键
		/// </summary>
		[Required]
		public Int32 AddManageId {get;set;}

		/// <summary>
		/// 操作人名称
		/// </summary>
		public String AddManagerNickName {get;set;}

		/// <summary>
		/// 操作时间
		/// </summary>
		[Required]
		public DateTime AddTime {get;set;}

		/// <summary>
		/// 操作IP
		/// </summary>
		public String AddIp {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		public String Remark {get;set;}


	}
}
