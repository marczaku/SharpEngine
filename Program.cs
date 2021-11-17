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
            window.Load(scene);

            var shape = new Triangle(material);
            shape.Transform.CurrentScale = new Vector(0.5f, 1f, 1f);
            shape.Transform.Position = new Vector(0f, 0.5f);
            scene.Add(shape);
            
            var rectangle = new Rectangle(material);
            scene.Add(rectangle);
            
            var circle = new Circle(material);
            circle.Transform.Position = new Vector(0.2f, 0f);
            scene.Add(circle);
            

            var ground = new Rectangle(material);
            ground.Transform.CurrentScale = new Vector(10f, 1f, 1f);
            ground.Transform.Position = new Vector(0f, -1f);
            scene.Add(ground);

            // engine rendering loop
            const int fixedStepNumberPerSecond = 30;
            const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            const float movementSpeed = 0.5f;
            double previousFixedStep = 0.0;
            while (window.IsOpen()) {
                while (Glfw.Time > previousFixedStep + fixedDeltaTime) {
                    previousFixedStep += fixedDeltaTime;
                    var walkDirection = new Vector();
                    if (window.GetKey(Keys.W)) {
                        walkDirection += shape.Transform.Forward;
                    }
                    if (window.GetKey(Keys.S)) {
                        walkDirection += shape.Transform.Backward;
                    }
                    if (window.GetKey(Keys.A)) {
                        walkDirection += shape.Transform.Left;
                    }
                    if (window.GetKey(Keys.D)) {
                        walkDirection += shape.Transform.Right;
                    }
                    if (window.GetKey(Keys.Q)) {
                        var rotation = shape.Transform.Rotation;
                        rotation.z += MathF.PI * fixedDeltaTime;
                        shape.Transform.Rotation = rotation;
                    }
                    if (window.GetKey(Keys.E)) {
                        var rotation = shape.Transform.Rotation;
                        rotation.z -= MathF.PI * fixedDeltaTime;
                        shape.Transform.Rotation = rotation;
                    }

                    walkDirection = walkDirection.Normalize();
                    shape.Transform.Position += walkDirection * movementSpeed * fixedDeltaTime;

                    float direction = Vector.Dot((rectangle.GetCenter() - shape.GetCenter()).Normalize(), shape.Transform.Forward);
                    bool doesThePlayerFaceTheRectangle = direction > 0;
                    if (doesThePlayerFaceTheRectangle) {
                        rectangle.SetColor(Color.Green);
                    } else {
                        rectangle.SetColor(Color.Red);
                    }

                    float dotProduct = Vector.Dot((circle.GetCenter() - shape.GetCenter()).Normalize(), shape.Transform.Forward);
                    float angle = MathF.Acos(dotProduct);
                    float factor = angle / MathF.PI;
                    circle.SetColor(new Color(factor, factor, factor, 1));
                }
                window.Render();
            }
        }
    }
}
