using System;
using System.IO;
using System.Numerics;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    struct Vector {
        public float x, y, z;

        public Vector(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y) {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector operator *(Vector v, float f) {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }

        public static Vector operator /(Vector v, float f) {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }
        
        public static Vector operator +(Vector lhs, Vector rhs) {
            return new Vector(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }
        
        public static Vector operator -(Vector lhs, Vector rhs) {
            return new Vector(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }
        
        public static Vector Max(Vector a, Vector b) {
            return new Vector(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y), MathF.Max(a.z, b.z));
        }
        public static Vector Min(Vector a, Vector b) {
            return new Vector(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y), MathF.Min(a.z, b.z));
        }
        
        // +
        // -
        // /
    }
    
    class Program
    {
        static Vector[] vertices = new Vector[] {
            // new Vector(-.1f, -.1f),
            // new Vector(.1f, -.1f),
            // new Vector(0f, .1f),
            new Vector(.4f, .2f),
            new Vector(.6f, .2f),
            new Vector(.5f, .4f)
        };
        
        static void Main(string[] args) {
            
            var window = CreateWindow();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();

            // engine rendering loop
            var direction = new Vector(0.0003f, 0.0003f);
            var multiplier = 0.999f;
            var scale = 1f;
            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
                
                // 1. Scale the Triangle without Moving it
                
                // 1.1 Move the Triangle to the Center, so we can scale it without Side Effects
                // 1.1.1 Find the Center of the Triangle
                // 1.1.1.1 Find the Minimum and Maximum
                var min = vertices[0];
                for (var i = 1; i < vertices.Length; i++) {
                    min = Vector.Min(min, vertices[i]);
                }
                var max = vertices[0];
                for (var i = 1; i < vertices.Length; i++) {
                    max = Vector.Max(max, vertices[i]);
                }
                // 1.1.1.2 Average out the Minimum and Maximum to get the Center
                var center = (min + max) / 2;
                // 1.1.2 Move the Triangle the Center
                for (var i = 0; i < vertices.Length; i++) {
                    vertices[i] -= center;
                }
                // 1.2 Scale the Triangle
                for (var i = 0; i < vertices.Length; i++) {
                    vertices[i] *= multiplier;
                }
                // 1.3 Move the Triangle Back to where it was before
                for (var i = 0; i < vertices.Length; i++) {
                    vertices[i] += center;
                }
                
                // 2. Keep track of the Scale, so we can reverse it
                scale *= multiplier;
                if (scale <= 0.5f) {
                    multiplier = 1.001f;
                }
                if (scale >= 1f) {
                    multiplier = 0.999f;
                }

                // 3. Move the Triangle by its Direction
                for (var i = 0; i < vertices.Length; i++) {
                    vertices[i] += direction;
                }
                // 4. Check the X-Bounds of the Screen
                for (var i = 0; i < vertices.Length; i++) {
                    if (vertices[i].x >= 1 && direction.x > 0 || vertices[i].x <= -1 && direction.x < 0) {
                        direction.x *= -1;
                        break;
                    }
                }
                // 5. Check the Y-Bounds of the Screen
                for (var i = 0; i < vertices.Length; i++) {
                    if (vertices[i].y >= 1 && direction.y > 0 || vertices[i].y <= -1 && direction.y < 0) {
                        direction.y *= -1;
                        break;
                    }
                }

                UpdateTriangleBuffer();
            }
        }

        static void Render(Window window) {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
            Glfw.SwapBuffers(window);
            //glFlush();
        }

        static void ClearScreen() {
            glClearColor(.2f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        static void CreateShaderProgram() {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/screen-coordinates.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/green.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }

        static unsafe void LoadTriangleIntoBuffer() {
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            UpdateTriangleBuffer();
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vector), NULL);
            glEnableVertexAttribArray(0);
        }
        
        static unsafe void UpdateTriangleBuffer() {
            fixed (Vector* vertex = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
        }

        static Window CreateWindow() {
            // initialize and configure
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}
