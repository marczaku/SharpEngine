namespace SharpEngine {
	public struct Vertex {
		public Vector position;
		public Color color;

		public Vertex(Vector position, Color color) {
			this.position = position;
			this.color = color;
		}
	}
}