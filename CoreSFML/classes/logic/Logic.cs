using CoreSFML.resources;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSFML.classes
{
    class Logic
    {
        private Renderer renderer;
        private Characters characters;
        private int x = 0, y = 0;

        public Logic(ref Renderer r, ref Characters charset)
        {
            this.renderer = r;
            this.characters = charset;
        }
        public void KeyHandler(object sender, KeyEventArgs key)
        {
            RenderWindow window = sender as RenderWindow;

            //Console.WriteLine((int)key.Code);

            if(key.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }

            if(key.Code == Keyboard.Key.Down)
            {
                renderer.SetPixel(x, ++y, 1);
            }
            if (key.Code == Keyboard.Key.Up)
            {
                renderer.SetPixel(x, --y, 1);
            }
            if (key.Code == Keyboard.Key.Left)
            {
                renderer.SetPixel(--x, y, 1);
            }
            if (key.Code == Keyboard.Key.Right)
            {
                renderer.SetPixel(++x, y, 1);
            }

            
        }

        public void TextHandler(object sender, TextEventArgs text)
        {
            renderer.PrintCharacter(0, 0, this.characters.GetCharacter((char)text.Unicode[0]));
        }
    }
}
