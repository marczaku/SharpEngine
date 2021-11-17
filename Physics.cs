namespace SharpEngine {
	public class Physics {
		readonly Scene scene;

		public Physics(Scene scene) {
			this.scene = scene;
		}

		public void Update(float deltaTime) {
			var gravitationalAcceleration = Vector.Down * 9.819649f / 100;
			for (int i = 0; i < this.scene.shapes.Count; i++) {
				Shape shape = this.scene.shapes[i];
				
				// linear velocity:
				shape.Transform.Position = shape.Transform.Position + shape.velocity * deltaTime;
				
				// a = F/m (another version ∏of F = ma)
				var acceleration = shape.linearForce * shape.MassInverse;
				
				// add gravity to acceleration
				acceleration += gravitationalAcceleration * shape.gravityScale;
				
				// linear acceleration:
				shape.Transform.Position = shape.Transform.Position + acceleration * deltaTime * deltaTime / 2;
				shape.velocity = shape.velocity + acceleration * deltaTime;
			}
		}
	}
}