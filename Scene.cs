using System.Collections.Generic;

namespace SharpEngine {
	public class Scene {

		public List<Triangle> triangles;

		public Scene() {
			triangles = new List<Triangle>();
		}
		
		public void Add(Triangle triangle) {
			triangles.Add(triangle);
		}

		public void Render() {
			for (int i = 0; i < this.triangles.Count; i++) {
				triangles[i].Render();
			}
		}
	}
}