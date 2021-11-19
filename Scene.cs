using System.Collections.Generic;

namespace SharpEngine {
	public class Scene {

		public List<Shape> shapes;
		public Camera camera;

		public Scene(Camera camera) {
			this.shapes = new List<Shape>();
			this.camera = camera;
		}
		
		public void Add(Shape shape) {
			this.shapes.Add(shape);
		}

		public void Render() {
			for (int i = 0; i < this.shapes.Count; i++) {
				this.shapes[i].Render(this.camera);
			}
		}
	}
}