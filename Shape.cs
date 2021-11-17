using System.Runtime.InteropServices;
using static OpenGL.Gl;

namespace SharpEngine {
	public class Shape {
            
		Vertex[] vertices;
		uint vertexArray;
		uint vertexBuffer;

		public Transform Transform { get; }
		public Material material;

		float mass;
		float massInverse;
		public float Mass {
			get => this.mass;
			set {
				this.mass = value;
				this.massInverse = float.IsPositiveInfinity(value) ? 0f : 1f / value;
			}
		}
		public float MassInverse => this.massInverse;
		
		public float gravityScale = 1f;
		public Vector velocity; // momentum = product of velocity and mass
		public Vector linearForce;
		
		public Shape(Vertex[] vertices, Material material) {
			this.vertices = vertices;
			this.material = material;
			LoadTriangleIntoBuffer();
			this.Transform = new Transform();
		}

		public void SetColor(Color color) {
			for (int i = 0; i < this.vertices.Length; i++) {
				vertices[i].color = color;
			}
		}
		
		 void LoadTriangleIntoBuffer() {
			vertexArray = glGenVertexArray();
			vertexBuffer = glGenBuffer();
			glBindVertexArray(vertexArray);
			glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
			glVertexAttribPointer(0, 3, GL_FLOAT, false, Marshal.SizeOf<Vertex>(), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
			glVertexAttribPointer(1, 4, GL_FLOAT, false, Marshal.SizeOf<Vertex>(), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));
			glEnableVertexAttribArray(0);
			glEnableVertexAttribArray(1);
			glBindVertexArray(0);
		}

		public Vector GetMinBounds() {
			var min = this.Transform.Matrix * this.vertices[0].position;
			for (var i = 1; i < this.vertices.Length; i++) {
				min = Vector.Min(min, this.Transform.Matrix * this.vertices[i].position);
			}
			return min;
		}
            
		public Vector GetMaxBounds() {
			var max = this.Transform.Matrix * this.vertices[0].position;
			for (var i = 1; i < this.vertices.Length; i++) {
				max = Vector.Max(max, this.Transform.Matrix * this.vertices[i].position);
			}

			return max;
		}

		public Vector GetCenter() {
			return (GetMinBounds() + GetMaxBounds()) / 2;
		}

		public unsafe void Render() {
			this.material.Use();
			this.material.SetTransform(this.Transform.Matrix);
			glBindVertexArray(vertexArray);
			glBindBuffer(GL_ARRAY_BUFFER, this.vertexBuffer);
			fixed (Vertex* vertex = &this.vertices[0]) {
				glBufferData(GL_ARRAY_BUFFER, Marshal.SizeOf<Vertex>() * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
			}
			glDrawArrays(GL_TRIANGLES, 0, this.vertices.Length);
			glBindVertexArray(0);
		}
	}
}