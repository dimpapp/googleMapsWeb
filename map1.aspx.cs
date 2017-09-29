using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace googleMapsWeb
{
    public partial class map1 : System.Web.UI.Page
    {
        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //φορτώνει στο listbox τα hashtags με περισσότερες εμφανίσεις
            DataView dv = data.getTopHashTags();//κλήση μεθόδου getTopHashTags από αντικείμενο data
            hashtags.DataSource = dv;//ως datasource το dataview που επέστρεψε η μέθοδος getTopHashTags
            hashtags.DataTextField = "toShow";//πεδίο εμφάνισης
            hashtags.DataValueField = "hashAA";//πεδίο value listbox
            hashtags.DataBind();//bind to control

            //fortonei sto listbox ta tweets me perissotera retweets
            dv = data.getTopRetweets();//κλήση μεθόδου getTopRetweets από αντικείμενο data
            retweets.DataSource = dv;//ως datasource τ dataview που επέστρεψε η μέθοδος getTopRetweets
            retweets.DataTextField = "toShow";//πεδίο εμφάνισης
            retweets.DataValueField = "hashAA";//πεδίο value listbox
            retweets.DataBind();//bind to control
        }
        #endregion

    }
}