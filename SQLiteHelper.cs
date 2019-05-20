using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;

namespace GrandoLib
{
    class SQLiteHelper
    {
        SQLiteConnection dbConnection;

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
                Insert("CREATE TABLE movies (name NVARCHAR(500), posterData BLOB NULL)");
            }
            dbConnection = new SQLiteConnection("Data Source=db.sqlite;Version=3;"); ;
        }

        public bool Insert(String qry)
        {
            try
            {
                dbConnection.Open();
                SQLiteCommand cmd = new SQLiteCommand(dbConnection)
                {
                    CommandText = qry
                };
                cmd.ExecuteNonQuery();
                dbConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SQLiteHelper", ex);
                return false;
            }
        }

        public bool Insert(string movie, Image posterData)
        {
            byte[] data = ImageToByte2(posterData);
            try
            {
                dbConnection.Open();
                SQLiteCommand cmd = new SQLiteCommand(dbConnection)
                {
                    CommandText = $"INSERT INTO movies (name, posterData) values ('{movie}', @img)"
                };
                cmd.Prepare();
                cmd.Parameters.Add("@img", DbType.Binary, data.Length);
                cmd.Parameters["@img"].Value = data;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SQLiteHelper", ex);
                return false;
            }
        }

        public List<Movie> ReadDB()
        {
            List<Movie> movieList = new List<Movie>();

            string sql = "SELECT * FROM movies ORDER BY name";
            try
            {
                dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    byte[] bytBLOB = new byte[reader.GetBytes(1, 0, null, 0, int.MaxValue) - 1];
                    reader.GetBytes(1, 0, bytBLOB, 0, bytBLOB.Length);
                    MemoryStream stmBLOB = new MemoryStream(bytBLOB);
                    movieList.Add(new Movie(reader["name"].ToString(), Image.FromStream(stmBLOB)));
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
