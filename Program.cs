using System;
using GLFW;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args) {
            Glfw.Init();

            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);

            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                // do nothing
            }
        }
    }
}
