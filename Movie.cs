using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandoLib
{
    class Movie
    {
        public string name;
        public Image image;

        public Movie() { }

        public Movie (string name, Image image)
        {
            this.name = name;
            this.image = image;
        }
    }
}
