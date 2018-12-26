/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：文章                                                    
*│　作    者：张玉龙                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2018-12-23 17:47:18                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Core.CMS.IRepository                                   
*│　接口名称： IArticleRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Core.CMS.Core.Repository;
using Core.CMS.Models;
using System;

namespace Core.CMS.IRepository
{
    public interface IArticleRepository : IBaseRepository<Article, Int32>
    {
    }
}