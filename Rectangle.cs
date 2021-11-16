namespace SharpEngine {
	public class Rectangle : Shape {
		public Rectangle(Material material) : base(CreateRectangle(), material) {
		}

		static Vertex[] CreateRectangle() {
			const float scale = .1f;
			return new Vertex[] {
				new Vertex(new Vector(-scale, -scale), Color.Red),
				new Vertex(new Vector(scale, -scale), Color.Green),
				new Vertex(new Vector(-scale, scale), Color.Blue),
				new Vertex(new Vector(scale, -scale), Color.Green),
				new Vertex(new Vector(scale, scale), Color.Red),
				new Vertex(new Vector(-scale, scale), Color.Blue)
			};
		}
	}
}