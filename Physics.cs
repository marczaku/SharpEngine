using System;
using System.Diagnostics;

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
					bool collision = deltaPosition.GetSquareMagnitude() <= (shape.Radius + other.Radius) * (shape.Radius + other.Radius);

					if (collision) {
						Vector collisionNormal = deltaPosition.Normalize();
						Vector shapeVelocity = Vector.Dot(shape.velocity, collisionNormal) * collisionNormal;
						Vector otherVelocity = Vector.Dot(other.velocity, collisionNormal) * collisionNormal;
						
						float totalMass = other.Mass + shape.Mass;

						Vector velocityChange = 2*other.Mass/totalMass * (otherVelocity - shapeVelocity);
						Vector otherVelocityChange = 2 * shape.Mass / totalMass * (shapeVelocity - otherVelocity); //-shape.Mass / other.Mass * velocityChange;

						AssertPhysicalCorrectness(shape.Mass, shape.velocity, other.Mass, other.velocity, shape.Mass, shape.velocity + velocityChange, other.Mass, other.velocity + otherVelocityChange);

						shape.velocity += velocityChange;
						other.velocity += otherVelocityChange;
					}
				}
			}
		}

		static Vector CalculateTotalMomentum(float m1, Vector v1, float m2, Vector v2) {
			return CalculateMomentum(m1, v1) + CalculateMomentum(m2, v2);
		}
		
		static Vector CalculateMomentum(float mass, Vector velocity) {
			return mass * velocity;
		}

		static float CalculateTotalKineticEnergy(float m1, Vector v1, float m2, Vector v2) {
			return CalculateKineticEnergy(m1, v1) + CalculateKineticEnergy(m2, v2);
		}

		static float CalculateKineticEnergy(float mass, Vector velocity) {
			return 0.5f * mass * velocity.GetSquareMagnitude();
		}

		static void AssertPhysicalCorrectness(float m1, Vector v1, float m2, Vector v2, float m1_, Vector v1_, float m2_, Vector v2_, float tolerance = 0.00001f) {
			AssertPreservationOfMomentum(m1, v1, m2, v2, m1_, v1_, m2_, v2_);
			AssertPreservationOfKineticEnergy(m1, v1, m2, v2, m1_, v1_, m2_, v2_);
		}

		static void AssertPreservationOfKineticEnergy(float m1, Vector v1, float m2, Vector v2, float m1_, Vector v1_, float m2_, Vector v2_, float tolerance = 0.00001f) {
			float oldTotalKineticEnergy = CalculateTotalKineticEnergy(m1, v1, m2, v2);
			float newTotalKineticEnergy = CalculateTotalKineticEnergy(m1_, v1_, m2_, v2_);
			Debug.Assert(MathF.Abs(oldTotalKineticEnergy - newTotalKineticEnergy) < tolerance, $"Kinetic energy was not preserved. Old: {oldTotalKineticEnergy} New: {newTotalKineticEnergy}");
		}

		static void AssertPreservationOfMomentum(float m1, Vector v1, float m2, Vector v2, float m1_, Vector v1_, float m2_, Vector v2_, float tolerance = 0.00001f) {
			Vector oldMomentum = CalculateTotalMomentum(m1, v1, m2, v2);
			Vector newMomentum = CalculateTotalMomentum(m1_, v1_, m2_, v2_);
			Debug.Assert((oldMomentum - newMomentum).GetMagnitude() < tolerance, $"Momentum was not preserved. Old: {oldMomentum} New: {newMomentum}");
		}
	}
}