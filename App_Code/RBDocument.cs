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

    public static List<RBDocument> GetDocumentsWithFilter(int startNumber, int numberDocs, string reference, string sortColumn, string sortOrder)
    {
        var con = ConfigurationManager.ConnectionStrings["RBDocMigration"].ToString();

        List<RBDocument> documents = new List<RBDocument>();

        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string oString = "select * from Documents where Reference = @reference ORDER BY " + sortColumn + " " + sortOrder + " OFFSET " + startNumber + " ROWS FETCH NEXT " + numberDocs + " ROWS ONLY";
            SqlCommand oCmd = new SqlCommand(oString, myConnection);
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

    public static List<RBDocument> GetDocuments(int startNumber, int numberDocs, string sortColumn, string sortOrder)
    {
        var con = ConfigurationManager.ConnectionStrings["RBDocMigration"].ToString();

        List<RBDocument> documents = new List<RBDocument>();

        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string oString = "select * from Documents ORDER BY Reference OFFSET " + startNumber + " ROWS FETCH NEXT " + numberDocs + " ROWS ONLY";
            SqlCommand oCmd = new SqlCommand(oString, myConnection);
            myConnection.Open();

            try
            {
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
            } catch (Exception ex) { }
        }
        return documents;
    }

    public static int CountDocuments()
    {
        var con = ConfigurationManager.ConnectionStrings["RBDocMigration"].ToString();
        int countDocs = 0;

        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string oString = "select count(*) As 'CountDocs' from Documents";
            SqlCommand oCmd = new SqlCommand(oString, myConnection);
            myConnection.Open();

            try
            {
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        countDocs = int.Parse(oReader["CountDocs"].ToString());
                    }

                    myConnection.Close();
                }
            } catch (Exception ex) { }
        }
        return countDocs;
    }

    public static int CountDocumentsWithFilter(string reference)
    {
        var con = ConfigurationManager.ConnectionStrings["RBDocMigration"].ToString();
        int countDocs = 0;

        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string oString = "select count(*) As 'CountDocs' from Documents where Reference = @reference";
            SqlCommand oCmd = new SqlCommand(oString, myConnection);
            oCmd.Parameters.AddWithValue("@reference", reference);
            myConnection.Open();

            try
            {
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        countDocs = int.Parse(oReader["CountDocs"].ToString());
                    }

                    myConnection.Close();
                }
            }
            catch (Exception ex) { }
        }
        return countDocs;
    }
}