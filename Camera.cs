using System;

namespace SharpEngine {
	public class Camera {
		
		public Transform Transform { get; }
		public Matrix Projection { get; private set; }
		public Matrix View => this.Transform.Matrix;
		
		public Camera() {
			this.Transform = new Transform();
			this.Projection = Matrix.Perspective(90f, 1f, 0.1f, 100f);
		}
	}
}