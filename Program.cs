using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OOP_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Connection string to your database
            string connectionString = "Data Source=LAPTOP-DMAMJ5FJ,1433;Initial Catalog=OOP;User Id=SuperAdmin;Password=SuperAdmin;";

            // Test database connection
            if (!TestDatabaseConnection(connectionString))
            {
                MessageBox.Show("Failed to connect to the database. Please check your network connection and try again.",
                                "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return; // Exit the application
            }

            // Start the application if the connection succeeds
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Uncomment the desired form to start
            //Application.Run(new frmPOS(null, null));
            //Application.Run(new frmSecurity());
            //Application.Run(new Form1());
            //Application.Run(new QRCodeGenerator());
            Application.Run(new mj_mainForm());
        }

        /// <summary>
        /// Function to test the database connection.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <returns>True if the connection is successful, otherwise false.</returns>
        private static bool TestDatabaseConnection(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Attempt to open the connection
                }
                return true; // Connection successful
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database connection error: " + ex.Message); // Log error for debugging
                return false; // Connection failed
            }
        }
    }
}
