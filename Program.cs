using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LM_Drawing
{
    public class Drawing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
    }

    public class Point
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int DrawingId { get; set; }
        public Drawing Drawing { get; set; }
    }

    public class DrawingContext : DbContext
    {
        public DbSet<Drawing> Drawings { get; set; }
        public DbSet<Point> Points { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=drawings.db");
        }
    }
    public int x = Console.WindowWidth / 2;
    public int y = Console.WindowHeight / 2;
    public int v = 0;
    public bool w = false;
    public bool h = false;
    public bool g = false;
    public internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DrawingContext())
            {
                // Az adatbázis létrehozása, ha nem létezik
                context.Database.EnsureCreated();
            }

            Console.Clear();
            DrawBorder();
            Console.CursorVisible = false;
            // █ ▓ ▒ ░
            string sryle = "█";

            do
            {
                DrawMenu(x, y, ref v);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        v = v > 0 ? v - 1 : 3;

                        break;
                    case ConsoleKey.DownArrow:
                        break;
                    case ConsoleKey.LeftArrow:

                    case ConsoleKey.Enter:
                        if (v == 0) CreateDrawing();
                        else if (v == 1) EditDrawing();
                        else if (v == 2) DeleteDrawing();
                        else if (v == 3) w = true;
                        break;
                    case ConsoleKey.Escape:
                        w = true;
                        break;
                }

            } while (!w);

            while (h)
            {
                switch (Console.ReadKey(true).Key) 
                { 
                    case ConsoleKey.UpArrow:
                        if (y > 0)
                        {
                            y = y - 1;
                        }
                        //Console.SetCursorPosition(x, y);
                        break;
                    case ConsoleKey.DownArrow:
                        if (y < Console.WindowHeight)
                        {
                            y = y + 1;
                        }
                        //Console.SetCursorPosition(x, y);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x < Console.WindowHeight)
                        {
                            x = x - 1;
                        }
                        //Console.SetCursorPosition(x, y);
                        break;
                    case ConsoleKey.RightArrow:
                        if (x < Console.WindowHeight)
                        {
                            x = x - 1;
                        }
                        //Console.SetCursorPosition(x, y);
                        break;
                    case ConsoleKey.Spacebar:
                        Console.Write(style);
                }
            }

        }

        static void CreateDrawing(string style, bool h, bool g)
        {
            Console.Clear();

            Console.Write("Stylus: █ ▓ ▒ ░\n");
            Console.WriteLine("Választott: ");
            string b = Console.ReadLine();
            b? "█" : "▓" : "▒" : "░";
            b = style;
            Console.Write("Szín: ");
            string a = Console.ReadLine();
            if (a != null) 
            {
                Console.Clear();
                h = true;
            }

            while(g):
            {
                Console.Write("Enter drawing name: ");
                string name = Console.ReadLine();

                var drawing = new Drawing { Name = name };

                using (var context = new DrawingContext())
                {
                    context.Drawings.Add(drawing);
                    context.SaveChanges();
                }

                Console.WriteLine("Add points (x y) for the drawing. Type 'end' to finish:");
                while (true)
                {
                    Console.Write("Point (x y): ");
                    string input = Console.ReadLine();
                    if (input.ToLower() == "end") break;

                    var parts = input.Split(' ');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
                    {
                        var point = new Point { X = x, Y = y, DrawingId = drawing.Id };

                        using (var context = new DrawingContext())
                        {
                            context.Points.Add(point);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid format. Please enter x and y coordinates.");
                    }
                }
            }
            
        }

        static void EditDrawing()
        {
            Console.Clear();
            Console.WriteLine("Edit drawing (feature not yet implemented)");
            Console.ReadKey();
        }

        static void DeleteDrawing()
        {
            Console.Clear();
            Console.WriteLine("Delete drawing (feature not yet implemented)");
            Console.ReadKey();
        }

        static void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write('╔');
            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                Console.Write('═');
            }
            Console.Write('╗');
            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("║");
            }
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write('╝');
            for (int i = Console.WindowWidth - 2; i > 0; i--)
            {
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write('═');
            }
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write('╚');
            for (int i = Console.WindowHeight - 2; i > 0; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
            }
        }

        static void DrawMenu(int x, ref int selectedIndex)
        {
            string[] options = { "Új", "Szerkesztés", "Törlés", "Kilépés" };

            for (int i = 0; i < options.Length; i++)
            {
                bool isSelected = (i == selectedIndex);
                DrawBlock(x, 5 + (i * 5), options[i], isSelected);
            }
        }

        static void DrawBlock(int x, int y, string text, bool selected)
        {
            if (selected)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.SetCursorPosition(x - 15, y);
            Console.Write("┌");
            for (int i = x - 14; i < x + 15; i++)
            {
                Console.SetCursorPosition(i, y);
                Console.Write("─");
            }
            Console.SetCursorPosition(x + 15, y);
            Console.Write("┐");

            for (int i = y + 1; i < y + 4; i++)
            {
                Console.SetCursorPosition(x - 15, i);
                Console.Write("│");
                Console.SetCursorPosition(x + 15, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(x - 15, y + 4);
            Console.Write("└");
            for (int i = x - 14; i < x + 15; i++)
            {
                Console.SetCursorPosition(i, y + 4);
                Console.Write("─");
            }
            Console.SetCursorPosition(x + 15, y + 4);
            Console.Write("┘");

            Console.SetCursorPosition(x - (text.Length / 2), y + 2);
            Console.Write(text);
            Console.ResetColor();


        }
    }
}
