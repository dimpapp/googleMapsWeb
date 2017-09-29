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
    public partial class topHashAll : System.Web.UI.Page
    {
        //Δήλωση μεταβλητών
        #region data
        private int year = 2015;
        private int month = 1;
        private int day = 1;
        #endregion

        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Διαβάζουμε τις τιμές που στάλθηκαν από τη σχετική κλήση jquery
                if (Request["year"]!=null)
                {
                    year = Convert.ToInt16(Request["year"]);
                }
                if (Request["month"] != null)
                {
                    month = Convert.ToInt16(Request["month"]);
                }
                if (Request["day"] != null)
                {
                    day = Convert.ToInt16(Request["day"]);
                }
                //Παίρνουμε αποτελέσματα σε datatable
                DataTable dt = data.topHashAll(day,month,year);

                //Περνάμε μια μια τις εγγραφές και φτιάχνουμε το αρχείο xml
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                Response.Write("<root>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Response.Write("<loop>");
                    Response.Write("<city><![CDATA[" + dt.Rows[i]["city"].ToString() + "]]></city>");
                    Response.Write("<word><![CDATA[" + dt.Rows[i]["word"].ToString() + "]]></word>");
                    Response.Write("<sinolo><![CDATA[" + dt.Rows[i]["sinolo"].ToString() + "]]></sinolo>");
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