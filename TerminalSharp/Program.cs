using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using CoreSFML.classes.logic;
using System;

namespace CoreSFML
{
    class Program
    {
        static uint W = 800, H = 600;

        static void Main(string[] args)
        {
            Console.WriteLine("Terminal loading started...");
            Terminal terminal = new Terminal();
        }
    }
}
