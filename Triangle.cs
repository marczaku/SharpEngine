using System;

namespace SharpEngine {
	public class Triangle : Shape {
		public Triangle(Material material) : base(CreateTriangle(), material) {
		}

		static Vertex[] CreateTriangle() {
			const float scale = .1f;
			float height = MathF.Sqrt(0.75f) * scale;
			return new Vertex[] {
				new Vertex(new Vector(-scale, -height/2), Color.Red),
				new Vertex(new Vector(scale, -height/2), Color.Green),
				new Vertex(new Vector(0f, height), Color.Blue)
			};
		}
	}
}