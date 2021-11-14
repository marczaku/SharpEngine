using GLFW;
using static OpenGL.Gl;

namespace SharpEngine {
	public class Window {
		readonly GLFW.Window window;
		Scene scene;

		public bool IsOpen() => !Glfw.WindowShouldClose(window);

		public void Load(Scene scene) {
			this.scene = scene;
		}
		
		public Window() {
			// initialize and configure
			Glfw.Init();
			Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
			Glfw.WindowHint(Hint.ContextVersionMajor, 3);
			Glfw.WindowHint(Hint.ContextVersionMinor, 3);
			Glfw.WindowHint(Hint.Decorated, true);
			Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
			Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
			Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

			// create and launch a window
			window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, GLFW.Window.None);
			Glfw.MakeContextCurrent(window);
			OpenGL.Gl.Import(Glfw.GetProcAddress);
		}

		static void ClearScreen() {
			glClearColor(.2f, .05f, .2f, 1);
			glClear(GL_COLOR_BUFFER_BIT);
		}

		public void Render() {
			Glfw.PollEvents(); // react to window changes (position etc.)
			ClearScreen();
			this.scene?.Render();
			Glfw.SwapBuffers(window);
		}
	}
}