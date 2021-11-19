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

            var random = new Random();
            var window = new Window();
            var material = new Material("shaders/projection-view-model-color.vert", "shaders/vertex-color.frag");
            var camera = new Camera();
            camera.Transform.Position = new Vector(0f, 0f, -1f);
            var scene = new Scene(camera);
            var physics = new Physics(scene);
            window.Load(scene);

            for (var i = 0; i < 800; i++) {
                var circle = new Circle(material);
                var radius = GetRandomFloat(random, 0.2f, 0.3f);
                circle.Transform.CurrentScale = new Vector(radius, radius, 1f);
                circle.Transform.Position = new Vector(GetRandomFloat(random, -1f), GetRandomFloat(random, -1), 0f);
                circle.velocity = -circle.Transform.Position.Normalize() * GetRandomFloat(random, 0.15f, 0.3f);
                circle.Mass = MathF.PI * radius * radius;
                scene.Add(circle);
            }

            const int fixedStepNumberPerSecond = 30;
            const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            const int maxStepsPerFrame = 5;
            var previousFixedStep = 0.0;
            while (window.IsOpen()) {
                var stepCount = 0;
                while (Glfw.Time > previousFixedStep + fixedDeltaTime && stepCount++ < maxStepsPerFrame) {
                    if (window.GetKey(Keys.W)) camera.Transform.Move(new Vector(0f, 0f, 1f) * fixedDeltaTime);
                    if (window.GetKey(Keys.S)) camera.Transform.Move(new Vector(0f, 0f, -1f) * fixedDeltaTime);
                    if (window.GetKey(Keys.A)) camera.Transform.Move(Vector.Right * fixedDeltaTime);
                    if (window.GetKey(Keys.D)) camera.Transform.Move(Vector.Left * fixedDeltaTime);
                    previousFixedStep += fixedDeltaTime;
                    physics.Update(fixedDeltaTime);
                }
                window.Render();
            }
        }
    }
}
