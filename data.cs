using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace googleMapsWeb
{

    //ενδιάμεσος κλήσεων δεδομένων από τη βάση δεδομένων, καλούνται από webservices ή κατα το φόρτωμα της σελίδας (λίστα με hashtags και λίστα με tweets με τα περισσότερα retweets)
    public class data
    {
        //connection string με βάση δεδομένων
        public static string constr = @"server=OWNEROR-L0I8K9M\SQLEXPRESS;database=twitter;uid=sa;pwd=4321asdfASDF;";

        //επιστρέφει λίστα με tweets που έχουν τα περισσότερα retweets
        public static DataView getTopRetweets()
        {
            //δημιουργία connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων
                SqlCommand com = new SqlCommand("getStartingListRetweet", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure;//πρόκειται για stored procedure
                com.CommandTimeout = 10000;//πότε να κάνει timeout η κλήση

                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"].DefaultView;//επιστρέφουμε αποτέλεσμα σε μορφή dataview για bind σε listbox
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();// κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();// κλείσιμο σύνδεσης με βάση δεδομένων
        }

        //επιστρέφει λίστα με τα hashtags με περισσότερα tweets
        public static DataView getTopHashTags()
        {
            //δημιουργούμε connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων 
                SqlCommand com = new SqlCommand("getStartingList", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure;//πρόκειται για stored procedure
                com.CommandTimeout = 10000;//πότε να κάνει timeout η κλήση
                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"].DefaultView;//επιστρέφουμε αποτέλεσμα σε μορφή dataview για bind σε listbox
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }

        //επιστρέφει στοιχεία με τα tweets που έχουν τα περισσότερα retweets, όπως αρχική-τελική ημερομηνία, σύνολο tweets
        public static DataTable getStartDataReTweet(string hashcode)
        {
            //δημιουργούμε connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων
                SqlCommand com = new SqlCommand("getStartInfoReTweet", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure; //πρόκειται για stored procedure
                com.CommandTimeout = 900000;//πότε να κάνει timeout η κλήση

                //Ως παράμετρο εισόδου το hashcode tweet
                SqlParameter g = com.Parameters.Add(new SqlParameter("@hashcode", SqlDbType.VarChar, 1000000000));
                g.Value = hashcode;

                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"];//επιστρέφουμε αποτελέσματα σε μορφή datatable για προσπέλαση εγγραφών και φτιάξιμο αρχείου xml για jquery
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }

        //επιστρέφει στοιχεία σχετικά με hashtags με τα περισσότερα retweets, όπως αρχική-τελική ημερομηνία, σύνολο tweets
        public static DataTable getStartData(int hashtag)
        {
            //δημιουργία connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων

                SqlCommand com = new SqlCommand("getStartInfo", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure; //πρόκειται για stored procedure
                com.CommandTimeout = 900000;//πότε να κάνει timeout η κλήση

                //Ως παράμετρο εισόδου τον κωδικό (αύξων αριθμό) hashtag
                SqlParameter g = com.Parameters.Add(new SqlParameter("@hashtag", SqlDbType.BigInt));
                g.Value = hashtag;

                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"];//επιστρέφουμε αποτελέσματα σε μορφή datatable για προσπέλαση εγγραφών και φτιάξιμο αρχείου xml για jquery
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }

        //επιστρέφει όλα τα tweets
        public static DataTable getAllTweets()
        {
            //δημιουργία connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων

                SqlCommand com = new SqlCommand("getAllTweets", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure; //πρόκειται για stored procedure
                com.CommandTimeout = 900000;//πότε να κάνει timeout η κλήση

                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"];//επιστρέφουμε αποτελέσματα σε μορφή datatable για προσπέλαση εγγραφών και φτιάξιμο αρχείου xml για jquery
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }

        //επιστρέφει words με περισσότερα tweets για συγκεκριμένη μέρα, μήνα, χρόνο. Αν δεν οριστεί μέρα, μήνας, χρόνος επιστρέφει για όλες τις μέρες
        public static DataTable topWordsAll(int day, int month, int year)
        {
            //δημιουργία connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων

                SqlCommand com = new SqlCommand("topWordsAll", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure; //πρόκειται για stored procedure
                com.CommandTimeout = 900000;//πότε να κάνει timeout η κλήση

                //ως παράμετροι εισόδου μέρα, μήνας, χρόνος 
                SqlParameter g1 = com.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt));
                g1.Value = year;

                SqlParameter g2 = com.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt));
                g2.Value = month;

                SqlParameter g3 = com.Parameters.Add(new SqlParameter("@day", SqlDbType.BigInt));
                g3.Value = day;


                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"];//επιστρέφουμε αποτελέσματα σε μορφή datatable για προσπέλαση εγγραφών και φτιάξιμο αρχείου xml για jquery
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }

        //επιστρέφει hashtags με περισσότερα tweets για συγκεκριμένη μέρα, μήνα, χρόνο. Αν δεν οριστεί μέρα, μήνας, χρόνος επιστρέφει για όλες τις μέρες
        public static DataTable topHashAll(int day, int month, int year)
        {
            //δημιουργία connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων

                SqlCommand com = new SqlCommand("topHashAll", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure; //πρόκειται για stored procedure
                com.CommandTimeout = 900000;//πότε να κάνει timeout η κλήση

                //ως παράμετροι εισόδου μέρα, μήνας και χρόνος
                SqlParameter g1 = com.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt));
                g1.Value = year;

                SqlParameter g2 = com.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt));
                g2.Value = month;

                SqlParameter g3 = com.Parameters.Add(new SqlParameter("@day", SqlDbType.BigInt));
                g3.Value = day;

                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"];//επιστρέφουμε αποτελέσματα σε μορφή datatable για προσπέλαση εγγραφών και φτιάξιμο αρχείου xml για jquery
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }

        //επιστρέφει tweets με συγκεκριμένο hashtag για συγκεκριμένη μέρα
        public static DataTable getAllByHashtag(int hashtag,int seira, DateTime imera)
        {
            //δημιουργία connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων

                SqlCommand com = new SqlCommand("getAllByHashtag", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure; //πρόκειται για stored procedure
                com.CommandTimeout = 900000;//πότε να κάνει timeout η κλήση

                //ως παράμετροι εισόδου ο κωδικός hashtag και ημέρα
                SqlParameter g = com.Parameters.Add(new SqlParameter("@hashtag", SqlDbType.BigInt));
                g.Value = hashtag;                

                SqlParameter g2 = com.Parameters.Add(new SqlParameter("@imera", SqlDbType.DateTime));
                g2.Value = imera;

                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"];//επιστρέφουμε αποτελέσματα σε μορφή datatable για προσπέλαση εγγραφών και φτιάξιμο αρχείου xml για jquery

            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }

        //επιστρέφει retweets συγκεκριμένου tweet για  συγκεκριμένη μέρα
        public static DataTable getAllByHashtagTweet(string hashtag, int seira, DateTime imera)
        {
            //δημιουργία connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων

                SqlCommand com = new SqlCommand("getAllByHashtagTweet", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure; //πρόκειται για stored procedure
                com.CommandTimeout = 900000;//πότε να κάνει timeout η κλήση

                //ως παράμετροι εισόδου hashcode αντίστοιχου tweet και ημέρα
                SqlParameter g = com.Parameters.Add(new SqlParameter("@hashcode", SqlDbType.VarChar,10000000));
                g.Value = hashtag;
                
                SqlParameter g2 = com.Parameters.Add(new SqlParameter("@imera", SqlDbType.DateTime));
                g2.Value = imera;

                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"];//επιστρέφουμε αποτελέσματα σε μορφή datatable για προσπέλαση εγγραφών και φτιάξιμο αρχείου xml για jquery
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }


        //επιστρέφει hashtag ανα πόλη για συγκεκριμένη ημερομηνία
        public static DataTable getAllByHashtagByState(int hashtag, int seira, DateTime imera)
        {
            //δημιουργία connection με βάση δεδομένων
            SqlConnection con = new SqlConnection(constr);
            try
            {
                con.Open();//άνοιγμα σύνδεσης με βάση δεδομένων

                SqlCommand com = new SqlCommand("getAllByHashtagByState", con);//όνομα stored procudure που θα κληθεί
                com.CommandType = CommandType.StoredProcedure; //πρόκειται για stored procedure
                com.CommandTimeout = 900000;//πότε να κάνει timeout η κλήση

                //Ως παράμετροι εισόδου ο κωδικός hashtag, ημέρα και σειρά εμφάνισης (το τελευταίο δε χρησιμοποιείται άμεσα)
                SqlParameter g = com.Parameters.Add(new SqlParameter("@hashtag", SqlDbType.BigInt));
                g.Value = hashtag;

                SqlParameter g1 = com.Parameters.Add(new SqlParameter("@seira", SqlDbType.BigInt));
                g1.Value = seira;

                SqlParameter g2 = com.Parameters.Add(new SqlParameter("@imera", SqlDbType.DateTime));
                g2.Value = imera;

                SqlDataAdapter adap = new SqlDataAdapter(com);//δημιουργούμε αντικείμενο sqldataadapter

                DataSet ds = new DataSet();//ορίζουμε κενό dataset
                adap.Fill(ds, "table");//τα αποτελέσματα του stored procedure αποθηκεύονται στο dataset που μόλις δημιουργήσαμε

                return ds.Tables["table"];//επιστρέφουμε αποτελέσματα σε μορφή datatable για προσπέλαση εγγραφών και φτιάξιμο αρχείου xml για jquery
            }
            catch (Exception ex)//αν υπάρχει λάθος ... αποθηκεύουμε το error που χτήπησε στη βάση δεδομένων για debugging με χρήση αντικειμένου error
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
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
                return null;
            }
            finally
            {
                if (con != null) con.Close();//κλείσιμο σύνδεσης με βάση δεδομένων
            }
        }
    }
}