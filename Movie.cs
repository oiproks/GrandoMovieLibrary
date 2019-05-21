using System.Drawing;

namespace GrandoLib
{
    public class Movie
    {
        public string id;
        public string name;
        public Image image;
        public string synopsis;

        public Movie() { }

        public Movie (string id, string name, Image image, string synopsis)
        {
            this.id = id;
            this.name = name;
            this.image = image;
            this.synopsis = synopsis;
        }
    }
}
