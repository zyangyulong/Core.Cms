/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限表接口实现                                                    
*│　作    者：张玉龙                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2018-12-23 17:47:18                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Core.CMS.Repository.SqlServer                                  
*│　类    名： RolePermissionRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Core.CMS.Core.DbHelper;
using Core.CMS.Core.Options;
using Core.CMS.Core.Repository;
using Core.CMS.IRepository;
using Core.CMS.Models;
using Microsoft.Extensions.Options;
using System;

namespace Core.CMS.Repository.SqlServer
{
    public class RolePermissionRepository:BaseRepository<RolePermission,Int32>, IRolePermissionRepository
    {
        public RolePermissionRepository(IOptionsSnapshot<DbOpion> options)
        {
            _dbOpion =options.Get("CzarCms");
            if (_dbOpion == null)
            {
                throw new ArgumentNullException(nameof(DbOpion));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOpion.DbType, _dbOpion.ConnectionString);
        }

    }
}