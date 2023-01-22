using feedbackMgmt.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;

namespace feedbackMgmt
{
    public class DBManager
    {
        public static string conString = @"server=localhost;port=3306;user=root;password=welcome@123;database=osho";

      

        public static List<Feedback> GetFeedbacks()
        {
            List<Feedback> allFeedbacks = new List<Feedback>();
            MySqlConnection con = new MySqlConnection(conString);
            string query = "SELECT * FROM feedback";
            try
            {   
                con.Open();  
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader["sid"].ToString());
                    string sname = reader["sname"].ToString();
                    string fdate = reader["fdate"].ToString();
                    string module = reader["module"].ToString();
                    string faculty = reader["faculty"].ToString();
                    int rating = int.Parse(reader["rating"].ToString());
                    int pskill = int.Parse(reader["pskill"].ToString());
                    string fcomment = reader["fcomment"].ToString();

                    Feedback feedback = new Feedback
                    {
                        Sid = sid,
                        Sname = sname,
                        Fdate = fdate,
                        Module = module,
                        Faculty = faculty,
                        Rating = rating,
                        Pskill = pskill,
                        Fcomment = fcomment
                    };
                    allFeedbacks.Add(feedback);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally{
                con.Close();
            }
            return allFeedbacks;
        }

        public static Feedback GetFeedback(int id)
        {
            Feedback feedback = null;
            MySqlConnection con = new MySqlConnection(conString);
            string query = "SELECT * FROM Feedback WHERE sid=" + id;
            try
            {
                con.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(query, con);
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                if(reader.Read()){
                    int sid = int.Parse(reader["sid"].ToString());
                    string sname = reader["sname"].ToString();
                    string fdate = reader["fdate"].ToString();
                    string module = reader["module"].ToString();
                    string faculty = reader["faculty"].ToString();
                    int rating = int.Parse(reader["rating"].ToString());
                    int pskill = int.Parse(reader["pskill"].ToString());
                    string fcomment = reader["fcomment"].ToString();

                   feedback = new Feedback
                    {
                        Sid = sid,
                        Sname = sname,
                        Fdate = fdate,
                        Module = module,
                        Faculty = faculty,
                        Rating = rating,
                        Pskill = pskill,
                        Fcomment = fcomment
                    };
               }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }finally{
                con.Close();
            }
            return feedback;
        }

        public static bool Delete(int id)
        {
            bool status = false;
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = conString;
            try
            {
                string query = "DELETE FROM feedback WHERE sid=" + id;
                MySqlCommand command = new MySqlCommand(query, con);
                con.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            return status;
        }

        public static bool Insert(Feedback newFeedback)
        {
            bool status = false;
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = conString;
            try
            {
                string query = "Insert into feedback (sname,fdate,module,faculty,rating,pskill,fcomment) Values('" + newFeedback.Sname + "','" + newFeedback.Fdate + "','" +newFeedback.Module + "','" +newFeedback.Faculty + "'," +newFeedback.Rating + "," +newFeedback.Pskill + ",'" +newFeedback.Fcomment+"')";
                MySqlCommand cmd = new MySqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            return status;

        }

        public static bool Update(Feedback feedback)
        {
            bool status = false;
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = conString;
            try
            {
                string query = "UPDATE Feedback SET sname='" +feedback.Sname+"', fdate='"+feedback.Fdate+"', module='"+feedback.Module+"', faculty='"+feedback.Faculty+", rating="+feedback.Rating+", pskill="+feedback.Pskill+", fcomment='"+feedback.Fcomment+"' WHERE sid=" + feedback.Sid;
                MySqlCommand command = new MySqlCommand(query, con);
                con.Open();
                command.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            return status;
        }

        public static bool ValidateCredentials(string email, string password)
        {
            bool status = false;

            using (MySqlConnection connection = new MySqlConnection(conString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT Email, Password FROM Credentials WHERE Email = @email", connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string emailFromDb = reader["Email"].ToString();
                            string passwordFromDb = reader["Password"].ToString();
                            if (email == emailFromDb && password == passwordFromDb)
                            {
                                status = true;
                            }
                            else
                            {
                                // Email or password do not match
                            }
                        }
                        else
                        {
                            // Email not found in database
                        }
                    }
                }
            }

            return status;
        }

        public static bool RegisterUser(string email, string password)
        {
            // Hash the password using a library such as bcrypt, scrypt or argon2
            string hashedPassword = HashPassword(password);

            // Insert the new user into the database
            using (MySqlConnection connection = new MySqlConnection(conString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO Credentials (Email, Password) VALUES (@email, @password)", connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", hashedPassword);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        private static string HashPassword(string password)
        {
            // Use a library such as bcrypt, scrypt or argon2 to hash the password
            // Example using bcrypt:
            // You can install the package by running this command in your package manager console
            // Install - Package BCrypt.Net - Core or
            // dotnet add package BCrypt.Net - Core.
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool ValidatePassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }



    }

}

