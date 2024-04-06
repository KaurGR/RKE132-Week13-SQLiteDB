using System;
using System.Data.SQLite;

class Program
{
    static void Main()
    {
        SQLiteConnection connection = CreateConnection();
        if (connection != null)
        {
            ReadData(connection);
            // InsertCustomer(connection);
            // RemoveCustomer(connection);
            FindCustomer(connection);
        }
    }

    static SQLiteConnection CreateConnection()
    {
        SQLiteConnection connection = new SQLiteConnection("Data Source=mydb.db; Version=3; New=True; Compress=True");
        try
        {
            connection.Open();
            Console.WriteLine("Database connected.");
            return connection;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    static void ReadData(SQLiteConnection connection)
    {
        Console.Clear();
        using (SQLiteCommand command = connection.CreateCommand())
        {
            command.CommandText = "SELECT rowid, * FROM customer";

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string readerRowId = reader["rowid"].ToString();
                    string readerStringFirstName = reader.GetString(1);
                    string readerStringLastName = reader.GetString(2);
                    string readerStringDoB = reader.GetString(3);
                    Console.WriteLine($"{readerRowId}. Full name: {readerStringFirstName} {readerStringLastName}; DoB: {readerStringDoB}");
                }
            }
        }
    }

    static void InsertCustomer(SQLiteConnection connection)
    {
        string fName, lName, dob;

        Console.WriteLine("Enter first name: ");
        fName = Console.ReadLine();
        Console.WriteLine("Enter last name:");
        lName = Console.ReadLine();
        Console.WriteLine("Enter date of birth (mm-dd-yyyy): ");
        dob = Console.ReadLine();

        try
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO customer (firstName, lastName, dateOfBirth) VALUES (@firstName, @lastName, @dateOfBirth)";
                command.Parameters.AddWithValue("@firstName", fName);
                command.Parameters.AddWithValue("@lastName", lName);
                command.Parameters.AddWithValue("@dateOfBirth", dob);
                int rowInserted = command.ExecuteNonQuery();
                Console.WriteLine($"Row inserted: {rowInserted}");
            }

            ReadData(connection);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void RemoveCustomer(SQLiteConnection connection)
    {
        // Implement the RemoveCustomer method here
    }

    static void FindCustomer(SQLiteConnection connection)
    {
        Console.WriteLine("Enter the first name of the customer to find:");
        string firstName = Console.ReadLine();

        try
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT rowid, * FROM customer WHERE firstName LIKE '%{firstName}%'";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string readerRowId = reader["rowid"].ToString();
                        string readerStringFirstName = reader.GetString(1);
                        string readerStringLastName = reader.GetString(2);
                        string readerStringDoB = reader.GetString(3);
                        Console.WriteLine($"ID: {readerRowId}, Full name: {readerStringFirstName} {readerStringLastName}, DoB: {readerStringDoB}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}