using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreSFML.resources
{
    class Characters
    {
        private byte[,] characters = new byte[char.MaxValue, 12];

        public Characters()
        {
        }

        public void LoadFromBytes(byte[] fontContents)
        {

        }
        public void LoadFromFile(string FileName)
        {
            byte[] bytes = File.ReadAllBytes(FileName);
            if (bytes.Length % 13 == 0)
            {
                for (int ch = 0; ch < bytes.Length;)
                {
                    byte code               = bytes[ch++];
                    try
                    {
                        characters[code, 0] = bytes[ch++];  // data 1 
                        characters[code, 1] = bytes[ch++];  // data 2 
                        characters[code, 2] = bytes[ch++];  // data 3 
                        characters[code, 3] = bytes[ch++];  // data 4 
                        characters[code, 4] = bytes[ch++];  // data 5 
                        characters[code, 5] = bytes[ch++];  // data 6 
                        characters[code, 6] = bytes[ch++];  // data 7 
                        characters[code, 7] = bytes[ch++];  // data 8
                        characters[code, 8] = bytes[ch++];  // data 9 
                        characters[code, 9] = bytes[ch++]; // data 10 
                        characters[code, 10] = bytes[ch++]; // data 11 
                        characters[code, 11] = bytes[ch++]; // data 12
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                    
                }
            }
        }

        public byte[] GetCharacter(char code)
        {
            return new byte[] { 
                this.characters[code, 0],
                this.characters[code, 1],
                this.characters[code, 2],
                this.characters[code, 3],
                this.characters[code, 4],
                this.characters[code, 5],
                this.characters[code, 6],
                this.characters[code, 7],
                this.characters[code, 8],
                this.characters[code, 9],
                this.characters[code, 10],
                this.characters[code, 11],
            };
        }
    }
}
