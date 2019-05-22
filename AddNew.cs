using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            flpCovers.BackgroundImage = null;
            string movieName = txtMovieName.Text.ToString().Replace(" ", "+");

            string url = "https://www.themoviedb.org/search?query=" + movieName + "&language=it-IT";

            try
            {
                using (WebClient client = new WebClient())
                {
                    var document = new HtmlWeb().Load(url);
                    List<string> urls = document.DocumentNode.Descendants("img")
                                                    .Select(ef => ef.GetAttributeValue("data-src", null))
                                                    .Where(s => !String.IsNullOrEmpty(s)).ToList();
                    List<string> names = document.DocumentNode.Descendants("img")
                                                    .Select(ef => ef.GetAttributeValue("alt", null))
                                                    .Where(s => !String.IsNullOrEmpty(s)).ToList();
                    names.RemoveAt(0);
                    names.RemoveAt(names.Count - 1);
                    int index = 0;
                    if (urls.Count() == 0)
                        flpCovers.BackgroundImage = Properties.Resources.no_result;
                    else
                    {
                        //foreach (string imageUrl in urls)
                        //{
                        //    if (!imageUrl.Contains("w185_and_h278_bestv2"))
                        //        continue;
                        //    PictureBox pictureBox = new PictureBox
                        //    {
                        //        Size = new Size(120, 175),
                        //        SizeMode = PictureBoxSizeMode.Zoom,
                        //        BackColor = Color.Transparent,
                        //        Tag = names[index]
                        //    };
                        //    index++;
                        //    pictureBox.Load(imageUrl);
                        //    Image image = new Bitmap(pictureBox.Image, new Size(120, 175));
                        //    pictureBox.Image = image;
                        //    pictureBox.Click += PictureBox_Click;
                        //    flpCovers.Controls.Add(pictureBox);
                        //}
                        List<PictureBox> pictureBoxes = new List<PictureBox>();
                        try
                        {
                            Parallel.ForEach(urls, imageUrl =>
                            {
                                if (!imageUrl.Contains("w185_and_h278_bestv2"))
                                    return;
                                index = urls.IndexOf(imageUrl);
                                PictureBox pictureBox = new PictureBox
                                {
                                    Size = new Size(120, 175),
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    BackColor = Color.Transparent,
                                    Tag = names[index]
                                };
                                pictureBox.Load(imageUrl);
                                Image image = new Bitmap(pictureBox.Image, new Size(120, 175));
                                pictureBox.Image = image;
                                pictureBox.Click += PictureBox_Click;
                                pictureBoxes.Add(pictureBox);
                            });
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog("Parallel Work", ex);
                        }
                        finally
                        {
                            foreach (PictureBox pictureBox in pictureBoxes)
                                flpCovers.Controls.Add(pictureBox);
                        }
                    }
                }
            } catch (Exception ex)
            {
                Logger.WriteLog("Connection", ex);
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
            //btnAdd.Visible = true;
            Add_Click(sender, e);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            string movieName = ShowDialog(tempPictureBox.Tag.ToString());
            if (string.IsNullOrEmpty(movieName))
                return;

            bool result = helper.Insert(movieName, poster);
            if (!result)
                MessageBox.Show(this, "Errore durante il salvataggio del film nel catalogo.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Text = "Conferma il nome del film",
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
                Text = "Il nome è corretto?",
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
                Text = "Annulla",
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
