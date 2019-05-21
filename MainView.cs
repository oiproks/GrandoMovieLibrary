using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GrandoLib
{
    public partial class MainView : Form
    {
        SQLiteHelper helper;
        List<Movie> movies;
        Movie selectedMovie;
        public bool moviesUpdates = false;
        int letterBars = 0;

        public MainView()
        {
            InitializeComponent();
            helper = new SQLiteHelper();
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            LoadMovies(false);
        }

        private void MainView_Active(object sender, EventArgs e)
        {
            if (moviesUpdates)
            {
                LoadMovies(false);
                moviesUpdates = false;
            }
            if (movies != null) {
                if (txtSearch.Text.ToString().Length <= 1)
                    if (movies.Count == 1)
                        lblCounter.Text = "Il catalogo contiene solo " + movies.Count + " film.";
                    else
                        lblCounter.Text = "Il catalogo contiene " + movies.Count + " film.";
                else
                    if (movies.Count == 1)
                        lblCounter.Text = "Il catalogo contiene solo " + movies.Count + " film con \"" + txtSearch.Text.ToString() + "\" nel titolo.";
                    else
                        lblCounter.Text = "Il catalogo contiene " + movies.Count + " film con \"" + txtSearch.Text.ToString() + "\" nel titolo.";
            }
            else
                lblCounter.Text = "";
            if (movies != null && flpContainer.Controls.Count != (movies.Count - letterBars))
            {
                letterBars = 0;
                flpContainer.Controls.Clear();
                string firstLetter = string.Empty;
                foreach (Movie movie in movies)
                {
                    //TODO: add block with first letter of movie to FLP
                    if (firstLetter != movie.name.Substring(0, 1))
                    {
                        firstLetter = movie.name.Substring(0, 1).ToUpper();
                        Button letter = new Button
                        {
                            BackColor = Color.Transparent,
                            //Enabled = false,
                            FlatStyle = FlatStyle.Flat,
                            FlatAppearance = {
                                BorderSize = 0,
                                MouseOverBackColor = Color.Transparent,
                                MouseDownBackColor = Color.Transparent
                            },
                            Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold),
                            ForeColor = Color.FromKnownColor(KnownColor.Control),
                            Image = Properties.Resources.line,
                            Size = new Size(496, 25),
                            Text = firstLetter,
                            TextAlign = ContentAlignment.TopLeft
                        };
                        letterBars++;
                        flpContainer.Controls.Add(letter);
                    }

                    Label movieName = new Label
                    {
                        Text = movie.name,
                        ForeColor = Color.FromKnownColor(KnownColor.Control),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font(Font, FontStyle.Bold),
                        MaximumSize = new Size(154, 26),
                        MinimumSize = new Size(154, 26),
                        Location = new Point(3, 3)
                    };
                    movieName.Click += MoreInfo;
                    PictureBox poster = new PictureBox
                    {
                        Image = movie.image,
                        Size = new Size(154, 170),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Location = new Point(3, 32)
                    };
                    poster.Click += MoreInfo;
                    Panel moviePanel = new Panel
                    {
                        Size = new Size(160, 205),
                        Tag = movie.id
                    };
                    moviePanel.Controls.Add(movieName);
                    moviePanel.Controls.Add(poster);
                    moviePanel.Click += MoreInfo;
                    flpContainer.Controls.Add(moviePanel);
                }
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Reset();
            AddNew addNew = new AddNew(this, helper);
            addNew.ShowDialog();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Stai cancellando \"" + selectedMovie.name + "\" dal catalogo.\r\nVuoi continuare?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                helper.Delete(selectedMovie.id);
                moviesUpdates = true;
                MainView_Active(sender, e);
            }
            Reset();
        }

        private void MoreInfo(object sender, EventArgs e)
        {
            string movieID;
            if (sender is Panel)
                movieID = ((Panel)sender).Tag.ToString();
            else if (sender is Label)
            {
                movieID = ((Label)sender).Parent.Tag.ToString();
            }
            else
                movieID = ((PictureBox)sender).Parent.Tag.ToString();
            string movieName = movies.Find(x => x.id == movieID).name;
            lblDelete.Text = "Elimina \"" + movieName + "\"";
            selectedMovie = new Movie(movieID, movieName, null, null);
            pbDelete.Visible = true;
            lblDelete.Visible = true;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.ToString().Length >= 2)
            {
                LoadMovies(true);
                MainView_Active(sender, e);
            } else
            {
                LoadMovies(false);
                MainView_Active(sender, e);
            }
            pbDelete.Visible = false;
            lblDelete.Visible = false;
        }

        private void LoadMovies(bool search)
        {
            if (search)
                movies = helper.ReadDB(txtSearch.Text.ToString());
            else
                movies = helper.ReadDB();
        }

        private void Reset()
        {
            selectedMovie = null;
            pbDelete.Visible = false;
            txtSearch.Text = string.Empty;
        }
    }
}
