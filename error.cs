using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;


namespace googleMapsWeb
{
    //βοηθητική κλάση λαθών που τυχόν χτυπάνε στον κώδικα. Σώζωνται στη βάση δεδομένων για debugging
    public class error
    {

        #region Data
        //πεδία error
        private int _aa;
        string _message = "";
        string _innerException = "";
        string _source = "";
        string _helpLink = "";
        string _stack = "";

        public string _Stack
        {
            get
            {
                return _stack;
            }
            set
            {
                _stack = value;
            }
        }
        public string _HelpLink
        {
            get
            {
                return _helpLink;
            }
            set
            {
                _helpLink = value;
            }
        }

        public string _Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }

        public string _InnerException
        {
            get
            {
                return _innerException;
            }
            set
            {
                _innerException = value;
            }
        }

        public string _Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

       

        #endregion

        #region Constructor - Destructor
        //αρχικοποίηση αντικειμένου 
        public error(string message,string innerException,string source,string helplink,string stack)
        {
            _message  = message ;
            _innerException = innerException;
            _source = source ;
            _helpLink = helplink;
            _stack = stack;
        }

        public error()
        {
        }


        public error(int aa)
        {
            _aa = aa;
        }

        
        #endregion

        #region Use Case
       //αποθήκευση error στη βάση
        public int save()
        {
            SqlConnection con = new SqlConnection(data.constr);
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("adminNewError", con);
                com.CommandType = CommandType.StoredProcedure;com.CommandTimeout = 900000;

                SqlParameter titlosT = com.Parameters.Add("@message", SqlDbType.VarChar, 8000);
                titlosT.Value = _message ;

                SqlParameter apantisi1 = com.Parameters.Add("@innerException", SqlDbType.VarChar, 8000);
                apantisi1.Value = _innerException ;
                SqlParameter apantisi2 = com.Parameters.Add("@source", SqlDbType.VarChar, 8000);
                apantisi2.Value = _source ;
                SqlParameter apantisi3 = com.Parameters.Add("@helpLink", SqlDbType.VarChar, 8000);
                apantisi3.Value = _helpLink;
                SqlParameter apantisi4 = com.Parameters.Add("@stack", SqlDbType.VarChar, 8000);
                apantisi4.Value = _stack ;

                
                com.ExecuteNonQuery();
                return 1;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;

            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        //διαγραφή error από τη βάση - όχι άμεση χρήση κάπου στον κώδικα
        public int delete()
        {
            SqlConnection con = new SqlConnection(data.constr);
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand("admindelerror", con);
                com.CommandType = CommandType.StoredProcedure;com.CommandTimeout = 900000;


                SqlParameter aa = com.Parameters.Add("@aa", SqlDbType.Int);
                aa.Value = Convert.ToInt32(_aa);

                int sinolo = (int)com.ExecuteNonQuery();
                return sinolo;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;


            }
            finally
            {
                if (con != null) con.Close();
            }

        }
        #endregion

        
    }
}
