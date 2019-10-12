﻿using CoreSFML.classes.logic;
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
        string command = "";

        int cursorPosX = 0;
        int cursorPosY = 0;

        private Renderer renderer;
        private Characters characters;
        private Terminal terminal;
        private int x = 0, y = 0;

        public Logic(Terminal t, ref Renderer r, ref Characters charset)
        {
            this.renderer = r;
            this.characters = charset;
            this.terminal = t;
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

            if(key.Code == Keyboard.Key.Enter)
            {
                cursorPosX = 0;
                if (cursorPosY + this.renderer.charSizeY > this.renderer.TermHeight - this.renderer.charSizeY)
                {
                    renderer.MoveScreen((int)renderer.TermHeight - cursorPosY - (int)renderer.charSizeY - 6);
                }
                else
                {
                    cursorPosY += (int)this.renderer.charSizeY;
                }

                this.renderer.PrintCharacter(cursorPosX, cursorPosY, this.characters.GetCharacter('>'));
                handleCommands(command);
                command = "";
            }
            
        }

        public void TextHandler(object sender, TextEventArgs text)
        {
            if (cursorPosX + this.renderer.charSizeX > this.renderer.TermWidth)
            {
                cursorPosX = 0;
                if (cursorPosY + this.renderer.charSizeY > this.renderer.TermHeight - this.renderer.charSizeY)
                {
                   
                    renderer.MoveScreen((int)renderer.TermHeight - cursorPosY - (int)renderer.charSizeY - 6);
                    
                }
                else
                {
                    cursorPosY += (int)this.renderer.charSizeY;
                }
            }

            command += text.Unicode.ToLower();
            renderer.PrintCharacter(cursorPosX, cursorPosY, this.characters.GetCharacter((char)(text.Unicode.ToUpper())[0]));
            cursorPosX += (int)this.renderer.charSizeX;

        }

        void handleCommands(string command)
        {
            Console.WriteLine(command);
        }
    }
}
