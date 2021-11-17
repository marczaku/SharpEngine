using System.Collections.Generic;

namespace SharpEngine {
	public class Scene {

		public List<Shape> shapes;

		public Scene() {
			this.shapes = new List<Shape>();
		}
		
		public void Add(Shape shape) {
			this.shapes.Add(shape);
		}

		public void Render() {
			for (int i = 0; i < this.shapes.Count; i++) {
				this.shapes[i].Render();
			}
		}
	}
}