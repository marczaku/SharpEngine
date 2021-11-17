using System;

namespace SharpEngine {
	public class Circle : Shape {
		public float Radius => .1f * Transform.CurrentScale.x;

		public Circle(Material material) : base(CreateCircle(), material) {
		}
		
		static Vertex[] CreateCircle() {
			const int numberOfSegments = 8;
			const int verticesPerSegment = 3;
			const float scale = .1f;
			Vertex[] result = new Vertex[numberOfSegments*verticesPerSegment];
			const float circleRadians = MathF.PI * 2;
			var oldAngle = 0f;
			for (int i = 0; i < numberOfSegments; i++) {
				int currentVertex = i * verticesPerSegment;
				var newAngle = circleRadians / numberOfSegments * (i + 1);
				result[currentVertex++] = new Vertex(new Vector(), Color.Blue);
				result[currentVertex++] = new Vertex(new Vector(MathF.Cos(oldAngle), MathF.Sin(oldAngle))*scale, Color.Green);
				result[currentVertex] = new Vertex(new Vector(MathF.Cos(newAngle), MathF.Sin(newAngle))*scale, Color.Red);
				oldAngle = newAngle;
			}

			return result;
		}
	}
}