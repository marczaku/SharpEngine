using System;
using System.Numerics;
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
            var physics = new Physics(scene);
            window.Load(scene);

            var circle = new Circle(material);
            circle.Transform.Position = Vector.Left;
            circle.velocity = Vector.Right * 0.3f;
            scene.Add(circle);
            
            var square = new Rectangle(material);
            square.Transform.Position = Vector.Left + Vector.Backward * 0.2f;
            square.linearForce = Vector.Right * 0.3f;
            square.Mass = 4f;
            scene.Add(square);
            

            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            ground.Mass = float.PositiveInfinity;
            ground.gravityScale = 0f;
            scene.Add(ground);

            // engine rendering loop
            const int fixedStepNumberPerSecond = 30;
            const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            const float movementSpeed = 0.5f;
            double previousFixedStep = 0.0;
            while (window.IsOpen()) {
                while (Glfw.Time > previousFixedStep + fixedDeltaTime) {
                    previousFixedStep += fixedDeltaTime;
                    physics.Update(fixedDeltaTime);
                }
                window.Render();
            }
        }
    }
}
