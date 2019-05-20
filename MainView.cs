using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrandoLib
{
    public partial class MainView : Form
    {
        SQLiteHelper helper;
        Logger logger;

        public MainView()
        {
            InitializeComponent();
            helper = new SQLiteHelper();
            logger = new Logger();
        }

        private void InsertTest(object sender, EventArgs e)
        {
            //string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "posters");
            //try
            //{
            //    data = File.ReadAllBytes(Path.Combine(folder, "gKGxZkx00NvLtRm2f0lDeLt3gZq.jpg"));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}

            bool result = helper.Insert("Il Ciclone", pbTest.Image);
            if (!result)
                MessageBox.Show(this, "Errore durante il salvataggio del film.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ReadTest(object sender, EventArgs e)
        {
            List<Movie> movies = helper.ReadDB();

            foreach (Movie movie in movies)
            {
                pbTest.Image = movie.image;
            }
        }

        //TODO: Scaricare HTML da qui --> https://www.themoviedb.org/search?query=il+ciclone&language=it-IT
        //Trovare tutti i --> IndexOf("data-src=\"" + 10, | fino a successivo " | )
        //Scaricare le thumb e metterle in PictureBox.
        //Profit.
    }
}
