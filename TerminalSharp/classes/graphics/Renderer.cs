using System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;

namespace CoreSFML
{
    class Renderer
    {
        private Texture tex;
        private uint Width, Height, DPP;

        private byte bpp = 1;
        private byte sizeX;
        private byte sizeY;

        private byte[,] PixelMap;

        public uint TermWidth, TermHeight;
        public uint charSizeX = 8 + 2; //size + offset
        public uint charSizeY = 12 + 3; //size + offset

        // Объявляем делегат
        public delegate void BufferStateHandler(string message);
        // Событие, возникающее при выводе денег
        public event BufferStateHandler onBufferUpdated;

        public Renderer(ref Texture window, uint width, uint height, uint dpp)
        {
            this.tex = window;
            this.Width = width;
            this.Height = height;
            this.DPP = dpp;

            this.TermWidth = width / dpp;
            this.TermHeight = height / dpp;

            this.PixelMap = new byte[width / dpp, height / dpp];
            for (int y = 0; y < (height / dpp); y++)
            {
                for (int x = 0; x < (width / dpp); x++)
                {
                    PixelMap[x, y] = 0;
                }
            }

            this.tex.Update(Convert(ref PixelMap));
        }

        public byte[] Convert(ref byte[] colors)
        {
            byte[] result = new byte[colors.Length * 4];

            if (bpp == 1)
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    result[4 * i + 0] = (colors[i] == 1) ? (byte)255 : (byte)0;
                    result[4 * i + 1] = (colors[i] == 1) ? (byte)255 : (byte)0;
                    result[4 * i + 2] = (colors[i] == 1) ? (byte)255 : (byte)0;
                    result[4 * i + 3] = 255;
                }
            }
            else
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    result[4 * i + 0] = (byte)((colors[i] * 255) / (bpp - 1));
                    result[4 * i + 1] = 0;
                    result[4 * i + 2] = 0;
                    result[4 * i + 3] = 255;
                }
            }

            return result;
        }

        public byte[] Convert(ref byte[,] colors)
        {
            int xs = colors.GetLength(0);
            int ys = colors.GetLength(1);
            byte[] result = new byte[xs * ys * 4];
            

            if (bpp == 1)
            {
                for (int y = 0; y < ys; y++)
                {
                    for (int x = 0; x < xs; x++)
                    {
                        result[4 * (x + y * xs) + 0] = (colors[x,y] == 1) ? (byte)255 : (byte)0;
                        result[4 * (x + y * xs) + 1] = (colors[x,y] == 1) ? (byte)255 : (byte)0;
                        result[4 * (x + y * xs) + 2] = (colors[x,y] == 1) ? (byte)255 : (byte)0;
                        result[4 * (x + y * xs) + 3] = 255;
                    }
                }
            }
            else
            {
                //for (int i = 0; i < colors.Length; i++)
                //{
                //    result[4 * i + 0] = (byte)((colors[i] * 255) / (bpp - 1));
                //    result[4 * i + 1] = 0;
                //    result[4 * i + 2] = 0;
                //    result[4 * i + 3] = 255;
                //}
            }

            return result;
        }

        public void SetPixel(int x, int y, byte color)
        {
            PixelMap[x, y] = color;
            UpdateBytes();
        }

        public void PrintCharacter(int x, int y, byte[] character)
        {
            if (character != null)
            {
                for (int j = 0; j < 12; j++)
                {
                    PixelMap[2 + x + 0, 3 + y + j] = (byte)((character[j] & (1 << 7)) > 0 ? 1 : 0);
                    PixelMap[2 + x + 1, 3 + y + j] = (byte)((character[j] & (1 << 6)) > 0 ? 1 : 0);
                    PixelMap[2 + x + 2, 3 + y + j] = (byte)((character[j] & (1 << 5)) > 0 ? 1 : 0);
                    PixelMap[2 + x + 3, 3 + y + j] = (byte)((character[j] & (1 << 4)) > 0 ? 1 : 0);
                    PixelMap[2 + x + 4, 3 + y + j] = (byte)((character[j] & (1 << 3)) > 0 ? 1 : 0);
                    PixelMap[2 + x + 5, 3 + y + j] = (byte)((character[j] & (1 << 2)) > 0 ? 1 : 0);
                    PixelMap[2 + x + 6, 3 + y + j] = (byte)((character[j] & (1 << 1)) > 0 ? 1 : 0);
                    PixelMap[2 + x + 7, 3 + y + j] = (byte)((character[j] & (1 << 0)) > 0 ? 1 : 0);
                }
            }

            UpdateBytes();
        }

        public void MoveScreen(int toY)
        {
            for (int y = 0; y < TermHeight; y++)
            {
                if (y < TermHeight - charSizeY)
                {
                    for (int x = 0; x < TermWidth; x++)
                    {
                        PixelMap[x, y] = PixelMap[x, y + charSizeY];
                    }
                }
                else
                {
                    for (int x = 0; x < TermWidth; x++)
                    {
                        PixelMap[x, y] = 0;
                    }
                }

            }
        }

        private void UpdateBytes()
        {
            this.tex.Update(Convert(ref PixelMap));
            onBufferUpdated("");
        }
    }
}
