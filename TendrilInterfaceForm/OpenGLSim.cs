using System;
using OpenGL;
using Tao.FreeGlut;
//using Tao.OpenGl;
using System.Threading.Tasks;

namespace TendrilInterfaceForm
{
    class OpenGLSim
    {
        private static int width, height;
        public JonesModel JModel;
        int[,] testFile;
        int testNdx;
        float[] EncoderCounts;


        private static ShaderProgram program;
        private static VBO<Vector3> background, floor;
        private static VBO<Vector3> backgroundColor, floorColor;
        private static VBO<int> backgroundElements, floorElements;

        private static System.Diagnostics.Stopwatch watch;

        private static Cylinder[] tipSection, midSection, baseSection;
        private static int tipCylCnt, midCylCnt, baseCylCnt;
        private static float segHeight, segRadius;
        private static Vector3 tipEnd, midEnd, baseEnd;

        private static Point[] mouseLast;


        private static float yOffset;
        private static float viewAngle;
        private static int viewDistance;

        public OpenGLSim(int w, int h, int bc, int mc, int tc)
        {
            //Vector4 v = new Vector4(new Vector3(1, 1, 1), 1);
            width = w;
            height = h;

            viewDistance = 70;
            viewAngle = 0;
            segHeight = 0.5f;
            segRadius = 0.5f;
            tipCylCnt = tc;
            midCylCnt = mc;
            baseCylCnt = bc;
            yOffset = -15;

            //EncoderCounts = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            testNdx = 1;
            testFile = new int[,]
            {
                { 2, 0, 3503, 0, -2, -1, -2, -2, -2 },
                { 0, 3000, 0, 0, 800, 0, 0, 0, 800},
                { 0, 3000, 0, 800, 0, 0, 0, 0, 800},
                { 2, 0, 3503, 0, 199, 199, 300, -3, 599},
                { 2, 0, 3503, 0, 199, 199, -3, 600, 599},
                { 2, 0, 2355, 0, -2, -1, -3, 212, 253}
            };
            JModel = new JonesModel();
            JModel.Update(new float[] { 2, 0, 3503, 0, -2, -1, -2, -2, -2 });

            baseEnd = new Vector3(0, baseCylCnt * segHeight, 0);
            midEnd = new Vector3(0, (baseCylCnt + midCylCnt) * segHeight, 0);
            tipEnd = new Vector3(0, (baseCylCnt + midCylCnt + tipCylCnt) * segHeight, 0);

            mouseLast = new Point[5] { new Point(0,0), new Point(0, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0) };

            // create an OpenGL window
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("Tendril Simulation");
            Glut.glutSetOption(Glut.GLUT_ACTION_ON_WINDOW_CLOSE, Glut.GLUT_ACTION_GLUTMAINLOOP_RETURNS);
            Glut.glutKeyboardFunc(new Glut.KeyboardCallback(keyboard));
            Glut.glutMouseFunc(new Glut.MouseCallback(mouse));
            
            // provide the Glut callbacks that are necessary for running
            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(OnDisplay);
            Glut.glutCloseFunc(OnClose);

            // enable depth testing to ensure correct z-ordering of our fragments
            Gl.Enable(EnableCap.DepthTest);

            // compile the shader program
            program = new ShaderProgram(VertexShader, FragmentShader);

            // set the view and projection matrix, which are static throughout the program
            program.Use();
            program["view_matrix"].SetValue(Matrix4.CreateRotationY(viewAngle)*Matrix4.LookAt(new Vector3(0, 0, viewDistance), Vector3.Zero, Vector3.UnitY));
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));

            baseSection = CreateCylinders(baseCylCnt, 0, segRadius, new Vector3(0, 0, 1));
            midSection = CreateCylinders(midCylCnt, baseCylCnt, segRadius, new Vector3(0, 1, 0));
            tipSection = CreateCylinders(tipCylCnt, baseCylCnt + midCylCnt, segRadius, new Vector3(1, 0, 0));

            floor = new VBO<Vector3>(new Vector3[] 
            {
                new Vector3(300, 0, 300), new Vector3(-300, 0, 300), new Vector3(300, 0, -300),
                new Vector3(-300, 0, -300), new Vector3(-300, 0, 300), new Vector3(300, 0, -300)
            });
            floorColor = new VBO<Vector3>(new Vector3[]
            {
                new Vector3(.5f, .5f, .5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, .5f, .5f),
                new Vector3(.5f, .5f, .5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, .5f, .5f)
            });
            floorElements = new VBO<int>(new int[] { 0, 1, 2, 3, 4, 5 });

            background = new VBO<Vector3>(new Vector3[]
            {
                new Vector3(300, 300, 300), new Vector3(300, 300, -300), new Vector3(-300, 300, 300),
                new Vector3(-300, 300, -300), new Vector3(300, 300, -300), new Vector3(-300, 300, 300),
                new Vector3(300, 0, 300), new Vector3(-300, 0, 300), new Vector3(300, 300, 300),
                new Vector3(-300, 300, 300), new Vector3(-300, 0, 300), new Vector3(300, 300, 300),
                new Vector3(300, 0, -300), new Vector3(300, 0, 300), new Vector3(300, 300, -300),
                new Vector3(300, 300, 300), new Vector3(300, 0, 300), new Vector3(300, 300, -300),
                new Vector3(-300, 0, -300), new Vector3(300, 0, -300), new Vector3(-300, 300, -300),
                new Vector3(300, 300, -300), new Vector3(300, 0, -300), new Vector3(-300, 300, -300),
                new Vector3(-300, 0, 300), new Vector3(-300, 0, -300), new Vector3(-300, 300, 300),
                new Vector3(-300, 300, -300), new Vector3(-300, 0, -300), new Vector3(-300, 300, 300)
            });
            backgroundColor = new VBO<Vector3>(new Vector3[]
            {
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1),
                new Vector3(.2f, .2f, .2f), new Vector3(.2f, .2f, .2f), new Vector3(1, 1, 1)
            });
            backgroundElements = new VBO<int>(new int[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29
            });

            watch = System.Diagnostics.Stopwatch.StartNew();

            Glut.glutMainLoop();

        }

        private void OnRenderFrame()
        {
            // calculate how much time has elapsed since the last frame
            watch.Stop();
            float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            watch.Restart();

            //angle += deltaTime;

            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            program.Use();
            program["view_matrix"].SetValue(Matrix4.CreateRotationY(viewAngle) * Matrix4.LookAt(new Vector3(0, 0, viewDistance), Vector3.Zero, Vector3.UnitY));
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
            
            uint vertexPositionindex = (uint)Gl.GetAttribLocation(program.ProgramID, "vertexPosition");
            Gl.EnableVertexAttribArray(vertexPositionindex);

            DrawCylinders(vertexPositionindex);

            // transform and draw my background
            program["model_matrix"].SetValue(Matrix4.CreateTranslation(new Vector3(0, yOffset, 0)));
            Gl.BindBufferToShaderAttribute(background, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(backgroundColor, program, "vertexColor");
            Gl.BindBuffer(backgroundElements);

            // draw the square
            Gl.DrawElements(BeginMode.Triangles, backgroundElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            // transform and draw my floor
            program["model_matrix"].SetValue(Matrix4.CreateTranslation(new Vector3(0, yOffset, 0)));
            Gl.BindBufferToShaderAttribute(floor, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(floorColor, program, "vertexColor");
            Gl.BindBuffer(floorElements);

            // draw the square
            Gl.DrawElements(BeginMode.Triangles, floorElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            Glut.glutSwapBuffers();


        }

        

        private void mouse(int button, int state, int x, int y)
        {
            int xDist = 0, yDist = 0;
            //Console.Write(button.ToString() + ", " + state.ToString() + ", ");
            //Console.WriteLine(x.ToString() + ", " + y.ToString());

            if (state == 0)
            {
                mouseLast[button].X = x;
                mouseLast[button].Y = y;
            } else
            {
                switch (button)
                {
                    case 0:

                        break;
                    case 1:
                        if (testNdx == 6) testNdx = 0;
                        float[] cnts = new float[9]; 
                        for (int ndx = 0; ndx < 9; ndx++) cnts[ndx] = testFile[testNdx,ndx];
                        testNdx++;
                        JModel.Update(cnts);
                        break;
                    case 2:
                        xDist = x - mouseLast[button].X;
                        viewAngle += (float)xDist / 100;
                        //Console.WriteLine(viewAngle.ToString());
                        break;
                    case 3:
                        viewDistance -= 5;
                        break;
                    case 4:
                        viewDistance += 5;
                        break;
                    default:

                        break;
                }
            }

        }

        private void keyboard(byte key, int x, int y)  // Create Keyboard Function
        {

            switch (key)
            {
                case 27:              // When Escape Is Pressed...
                    //exit(0);   // Exit The Program
                    break;
                case 1:             // stub for new screen
                    //printf("New screen\n");
                    break;
                default:
                    break;
            }
        }

        private static Cylinder[] CreateCylinders(int cnt, int prev, float r, Vector3 color)
        {
            Cylinder[] cylinders = new Cylinder[cnt];
            if (cnt == 0) return cylinders;
            else
            {
                for (int ndx = 0; ndx < cnt; ndx++)
                {
                    cylinders[ndx] = new Cylinder(new Vector3(0, 0, 0), segHeight, r, 72, 10, color);
                }
            }
            return cylinders;
        }

        public void SetCounts(float[] cnts)
        {
            EncoderCounts = cnts;
        }

        private void DrawCylinders(uint vertexPositionindex)
        {
            float byAngle, myAngle, tyAngle;
            float bxAngle, mxAngle, txAngle;
            Vector3 bTrans, mTrans, tTrans;
            Matrix4 bH, mH, tH;

            //JModel.Update(EncoderCounts);

            byAngle = (float)JModel.phi_bm;
            myAngle = (float)JModel.phi_mm;
            tyAngle = (float)JModel.phi_tm; // (float)Math.PI / 2;

            for (int ndx = 0; ndx < baseCylCnt; ndx++)
            {
                bxAngle = (float)JModel.k_bm * segHeight / baseCylCnt * ndx;
                bTrans = new Vector3(0, (ndx) * segHeight * (float)Math.Cos(bxAngle/2), (ndx) * segHeight * (float)Math.Sin(bxAngle/2));
                bH = Matrix4.CreateRotationX(bxAngle) * Matrix4.CreateTranslation(bTrans);
                bH = Matrix4.CreateRotationY(-byAngle) * bH * Matrix4.CreateRotationY(byAngle);
                program["model_matrix"].SetValue(bH * Matrix4.CreateTranslation(new Vector3(0, yOffset, 0)));

                Gl.BindBuffer(baseSection[ndx].vertices);
                Gl.VertexAttribPointer(vertexPositionindex, baseSection[ndx].vertices.Size, baseSection[ndx].vertices.PointerType, true, 12, IntPtr.Zero);
                Gl.BindBufferToShaderAttribute(baseSection[ndx].vertColors, program, "vertexColor");
                Gl.BindBuffer(baseSection[ndx].vertElements);
                Gl.DrawElements(BeginMode.Points, baseSection[ndx].vertElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

            bxAngle = (float)JModel.k_bm * segHeight;
            bTrans = new Vector3(0, baseCylCnt * segHeight * (float)Math.Cos(bxAngle / 2), baseCylCnt * segHeight * (float)Math.Sin(bxAngle / 2));
            bH = Matrix4.CreateRotationX(bxAngle) * Matrix4.CreateTranslation(bTrans);
            bH = Matrix4.CreateRotationY(-byAngle) * bH * Matrix4.CreateRotationY(byAngle);

            for (int ndx = 0; ndx < midCylCnt; ndx++)
            {
                mxAngle = (float)JModel.k_mm * segHeight / midCylCnt * ndx;
                mTrans = new Vector3(0, (ndx) * segHeight * (float)Math.Cos(mxAngle/2), (ndx) * segHeight * (float)Math.Sin(mxAngle / 2));
                mH = Matrix4.CreateRotationX(mxAngle) * Matrix4.CreateTranslation(mTrans);
                mH = Matrix4.CreateRotationY(-myAngle) * mH * Matrix4.CreateRotationY(myAngle);

                program["model_matrix"].SetValue(mH * bH * Matrix4.CreateTranslation(new Vector3(0, yOffset, 0)));

                Gl.BindBuffer(midSection[ndx].vertices);
                Gl.VertexAttribPointer(vertexPositionindex, midSection[ndx].vertices.Size, midSection[ndx].vertices.PointerType, true, 12, IntPtr.Zero);
                Gl.BindBufferToShaderAttribute(midSection[ndx].vertColors, program, "vertexColor");
                Gl.BindBuffer(midSection[ndx].vertElements);
                Gl.DrawElements(BeginMode.Points, midSection[ndx].vertElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

            mxAngle = (float)JModel.k_mm * segHeight;
            mTrans = new Vector3(0, midCylCnt * segHeight * (float)Math.Cos(mxAngle / 2), midCylCnt * segHeight * (float)Math.Sin(mxAngle / 2));
            mH = Matrix4.CreateRotationX(mxAngle) * Matrix4.CreateTranslation(mTrans);
            mH = Matrix4.CreateRotationY(-myAngle) * mH * Matrix4.CreateRotationY(myAngle);

            for (int ndx = 0; ndx < tipCylCnt; ndx++)
            {
                
                txAngle = (float)JModel.k_tm * segHeight / tipCylCnt * ndx; // -(float)Math.PI / 8 / tipCylCnt * ndx;
                tTrans = new Vector3(0, (ndx) * segHeight * (float)Math.Cos(txAngle / 2), (ndx) * segHeight * (float)Math.Sin(txAngle / 2));
                tH = Matrix4.CreateRotationX(txAngle) * Matrix4.CreateTranslation(tTrans);
                tH = Matrix4.CreateRotationY(-tyAngle) * tH * Matrix4.CreateRotationY(tyAngle);
                program["model_matrix"].SetValue(tH * mH * bH * Matrix4.CreateTranslation(new Vector3(0, yOffset, 0)));

                Gl.BindBuffer(tipSection[ndx].vertices);
                Gl.VertexAttribPointer(vertexPositionindex, tipSection[ndx].vertices.Size, tipSection[ndx].vertices.PointerType, true, 12, IntPtr.Zero);
                Gl.BindBufferToShaderAttribute(tipSection[ndx].vertColors, program, "vertexColor");
                Gl.BindBuffer(tipSection[ndx].vertElements);
                Gl.DrawElements(BeginMode.Points, tipSection[ndx].vertElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }
        }


        private static void OnClose()
        {
            // dispose of all of the resources that were created

            for (int ndx = 0; ndx < baseCylCnt; ndx++)
            {
                baseSection[ndx].DisposeCylinder();
            }
            for (int ndx = 0; ndx < midCylCnt; ndx++)
            {
                midSection[ndx].DisposeCylinder();
            }
            for (int ndx = 0; ndx < tipCylCnt; ndx++)
            {
                tipSection[ndx].DisposeCylinder();
            }
            program.DisposeChildren = true;
            program.Dispose();
        }

        private static void OnDisplay()
        {
            //Glut.glutWireCylinder(2, 2, 30, 30);
        }
        
        public static string VertexShader = @"
#version 130

in vec3 vertexPosition;
in vec3 vertexColor;

out vec3 color;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void)
{
    color = vertexColor;
    gl_Position = projection_matrix * (view_matrix * (model_matrix * vec4(vertexPosition, 1)));
}
";

        public static string FragmentShader = @"
#version 130

in vec3 color;

out vec4 fragment;

void main(void)
{
    fragment = vec4(color, 1);
    //gl_FragColor = vec4(color, 1);
}
";
    }
}
