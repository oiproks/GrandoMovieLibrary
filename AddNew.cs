using HtmlAgilityPack;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace GrandoLib
{
    public partial class AddNew : Form
    {
        Image poster;
        SQLiteHelper helper;
        PictureBox tempPictureBox;
        MainView mainView;

        public AddNew(MainView mainView, SQLiteHelper helper)
        {
            InitializeComponent();
            this.helper = helper;
            this.mainView = mainView;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            flpCovers.Controls.Clear();

            string movieName = txtMovieName.Text.ToString().Replace(" ", "+");

            string url = "https://www.themoviedb.org/search?query=" + movieName + "&language=it-IT";

            using (WebClient client = new WebClient())
            {
                var document = new HtmlWeb().Load(url);
                var urls = document.DocumentNode.Descendants("img")
                                                .Select(ef => ef.GetAttributeValue("data-src", null))
                                                .Where(s => !String.IsNullOrEmpty(s));
                foreach (string imageUrl in urls)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Size = new Size(120, 175),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent
                    };
                    pictureBox.Load(imageUrl);
                    pictureBox.Click += PictureBox_Click;
                    flpCovers.Controls.Add(pictureBox);
                }

                //Parallel.ForEach(urls, imageUrl => {
                //    Populate_View(imageUrl);
                //});
            }
        }

        private void Populate_View(string imageUrl)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate {
                    Populate_View(imageUrl);
                });
                return;
            }
            else
            {
                PictureBox pictureBox = new PictureBox
                {
                    Size = new Size(120, 175),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent
                };
                pictureBox.Load(imageUrl);
                pictureBox.Click += PictureBox_Click;
                flpCovers.Controls.Add(pictureBox);
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (tempPictureBox != null)
                tempPictureBox.BackColor = Color.Transparent;
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.Green;
            tempPictureBox = pictureBox;
            poster = pictureBox.Image;
            btnAdd.Visible = true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            string movieName = ShowDialog(txtMovieName.Text.ToString());
            if (string.IsNullOrEmpty(movieName))
                return;

            bool result = helper.Insert(movieName, poster);
            if (!result)
                MessageBox.Show(this, "Errore durante il salvataggio del film.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                mainView.moviesUpdates = true;
                Close();
            }
        }

        public static string ShowDialog(string text)
        {
            Form prompt = new Form()
            {
                Width = 304,
                Height = 154,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.FromKnownColor(KnownColor.Control),
                StartPosition = FormStartPosition.CenterScreen
            };
            Panel panel = new Panel() {
                Width = 300,
                Height = 150,
                Location = new Point(2,2),
                BackColor = Color.FromKnownColor(KnownColor.ControlDarkDark)
            };
            Label title = new Label() {
                Text = "Confirm movie name",
                AutoSize = true,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold),
                ForeColor = Color.FromKnownColor(KnownColor.Control),
                Location = new Point(12, 9),
                MaximumSize = new Size(276,0),
                MinimumSize = new Size(276, 0),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Label description = new Label()
            {
                Text = "Is movie name correct?",
                AutoSize = true,
                ForeColor = Color.FromKnownColor(KnownColor.Control),
                Location = new Point(12, 44),
                MaximumSize = new Size(276, 0),
                MinimumSize = new Size(276, 0)
            };
            TextBox textBox = new TextBox()
            {
                Text = text,
                BackColor = Color.FromKnownColor(KnownColor.Window),
                Location = new Point(12, 67),
                Size = new Size(276, 40),
                Multiline = true
            };
            Button confirm = new Button()
            {
                Text = "OK",
                BackColor = Color.FromKnownColor(KnownColor.Control),
                Location = new Point(55, 116),
                Size = new Size(75, 20),
                DialogResult = DialogResult.OK
            };
            Button cancel = new Button()
            {
                Text = "Cancel",
                BackColor = Color.FromKnownColor(KnownColor.Control),
                Location = new Point(170, 116),
                Size = new Size(75, 20),
                DialogResult = DialogResult.Cancel
            };
            confirm.Click += (sender, e) => { prompt.Close(); };
            panel.Controls.Add(textBox);
            panel.Controls.Add(confirm);
            panel.Controls.Add(cancel);
            panel.Controls.Add(title);
            panel.Controls.Add(description);
            prompt.Controls.Add(panel);
            prompt.AcceptButton = confirm;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
