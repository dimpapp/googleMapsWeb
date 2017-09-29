using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace googleMapsWeb
{
    //Η σελίδα αποτελεί web service, καλείται μέσω jquery
    public partial class getAllTweets : System.Web.UI.Page
    {
      

        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Παίρνουμε αποτελέσματα σε datatable
                DataTable dt = data.getAllTweets();

                //Περνάμε μια μια τις εγγραφές και φτιάχνουμε το αρχείο xml
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                Response.Write("<root>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Response.Write("<loop>");
                    Response.Write("<lat><![CDATA[" + dt.Rows[i]["lat"].ToString() + "]]></lat>");
                    Response.Write("<lon><![CDATA[" + dt.Rows[i]["lon"].ToString() + "]]></lon>");
                    Response.Write("</loop>");
                }
                Response.Write("</root>");


            }//Αν κάπου χτυπήσει λάθος, δεν επιστρέφουμε τίποτα. Επιστρέφουμε τύπο εικόνα για να καταλάβει η jquery ότι υπάρχει λάθος
            catch (Exception ex)
            {
                Response.Clear();
                Response.ContentType = "image/png";
                throw new Exception("");
                return;
            }


        }
        #endregion
    }
}