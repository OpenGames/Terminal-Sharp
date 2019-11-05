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
        private uint W = 800, H = 600, DPP = 2;
        private bool work = true;

        RenderWindow window;
        VertexArray Screen;
        Texture TermTexture;
        Shader Shader;
        Renderer renderer;
        Characters set;
        Logic logic;

        public Terminal()
        {
            //W = VideoMode.DesktopMode.Width;
            //H = VideoMode.DesktopMode.Height;

            window = new RenderWindow(new VideoMode(W, H), "Shaders yay", Styles.None);
            window.Position = new Vector2i(0, 0);
            Screen = new VertexArray(PrimitiveType.Quads, 4);

            Screen.Append(new Vertex(new Vector2f(0, 0), Color.Black, new Vector2f(0, 0)));
            Screen.Append(new Vertex(new Vector2f(W, 0), Color.Black, new Vector2f(1, 0)));
            Screen.Append(new Vertex(new Vector2f(W, H), Color.Black, new Vector2f(1, 1)));
            Screen.Append(new Vertex(new Vector2f(0, H), Color.Black, new Vector2f(0, 1)));
            TermTexture = new Texture(W / DPP, H / DPP);

            set = new Characters();
            set.LoadFromFile("resources/fonts/termfont.zf");

            renderer = new Renderer(TermTexture, ref set, W, H, DPP);
            Shader = new Shader(null, null, "shaders/fragment.glsl");//"shaders/vertex.glsl"

            Shader.SetUniform("TermTex", TermTexture);
            
            renderer.onBufferUpdated += (s) => {
                Shader.SetUniform("TermTex", TermTexture);
            };

            logic = new Logic(this, renderer, ref set);
            window.KeyPressed += logic.KeyHandler;
            window.TextEntered += logic.TextHandler;

            //window.Size = new SFML.System.Vector2u(1920, 1080);

            var r = new RenderStates(Shader);
            var bg = new Color(0, 0, 25);

            while (window.IsOpen && work)
            {
                window.DispatchEvents();
                window.Clear(bg);
                window.Draw(Screen, r);
                window.Display();
            }
            window.Close();
        }
        public void Close()
        {
            work = false;
        }
        public void Resize(Vector2u vector)
        {
            W = vector.X;
            H = vector.Y;

            TermTexture = new Texture(W / DPP, H / DPP);
            renderer.ResetTexture(TermTexture, W, H, DPP);

            window.Size = vector;
            
        }
        public void ChangeResolution(uint dpp)
        {
            DPP = dpp;

            TermTexture = new Texture(W / DPP, H / DPP);
            renderer.ResetTexture(TermTexture, W, H, DPP);
        }
        public void Clear()
        {
            renderer.ResetScreen();
        }
    }
}
