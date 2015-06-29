using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RBDocument
/// </summary>
public class RBDocument
{
    public RBDocument() { }

    public string Reference { get; set; }
    public DateTime CreatedDate { get; set; }
    public string FileReference { get; set; }
    public string FileLocation { get; set; }
    public string DocumentLabelCode { get; set; }
    public string DocumentLabel { get; set; }
    public string DocumentLabelGuid { get; set; }

    public List<RBDocument> GetDocumentsWithFilter(string functionalArea, int numberDocs, string reference)
    {
        var con = ConfigurationManager.ConnectionStrings["RBDocMigration"].ToString();

        List<RBDocument> documents = new List<RBDocument>();

        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string oString = "select top " + numberDocs.ToString() + " from Documents where FunctionalArea = @fArea and Reference = @reference";
            SqlCommand oCmd = new SqlCommand(oString, myConnection);
            oCmd.Parameters.AddWithValue("@fArea", functionalArea);
            oCmd.Parameters.AddWithValue("@reference", reference);
            myConnection.Open();

            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    RBDocument doc = new RBDocument();
                    doc.CreatedDate = DateTime.Parse(oReader["CreatedDate"].ToString());
                    doc.DocumentLabel = oReader["DocumentLabel"].ToString();
                    doc.DocumentLabelCode = oReader["DocumentLabelCode"].ToString();
                    doc.DocumentLabelGuid = oReader["DocumentLabelGuid"].ToString();
                    doc.FileLocation = oReader["FileLocation"].ToString();
                    doc.FileReference = oReader["FileReference"].ToString();
                    doc.Reference = oReader["Reference"].ToString();

                    documents.Add(doc);
                }

                myConnection.Close();
            }
        }
        return documents;
    }
}