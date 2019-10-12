using CoreSFML.resources;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSFML.classes.logic
{
    class Terminal
    {
        private uint W = 800, H = 600, DPP = 6;
        public Terminal()
        {
            //W = VideoMode.DesktopMode.Width;
            //H = VideoMode.DesktopMode.Height;

            var window = new RenderWindow(new VideoMode(W, H), "Shaders yay", Styles.None);
            var Screen = new VertexArray(PrimitiveType.Quads, 4);

            Screen.Append(new Vertex(new Vector2f(0, 0), Color.Black, new Vector2f(0, 0)));
            Screen.Append(new Vertex(new Vector2f(W, 0), Color.Black, new Vector2f(1, 0)));
            Screen.Append(new Vertex(new Vector2f(W, H), Color.Black, new Vector2f(1, 1)));
            Screen.Append(new Vertex(new Vector2f(0, H), Color.Black, new Vector2f(0, 1)));
            var TermTexture = new Texture(W / DPP, H / DPP);

            Characters set = new Characters();
            set.LoadFromFile("resources/fonts/termfont.zf");

            Renderer renderer = new Renderer(ref TermTexture, ref set, W, H, DPP);
            Logic logic = new Logic(this, ref renderer, ref set);

            window.KeyPressed += logic.KeyHandler;
            window.TextEntered += logic.TextHandler;

            var Shader = new Shader(null, null, "shaders/fragment.glsl");//"shaders/vertex.glsl"

            Shader.SetUniform("TermTex", TermTexture);

            var r = new RenderStates(Shader);
            var bg = new Color(0, 0, 25);

            renderer.onBufferUpdated += (s) => {
                Shader.SetUniform("TermTex", TermTexture);
            };

            //var c = set.GetCharacter('A');
            //renderer.PrintCharacter(0, 0, c);

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(bg);
                window.Draw(Screen, r);
                window.Display();
            }
        }
    }
}
