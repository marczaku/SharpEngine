namespace SharpEngine {
	public class Physics {
		readonly Scene scene;

		public Physics(Scene scene) {
			this.scene = scene;
		}

		public void Update(float deltaTime) {
			var gravitationalAcceleration = Vector.Down * 9.819649f * 0;
			for (int i = 0; i < this.scene.shapes.Count; i++) {
				Circle shape = this.scene.shapes[i] as Circle;
				
				// linear velocity:
				shape.Transform.Position = shape.Transform.Position + shape.velocity * deltaTime;
				
				// a = F/m (another version ∏of F = ma)
				var acceleration = shape.linearForce * shape.MassInverse;
				
				// add gravity to acceleration
				acceleration += gravitationalAcceleration * shape.gravityScale;
				
				// linear acceleration:
				shape.Transform.Position = shape.Transform.Position + acceleration * deltaTime * deltaTime / 2;
				shape.velocity = shape.velocity + acceleration * deltaTime;
				
				// collision detection:
				for (int j = i+1; j < this.scene.shapes.Count; j++) {
					Circle other = this.scene.shapes[j] as Circle;
					// check for collision
					Vector deltaPosition = other.GetCenter() - shape.GetCenter();
					bool collision = deltaPosition.GetMagnitude() < shape.Radius + other.Radius;

					if (collision) {
						Vector collisionNormal = deltaPosition.Normalize();
						Vector otherCollisionNormal = (shape.GetCenter() - other.GetCenter()).Normalize();
						Vector collisionVelocity = Vector.Dot(shape.velocity, collisionNormal) * collisionNormal;
						Vector otherCollisionVelocity = Vector.Dot(other.velocity, otherCollisionNormal) * otherCollisionNormal;
						Vector newCollisionVelocity = collisionVelocity * (shape.Mass - other.Mass) + otherCollisionVelocity * (other.Mass + other.Mass) / (shape.Mass + other.Mass);
						Vector newOtherCollisionVelocity = otherCollisionVelocity * (other.Mass - shape.Mass) + collisionVelocity * (shape.Mass + shape.Mass) / (other.Mass + shape.Mass);

						shape.velocity += newCollisionVelocity - collisionVelocity;
						other.velocity += newOtherCollisionVelocity - otherCollisionVelocity;
					}
				}
			}
		}
	}
}