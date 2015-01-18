using CSharp.Wrapper.Angular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jwtTest.AppScript.Controllers
{
    public class GridOptions
    {
        public List<int> pagingPageSizes { get; set; }
        public int pagingPageSize { get; set; }
        public dynamic data { get; set; }
        public bool useExternalPaging { get; set; }

        public List<ColumnDef> columnDefs { get; set; }
        public GridRegisterApi onRegisterApi { get; set; }
        public int totalItems { get; set; }
        public bool enableSorting { get; set; }

    }

    public delegate void GridRegisterApi(GridApi gridApi);
    public delegate void PagingChangedCallback(int newPage, int pageSize);
    public class ColumnDef
    {
        public string name { get; set; }
        public string field { get; set; }
        public bool visible { get; set; }
        public string cellTemplate { get; set; }
        public int width { get; set; }
        public bool enableSorting { get; set; }
        public string cellFilter { get; set; }
        public string type { get; set; }
        
    }
    public class GridApi
    {
        public Paging paging { get; set; }
    }
    public class Paging
    {
        public PagingEvent on { get; set; }
    }
    public class PagingEvent
    {
        public void pagingChanged(IScope scope, PagingChangedCallback callback) { }
    }
    public delegate void EditActionHandler(dynamic item);
    public class GridActionExternalScope
    {
        public EditActionHandler EditAction { get; set; }
        public EditActionHandler RemoveAction { get; set; }
    }
}
