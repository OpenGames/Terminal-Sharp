using CoreSFML.classes.logic;
using CoreSFML.resources;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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

        private bool regexDebugInfo = true;

        public Logic(Terminal t, Renderer r, ref Characters charset)
        {
            this.renderer = r;
            this.characters = charset;
            this.terminal = t;

            this.renderer.PrintCharacter(cursorPosX, cursorPosY, this.characters.GetCharacter('>'));
            cursorPosX += (int)this.renderer.charSizeX;
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

            if (text.Unicode == "\r" || text.Unicode == "\n")
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

                
                //cursorPosX += (int)this.renderer.charSizeX;
                handleCommands(parseCommand(command));
                this.renderer.PrintCharacter(cursorPosX, cursorPosY, this.characters.GetCharacter('>'));
                cursorPosX += (int)this.renderer.charSizeX;
                command = "";
            }
            else if(text.Unicode == "\b")
            {
                command = command.Remove(command.Length - 1);
               
                cursorPosX -= (int)this.renderer.charSizeX;
                renderer.PrintCharacter(cursorPosX, cursorPosY, this.characters.GetCharacter(' '));
            }
            else
            {
                command += text.Unicode.ToLower();
                renderer.PrintCharacter(cursorPosX, cursorPosY, this.characters.GetCharacter((char)(text.Unicode.ToUpper())[0]));
                cursorPosX += (int)this.renderer.charSizeX;
            }

            

        }

        Command parseCommand(string str)
        {
            var m = Regex.Match(str, "^\\w+|\\d+|-\\d+");
            Command result = new Command();

            if (!m.Success)
            {
                result.success = false;
                return result;
            }
            else
            {
                if(regexDebugInfo)
                    println(string.Format("Command: {0}", m.Value));
                result.command = m.Value;
                m = m.NextMatch();
            }

            while (m.Success)
            {
                if(regexDebugInfo)
                    println(string.Format("Arg {0}: {1}", result.arguments.Count, m.Value));

                result.arguments.Add(m.Value);
                m = m.NextMatch();
            }

            result.success = true;
            return result;
        }

        void handleCommands(Command command)
        {
            if (command.command == "help")
            {
                println("no one will");
            }
            else if (command.command == "exit")
            {
                this.terminal.Close();
            }
            else if (command.command == "sfc")
            {
                if(command.arguments.Count != 2)
                {
                    println("this command requres 2 arguments <x> and <y>");
                }
                else
                {
                    terminal.Resize(new SFML.System.Vector2u(uint.Parse(command.arguments[0]), uint.Parse(command.arguments[1])));
                    terminal.Clear();
                    cursorPosX = 0;
                    cursorPosY = 0;
                }
                
            }
            else if (command.command == "clear")
            {
                this.terminal.Clear();
                cursorPosX = 0;
                cursorPosY = 0;
            }
            else if(command.command == "set")
            {
                switch (int.Parse(command.arguments[0]))
                {
                    case 0:
                        if (command.arguments.Count - 1 == 1)
                        {
                            regexDebugInfo = (int.Parse(command.arguments[1]) != 0);
                        }
                        break;

                    case 1:
                        if (command.arguments.Count - 1 == 1)
                        {
                            terminal.Clear();
                            cursorPosX = 0;
                            cursorPosY = 0;
                            terminal.ChangeResolution(uint.Parse(command.arguments[1]));
                        }
                        break;
                }

                
            }
        }

        void println(string text)
        {
            for (int i = 0; i < text.Length; i++)
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


                this.renderer.PrintCharacter(cursorPosX, cursorPosY, characters.GetCharacter(text.ToUpper()[i]));
                cursorPosX += (int)this.renderer.charSizeX;
            }

            //aka \n
            //-----------------------------------
            cursorPosX = 0;
            if (cursorPosY + this.renderer.charSizeY > this.renderer.TermHeight - this.renderer.charSizeY)
            {
                renderer.MoveScreen((int)renderer.TermHeight - cursorPosY - (int)renderer.charSizeY - 6);
            }
            else
            {
                cursorPosY += (int)this.renderer.charSizeY;
            }
            //-----------------------------------
        }
    }
    class Command
    {
        internal bool success;
        internal string command;
        internal List<string> arguments = new List<string>();
    }
}
