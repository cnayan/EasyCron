using System;
using System.Collections.Generic;

namespace EasyCron.UI.Models
{
    public class PagedResult<T> where T : class, new()
    {
        /// <summary>
        /// 当前页记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "success";

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> List { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                return PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount * 1.00M / PageSize);
            }
        }
    }
}
