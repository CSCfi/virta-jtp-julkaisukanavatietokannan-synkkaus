using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace JulkaisukanavatietokannanSynkkaus
{
    class Tietokantaoperaatiot
    {

        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Tarkistetaan löytyykö parametrina annettu Jufo_ID jo Virran julkaisukanavatietokannasta
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool kanavaLoytyyVirrasta(string server, string jufoID, string channelID)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand();

            if (((jufoID == null) || jufoID.Equals("")) && ((channelID == null) || channelID.Equals("")))
            {
                cmd.CommandText = "SELECT COUNT(*) FROM dbo.Julkaisukanavatietokanta WHERE Jufo_ID = '98765";   // dummy value
            }
            else if (((jufoID == null) || jufoID.Equals("")) && (channelID != null) && !(channelID.Equals(""))) 
            {
                cmd.CommandText = "SELECT COUNT(*) FROM dbo.Julkaisukanavatietokanta WHERE Jufo_ID = @Channel_ID OR Channel_ID = @Channel_ID";
            }
            else if (((channelID == null) || channelID.Equals("")) && (jufoID != null) && !(jufoID.Equals("")))
            {
                cmd.CommandText = "SELECT COUNT(*) FROM dbo.Julkaisukanavatietokanta WHERE Jufo_ID = @Jufo_ID OR Channel_ID = @Jufo_ID";
            }
            else if ((jufoID != null) && !(jufoID.Equals("")) && (channelID != null) && !(channelID.Equals("")))
            {
                cmd.CommandText = "SELECT COUNT(*) FROM dbo.Julkaisukanavatietokanta WHERE Jufo_ID = @Jufo_ID OR Jufo_ID = @Channel_ID OR Channel_ID = @Jufo_ID OR Channel_ID = @Channel_ID";
            }         

            if (String.IsNullOrEmpty(jufoID))
            {
                cmd.Parameters.AddWithValue("@Jufo_ID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Jufo_ID", jufoID);
            }

            if (String.IsNullOrEmpty(channelID))
            {
                cmd.Parameters.AddWithValue("@Channel_ID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Channel_ID", channelID);
            }


            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            int maara = (int)cmd.ExecuteScalar();

            if (maara > 0)
            {

                conn.Close();
                return true;
            }

            conn.Close();
            return false;

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Lisataan julkaisut_mds.dbo.Julkaisukanavatietokanta -tauluun uusi rivi, joille annetaan parametrien arvot
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void insert_Julkaisukanavatietokanta(string server, string channelJufoID, string channelChannelID, string channelJufoLuokka, string channelName, string channelOtherTitle, string channelPublisher, string channelType, string channelISSNL, string channelISSN1, string channelISSN2, string channelISBN, string channelActive, string channelJufoHistory, int channelYearEnd, int activeBinary, string OrigName, string OrigOtherTitle)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Julkaisukanavatietokanta (Jufo_ID, Channel_ID, Jufo_Luokka, Name, Other_Title, Publisher, Type, ISSNL, ISSN1, ISSN2, ISBN, Active, Jufo_history, Year_End, Active_binary, Orig_name, Orig_other_title) VALUES (@Jufo_ID, @Channel_ID, @Jufo_Luokka, @Name, @Other_Title, @Publisher, @Type, @ISSNL, @ISSN1, @ISSN2, @ISBN, @Active, @Jufo_History, @Year_End, @Active_binary, @Orig_name, @Orig_other_title)");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                //Jufo_ID
                if (String.IsNullOrEmpty(channelJufoID))
                {
                    cmd.Parameters.AddWithValue("@Jufo_ID", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Jufo_ID", channelJufoID);

                //Channel_ID
                if (String.IsNullOrEmpty(channelChannelID))
                {
                    cmd.Parameters.AddWithValue("@Channel_ID", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Channel_ID", channelChannelID);

                //Jufo_Luokka
                if (String.IsNullOrEmpty(channelJufoLuokka))
                {
                    cmd.Parameters.AddWithValue("@Jufo_Luokka", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Jufo_Luokka", channelJufoLuokka);

                //Name
                if (String.IsNullOrEmpty(channelName))
                {
                    cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Name", channelName);

                //Other_Title
                if (String.IsNullOrEmpty(channelOtherTitle))
                {
                    cmd.Parameters.AddWithValue("@Other_Title", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Other_Title", channelOtherTitle);

                // Publisher
                if (String.IsNullOrEmpty(channelPublisher))
                {
                    cmd.Parameters.AddWithValue("@Publisher", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Publisher", channelPublisher);


                //Type
                if (String.IsNullOrEmpty(channelType))
                {
                    cmd.Parameters.AddWithValue("@Type", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Type", channelType);

                //ISSNL
                if (String.IsNullOrEmpty(channelISSNL))
                {
                    cmd.Parameters.AddWithValue("@ISSNL", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@ISSNL", channelISSNL);

                //ISSN1
                if (String.IsNullOrEmpty(channelISSN1))
                {
                    cmd.Parameters.AddWithValue("@ISSN1", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@ISSN1", channelISSN1);

                //ISSN2
                if (String.IsNullOrEmpty(channelISSN2))
                {
                    cmd.Parameters.AddWithValue("@ISSN2", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@ISSN2", channelISSN2);

                //ISBN
                if (String.IsNullOrEmpty(channelISBN))
                {
                    cmd.Parameters.AddWithValue("@ISBN", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@ISBN", channelISBN);

                //Active
                if (String.IsNullOrEmpty(channelActive))
                {
                    cmd.Parameters.AddWithValue("@Active", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Active", channelActive);

                //Jufo_History
                if (String.IsNullOrEmpty(channelJufoHistory))
                {
                    cmd.Parameters.AddWithValue("@Jufo_History", DBNull.Value);
                }
                else
                    cmd.Parameters.AddWithValue("@Jufo_History", channelJufoHistory);

                // Year_End
                cmd.Parameters.AddWithValue("@Year_End", channelYearEnd);

                // Active_binary
                cmd.Parameters.AddWithValue("@Active_binary", activeBinary);

                // Uudet sarakkeet
                cmd.Parameters.AddWithValue("@Orig_name", OrigName);
                cmd.Parameters.AddWithValue("@Orig_other_title", OrigOtherTitle);

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Haetaan Julkaisukanavatietokanta-taulusta kaikki tiedot parametria vastaavalle julkaisulle
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public SqlDataReader hae_julkaisukanavatietokannasta(SqlConnection conn, string jufoID, string channelID)
        {

            conn.Open();

            SqlCommand cmd = new SqlCommand();

            if ((jufoID == null) || jufoID.Equals(""))
            {
                cmd.CommandText = "SELECT * FROM dbo.Julkaisukanavatietokanta WHERE Channel_ID = @Channel_ID";
            }
            else if ((channelID == null) || channelID.Equals(""))
            {
                cmd.CommandText = "SELECT * FROM dbo.Julkaisukanavatietokanta WHERE Jufo_ID = @Jufo_ID";
            }
            else
            {
                cmd.CommandText = "SELECT * FROM dbo.Julkaisukanavatietokanta WHERE Jufo_ID = 'thisIsDummyValue'";
            }

            
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            if (String.IsNullOrEmpty(jufoID))
            {
                cmd.Parameters.AddWithValue("@Jufo_ID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Jufo_ID", jufoID);
            }

            if (String.IsNullOrEmpty(channelID))
            {
                cmd.Parameters.AddWithValue("@Channel_ID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Channel_ID", channelID);
            }

            SqlDataReader reader = cmd.ExecuteReader();

            return reader;

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Jufo_Luokka
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_jufo_Luokka(string server, int id, string channelJufoLevel)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Jufo_Luokka = @Jufo_Luokka WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelJufoLevel))
                {
                    cmd.Parameters.AddWithValue("@Jufo_Luokka", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Jufo_Luokka", channelJufoLevel);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Name
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_name(string server, int id, string channelName)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Name = @Name WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelName))
                {
                    cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Name", channelName);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Other_Title
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_other_Title(string server, int id, string channelOtherTitle)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Other_Title = @Other_Title WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelOtherTitle))
                {
                    cmd.Parameters.AddWithValue("@Other_Title", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Other_Title", channelOtherTitle);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }

        // uudet sarakkeet
        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun OrigName
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_Orig_name(string server, int id, string channelOrigName)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Orig_name = @Orig_name WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelOrigName))
                {
                    cmd.Parameters.AddWithValue("@Orig_name", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Orig_name", channelOrigName);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Orig_Other_Title
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_Orig_other_Title(string server, int id, string channelOrigOtherTitle)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Orig_other_title = @Orig_other_title WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelOrigOtherTitle))
                {
                    cmd.Parameters.AddWithValue("@Orig_other_title", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Orig_other_title", channelOrigOtherTitle);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Publisher
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_publisher(string server, int id, string channelPublisher)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Publisher = @Publisher WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelPublisher))
                {
                    cmd.Parameters.AddWithValue("@Publisher", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Publisher", channelPublisher);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Type
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_type(string server, int id, string channelType)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Type = @Type WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelType))
                {
                    cmd.Parameters.AddWithValue("@Type", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Type", channelType);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun ISSNL
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_ISSNL(string server, int id, string channelISSNL)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET ISSNL = @ISSNL WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelISSNL))
                {
                    cmd.Parameters.AddWithValue("@ISSNL", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ISSNL", channelISSNL);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun ISSN1
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_ISSN1(string server, int id, string channelISSN1)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET ISSN1 = @ISSN1 WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelISSN1))
                {
                    cmd.Parameters.AddWithValue("@ISSN1", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ISSN1", channelISSN1);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun ISSN2
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_ISSN2(string server, int id, string channelISSN2)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET ISSN2 = @ISSN2 WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelISSN2))
                {
                    cmd.Parameters.AddWithValue("@ISSN2", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ISSN2", channelISSN2);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun ISBN
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_ISBN(string server, int id, string channelISBN)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET ISBN = @ISBN WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelISBN))
                {
                    cmd.Parameters.AddWithValue("@ISBN", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ISBN", channelISBN);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Active
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_active(string server, int id, string channelActive)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Active = @Active WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelActive))
                {
                    cmd.Parameters.AddWithValue("@Active", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Active", channelActive);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Jufo_History
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_jufo_history(string server, int id, string channelJufoHistory)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Jufo_History = @Jufo_History WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                if (String.IsNullOrEmpty(channelJufoHistory))
                {
                    cmd.Parameters.AddWithValue("@Jufo_History", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Jufo_History", channelJufoHistory);
                }

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Year_End
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_year_end(string server, int id, int channelYearEnd)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Year_End = @Year_End WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);


                cmd.Parameters.AddWithValue("@Year_End", channelYearEnd);

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        // julkaisut_mds.dbo.Julkaisukanavatietokanta                   
        //
        // Paivitetaan Julkaisukanavatietokanta-tauluun Active_binary
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public void update_julkaisukanavatietokanta_active_binary(string server, int id, int activeBinary)
        {

            string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using (conn)
            {

                SqlCommand cmd = new SqlCommand("UPDATE dbo.Julkaisukanavatietokanta SET Active_binary = @Active_binary WHERE ID = @ID");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@ID", id);

                cmd.Parameters.AddWithValue("@Active_binary", activeBinary);

                cmd.ExecuteNonQuery();

            }

            conn.Close();

        }

    }

}

