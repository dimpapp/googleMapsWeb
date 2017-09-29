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
    public partial class getStartDataRetweet : System.Web.UI.Page
    {
        //Δήλωση μεταβλητών
        #region data
        string hashtag;
        #endregion

        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Διαβάζουμε τις τιμές που στάλθηκαν από τη σχετική κλήση jquery
                if (Request["hashtag"] != null)
                {
                    try
                    {
                        hashtag = Request["hashtag"];
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("");
                    }

                }
                else
                {
                    throw new Exception("");
                }

                //Παίρνουμε αποτελέσματα σε datatable
                DataTable dt = data.getStartDataReTweet(hashtag);

                //Φτιάχνουμε το αρχείο xml για τη συνάρτηση jquery
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                Response.Write("<root>");
                Response.Write("<mini><![CDATA[" + dt.Rows[0]["mini"].ToString() + "]]></mini>");
                Response.Write("<maxi><![CDATA[" + dt.Rows[0]["maxi"].ToString() + "]]></maxi>");
                Response.Write("<lat><![CDATA[" + dt.Rows[0]["lat"].ToString() + "]]></lat>");
                Response.Write("<lon><![CDATA[" + dt.Rows[0]["lon"].ToString() + "]]></lon>");
                Response.Write("<arithmos><![CDATA[" + dt.Rows[0]["arithmos"].ToString() + "]]></arithmos>");
                Response.Write("<day1_1><![CDATA[" + Convert.ToDateTime(dt.Rows[0]["mini"]).Day + "]]></day1_1>");
                Response.Write("<day1_2><![CDATA[" + Convert.ToDateTime(dt.Rows[0]["mini"]).Month + "]]></day1_2>");
                Response.Write("<day1_3><![CDATA[" + Convert.ToDateTime(dt.Rows[0]["mini"]).Year + "]]></day1_3>");
                Response.Write("<day2_1><![CDATA[" + Convert.ToDateTime(dt.Rows[0]["maxi"]).Day + "]]></day2_1>");
                Response.Write("<day2_2><![CDATA[" + Convert.ToDateTime(dt.Rows[0]["maxi"]).Month + "]]></day2_2>");
                Response.Write("<day2_3><![CDATA[" + Convert.ToDateTime(dt.Rows[0]["maxi"]).Year + "]]></day2_3>");
                Response.Write("</root>");

            }//Αν κάπου χτυπήσει λάθος, δεν επιστρέφουμε τίποτα. Επιστρέφουμε τύπο εικόνα για να καταλάβει η jquery ότι υπάρχει λάθος
            catch (Exception ex)
            {
                #region Serialize Error
                string message = "";
                string innerException = "";
                string source = "";
                string helpLink = "";
                string stack = "";
                if (ex.StackTrace != null) stack = ex.StackTrace;
                if (ex.Message != null) message = ex.Message;
                if (ex.InnerException != null) innerException = ex.InnerException.ToString(); ;
                if (ex.Source != null) source = ex.Source;
                if (ex.HelpLink != null) helpLink = ex.HelpLink;
                error er = new error(message, innerException, source, helpLink, stack);
                er.save();
                #endregion
                Response.Clear();
                Response.ContentType = "image/png";
                throw new Exception("");
                return;
            }


        }
        #endregion
    }
}