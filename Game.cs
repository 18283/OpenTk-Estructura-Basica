using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using BasicOpenTK; 

namespace ConsoleApp6
{
    public class Game : GameWindow
    {
        private int shaderProgramObject;
        private Objeto objeto; 
        private Matrix4 model;
        private Matrix4 view;
        private Matrix4 projection;
        private float rotationAngle;
        private float rotationSpeed = 0.3f; // Velocidad de rotación

        public Game()
            : base(GameWindowSettings.Default, NativeWindowSettings.Default)
            //: base(DisplayDivice, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(600, 600));
            objeto = new Objeto(); // Inicializar 'objeto' aquí
        }


        protected override void OnLoad()
        {
            GL.ClearColor(new Color4(1.0f, 1.0f, 1.0f, 1.0f));
            GL.Enable(EnableCap.DepthTest);

            string vertexShaderCode =
                @"
                 #version 330
                 uniform mat4 model;
                 uniform mat4 view;
                 uniform mat4 projection;
                 in vec3 position;
                 in vec3 color;
                 out vec3 fragColor;
                 void main()
                 {
                     gl_Position = projection * view * model * vec4(position, 1.0);
                     fragColor = color;
                 }
                 ";

            string fragmentShaderCode =
                @"
                 #version 330
                 in vec3 fragColor;
                 out vec4 color;
                 void main()
                 {
                     color = vec4(fragColor, 1.0);
                 }
                 ";
            ////////
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderCode);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderCode);
            GL.CompileShader(fragmentShader);

            shaderProgramObject = GL.CreateProgram();
            GL.AttachShader(shaderProgramObject, vertexShader);
            GL.AttachShader(shaderProgramObject, fragmentShader);
            GL.LinkProgram(shaderProgramObject);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            //////
            objeto = new Objeto();

            view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f), Vector3.Zero, Vector3.UnitY);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            rotationAngle += rotationSpeed * (float)args.Time;
            model = Matrix4.CreateRotationY(rotationAngle);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Dibujar el objeto
            objeto.Dibujar(shaderProgramObject, model, view, projection);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);
            GL.Viewport(0, 0, Size.X, Size.Y);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            objeto.Dispose(); 
            GL.DeleteProgram(shaderProgramObject);
            base.OnUnload();
        }
    }
}
