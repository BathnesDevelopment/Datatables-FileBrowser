using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static string GetTableData()
    {
        var echo = int.Parse(HttpContext.Current.Request.Params["sEcho"]);
        var displayLength = int.Parse(HttpContext.Current.Request.Params["iDisplayLength"]);
        var displayStart = int.Parse(HttpContext.Current.Request.Params["iDisplayStart"]);
        var sortColumn = HttpContext.Current.Request.Params["iSortCol_0"].ToString(CultureInfo.CurrentCulture);
        var sortOrder = HttpContext.Current.Request.Params["sSortDir_0"].ToString(CultureInfo.CurrentCulture);
        var search = HttpContext.Current.Request.Params["sSearch"];

        switch (sortColumn) {
            case "0":
                sortColumn = "CreatedDate"; 
                break;
            case "1":
                sortColumn = "DocumentLabel"; 
                break;
            case "3":
                sortColumn = "FileLocation"; 
                break;
            case "4":
                sortColumn = "Reference"; 
                break;
        }

        var records = GetRecordsFromDatabaseWithFilter(search, sortColumn, sortOrder, displayLength, displayStart);
        if (records == null)
        {
            return string.Empty;
        }

        var hasMoreRecords = false;
        var totalRecords = GetTotalRecords();

        var sb = new StringBuilder();
        sb.Append(@"{" + "\"sEcho\": " + echo + ",");
        sb.Append("\"recordsTotal\": " + totalRecords + ",");

        if (search != null)
        {
            var totalFiltered = GetTotalRecordsWithFilter(search);
            sb.Append("\"recordsFiltered\": " + totalFiltered + ",");
            sb.Append("\"iTotalRecords\": " + totalRecords + ",");
            sb.Append("\"iTotalDisplayRecords\": " + totalFiltered + ",");
        }
        else
        {
            sb.Append("\"recordsFiltered\": " + totalRecords + ",");
            sb.Append("\"iTotalRecords\": " + totalRecords + ",");
            sb.Append("\"iTotalDisplayRecords\": " + totalRecords + ",");
        }

        sb.Append("\"aaData\": [");

        foreach (var result in records)
        {
            if (hasMoreRecords)
            {
                sb.Append(",");
            }

            sb.Append("[");
            sb.Append("\"" + result.CreatedDate.ToString("dd/MM/yyyy") + "\",");
            sb.Append("\"" + result.DocumentLabel + "\",");
            //sb.Append("\"" + result.DocumentLabelCode + "\",");
            //sb.Append("\"" + result.DocumentLabelGuid + "\",");
            sb.Append("\"<a href='" + result.FileLocation.Replace("T:","file://VM-MS-SPT-1B/t").Replace("\\","/") + "'>" + result.FileReference + "</a>\",");
            sb.Append("\"" + result.Reference + "\",");
            sb.Append("\"" + result.FunctionalArea + "\"");
            sb.Append("]");
            hasMoreRecords = true;
        }
        sb.Append("]}");
        return sb.ToString();
    }

    private static int GetTotalRecordsWithFilter(string search)
    {
        return RBDocument.CountDocumentsWithFilter(search);
    }

    private static int GetTotalRecords()
    {
        return RBDocument.CountDocuments();
    }

    private static IEnumerable<RBDocument> GetRecordsFromDatabaseWithFilter(string search, string sortColumn, string sortOrder, int displayLength, int displayStart)
    {
        // At this point you can call to your database to get the data
        // but I will just populate a sample collection in code
        var records = new List<RBDocument>();

        if (search != null)
        {
            records = RBDocument.GetDocumentsWithFilter(displayStart, displayLength, search, sortColumn, sortOrder);
        }
        else
        {
            records = RBDocument.GetDocuments(displayStart, displayLength, sortColumn, sortOrder);
        }

        return records;
    }
}