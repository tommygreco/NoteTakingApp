using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        // Store the database file in the project directory.
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");

        // Inserts an object into the database i.e. Note, Notebook, User.
        public static bool Insert<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                // Create table if it doesn't exist. Insert the item and if insertion was successful, update retval.
                conn.CreateTable<T>();
                int rowsInserted = conn.Insert(item);
                if (rowsInserted > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        // Updates an object in the database.
        public static bool Update<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                // Create table if it doesn't exist. Update the item and if updating was successful, update retval.
                conn.CreateTable<T>();
                int rowsInserted = conn.Update(item);
                if (rowsInserted > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        // Deletes an item in the database.
        public static bool Delete<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                // Create table if it doesn't exist. Delete the item and if deletion was successful, update retval.
                conn.CreateTable<T>();
                int rowsInserted = conn.Delete(item);
                if (rowsInserted > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        // Gets the list of items from a table in the database.
        public static List<T> Read<T>() where T : new()
        {
            List<T> items;

            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                // Create table if it doesn't exist. Read all the items in the table and return them.
                conn.CreateTable<T>();
                items = conn.Table<T>().ToList();
            }

            return items;
        }
    }
}
