namespace SharpEngine
{
    class Program
    {
        static Triangle[] triangles = new Triangle[] {
            new Triangle (
                new Vertex[] {
                    new Vertex(new Vector(0f, 0f), Color.Red),
                    new Vertex(new Vector(0.2f, 0f), Color.Green),
                    new Vertex(new Vector(0f, 0.2f), Color.Blue)
                }
            ),
            new Triangle (
                new Vertex[] {
                    new Vertex(new Vector(-0.4f, 0f), Color.Red),
                    new Vertex(new Vector(-0.2f, 0f), Color.Green),
                    new Vertex(new Vector(-0.3f, 0.133f), Color.Blue)
                }
            )
        };
        
        static void Main(string[] args) {
            
            var window = new Window();
            var material = new Material("shaders/position-color.vert", "shaders/vertex-color.frag");
            material.Use();

            // engine rendering loop
            var direction = new Vector(0.0003f, 0.0003f);
            var multiplier = 0.999f;
            while (window.IsOpen()) {
                window.BeginRender();

                // Update Triangles
                for (var i = 0; i < triangles.Length; i++) {
                    var triangle = triangles[i];
                    triangle.Scale(multiplier);
                
                    // 2. Keep track of the Scale, so we can reverse it
                    if (triangle.CurrentScale <= 0.5f) {
                        multiplier = 1.001f;
                    }
                    if (triangle.CurrentScale >= 1f) {
                        multiplier = 0.999f;
                    }

                    // 3. Move the Triangle by its Direction
                    triangle.Move(direction);
                
                    // 4. Check the X-Bounds of the Screen
                    if (triangle.GetMaxBounds().x >= 1 && direction.x > 0 || triangle.GetMinBounds().x <= -1 && direction.x < 0) {
                        direction.x *= -1;
                    }
                
                    // 5. Check the Y-Bounds of the Screen
                    if (triangle.GetMaxBounds().y >= 1 && direction.y > 0 || triangle.GetMinBounds().y <= -1 && direction.y < 0) {
                        direction.y *= -1;
                    }
                }

                RenderTriangles();
                
                window.EndRender();
            }
        }

        static void RenderTriangles() {
            for (var i = 0; i < triangles.Length; i++) {
                var triangle = triangles[i];
                triangle.Render();
            }
        }
    }
}
