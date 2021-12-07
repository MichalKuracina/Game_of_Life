using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Game_of_life
{
    public class NewRect
    {
        private int _x;
        private int _y;
        private int _alive;
        private int _before;
        private Rectangle _rect;
        private Storyboard myStoryboard;

        public NewRect(Rectangle rect, int x, int y)
        {
            _x = x;
            _y = y;
            _rect = rect;
            Random randomSeed = new Random();
            _alive = Convert.ToInt32(randomSeed.Next(0, 2));
            _before = 0;

            _rect.Fill = new SolidColorBrush(Color.FromRgb(204, 230, 255));
            _rect.Stroke = new SolidColorBrush(Color.FromRgb(255,255,255));
            _rect.StrokeThickness = 2;
            _rect.RadiusX = 5;
            _rect.RadiusY = 5 ;

        }

        public int Alive
        {
            get { return _alive; }
            set { _alive = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }


        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public int Before
        {
            get { return _before; }
            set { _before = value; }
        }

        public void Animate(Grid grid, string action, int miliseconds)
        {
            _rect.Name = "myRect";
            NameScope.SetNameScope(grid, new NameScope());
            grid.RegisterName(_rect.Name, _rect);

            // Create animation
            var colorAnimation = new ColorAnimation();

            switch (action)
            {
                case "die":
                    colorAnimation.From = Color.FromRgb(0, 0, 0);
                    colorAnimation.To = Color.FromRgb(204, 230, 255);
                    break;
                case "live":
                    colorAnimation.From = Color.FromRgb(204, 230, 255);
                    colorAnimation.To = Color.FromRgb(0, 0, 0);
                    break;
                default:
                    throw new NotImplementedException();
            }

            // Set duration
            colorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(miliseconds));

            // Set behaviour
            colorAnimation.AutoReverse = false;
            colorAnimation.RepeatBehavior = new RepeatBehavior(1);

            // Add Storyboard
            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(colorAnimation);

            // Where to apply animation
            Storyboard.SetTargetName(colorAnimation, "myRect");

            // Which property to animate
            PropertyPath colorTargetPath = new PropertyPath("Fill.Color");
            Storyboard.SetTargetProperty(colorAnimation, colorTargetPath);

            myStoryboard.Begin(grid); 
        }

    }
}
