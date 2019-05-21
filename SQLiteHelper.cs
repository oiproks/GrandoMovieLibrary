using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;

namespace GrandoLib
{
    public class SQLiteHelper
    {
        SQLiteConnection dbConnection;
        static readonly object mono = new object();

        public SQLiteHelper()
        {
            CreateDB();
        }

        public void CreateDB()
        {
            if (!File.Exists("db.sqlite"))
            {
                SQLiteConnection.CreateFile("db.sqlite");
                dbConnection = new SQLiteConnection("Data Source=db.sqlite;Version=3;");
                Insert("CREATE TABLE \"movies\" " +
                    "(\"id\"INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                    "\"name\" NVARCHAR(500), " +
                    "\"posterData\" BLOB NULL," +
                    "\"synopsis\" NVARCHAR(2000))");
            }
            dbConnection = new SQLiteConnection("Data Source=db.sqlite;Version=3;"); ;
        }

        public bool Insert(string qry)
        {
            try
            {
                lock (mono)
                {
                    dbConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(dbConnection)
                    {
                        CommandText = qry
                    };
                    cmd.ExecuteNonQuery();
                    dbConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SQLiteHelper", ex);
                return false;
            }
        }

        public bool Insert(string movie, Image posterData, string synopsis = "")
        {
            byte[] data = ImageToByte2(posterData);
            //movie = movie.Replace
            try
            {
                lock (mono)
                {
                    dbConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(dbConnection)
                    {
                        CommandText = $"INSERT INTO movies (name, posterData, synopsis) values (@name, @img, '{synopsis}')"
                    };
                    cmd.Prepare();
                    cmd.Parameters.Add("@img", DbType.Binary, data.Length);
                    cmd.Parameters["@img"].Value = data;
                    cmd.Parameters.Add(new SQLiteParameter("@name", movie));
                    cmd.ExecuteNonQuery();
                    dbConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SQLiteHelper", ex);
                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                lock (mono)
                {
                    dbConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(dbConnection)
                    {
                        CommandText = $"DELETE FROM movies WHERE id = {id}"
                    };
                    cmd.ExecuteNonQuery();
                    dbConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SQLiteHelper", ex);
                return false;
            }
        }

        public List<Movie> ReadDB(string search = "")
        {
            List<Movie> movieList = new List<Movie>();
            string sql = string.IsNullOrEmpty(search) ? 
                "SELECT * FROM movies ORDER BY name" : 
                $"SELECT * FROM movies WHERE name LIKE '%{search}%' ORDER BY name";
            try
            {
                lock (mono)
                {
                    dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        byte[] bytBLOB = new byte[reader.GetBytes(2, 0, null, 0, int.MaxValue) - 1];
                        reader.GetBytes(2, 0, bytBLOB, 0, bytBLOB.Length);
                        MemoryStream stmBLOB = new MemoryStream(bytBLOB);
                        movieList.Add(new Movie(reader["id"].ToString(), reader["name"].ToString(), Image.FromStream(stmBLOB), ""));
                    }
                    dbConnection.Close();
                }
                return movieList;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SQLiteHelper", ex);
                return null;
            }
        }

        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
