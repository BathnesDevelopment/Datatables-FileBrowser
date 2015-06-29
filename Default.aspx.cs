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
            case "2":
                sortColumn = "DocumentLabelCode"; 
                break;
            case "3":
                sortColumn = "DocumentLabelGuid"; 
                break;
            case "4":
                sortColumn = "FileLocation"; 
                break;
            case "5":
                sortColumn = "FileReference"; 
                break;
            case "6":
                sortColumn = "Reference"; 
                break;
            case "7":
                sortColumn = "FileLink"; 
                break;
        }

        var records = GetRecordsFromDatabaseWithFilter(search, sortColumn, sortOrder, displayLength, displayStart);
        if (records == null)
        {
            return string.Empty;
        }

        var hasMoreRecords = false;

        var sb = new StringBuilder();
        sb.Append(@"{" + "\"sEcho\": " + echo + ",");
        sb.Append("\"recordsTotal\": " + GetTotalRecords(sortColumn, sortOrder) + ",");
        sb.Append("\"recordsFiltered\": " + GetTotalRecordsWithFilter(search, sortColumn, sortOrder) + ",");
        sb.Append("\"iTotalRecords\": " + records.Count() + ",");
        sb.Append("\"iTotalDisplayRecords\": " + records.Count() + ",");
        sb.Append("\"aaData\": [");

        foreach (var result in records)
        {
            if (hasMoreRecords)
            {
                sb.Append(",");
            }

            sb.Append("[");
            sb.Append("\"" + result.CreatedDate + "\",");
            sb.Append("\"" + result.DocumentLabel + "\",");
            sb.Append("\"" + result.DocumentLabelCode + "\",");
            sb.Append("\"" + result.DocumentLabelGuid + "\",");
            sb.Append("\"" + result.FileLocation + "\",");
            sb.Append("\"" + result.FileReference + "\",");
            sb.Append("\"" + result.Reference + "\",");
            sb.Append("\"<img class='image-details' src='images/details-icon.png' runat='server' height='16' width='16' alt='View Details'/>\"");
            sb.Append("]");
            hasMoreRecords = true;
        }
        sb.Append("]}");
        return sb.ToString();
    }


    private static int GetTotalRecordsWithFilter(string search, string sortColumn, string sortOrder)
    {
        return 10;
    }

    private static int GetTotalRecords(string sortColumn, string sortOrder)
    {
        return 10;
    }

    private static IEnumerable<RBDocument> GetRecordsFromDatabaseWithFilter(string search, string sortColumn, string sortOrder, int displayLength, int displayStart)
    {
        // At this point you can call to your database to get the data
        // but I will just populate a sample collection in code
        var records = new List<RBDocument>
            {
                new RBDocument
                {
                    CreatedDate = DateTime.Now,
                    DocumentLabel = "Label",
                    DocumentLabelCode = "Code",
                    DocumentLabelGuid = "Guid",
                    FileLocation = "Location",
                    FileReference = "FileRef",
                    Reference = "123456"
                },
                new RBDocument
                {
                    CreatedDate = DateTime.Now,
                    DocumentLabel = "Label",
                    DocumentLabelCode = "Code",
                    DocumentLabelGuid = "Guid",
                    FileLocation = "Location",
                    FileReference = "FileRef",
                    Reference = "7648747"
                },
                new RBDocument
                {
                    CreatedDate = DateTime.Now,
                    DocumentLabel = "Label",
                    DocumentLabelCode = "Code",
                    DocumentLabelGuid = "Guid",
                    FileLocation = "Location",
                    FileReference = "FileRef",
                    Reference = "45624626"
                }
            };

        var orderedResults = sortOrder == "asc"
                         ? records.OrderBy(o => o.CreatedDate)
                         : records.OrderByDescending(o => o.CreatedDate);

        var itemsToSkip = displayStart == 0
                          ? 0
                          : displayStart + 1;

        var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();

        return pagedResults;
    }
}