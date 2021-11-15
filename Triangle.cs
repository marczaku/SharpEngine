using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;

namespace SharpEngine {
	public class Triangle {
            
		Vertex[] vertices;
		Matrix transform = Matrix.Identity;
		uint vertexArray;
		uint vertexBuffer;

		public float CurrentScale { get; private set; }

		public Material material;
            
		public Triangle(Vertex[] vertices, Material material) {
			this.vertices = vertices;
			this.material = material;
			LoadTriangleIntoBuffer();
			this.CurrentScale = 1f;
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
			var min = this.vertices[0].position;
			for (var i = 1; i < this.vertices.Length; i++) {
				min = Vector.Min(min, this.vertices[i].position);
			}
			return min;
		}
            
		public Vector GetMaxBounds() {
			var max = this.vertices[0].position;
			for (var i = 1; i < this.vertices.Length; i++) {
				max = Vector.Max(max, this.vertices[i].position);
			}

			return max;
		}

		public Vector GetCenter() {
			return (GetMinBounds() + GetMaxBounds()) / 2;
		}

		public void Scale(float multiplier) {

		}

		public void Move(Vector direction) {
			this.transform *= Matrix.Translation(direction);
		}

		public unsafe void Render() {
			this.material.Use();
			this.material.SetTransform(this.transform);
			glBindVertexArray(vertexArray);
			glBindBuffer(GL_ARRAY_BUFFER, this.vertexBuffer);
			fixed (Vertex* vertex = &this.vertices[0]) {
				glBufferData(GL_ARRAY_BUFFER, Marshal.SizeOf<Vertex>() * this.vertices.Length, vertex, GL_DYNAMIC_DRAW);
			}
			glDrawArrays(GL_TRIANGLES, 0, this.vertices.Length);
			glBindVertexArray(0);
		}

		public void Rotate(float rotation) {
			
		}
	}
}