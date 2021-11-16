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

            //FillSceneWithTriangles(scene, material);
            var shape = new Triangle(material);
            scene.Add(shape);
            
            // engine rendering loop
            var direction = new Vector(0.01f, 0.01f);
            var multiplier = 0.95f;
            var rotation = 0.05f;
            const int fixedStepNumberPerSecond = 30;
            const double fixedStepDuration = 1.0 / fixedStepNumberPerSecond;
            double previousFixedStep = 0.0;
            while (window.IsOpen()) {
                while (Glfw.Time > previousFixedStep + fixedStepDuration) {
                    previousFixedStep += fixedStepDuration;
                    // Fixed Update:
                    for (var i = 0; i < scene.triangles.Count; i++) {
                        var triangle = scene.triangles[i];
                
                        // 2. Keep track of the Scale, so we can reverse it
                        if (triangle.Transform.CurrentScale.GetMagnitude() <= 0.5f) {
                            multiplier = 1.05f;
                        }
                        if (triangle.Transform.CurrentScale.GetMagnitude() >= 2f) {
                            multiplier = 0.95f;
                        }
                    
                        triangle.Transform.Scale(multiplier);
                        triangle.Transform.Rotate(rotation);
                
                        // 4. Check the X-Bounds of the Screen
                        if (triangle.GetMaxBounds().x >= 1 && direction.x > 0 || triangle.GetMinBounds().x <= -1 && direction.x < 0) {
                            direction.x *= -1;
                        }
                
                        // 5. Check the Y-Bounds of the Screen
                        if (triangle.GetMaxBounds().y >= 1 && direction.y > 0 || triangle.GetMinBounds().y <= -1 && direction.y < 0) {
                            direction.y *= -1;
                        }
                    
                    
                        triangle.Transform.Move(direction);
                    }
                }
                window.Render();
            }
        }
    }
}
