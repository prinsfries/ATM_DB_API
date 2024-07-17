using ATMMODEL;
using System.Data;
using System.Data.SqlClient;

namespace atmDL
{
    public class SqlDbData
    {
        static string connectionString
        //= "Data Source =PRINS\\SQLEXPRESS; Initial Catalog = ATM; Integrated Security = True;";
        = "Server=tcp:104.214.168.6,1433;Database=ATM;User Id=sa;Password=Password1234!";

        static SqlConnection sqlConnection = new SqlConnection(connectionString);

        public List<EWallet> GetEW()
        {
            string selectStatement = "SELECT name, EWPin, Money FROM EWallet";
            List<EWallet> ews = new List<EWallet>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);
                sqlConnection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    string namee = reader["name"].ToString();
                    string ewpin = reader["EWPin"].ToString();
                    //int moneyy = reader.GetInt32(0);

                    EWallet readUser = new EWallet
                    {
                        name = namee,
                        EWPin = ewpin,
                        //money = moneyy
                    };

                    ews.Add(readUser);
                }
            }

            return ews;
        }
        public int AddUser(string name, string pin)
        {
            int success, k;
            k = 1000;
            string insertStatement = "INSERT INTO EWallet VALUES (@Name, @EWPin, @Money)";

            SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);

            insertCommand.Parameters.AddWithValue("@Name", name);
            insertCommand.Parameters.AddWithValue("@EWPin", pin);
            insertCommand.Parameters.AddWithValue("@Money", k);
            sqlConnection.Open();

            success = insertCommand.ExecuteNonQuery();

            sqlConnection.Close();

            return success;
        }

        public bool DelUser(string pin)
        {
            bool isDeleted = false;
            string deleteStatement = "DELETE FROM EWallet WHERE EWPin = @EWPin";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(deleteStatement, sqlConnection);
                cmd.Parameters.AddWithValue("@EWPin", pin);
                sqlConnection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                isDeleted = rowsAffected > 0;
            }

            return isDeleted;
        }

        public void VerifyDB(string EWPin)
        {
            SqlCommand cmd = new SqlCommand("SELECT EWPin FROM EWallet WHERE EWPin=" + EWPin, sqlConnection);
            SqlParameter pinparam;
            pinparam = new SqlParameter("@EWPin", EWPin);

            cmd.Parameters.Add(pinparam);
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                sqlConnection.Close();
                GetEWalletDB(EWPin);
            }
            else
            {
                sqlConnection.Close();
                Console.WriteLine("\nNot found");
            }

        }
        public EWallet GetEWalletDB(string EWPin)
        {
            SqlCommand cmd = new SqlCommand("SELECT Name,EWPin,Money FROM EWallet WHERE EWPin=" + EWPin, sqlConnection);
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            EWallet readUser = new EWallet();
            if (dr.Read())
            {
                string namee = dr["name"].ToString();
                string ewpin = dr["ewpin"].ToString();
                int monkey = Convert.ToInt32(dr["money"].ToString());

                readUser.name = namee;
                readUser.EWPin = ewpin;
                readUser.money = monkey;
            }
            sqlConnection.Close();
            return readUser;
        }

        public bool TransacDB(int p, string EWPin)
        {
            bool isUpdated = false;
            string updateStatement = "UPDATE EWallet SET Money = @Money WHERE EWPin = @EWPin";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(updateStatement, sqlConnection);
                cmd.Parameters.AddWithValue("@Money", p);
                cmd.Parameters.AddWithValue("@EWPin", EWPin);
                sqlConnection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                isUpdated = rowsAffected > 0;
            }

            return isUpdated;
        }


    }

}
