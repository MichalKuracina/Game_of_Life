using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Game_of_life
{
    public class World
    {
        private Grid _grid { get; set; }
        private int _x { get; set; }
        private int _y { get; set; }
        public DispatcherTimer timer = new DispatcherTimer();

        public World(Grid grid, int x, int y)
        {
            _grid = grid;
            _x = x;
            _y = y;
        }


        public void design()
        {
            for (int j = 0; j < _y; j++)
            {
                ColumnDefinition columnDef = new ColumnDefinition();
                columnDef.Width = new GridLength(1, GridUnitType.Star);
                _grid.ColumnDefinitions.Add(columnDef);
            }

            for (int i = 0; i < _x; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                _grid.RowDefinitions.Add(rowDef);
            }
        }

        public List<List<NewRect>> create()
        {
            List<List<NewRect>> world = new List<List<NewRect>>();

            for (int j = 0; j < _y; j++)
            {
                List<NewRect> row = new List<NewRect>();
                for (int i = 0; i < _x; i++)
                {
                    NewRect rect = new NewRect(new Rectangle(), j, i);
                    row.Add(rect);
                }
                world.Add(row);
            }
            return world;
        }

        public void draw(List<List<NewRect>> world)
        {
            styleRectange(world);

            for (int i = 0; i < _y; i++)
            {
                for (int j = 0; j < _x; j++)
                {
                    Grid.SetRow(world[i][j].Rect, world[i][j].X);
                    Grid.SetColumn(world[i][j].Rect, world[i][j].Y);

                    _grid.Children.Add(world[i][j].Rect);
                }
            } 
        }

        public List<List<NewRect>> mutate(List<List<NewRect>> world, List<List<NewRect>> newWorld)
        {
            // RULES
            //Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            //Any live cell with two or three live neighbours lives on to the next generation.
            //Any live cell with more than three live neighbours dies, as if by overpopulation.
            //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

            _grid.Children.RemoveRange(0, _x*_y);

            for (int j = 0; j < _y; j++)
            {
                for (int i = 0; i < _x; i++)
                {
                    newWorld[j][i].Before = world[j][i].Alive;
                    newWorld[j][i].Alive = 0;
                    int count = validate(world, i, j);

                    if (world[j][i].Alive == 1)
                    {
                        newWorld[j][i].Alive = 1;
                        if (count < 2 | count > 3) { newWorld[j][i].Alive = 0; }
                    }
           
                    if (count == 3) { newWorld[j][i].Alive = 1; }
                }
            }

            return newWorld;
        }

        private void styleRectange(List<List<NewRect>> world)
        {
            Random r = new Random();

            foreach (NewRect rect in world.SelectMany(s => s).OrderBy(x => r.Next()))
            {
                switch (rect.Before)
                {
                    case 0 when rect.Alive == 0:
                        rect.Rect.Fill =  new SolidColorBrush(Color.FromRgb(204, 230, 255));
                        break;
                    case 0 when rect.Alive == 1:
                        rect.Animate(_grid, "live", r.Next(1, 250));
                        break;
                    case 1 when rect.Alive == 0:
                        rect.Animate(_grid, "die", r.Next(250, 500));
                        break;
                    case 1 when rect.Alive == 1:
                        rect.Rect.Fill = Brushes.Black;
                        break;
                }   
            }
        }

        private int validate (List<List<NewRect>> world, int x, int y)
        {
            int count = 0;

            int[][] neighbours = new int[][]
                {
                new int[] { x - 1, y - 1},
                new int[] { x, y - 1},
                new int[] {x + 1, y - 1 },
                new int[] { x - 1, y },
                new int[] { x + 1, y },
                new int[] {x - 1, y + 1 },
                new int[] {x, y + 1 },
                new int[] {x + 1, y + 1 }
                };

            foreach (Array neighbour in neighbours)
            {

                int coordinate_x = Convert.ToInt32(neighbour.GetValue(1));
                int coordinate_y = Convert.ToInt32(neighbour.GetValue(0));

                if (world.ElementAtOrDefault(coordinate_x) != null)
                {
                    if (world[coordinate_x].ElementAtOrDefault(coordinate_y) != null)
                    {
                        count = count + world[coordinate_x][coordinate_y].Alive;
                    }
                }
            }

            return count;
        }

        public int CountLives(List<List<NewRect>> world)
        {
            int lives = 0;

            foreach (NewRect live in world.SelectMany(r => r))
            {
                lives = lives + live.Alive;
            }

            return lives;
        }
    }
}
