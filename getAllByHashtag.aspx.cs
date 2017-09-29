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
    public partial class getAllByHashtag : System.Web.UI.Page
    {
        //Δήλωση μεταβλητών
        #region data
        string hashtag;
        DateTime imera;
        int seira;
        #endregion

        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Διαβάζουμε τις τιμές που στάλθηκαν από τη σχετική κλήση jquery
                if (Request.QueryString["hashtag"] != null)
                {
                    try
                    {
                        hashtag = Request.QueryString["hashtag"];
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

                if (Request.QueryString["seira"] != null)
                {
                    try
                    {
                        seira = Convert.ToInt32(Request.QueryString["seira"]);
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

                if (Request.QueryString["year"] != null)
                {
                    try
                    {
                        imera = new DateTime (Convert.ToInt32(Request.QueryString["year"]), Convert.ToInt32(Request.QueryString["month"]), Convert.ToInt32(Request.QueryString["day"]));
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
                DataTable dt = data.getAllByHashtag(Convert.ToInt32(hashtag),seira,imera);


                //Περνάμε μια μια τις εγγραφές και φτιάχνουμε το αρχείο xml
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                Response.Write("<root>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Response.Write("<loop>");
                    Response.Write("<imerominia><![CDATA[" + dt.Rows[i]["imerominia"].ToString() + "]]></imerominia>");
                    Response.Write("<lat><![CDATA[" + dt.Rows[i]["lat"].ToString() + "]]></lat>");
                    Response.Write("<lon><![CDATA[" + dt.Rows[i]["lon"].ToString() + "]]></lon>");
                    Response.Write("<fatherlLat><![CDATA[" + dt.Rows[i]["fatherlLat"].ToString() + "]]></fatherlLat>");
                    Response.Write("<fatherlLon><![CDATA[" + dt.Rows[i]["fatherlLon"].ToString() + "]]></fatherlLon>");
                    Response.Write("<drawLine><![CDATA[" + dt.Rows[i]["drawLine"].ToString() + "]]></drawLine>");
                    Response.Write("<noumero><![CDATA[" + dt.Rows[i]["lon"].ToString() + "]]></noumero>");
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