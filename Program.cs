using System;
using GLFW;

namespace SharpEngine
{
    class Program {
        static float Lerp(float from, float to, float t) {
            return from + (to - from) * t;
        }

        static float GetRandomFloat(Random random, float min = 0, float max = 1) {
            return Lerp(min, max, (float)random.Next() / int.MaxValue);
        }
        
        static void Main(string[] args) {
            
            var window = new Window();
            var material = new Material("shaders/world-position-color.vert", "shaders/vertex-color.frag");
            var scene = new Scene();
            window.Load(scene);

            var shape = new Circle(material);
            scene.Add(shape);

            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            scene.Add(ground);

            // engine rendering loop
            const int fixedStepNumberPerSecond = 30;
            const double fixedStepDuration = 1.0 / fixedStepNumberPerSecond;
            double previousFixedStep = 0.0;
            while (window.IsOpen()) {
                while (Glfw.Time > previousFixedStep + fixedStepDuration) {
                    previousFixedStep += fixedStepDuration;
                    
                }
                window.Render();
            }
        }
    }
}
