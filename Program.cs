using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_immediate_mode
{
    class Program : GameWindow
    {

        private const int XYZ_SIZE = 25;
        public Program() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            KeyUp += Keyboard_KeyUp;
            VSync = VSyncMode.On;

            Console.WriteLine("OpenGl versiunea: " + GL.GetString(StringName.Version));
            Title = "OpenGl versiunea: " + GL.GetString(StringName.Version) + " (mod imediat)";

        }

        private int[,] objVertices = {
            {5, 10, 5,
                10, 5, 10,
                5, 10, 5,
                10, 5, 10,
                5, 5, 5,
                5, 5, 5,
                5, 10, 5,
                10, 10, 5,
                10, 10, 10,
                10, 10, 10,
                5, 10, 5,
                10, 10, 5},
            {5, 5, 12,
                5, 12, 12,
                5, 5, 5,
                5, 5, 5,
                5, 12, 5,
                12, 5, 12,
                12, 12, 12,
                12, 12, 12,
                5, 12, 5,
                12, 5, 12,
                5, 5, 12,
                5, 12, 12},
            {6, 6, 6,
                6, 6, 6,
                6, 6, 12,
                6, 12, 12,
                6, 6, 12,
                6, 12, 12,
                6, 6, 12,
                6, 12, 12,
                6, 6, 12,
                6, 12, 12,
                12, 12, 12,
                12, 12, 12}};
        private Color[] colorVertices = { Color.YellowGreen, Color.YellowGreen, Color.Tomato, Color.Tomato, Color.OldLace, Color.OldLace, Color.Olive, Color.Olive, Color.PeachPuff, Color.PeachPuff, Color.MediumAquamarine, Color.MediumAquamarine };


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            Matrix4 lookat = Matrix4.LookAt(30, 30, 30, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);


        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyboard[Key.Escape])
            {
                Exit();
            }

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);


            DrawAxes();

            DrawObjects();

            SwapBuffers();
        }
        private void DrawAxes()
        {
            //Tema laborator 3 - punctele 3 si 4
            
            //seteaza dimensiunea la 5 pentru un alt grup de puncte
            GL.PointSize(5.0f);

            GL.Begin(PrimitiveType.Lines);
            
            //nu se modifica liniile
            //GL.Begin(PrimitiveType.LineLoop);
            //nu se modifica liniile
            //GL.Begin(PrimitiveType.LineStrip);
            //dispar liniile
            //GL.Begin(PrimitiveType.TriangleFan);
            //creeaza doua triunghiuri lipite unul de altul
            //GL.Begin(PrimitiveType.TriangleStrip);
            
            // Desenează axa Ox (cu roșu).
            GL.Color3(Color.Aquamarine);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(XYZ_SIZE, 0, 0);
            //GL.PointSize(5.0f);

            // Desenează axa Oy (cu galben).
            GL.Color3(Color.CornflowerBlue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, XYZ_SIZE, 0); ;

            // Desenează axa Oz (cu verde).
            GL.Color3(Color.YellowGreen);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, XYZ_SIZE);
            //GL.LineWidth(5.0f);

            GL.End();
           
            //liniile devin mai groase/subtiri in functie de valoarea setata
            GL.LineWidth(5.0f);
        }

        private void DrawObjects()
        {

            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < 35; i = i + 3)
            {
                GL.Color3(colorVertices[i / 3]);
                GL.Vertex3(objVertices[0, i], objVertices[1, i], objVertices[2, i]);
                GL.Vertex3(objVertices[0, i + 1], objVertices[1, i + 1], objVertices[2, i + 1]);
                GL.Vertex3(objVertices[0, i + 2], objVertices[1, i + 2], objVertices[2, i + 2]);
            }
            GL.End();

        }

        //TEMA
        //laboratorul 2 - punctul 2: controlul obiectului randat prin apăsarea a 2 taste
        //tasta F8 maximizeaza fereastra; tasta F7 minimizeaza fereastra

        void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.F8)
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;

            if (e.Key == Key.F7)
                if (this.WindowState == WindowState.Minimized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Minimized;
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (Program example = new Program())
            {
                example.Run(30.0, 0.0);
            }
        }
    }

}