using System;
using GLFW;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new NativeWindow(800, 600, "MyWindowTitle"))
            {
                // Main application loop
                while (!window.IsClosing)
                {
                    // OpenGL rendering
                    // Implement any timing for flow control, etc (see Glfw.GetTime())
                    
                    // Swap the front/back buffers
                    window.SwapBuffers();
                    
                    // Poll native operating system events (must be called or OS will think application is hanging)
                    Glfw.PollEvents();
                }
            }
        }
    }
}
