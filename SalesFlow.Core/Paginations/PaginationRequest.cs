using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Core.Paginations
{
    public class PaginationRequest
    {
        private const int MaxPageSize = 100;

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value switch
            {
                <= 0 => 10,
                > MaxPageSize => MaxPageSize,
                _ => value
            };
        }
    }
}