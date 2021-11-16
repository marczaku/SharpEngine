using System.Collections.Generic;

namespace SharpEngine {
	public class Scene {

		public List<Shape> triangles;

		public Scene() {
			triangles = new List<Shape>();
		}
		
		public void Add(Shape shape) {
			triangles.Add(shape);
		}

		public void Render() {
			for (int i = 0; i < this.triangles.Count; i++) {
				triangles[i].Render();
			}
		}
	}
}