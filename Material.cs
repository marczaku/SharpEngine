using System.IO;
using static OpenGL.Gl;

namespace SharpEngine {
	public class Material {
		readonly uint program;

		public Material(string vertexShaderPath, string fragmentShaderPath) {
			// create vertex shader
			var vertexShader = glCreateShader(GL_VERTEX_SHADER);
			glShaderSource(vertexShader, File.ReadAllText(vertexShaderPath));
			glCompileShader(vertexShader);

			// create fragment shader
			var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
			glShaderSource(fragmentShader, File.ReadAllText(fragmentShaderPath));
			glCompileShader(fragmentShader);

			// create shader program - rendering pipeline
			program = glCreateProgram();
			glAttachShader(program, vertexShader);
			glAttachShader(program, fragmentShader);
			glLinkProgram(program);
			glDeleteShader(vertexShader);
			glDeleteShader(fragmentShader);
		}

		public void Use() {
			glUseProgram(program);
		}
	}
}