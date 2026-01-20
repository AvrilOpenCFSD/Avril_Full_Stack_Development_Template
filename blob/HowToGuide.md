Execute Client 
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/engine/Execute.cs
````
namespace Avril_FSD.ClientAssembly
{
    public class Execute
    {
        private Avril_FSD.ClientAssembly.Execute_Control _execute_Control;
        private Avril_FSD.ClientAssembly.Networking_Client _networking_Client;
        private IntPtr programId_ConcurrentQue_C;
        private IntPtr programId_WriteQue_C_IA;
        private IntPtr programId_WriteQue_C_OR;

        private Thread[] _threads = {null, null, null, null, null, null};//number of app shell threads.

        public Execute(int numberOfCores) 
        {
            Set_execute_Control(null);
        }

        public void Initialise_Control(int numberOfCores, Global global)
        {
            Set_execute_Control(new Avril_FSD.ClientAssembly.Execute_Control(numberOfCores));
            while (Get_execute_Control() == null) { }
        }

        public void Initialise_NetworkingPipes(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            Set_networking_Client(new Avril_FSD.ClientAssembly.Networking_Client());
            Get_networking_Client().Initialise_networking_Client();
        }

        public void Initialise_Libraries()
        {
            programId_ConcurrentQue_C = Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Initialise_LaunchEnableForConcurrentThreadsAt();
            System.Console.WriteLine("created Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT");

            programId_WriteQue_C_IA = Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTINPUTACTION.Initialise_WriteEnable();
            System.Console.WriteLine("created Library_For_WriteEnableForThreadsAt_CLIENTINPUTACTION");

            programId_WriteQue_C_OR = Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE.Initialise_WriteEnable();
            System.Console.WriteLine("created Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE");

        }
        public void Initialise_Threads(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            byte threadIdCounter = 0;
            obj.Get_client().Get_execute().Set_thread(threadIdCounter, Thread.CurrentThread);
            obj.Get_client().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(obj, threadIdCounter, false);
            System.Console.WriteLine("Thread Initalised => CurrentThread()" + (threadIdCounter).ToString());//TESTBENCH

            threadIdCounter++;
            obj.Get_client().Get_execute().Set_thread(threadIdCounter, new Thread(() => _networking_Client.Thread_IO_Client(threadIdCounter)));
            obj.Get_client().Get_execute().Get_thread(threadIdCounter).Start();
            System.Console.WriteLine("Thread Initalised => Thread_IO_Client on core " + (threadIdCounter).ToString());//TESTBENCH

            threadIdCounter++;
            while (threadIdCounter < obj.Get_client().Get_global().Get_numberOfCores())
            {
                obj.Get_client().Get_execute().Set_thread(threadIdCounter, new Thread(() => obj.Get_client().Get_algorithms().Get_concurrent((byte)(threadIdCounter - (byte)2)).Thread_Concurrent(threadIdCounter)));
                obj.Get_client().Get_execute().Get_thread(threadIdCounter).Start();
                System.Console.WriteLine("Thread Initalised => Thread_Concurrent on core " + (threadIdCounter).ToString());//TESTBENCH
                threadIdCounter++;
            }
        }

        public void Create_And_Run_Graphics(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            System.Console.WriteLine("starting = > gameInstance");//TESTBENCH
            using (Avril_FSD.ClientAssembly.Game_Instance gameInstance = new Avril_FSD.ClientAssembly.Game_Instance())
            {
                gameInstance.Run(obj.Get_client().Get_data().Get_settings().Get_refreshRate());
            }
        }

        public Avril_FSD.ClientAssembly.Execute_Control Get_execute_Control()
        {
            return _execute_Control;
        }
        public Avril_FSD.ClientAssembly.Networking_Client Get_networking_Client()
        {
            return _networking_Client;
        }


        public IntPtr Get_program_ConcurrentQue_C()
        {
            return programId_ConcurrentQue_C;

        }
        public IntPtr Get_program_WriteQue_C_IA()
        {
            return programId_WriteQue_C_IA;
        }
        public IntPtr Get_program_WriteQue_C_OR()
        {
            return programId_WriteQue_C_OR;
        }
        public Thread Get_thread(byte index)
        {
            return _threads[index];
        }

        private void Set_execute_Control(Avril_FSD.ClientAssembly.Execute_Control execute_Control)
        {
            _execute_Control = execute_Control;
        }
        private void Set_networking_Client(Avril_FSD.ClientAssembly.Networking_Client networking_Client)
        {
            _networking_Client = networking_Client;
        }
        private void Set_program_ConcurrentQue_C(IntPtr programID)
        {
            programId_ConcurrentQue_C = programID;
        }
        private void Set_programId_WriteQue_C_IA(IntPtr programId)
        {
            programId_WriteQue_C_IA = programId;
        }
        private void Set_programId_WriteQue_C_OR(IntPtr programId)
        {
            programId_WriteQue_C_OR = programId;
        }
        private void Set_thread(byte index, Thread thread) 
        {
            _threads[index] = thread;
        }
    }   
}
````
Execute Server
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/engine/Execute.cs
````
namespace Avril_FSD.ServerAssembly
{
    public class Execute
    {
        private Avril_FSD.ServerAssembly.Execute_Control _execute_Control;
        private Avril_FSD.ServerAssembly.Networking_Server _networking_Server;
        private IntPtr _program_ServerConcurrency;
        private Thread[] _threads = {null, null};
        
        public Execute(int numberOfCores) 
        {
            Set_execute_Control(null);
        }

        public void Initialise_Control(int numberOfCores, Global global)
        {
            Set_execute_Control(new Avril_FSD.ServerAssembly.Execute_Control(numberOfCores));
            while (Get_execute_Control() == null) { }
        }

        public void Initialise_Libraries(Avril_FSD.ServerAssembly.Framework_Server obj)
        {
            obj.Get_server().Get_execute().Set_program_ServerConcurrency(Avril_FSD.Library_For_Server_Concurrency.Initialise_Server_Concurrency());
            System.Console.WriteLine("created server concurrency.");//TESTBENCH
        }

        public void Initialise_NetworkingPipes(Avril_FSD.ServerAssembly.Framework_Server obj)
        {
            obj.Get_server().Get_execute().Set_networking_Server(new Avril_FSD.ServerAssembly.Networking_Server());
            obj.Get_server().Get_execute().Get_networking_Server().Initialise_networking_Server();
        }

        public void Initialise_Threads(Avril_FSD.ServerAssembly.Framework_Server obj)
        {
            byte threadIdCounter = 0;
            obj.Get_server().Get_execute().Set_thread(threadIdCounter, Thread.CurrentThread);
            
            threadIdCounter++;
            obj.Get_server().Get_execute().Set_thread(threadIdCounter, new Thread(() => obj.Get_server().Get_execute().Get_networking_Server().Thread_IO_Server(threadIdCounter)));
            obj.Get_server().Get_execute().Get_thread(threadIdCounter).Start();
            System.Console.WriteLine("starting = > Thread_IO_Server on core " + (threadIdCounter).ToString());//TESTBENCH
        }

        public void Create_And_Run_Graphics(Avril_FSD.ServerAssembly.Framework_Server obj)
        {
            System.Console.WriteLine("starting = > gameInstance");//TESTBENCH
            using (Avril_FSD.ServerAssembly.Game_Instance gameInstance = new Avril_FSD.ServerAssembly.Game_Instance())
            {
                gameInstance.Run(obj.Get_server().Get_data().Get_settings().Get_refreshRate());
            }
        }

        public Avril_FSD.ServerAssembly.Execute_Control Get_execute_Control()
        {
            return _execute_Control;
        }
        public Avril_FSD.ServerAssembly.Networking_Server Get_networking_Server()
        {
            return _networking_Server;
        }

        public Thread Get_thread(int index)
        {
            return _threads[index];
        }
        public IntPtr Get_program_ServerConcurrency()
        {
            return _program_ServerConcurrency;
        }

        private void Set_execute_Control(Avril_FSD.ServerAssembly.Execute_Control execute_Control)
        {
            _execute_Control = execute_Control;
        }
        public void Set_networking_Server(Avril_FSD.ServerAssembly.Networking_Server networking_Server)
        {
            _networking_Server = networking_Server;
        }
        private void Set_thread(byte index, Thread thread) 
        {
            _threads[index] = thread;
        }

        private void Set_program_ServerConcurrency(IntPtr handle)
        {
            _program_ServerConcurrency = handle;
        }
    }   
}
````
Loading Server Shell Framework
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/gameInstance/Game_Instance.cs
````
namespace Avril_FSD.ServerAssembly
{
    public sealed class Game_Instance : GameWindow
    {
        private bool done_once;
        private byte _coreId;
        private readonly string _title;
        private double _time;
        private readonly Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private FirstPersonCamera _cameraFP;
        private ThirdPersonCamera _cameraTP;
        private Matrix4 _projectionMatrix;
        private float _fov = 45f;

        private KeyboardState _lastKeyboardState;
        private MouseState _lastMouseState;

        private GameObjectFactory _gameObjectFactory;
        private readonly List<AGameObject> _gameObjects = new List<AGameObject>();
        
        
        private ShaderProgram _texturedProgram;
        private ShaderProgram _solidProgram;
        
        private bool cameraSelector = false;
       
        public Game_Instance()
            : base(960, // initial width
                540, // initial height
                GraphicsMode.Default,
                "",  // initial title
                GameWindowFlags.FixedWindow,
                DisplayDevice.Default,
                4, // OpenGL major version
                5, // OpenGL minor version
                GraphicsContextFlags.ForwardCompatible)
        {
            _title += "dreamstatecoding.blogspot.com: OpenGL Version: " + GL.GetString(StringName.Version);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CreateProjection();
        }

        protected override void OnLoad(EventArgs e)
        {
            System.Console.WriteLine("OnLoad");
            VSync = VSyncMode.Off;
            CreateProjection();
#if DEBUG
            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\APP_ServerAssembly\\graphics\\Shaders\\1Vert\\simplePipeVert.c");
            _solidProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\APP_ServerAssembly\\graphics\\Shaders\\5Frag\\simplePipeFrag.c");
            _solidProgram.Link();

            _texturedProgram = new ShaderProgram();
            _texturedProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\APP_ServerAssembly\\graphics\\Shaders\\1Vert\\simplePipeTexVert.c");
            _texturedProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\APP_ServerAssembly\\graphics\\Shaders\\5Frag\\simplePipeTexFrag.c");
            _texturedProgram.Link();

            var models = new Dictionary<string, ARenderable>();
            models.Add("Player", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\gameover.png", 8));
            models.Add("Wooden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\wooden.png", 8));
            models.Add("Golden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\golden.bmp", 8));
            models.Add("Asteroid", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\moonmap1k.jpg", 8));
            models.Add("Spacecraft", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\spacecraft.png", 8));
            models.Add("Gameover", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\gameover.png", 8));
            models.Add("Bullet", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\dotted.png", 8));
#else
            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\APP_ServerAssembly\\graphics\\Shaders\\1Vert\\simplePipeVert.c");
            _solidProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\APP_ServerAssembly\\graphics\\Shaders\\5Frag\\simplePipeFrag.c");
            _solidProgram.Link();

            _texturedProgram = new ShaderProgram();
            _texturedProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\APP_ServerAssembly\\graphics\\Shaders\\1Vert\\simplePipeTexVert.c");
            _texturedProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\APP_ServerAssembly\\graphics\\Shaders\\5Frag\\simplePipeTexFrag.c");
            _texturedProgram.Link();

            var models = new Dictionary<string, ARenderable>();
            models.Add("Player", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\gameover.png", 8));
            models.Add("Wooden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\wooden.png", 8));
            models.Add("Golden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\golden.bmp", 8));
            models.Add("Asteroid", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\moonmap1k.jpg", 8));
            models.Add("Spacecraft", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\spacecraft.png", 8));
            models.Add("Gameover", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\gameover.png", 8));
            models.Add("Bullet", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ServerAssembly\\graphics\\Textures\\dotted.png", 8));
#endif
            //models.Add("TestObject", new TexturedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side.jpg"));
            //models.Add("TestObjectGen", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side.jpg", 8));
            //models.Add("TestObjectPreGen", new MipMapManualRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side mipmap levels 0 to 8.bmp", 9));

            _gameObjectFactory = new GameObjectFactory(models);

            _gameObjectFactory.Create_PlayerOnClient();
            _gameObjects.Add(_gameObjectFactory.Get_player());

            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Wooden", new Vector3(10f, 0f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Wooden", new Vector3(-10f, 0f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Golden", new Vector3(0f, 10f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Golden", new Vector3(0f, -10f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Asteroid", new Vector3(0f, 0f, 10f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Asteroid", new Vector3(0f, 0f, -10f), new Vector3(1f)));

            //_camera = new StaticCamera();

            _gameObjectFactory.Get_player().Create_Cameras();

            CursorVisible = false;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);            
            Closed += OnClosed;

            Avril_FSD.ServerAssembly.Framework_Server obj = Avril_FSD.ServerAssembly.Program.Get_framework_Server();
            obj.Get_server().Get_execute().Initialise_Threads(obj);

            System.Console.WriteLine("OnLoad .. done");
        }

        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();
        }

        public override void Exit()
        {
            System.Console.WriteLine("Exit called");
            Avril_FSD.ServerAssembly.Framework_Server obj = Avril_FSD.ServerAssembly.Program.Get_framework_Server();
            obj.Get_server().Get_execute().Get_execute_Control().Set_exitApplication(true);
            obj.Get_server().Get_execute().Get_networking_Server().DeInitialise_networking_Server();
            _gameObjectFactory.Dispose();
            _solidProgram.Dispose();
            _texturedProgram.Dispose();
            base.Exit();
        }

        private void CreateProjection()
        {

            var aspectRatio = (float)Width / Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov * ((float)Math.PI / 180f), // field of view angle, in radians
                aspectRatio,                // current window aspect ratio
                0.1f,                       // near plane
                4000f);                     // far plane
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _time += e.Time;
            foreach (var item in _gameObjects)
            {
                item.Update(_time, e.Time);
            }
            if(Avril_FSD.ServerAssembly.Program.Get_framework_Server().Get_server().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                HandleKeyboard(e.Time);
                HandleMouse();
                switch (cameraSelector)
                {
                    case false:
                        Get_gameObjectFactory().Get_player().Get_CameraFP().Update(_time, e.Time);
                        break;

                    case true:
                        Get_gameObjectFactory().Get_player().Get_CameraTP().Update(_time, e.Time);
                        break;
                }
            }
        }
        private void HandleMouse()
        {
            //Console.WriteLine("TESTBENCH => HandleMouse");
            MouseState mouseState = Mouse.GetCursorState();
        }
        private void HandleKeyboard(double dt)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (keyState.IsKeyDown(Key.W))
            {
                
            }
            if (keyState.IsKeyDown(Key.S))
            {
                
            }

            if (keyState.IsKeyDown(Key.A))
            {
                
            }
            if (keyState.IsKeyDown(Key.D))
            {
                
            }
            _lastKeyboardState = keyState;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"{_title}: FPS:{1f / e.Time:0000.0}, obj:{_gameObjects.Count}";
            GL.ClearColor(Color.Black);// _backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int lastProgram = -1;
            foreach (var obj in _gameObjects)
            {
                var program = obj.Model.Program;
                if (lastProgram != program)
                    GL.UniformMatrix4(20, false, ref _projectionMatrix);
                lastProgram = obj.Model.Program;
                switch (cameraSelector)
                {
                    case false:
                        obj.Render(Get_gameObjectFactory().Get_player().Get_CameraFP());
                        break;

                    case true:
                        obj.Render(Get_gameObjectFactory().Get_player().Get_CameraTP());
                        break;
                }
                

            }
            SwapBuffers();
        }
        public byte Get_coreId()
        {
            return _coreId;
        }
        public bool Get_cameraSelector()
        {
            return cameraSelector;
        }
        public GameObjectFactory Get_gameObjectFactory()
        {
            return _gameObjectFactory;
        }
        public byte Set_coreId(byte value)
        {
            return _coreId = value;
        }
    }
}
````
Loading Client Shell Framwork
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/gameInstance/Game_Instance.cs
````
namespace Avril_FSD.ClientAssembly
{
    public sealed class Game_Instance : GameWindow
    {
        private bool done_once;
        private byte _coreId;
        private readonly string _title;
        private double _time;
        private readonly Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private FirstPersonCamera _cameraFP;
        private ThirdPersonCamera _cameraTP;
        private Matrix4 _projectionMatrix;
        private float _fov = 45f;

        private KeyboardState _lastKeyboardState;
        private MouseState _lastMouseState;

        private GameObjectFactory _gameObjectFactory;
        private readonly List<AGameObject> _gameObjects = new List<AGameObject>();
        
        private Outputs.Output _gameData_Output;

        private ShaderProgram _texturedProgram;
        private ShaderProgram _solidProgram;
        
        private bool cameraSelector = false;
       
        public Game_Instance()
            : base(1920, // initial width
                1080, // initial height
                GraphicsMode.Default,
                "",  // initial title
                GameWindowFlags.Fullscreen,
                DisplayDevice.Default,
                4, // OpenGL major version
                5, // OpenGL minor version
                GraphicsContextFlags.ForwardCompatible)
        {
            _title += "dreamstatecoding.blogspot.com: OpenGL Version: " + GL.GetString(StringName.Version);
            _gameData_Output = new Outputs.Output();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CreateProjection();
        }

        protected override void OnLoad(EventArgs e)
        {
            System.Console.WriteLine("OnLoad");
            VSync = VSyncMode.Off;
            CreateProjection();
#if DEBUG
            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\APP_ClientAssembly\\graphics\\Shaders\\1Vert\\simplePipeVert.c");
            _solidProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\APP_ClientAssembly\\graphics\\Shaders\\5Frag\\simplePipeFrag.c");
            _solidProgram.Link();

            _texturedProgram = new ShaderProgram();
            _texturedProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\APP_ClientAssembly\\graphics\\Shaders\\1Vert\\simplePipeTexVert.c");
            _texturedProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\APP_ClientAssembly\\graphics\\Shaders\\5Frag\\simplePipeTexFrag.c");
            _texturedProgram.Link();

            var models = new Dictionary<string, ARenderable>();
            models.Add("Player", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\gameover.png", 8));
            models.Add("Wooden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\wooden.png", 8));
            models.Add("Golden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\golden.bmp", 8));
            models.Add("Asteroid", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\moonmap1k.jpg", 8));
            models.Add("Spacecraft", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\spacecraft.png", 8));
            models.Add("Gameover", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\gameover.png", 8));
            models.Add("Bullet", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\dotted.png", 8));
#else
            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\APP_ClientAssembly\\graphics\\Shaders\\1Vert\\simplePipeVert.c");
            _solidProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\APP_ClientAssembly\\graphics\\Shaders\\5Frag\\simplePipeFrag.c");
            _solidProgram.Link();

            _texturedProgram = new ShaderProgram();
            _texturedProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\APP_ClientAssembly\\graphics\\Shaders\\1Vert\\simplePipeTexVert.c");
            _texturedProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\APP_ClientAssembly\\graphics\\Shaders\\5Frag\\simplePipeTexFrag.c");
            _texturedProgram.Link();

            var models = new Dictionary<string, ARenderable>();
            models.Add("Player", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\gameover.png", 8));
            models.Add("Wooden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\wooden.png", 8));
            models.Add("Golden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\golden.bmp", 8));
            models.Add("Asteroid", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\moonmap1k.jpg", 8));
            models.Add("Spacecraft", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\spacecraft.png", 8));
            models.Add("Gameover", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\gameover.png", 8));
            models.Add("Bullet", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\APP_ClientAssembly\\graphics\\Textures\\dotted.png", 8));
#endif
            //models.Add("TestObject", new TexturedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side.jpg"));
            //models.Add("TestObjectGen", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side.jpg", 8));
            //models.Add("TestObjectPreGen", new MipMapManualRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side mipmap levels 0 to 8.bmp", 9));

            _gameObjectFactory = new GameObjectFactory(models);

            _gameObjectFactory.Create_PlayerOnClient();
            _gameObjects.Add(_gameObjectFactory.Get_player());

            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Wooden", new Vector3(10f, 0f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Wooden", new Vector3(-10f, 0f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Golden", new Vector3(0f, 10f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Golden", new Vector3(0f, -10f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Asteroid", new Vector3(0f, 0f, 10f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Asteroid", new Vector3(0f, 0f, -10f), new Vector3(1f)));

            //_camera = new StaticCamera();

            _gameObjectFactory.Get_player().Create_Cameras();

            CursorVisible = false;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);           
            Closed += OnClosed;

            Avril_FSD.ClientAssembly.Framework_Client obj = Avril_FSD.ClientAssembly.Program.Get_framework_Client();
            obj.Get_client().Get_execute().Initialise_Threads(obj);

            System.Console.WriteLine("OnLoad .. done");
        }

        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();

        }

        public override void Exit()
        {
            System.Console.WriteLine("Exit called");
            Avril_FSD.ClientAssembly.Framework_Client obj = Avril_FSD.ClientAssembly.Program.Get_framework_Client();
            obj.Get_client().Get_execute().Get_execute_Control().Set_exitApplication(true);
            obj.Get_client().Get_execute().Get_networking_Client().DeInitialise_networking_Server();
            _gameObjectFactory.Dispose();
            _solidProgram.Dispose();
            _texturedProgram.Dispose();
            base.Exit();
        }

        private void CreateProjection()
        {

            var aspectRatio = (float)Width / Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov * ((float)Math.PI / 180f), // field of view angle, in radians
                aspectRatio,                // current window aspect ratio
                0.1f,                       // near plane
                4000f);                     // far plane
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Avril_FSD.ClientAssembly.Framework_Client obj = Program.Get_framework_Client();
            if (obj.Get_client().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                Console.WriteLine("TESTBENCH => Get_exitApplication() = " + obj.Get_client().Get_execute().Get_execute_Control().Get_exitApplication());
                Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTINPUTACTION.Write_Start(obj.Get_client().Get_execute().Get_program_WriteQue_C_IA(), 0);
                HandleKeyboard(obj, e.Time);
                HandleMouse(obj);
                Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTINPUTACTION.Write_End(obj.Get_client().Get_execute().Get_program_WriteQue_C_IA(), 0);
                _time += e.Time;
                foreach (var item in _gameObjects)
                {
                    item.Update(_time, e.Time);
                }
                switch (cameraSelector)
                {
                    case false:
                        Get_gameObjectFactory().Get_player().Get_CameraFP().Update(_time, e.Time);
                        break;

                    case true:
                        Get_gameObjectFactory().Get_player().Get_CameraTP().Update(_time, e.Time);
                        break;
                }
            }
        }
        private void HandleMouse(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            //Console.WriteLine("TESTBENCH => HandleMouse");
            MouseState mouseState = Mouse.GetCursorState();
            var buffer = obj.Get_client().Get_data().Get_input_Instnace().Get_FRONT_inputDoubleBuffer(obj);
            //Console.WriteLine("Get_isPraiseActive() = " + obj.Get_client().Get_data().Get_data_Control().Get_isPraiseActive(1));
            if (obj.Get_client().Get_data().Get_data_Control().Get_isPraiseActive(1) == false)
            {
                Console.WriteLine("TESTBENCH => Do PRAISE 1 start");
                obj.Get_client().Get_data().Get_data_Control().Set_isPraiseActive(1, true);
                buffer.Set_playerId(0);
                buffer.Set_praiseEventId(1);
                buffer.Get_input_Control().SelectSetIntputSubset(obj, buffer.Get_praiseEventId());
                var subset = (Avril_FSD.ClientAssembly.Praise_Files.Praise1_Input)buffer.Get_praiseInputBuffer_Subset();
                subset.Set_Mouse_X(mouseState.X);
                subset.Set_Mouse_Y(mouseState.Y);
                obj.Get_client().Get_data().Flip_InBufferToWrite();
                obj.Get_client().Get_data().Get_data_Control().Push_Stack_Client_InputAction(obj, obj.Get_client().Get_data().Get_input_Instnace().Get_stack_Client_InputSend(), obj.Get_client().Get_data().Get_input_Instnace().Get_BACK_inputDoubleBuffer(obj));
                Console.WriteLine("TESTBENCH => Do PRAISE 1 end");
            }
        }
        private void HandleKeyboard(Avril_FSD.ClientAssembly.Framework_Client obj, double dt)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (keyState.IsKeyDown(Key.W))
            {
                
            }
            if (keyState.IsKeyDown(Key.S))
            {
                
            }

            if (keyState.IsKeyDown(Key.A))
            {
                
            }
            if (keyState.IsKeyDown(Key.D))
            {
                
            }
            _lastKeyboardState = keyState;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"{_title}: FPS:{1f / e.Time:0000.0}, obj:{_gameObjects.Count}";
            GL.ClearColor(Color.Black);// _backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int lastProgram = -1;
            foreach (var obj in _gameObjects)
            {
                var program = obj.Model.Program;
                if (lastProgram != program)
                    GL.UniformMatrix4(20, false, ref _projectionMatrix);
                lastProgram = obj.Model.Program;
                switch (cameraSelector)
                {
                    case false:
                        obj.Render(Get_gameObjectFactory().Get_player().Get_CameraFP());
                        break;

                    case true:
                        obj.Render(Get_gameObjectFactory().Get_player().Get_CameraTP());
                        break;
                }
                

            }
            SwapBuffers();
        }

        public byte Get_coreId()
        {
            return _coreId;
        }
        public bool Get_cameraSelector()
        {
            return cameraSelector;
        }
        public Outputs.Output Get_gameData_Output()
        {
            return _gameData_Output;
        }
        public GameObjectFactory Get_gameObjectFactory()
        {
            return _gameObjectFactory;
        }

        public byte Set_coreId(byte value)
        {
            return _coreId = value;
        }
    }
}
````
STACK Client Input Action
line 9:	private List<Avril_FSD.ClientAssembly.Inputs.Input> _stack_Client_InputSend;
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/engine/Input_Instance.cs

NETWORKING Client Input Action Send - Client Shell Input/Output (IO) Thread
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/Networking_Client.cs
````
public void Thread_IO_Client(byte threadId)
        {
            Avril_FSD.ClientAssembly.Framework_Client obj = Avril_FSD.ClientAssembly.Program.Get_framework_Client();
            bool doneOnce = false;
            while (obj.Get_client().Get_execute().Get_execute_Control().Get_flag_SystemInitialised() == true)
            {
                if (doneOnce == false)
                {
                    doneOnce = true;
                    obj.Get_client().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(obj, threadId, false);
                }
            }
            while (obj.Get_client().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                StatusCallback status = (ref StatusInfo info) => {
                    switch (info.connectionInfo.state)
                    {
                        case ConnectionState.None:
                            break;

                        case ConnectionState.Connected:
                            Console.WriteLine("Client connected to server - ID: " + _connection);
                            if (obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_InputAction() == true)
                            {
                                System.Console.WriteLine("Thread[" + (threadId).ToString() + "] => IsLoaded_Stack_InputAction = " + obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_InputAction());//TestBench
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTINPUTACTION.Write_Start(obj.Get_client().Get_execute().Get_program_WriteQue_C_IA(), 1);
                                byte[] data = new byte[64];
                                obj.Get_client().Get_data().Get_data_Control().Pop_Stack_InputAction(obj, obj.Get_client().Get_data().Get_input_Instnace().Get_FRONT_inputDoubleBuffer(obj), obj.Get_client().Get_data().Get_input_Instnace().Get_stack_Client_InputSend());
                                obj.Get_client().Get_data().Flip_InBufferToWrite();
                                obj.Get_client().Get_algorithms().Get_io_ListenRespond().Encode_NetworkingSteam_At_Client_Input(obj, obj.Get_client().Get_data().Get_input_Instnace().Get_BACK_inputDoubleBuffer(obj), data);
                                _client_SOCKET.SendMessageToConnection(_connection, data);
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTINPUTACTION.Write_End(obj.Get_client().Get_execute().Get_program_WriteQue_C_IA(), 1);
                            }
                            break;

                        case ConnectionState.ClosedByPeer:
                        case ConnectionState.ProblemDetectedLocally:
                            _client_SOCKET.CloseConnection(_connection);
                            Console.WriteLine("Client disconnected from server");
                            break;
                    }
                };
                _utils = new NetworkingUtils();
                _utils.SetStatusCallback(status);
                    
                _connection = _client_SOCKET.Connect(ref address_SERVER);
                System.Console.WriteLine("Thread[" + (threadId).ToString() + "] :: _connection = " + (_connection).ToString());//TestBench
#if VALVESOCKETS_SPAN
MessageCallback message = (in NetworkingMessage netMessage) => {
	Console.WriteLine("Message received from server - Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
};
#else
                const int maxMessages = 20;
                NetworkingMessage[] netMessages = new NetworkingMessage[maxMessages];
#endif
                System.Console.WriteLine("Thread[" + (threadId).ToString() + "] :: ALPHA");//TestBench
                while (!Console.KeyAvailable)
                {
                    System.Console.WriteLine("Thread[" + (threadId).ToString() + "] :: BRAVO");//TestBench
                    _client_SOCKET.RunCallbacks();

#if VALVESOCKETS_SPAN
	client.ReceiveMessagesOnConnection(connection, message, 20);
#else
                    int netMessagesCount = _client_SOCKET.ReceiveMessagesOnConnection(_connection, netMessages, maxMessages);
                    System.Console.WriteLine("Thread[" + (threadId).ToString() + "] :: netMessagesCount = " + netMessagesCount);//TestBench
                    if (netMessagesCount > 0)
                    {
                        for (int i = 0; i < netMessagesCount; i++)
                        {
                            ref NetworkingMessage netMessage = ref netMessages[i];

                            Console.WriteLine("Message received from server - Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE.Write_Start(obj.Get_client().Get_execute().Get_program_WriteQue_C_OR(), 1);
                            byte[] buffer = new byte[1024];
                            netMessage.CopyTo(buffer);
                            obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj).Set_praiseOutputBuffer_Subset(buffer[0]);
                            obj.Get_client().Get_algorithms().Get_io_ListenRespond().Decode_NetworkingSteam_At_Client_Recieve(obj, obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj), buffer);
                            obj.Get_client().Get_data().Flip_OutBufferToWrite();
                            obj.Get_client().Get_data().Get_data_Control().Push_Stack_Client_OutputRecieve(obj, obj.Get_client().Get_data().Get_output_Instnace().Get_stack_Client_OutputRecieves(), obj.Get_client().Get_data().Get_output_Instnace().Get_FRONT_outputDoubleBuffer(obj));
                            if (obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputRecieve())
                            {
                                if (Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Get_State_LaunchBit(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C()))
                                {
                                    Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Request_Wait_Launch(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Get_coreId_To_Launch(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C()));
                                }
                            }
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE.Write_End(obj.Get_client().Get_execute().Get_program_WriteQue_C_OR(), 1);
                            netMessage.Destroy();
                        }
                    }
#endif

                    Thread.Sleep(15);
                }
            }
        }

````
NETWORKING Server Input Action Recieve - Server Shell Input/Output (IO) Thread
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/Networking_Client.cs
````
 public void Thread_IO_Server(byte threadId)
        {
            Avril_FSD.ServerAssembly.Framework_Server obj = Avril_FSD.ServerAssembly.Program.Get_framework_Server();
            bool doneOnce = false;
            while (obj.Get_server().Get_execute().Get_execute_Control().Get_flag_isInitialised_ServerShell(obj) == true)
            {
                if (doneOnce == false)
                {
                    doneOnce = true;
                    obj.Get_server().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(threadId, false);
                }
            }
            while (obj.Get_server().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                //System.Console.WriteLine("Thread[" + (threadId).ToString() + "] ALPHA");
                uint pollGroup = _server_SOCKET.CreatePollGroup();

                StatusCallback status = (ref StatusInfo info) => {
                    switch (info.connectionInfo.state)
                    {
                        case ConnectionState.None:
                            break;

                        case ConnectionState.Connecting:
                            _server_SOCKET.AcceptConnection(info.connection);
                            _server_SOCKET.SetConnectionPollGroup(pollGroup, info.connection);
                            break;

                        case ConnectionState.Connected:
                            System.Console.WriteLine("Thread[" + (threadId).ToString() + "] => Get_flag_IsLoaded_Stack_OutputAction = " + obj.Get_server().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputAction());//TestBench
                            if (obj.Get_server().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputAction())
                            {
                                Console.WriteLine("Client connected - ID: " + info.connection + ", IP: " + info.connectionInfo.address.GetIP());
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE.Write_Start(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerOutputRecieve(), 0);
                                byte[] data = new byte[64];
                                var output = obj.Get_server().Get_data().Get_output_Instnace().Get_FRONT_outputDoubleBuffer(obj);
                                output.Get_output_Control().SelectSetOutputSubset(obj, output.Get_praiseEventId());
                                obj.Get_server().Get_algorithms().Get_io_ListenRespond().Encode_NetworkingSteam_At_Server_Output(obj, output, data);
                                address_CLIENT.SetAddress(info.connectionInfo.address.GetIP(), 27001);
                                uint connection = _server_SOCKET.Connect(ref address_CLIENT);
                                _server_SOCKET.SendMessageToConnection(connection, data);
                                _server_SOCKET.CloseConnection(info.connection);
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE.Write_End(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerOutputRecieve(), 0);
                            }
                            break;
                    }
                };

                _utils.SetStatusCallback(status);



#if VALVESOCKETS_SPAN
MessageCallback message = (in NetworkingMessage netMessage) => {
	Console.WriteLine("Message received from - ID: " + netMessage.connection + ", Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
};
#else
                const int maxMessages = 20;

                NetworkingMessage[] netMessages = new NetworkingMessage[maxMessages];
#endif

                while (!Console.KeyAvailable)
                {
                    _server_SOCKET.RunCallbacks();

#if VALVESOCKETS_SPAN
	server.ReceiveMessagesOnPollGroup(pollGroup, message, 20);
#else
                    int netMessagesCount = _server_SOCKET.ReceiveMessagesOnPollGroup(pollGroup, netMessages, maxMessages);

                    if (netMessagesCount > 0)
                    {
                        for (int i = 0; i < netMessagesCount; i++)
                        {
                            ref NetworkingMessage netMessage = ref netMessages[i];

                            Console.WriteLine("Message received from - ID: " + netMessage.connection + ", Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVERINPUTACTION.Write_Start(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerInputAction(), 0);
                            byte[] buffer = new byte[1024];
                            netMessage.CopyTo(buffer);
                            Avril_FSD.Library_For_Server_Concurrency.Select_Set_Intput_Subset(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), buffer[0]);
                            obj.Get_server().Get_algorithms().Get_io_ListenRespond().Decode_NetworkingSteam_At_Server_Input(obj, obj.Get_server().Get_data().Get_input_Instnace().Get_FRONT_inputDoubleBuffer(obj), buffer);
                            Avril_FSD.Library_For_Server_Concurrency.Flip_InBufferToWrite(obj.Get_server().Get_execute().Get_program_ServerConcurrency());
                            Avril_FSD.Library_For_Server_Concurrency.Push_Stack_InputPraises(obj.Get_server().Get_execute().Get_program_ServerConcurrency());
                            if (Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_Flag_ConcurrentCoreState(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_coreId_To_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency())) == Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_Flag_Idle(obj.Get_server().Get_execute().Get_program_ServerConcurrency()))
                            {
                                Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Request_Wait_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_coreId_To_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency()));
                            }
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVERINPUTACTION.Write_End(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerInputAction(), 0);
                            netMessage.Destroy();
                        }
                    }
#endif
                    Thread.Sleep(15);
                }
                _server_SOCKET.DestroyPollGroup(pollGroup);
            }
        }
        public NetworkingUtils Get_utils()
        {
            return _utils;
        }
        public uint Get_pollGroup()
        {
            return _pollGroup;
        }
    }
````
STACK Server Input Action
line 11:	private List<Avril_FSD.ServerAssembly.Outputs.Output> _stack_Client_OutputRecieves;
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/engine/Data.cs

LIB_Concurrent_IO_Server - Execute
https://github.com/OpenFSD/LIB_Concurrent_IO_Server/blob/master/Execute.cpp
````
void Avril_FSD::Execute::Initialise_Libraries()
{
    program_ConcurrentQue_Server = static_cast<Avril_FSD::LaunchEnableForConcurrentThreadsAt_SERVER_Framework*>(Avril_FSD::CLIBLaunchEnableForConcurrentThreadsAtSERVER::Initialise_LaunchEnableForConcurrentThreadsAt());
    while (program_ConcurrentQue_Server == NULL) {}

    program_WriteEnableStack_ServerInputAction = static_cast<Avril_FSD::WriteEnableForThreadsAt_SERVERINPUTACTION_Framework*>(Avril_FSD::CLIBWriteEnableForThreadsAtSERVERINPUTACTION::Initialise_WriteEnable());
    while (program_WriteEnableStack_ServerInputAction == NULL) {}

    program_WriteEnableStack_ServerOutputRecieve = static_cast<Avril_FSD::WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE_Framework*>(Avril_FSD::Library_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE::Initialise_WriteEnable());
    while (program_WriteEnableStack_ServerOutputRecieve == NULL) {}
}
void Avril_FSD::Execute::Initialise_Threads(class Avril_FSD::Framework_Server* obj)
{
    for (__int8 coreId = 0; coreId < obj->Get_Server_Assembly()->Get_Global()->Get_NumCores(); coreId++)
    {
        ptr_Thread_WithCoreId[coreId] = new std::thread(obj->Get_Server_Assembly()->Get_Algorithms()->Get_Concurrent(coreId)->Thread_Concurrency, obj, coreId);
    }
}
````
Thread Concurrency of Server
https://github.com/OpenFSD/LIB_Concurrent_IO_Server/blob/master/Concurrent.cpp
````
void Avril_FSD::Concurrent::Thread_Concurrency(Avril_FSD::Framework_Server* obj, __int8 concurrent_coreId)
{
    bool doneOnce = true;
    while (obj->Get_Server_Assembly()->Get_Execute()->Get_Control_Of_Execute()->GetFlag_ThreadInitialised(concurrent_coreId) == true)
    {
        if (doneOnce == true)
        {
            obj->Get_Server_Assembly()->Get_Execute()->Get_Control_Of_Execute()->SetConditionCodeOfThisThreadedCore(concurrent_coreId);
            doneOnce = false;
        }

    }
    std::cout << "Thread Initialised: ID=" << (concurrent_coreId) << " => Thread_Concurrency()" << std::endl;//TestBench
    while (obj->Get_Server_Assembly()->Get_Execute()->Get_Control_Of_Execute()->GetFlag_SystemInitialised(obj) == true)
    {

    }
    std::cout << "Thread Starting " << (concurrent_coreId) << " => Thread_Concurrency()" << std::endl;//TestBench
    while (obj->Get_Server_Assembly()->Get_Execute()->Get_Control_Of_Execute()->GetFlag_SystemInitialised(obj) == false)
    {
        switch (Avril_FSD::CLIBLaunchEnableForConcurrentThreadsAtSERVER::Get_Flag_ConcurrentCoreState(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_ConcurrentQue_Server(), concurrent_coreId))
        {
        case false:

            break;

        case true:
            if(obj->Get_Server_Assembly()->Get_Data()->Get_Data_Control()->Get_flag_IsStackLoaded_Server_InputAction() == true)
            {
                    Avril_FSD::CLIBWriteEnableForThreadsAtSERVERINPUTACTION::Write_Start(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_WriteEnable_ServerInputAction(), (__int8)(concurrent_coreId + (__int8)1));
                    obj->Get_Server_Assembly()->Get_Data()->Get_OutputRefferenceOfCore(concurrent_coreId)->Get_control_Of_Output()->SelectSet_Output_Subset(obj, obj->Get_Server_Assembly()->Get_Data()->Get_OutputRefferenceOfCore(concurrent_coreId)->Get_out_praiseEventId(), concurrent_coreId);
                    obj->Get_Server_Assembly()->Get_Algorithms()->Get_Concurrent(concurrent_coreId)->Get_Concurrent_Control()->SelectSet_Algorithm_Subset(obj, obj->Get_Server_Assembly()->Get_Data()->Get_InputRefferenceOfCore(concurrent_coreId)->GetPraiseEventId(), concurrent_coreId);
                    obj->Get_Server_Assembly()->Get_Data()->Get_Data_Control()->Store_Praise_In_Data_To_GameInstance_Data(obj, obj->Get_Server_Assembly()->Get_Data()->Get_Stack_InputPraise());
                    obj->Get_Server_Assembly()->Get_Data()->Get_Data_Control()->Pop_Stack_InputPraises(obj, concurrent_coreId);
                    Avril_FSD::CLIBWriteEnableForThreadsAtSERVERINPUTACTION::Write_End(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_WriteEnable_ServerInputAction(), (__int8)(concurrent_coreId + (__int8)1));
                    obj->Get_Server_Assembly()->Get_Algorithms()->Get_Concurrent(concurrent_coreId)->Do_Concurrent_Algorithm_For_PraiseEventId(
                        obj,
                        obj->Get_Server_Assembly()->Get_Data()->Get_InputRefferenceOfCore(concurrent_coreId)->Get_playerId(),
                        obj->Get_Server_Assembly()->Get_Data()->Get_InputRefferenceOfCore(concurrent_coreId)->GetPraiseEventId(),
                        obj->Get_Server_Assembly()->Get_Algorithms()->Get_Concurrent(concurrent_coreId)->Get_Algorithm_Subset(),
                        obj->Get_Server_Assembly()->Get_Data()->Get_InputRefferenceOfCore(concurrent_coreId)->Get_InputBuffer_Subset(),
                        obj->Get_Server_Assembly()->Get_Data()->Get_OutputRefferenceOfCore(concurrent_coreId)->Get_praiseOutputBuffer_Subset()
                    );
                    Avril_FSD::Library_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE::Write_Start(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_WriteEnable_ServerOutputRecieve(), (__int8)(concurrent_coreId + (__int8)1));
                    obj->Get_Server_Assembly()->Get_Data()->Get_Data_Control()->Push_Stack_Output(obj, concurrent_coreId);
                    obj->Get_Server_Assembly()->Get_Data()->Get_Data_Control()->Store_Praise_Out_Data_To_GameInstance_Data(obj, obj->Get_Server_Assembly()->Get_Data()->Get_Stack_OutputPraise());
                    Avril_FSD::CLIBLaunchEnableForConcurrentThreadsAtSERVER::Thread_End(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_ConcurrentQue_Server(), concurrent_coreId);
                    if (obj->Get_Server_Assembly()->Get_Data()->Get_Data_Control()->Get_flag_IsStackLoaded_Server_OutputRecieve() == true)
                    {
                        if (Avril_FSD::CLIBLaunchEnableForConcurrentThreadsAtSERVER::Get_Flag_ConcurrentCoreState(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_ConcurrentQue_Server(), Avril_FSD::CLIBLaunchEnableForConcurrentThreadsAtSERVER::Get_coreId_To_Launch(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_ConcurrentQue_Server())) == Avril_FSD::CLIBLaunchEnableForConcurrentThreadsAtSERVER::Get_Flag_Idle(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_ConcurrentQue_Server()))
                        {
                            Avril_FSD::CLIBLaunchEnableForConcurrentThreadsAtSERVER::Request_Wait_Launch(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_ConcurrentQue_Server(), Avril_FSD::CLIBLaunchEnableForConcurrentThreadsAtSERVER::Get_coreId_To_Launch(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_ConcurrentQue_Server()));
                        }
                    }
                   Avril_FSD::Library_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE::Write_End(obj->Get_Server_Assembly()->Get_Execute()->Get_Program_WriteEnable_ServerOutputRecieve(), (__int8)(concurrent_coreId + (__int8)1));
                //}
            }
            break;
        }
    }
}
````
Network Data Send From Client To Server
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/SIM_Networking.cs
````
    static public void Do_Client_Send(Avril_FSD.ClientAssembly.Framework_Client obj)
    {
        
        _client_Send.Connect();
        byte praiseEventId = obj.Get_client().Get_data().Get_input_Instnace().Get_BACK_inputDoubleBuffer(obj).Get_praiseEventId();
        byte playerId = obj.Get_client().Get_data().Get_input_Instnace().Get_BACK_inputDoubleBuffer(obj).Get_playerId();
        switch (praiseEventId)
        {
        // USER IMPLAEMENTATION - ABCDE
        case 0:

            break;

        case 1:
            byte[] buffer = new byte[10];
            buffer[0] = praiseEventId;
            buffer[1] = playerId;
            Avril_FSD.ClientAssembly.Praise_Files.Praise1_Input subset = (Avril_FSD.ClientAssembly.Praise_Files.Praise1_Input)obj.Get_client().Get_data().Get_input_Instnace().Get_BACK_inputDoubleBuffer(obj).Get_praiseInputBuffer_Subset();
            byte[] byteArray = BitConverter.GetBytes(subset.Get_Mouse_X());
            for (ushort index = 0; index < 4; index++)
            {
                buffer[index + 2] = byteArray[index];
            }
            byteArray = BitConverter.GetBytes(subset.Get_Mouse_Y());
            for (ushort index = 0; index < 4; index++)
            {
                buffer[index + 6] = byteArray[index];
            }
            _client_Send.Write(buffer, 0, buffer.Length);
            break;
        }
        _client_Send.Close();
    }
````
Server Thread Input/Output Do Loop
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/Networking_Server.cs
````
        public void Thread_IO_Server(byte threadId)
        {
            Avril_FSD.ServerAssembly.Framework_Server obj = Avril_FSD.ServerAssembly.Program.Get_framework_Server();
            bool doneOnce = false;
            while (obj.Get_server().Get_execute().Get_execute_Control().Get_flag_isInitialised_ServerShell(obj) == true)
            {
                if (doneOnce == false)
                {
                    doneOnce = true;
                    obj.Get_server().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(threadId, false);
                }
            }
            while (obj.Get_server().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                //System.Console.WriteLine("Thread[" + (threadId).ToString() + "] ALPHA");
                uint pollGroup = _server_SOCKET.CreatePollGroup();

                StatusCallback status = (ref StatusInfo info) => {
                    switch (info.connectionInfo.state)
                    {
                        case ConnectionState.None:
                            break;

                        case ConnectionState.Connecting:
                            _server_SOCKET.AcceptConnection(info.connection);
                            _server_SOCKET.SetConnectionPollGroup(pollGroup, info.connection);
                            break;

                        case ConnectionState.Connected:
                            System.Console.WriteLine("Thread[" + (threadId).ToString() + "] => Get_flag_IsLoaded_Stack_OutputAction = " + obj.Get_server().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputAction());//TestBench
                            if (obj.Get_server().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputAction())
                            {
                                Console.WriteLine("Client connected - ID: " + info.connection + ", IP: " + info.connectionInfo.address.GetIP());
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE.Write_Start(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerOutputRecieve(), 0);
                                byte[] data = new byte[64];
                                var output = obj.Get_server().Get_data().Get_output_Instnace().Get_FRONT_outputDoubleBuffer(obj);
                                output.Get_output_Control().SelectSetOutputSubset(obj, output.Get_praiseEventId());
                                obj.Get_server().Get_algorithms().Get_io_ListenRespond().Encode_NetworkingSteam_At_Server_Output(obj, output, data);
                                address_CLIENT.SetAddress(info.connectionInfo.address.GetIP(), 27001);
                                uint connection = _server_SOCKET.Connect(ref address_CLIENT);
                                _server_SOCKET.SendMessageToConnection(connection, data);
                                _server_SOCKET.CloseConnection(info.connection);
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE.Write_End(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerOutputRecieve(), 0);
                            }
                            break;
                    }
                };

                _utils.SetStatusCallback(status);



#if VALVESOCKETS_SPAN
MessageCallback message = (in NetworkingMessage netMessage) => {
	Console.WriteLine("Message received from - ID: " + netMessage.connection + ", Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
};
#else
                const int maxMessages = 20;

                NetworkingMessage[] netMessages = new NetworkingMessage[maxMessages];
#endif

                while (!Console.KeyAvailable)
                {
                    _server_SOCKET.RunCallbacks();

#if VALVESOCKETS_SPAN
	server.ReceiveMessagesOnPollGroup(pollGroup, message, 20);
#else
                    int netMessagesCount = _server_SOCKET.ReceiveMessagesOnPollGroup(pollGroup, netMessages, maxMessages);

                    if (netMessagesCount > 0)
                    {
                        for (int i = 0; i < netMessagesCount; i++)
                        {
                            ref NetworkingMessage netMessage = ref netMessages[i];

                            Console.WriteLine("Message received from - ID: " + netMessage.connection + ", Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVERINPUTACTION.Write_Start(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerInputAction(), 0);
                            byte[] buffer = new byte[1024];
                            netMessage.CopyTo(buffer);
                            Avril_FSD.Library_For_Server_Concurrency.Select_Set_Intput_Subset(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), buffer[0]);
                            obj.Get_server().Get_algorithms().Get_io_ListenRespond().Decode_NetworkingSteam_At_Server_Input(obj, obj.Get_server().Get_data().Get_input_Instnace().Get_FRONT_inputDoubleBuffer(obj), buffer);
                            Avril_FSD.Library_For_Server_Concurrency.Flip_InBufferToWrite(obj.Get_server().Get_execute().Get_program_ServerConcurrency());
                            Avril_FSD.Library_For_Server_Concurrency.Push_Stack_InputPraises(obj.Get_server().Get_execute().Get_program_ServerConcurrency());
                            if (Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_Flag_ConcurrentCoreState(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_coreId_To_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency())) == Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_Flag_Idle(obj.Get_server().Get_execute().Get_program_ServerConcurrency()))
                            {
                                Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Request_Wait_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_coreId_To_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency()));
                            }
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVERINPUTACTION.Write_End(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerInputAction(), 0);
                            netMessage.Destroy();
                        }
                    }
#endif
                    Thread.Sleep(15);
                }
                _server_SOCKET.DestroyPollGroup(pollGroup);
            }
        }
````
Network Data Server Recieve From Client
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/SIM_Networking.cs
````
static public void Do_Client_Recieve(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            
            _client_Recieve.Connect();
            Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE.Write_Start(obj.Get_client().Get_execute().Get_program_WriteQue_C_OR(), 1);

            byte[] buffer = new byte[1];
            int bytesRead = _client_Recieve.Read(buffer, 0, buffer.Length);
            byte priaseEventId = buffer[0];
            obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj).Set_praiseEventId(priaseEventId);

            buffer = new byte[1];
            bytesRead = _client_Recieve.Read(buffer, 1, buffer.Length);
            byte playerId = buffer[1];
            obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj).Set_playerId(playerId);

            switch (priaseEventId)
            {
            case 0:

                break;

            case 1:

                Avril_FSD.ClientAssembly.Praise_Files.Praise1_Output output_Subset = (Avril_FSD.ClientAssembly.Praise_Files.Praise1_Output)obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj).Get_praiseOutputBuffer_Subset();
                buffer = new byte[12];
                bytesRead = _client_Recieve.Read(buffer, 2, buffer.Length);
                float temp_X = System.BitConverter.ToSingle(buffer, 0);
                float temp_Y = System.BitConverter.ToSingle(buffer, 4);
                float temp_Z = System.BitConverter.ToSingle(buffer, 8);
                Vector3 temp_Vec = new Vector3(temp_X, temp_Y, temp_Z);
                output_Subset.Set_fowards(temp_Vec);

                buffer = new byte[12];
                bytesRead = _client_Recieve.Read(buffer, 14, buffer.Length);
                temp_X = System.BitConverter.ToSingle(buffer, 0);
                temp_Y = System.BitConverter.ToSingle(buffer, 4);
                temp_Z = System.BitConverter.ToSingle(buffer, 8);
                temp_Vec = new Vector3(temp_X, temp_Y, temp_Z);
                output_Subset.Set_right(temp_Vec);

                buffer = new byte[12];
                bytesRead = _client_Recieve.Read(buffer, 26, buffer.Length);
                temp_X = System.BitConverter.ToSingle(buffer, 0);
                temp_Y = System.BitConverter.ToSingle(buffer, 4);
                temp_Z = System.BitConverter.ToSingle(buffer, 8);
                temp_Vec = new Vector3(temp_X, temp_Y, temp_Z);
                output_Subset.Set_up(temp_Vec);
                break;
            }
            _client_Recieve.Close();
        }
    }
````
Server Input/Output Thread - Pop And Do Stack Server Input Action Praise Event Id
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/Networking_Server.cs
````
public void Thread_IO_Server(byte threadId)
        {
            Avril_FSD.ServerAssembly.Framework_Server obj = Avril_FSD.ServerAssembly.Program.Get_framework_Server();
            bool doneOnce = false;
            while (obj.Get_server().Get_execute().Get_execute_Control().Get_flag_isInitialised_ServerShell(obj) == true)
            {
                if (doneOnce == false)
                {
                    doneOnce = true;
                    obj.Get_server().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(threadId, false);
                }
            }
            while (obj.Get_server().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                //System.Console.WriteLine("Thread[" + (threadId).ToString() + "] ALPHA");
                uint pollGroup = _server_SOCKET.CreatePollGroup();

                StatusCallback status = (ref StatusInfo info) => {
                    switch (info.connectionInfo.state)
                    {
                        case ConnectionState.None:
                            break;

                        case ConnectionState.Connecting:
                            _server_SOCKET.AcceptConnection(info.connection);
                            _server_SOCKET.SetConnectionPollGroup(pollGroup, info.connection);
                            break;

                        case ConnectionState.Connected:
                            System.Console.WriteLine("Thread[" + (threadId).ToString() + "] => Get_flag_IsLoaded_Stack_OutputAction = " + obj.Get_server().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputAction());//TestBench
                            if (obj.Get_server().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputAction())
                            {
                                Console.WriteLine("Client connected - ID: " + info.connection + ", IP: " + info.connectionInfo.address.GetIP());
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE.Write_Start(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerOutputRecieve(), 0);
                                byte[] data = new byte[64];
                                var output = obj.Get_server().Get_data().Get_output_Instnace().Get_FRONT_outputDoubleBuffer(obj);
                                output.Get_output_Control().SelectSetOutputSubset(obj, output.Get_praiseEventId());
                                obj.Get_server().Get_algorithms().Get_io_ListenRespond().Encode_NetworkingSteam_At_Server_Output(obj, output, data);
                                address_CLIENT.SetAddress(info.connectionInfo.address.GetIP(), 27001);
                                uint connection = _server_SOCKET.Connect(ref address_CLIENT);
                                _server_SOCKET.SendMessageToConnection(connection, data);
                                _server_SOCKET.CloseConnection(info.connection);
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE.Write_End(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerOutputRecieve(), 0);
                            }
                            break;
                    }
                };

                _utils.SetStatusCallback(status);



#if VALVESOCKETS_SPAN
MessageCallback message = (in NetworkingMessage netMessage) => {
	Console.WriteLine("Message received from - ID: " + netMessage.connection + ", Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
};
#else
                const int maxMessages = 20;

                NetworkingMessage[] netMessages = new NetworkingMessage[maxMessages];
#endif

                while (!Console.KeyAvailable)
                {
                    _server_SOCKET.RunCallbacks();

#if VALVESOCKETS_SPAN
	server.ReceiveMessagesOnPollGroup(pollGroup, message, 20);
#else
                    int netMessagesCount = _server_SOCKET.ReceiveMessagesOnPollGroup(pollGroup, netMessages, maxMessages);

                    if (netMessagesCount > 0)
                    {
                        for (int i = 0; i < netMessagesCount; i++)
                        {
                            ref NetworkingMessage netMessage = ref netMessages[i];

                            Console.WriteLine("Message received from - ID: " + netMessage.connection + ", Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVERINPUTACTION.Write_Start(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerInputAction(), 0);
                            byte[] buffer = new byte[1024];
                            netMessage.CopyTo(buffer);
                            Avril_FSD.Library_For_Server_Concurrency.Select_Set_Intput_Subset(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), buffer[0]);
                            obj.Get_server().Get_algorithms().Get_io_ListenRespond().Decode_NetworkingSteam_At_Server_Input(obj, obj.Get_server().Get_data().Get_input_Instnace().Get_FRONT_inputDoubleBuffer(obj), buffer);
                            Avril_FSD.Library_For_Server_Concurrency.Flip_InBufferToWrite(obj.Get_server().Get_execute().Get_program_ServerConcurrency());
                            Avril_FSD.Library_For_Server_Concurrency.Push_Stack_InputPraises(obj.Get_server().Get_execute().Get_program_ServerConcurrency());
                            if (Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_Flag_ConcurrentCoreState(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_coreId_To_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency())) == Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_Flag_Idle(obj.Get_server().Get_execute().Get_program_ServerConcurrency()))
                            {
                                Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Request_Wait_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_coreId_To_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency()));
                            }
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVERINPUTACTION.Write_End(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerInputAction(), 0);
                            netMessage.Destroy();
                        }
                    }
#endif
                    Thread.Sleep(15);
                }
                _server_SOCKET.DestroyPollGroup(pollGroup);
            }
        }
        public NetworkingUtils Get_utils()
        {
            return _utils;
        }
        public uint Get_pollGroup()
        {
            return _pollGroup;
        }
    }
````
Encode NetworkingSteam At Server Output
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/engine/IO_Listen_Respond.cs
````
        public void Encode_NetworkingSteam_At_Server_Output(Avril_FSD.ServerAssembly.Framework_Server obj, Avril_FSD.ServerAssembly.Outputs.Output output, byte[] data)
        {
            data[0] = output.Get_praiseEventId();
            data[1] = output.Get_out_playerId();
            switch (output.Get_praiseEventId())
            {
                case 0:
                    break;

                case 1:
                    var subset = (Avril_FSD.ServerAssembly.Praise_Files.Praise1_Output)output.GetOutputBufferSubset();
                    byte[] tempFloat = BitConverter.GetBytes(subset.Get_fowards().X);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 2] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_fowards().Y);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 6] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_fowards().Z);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 10] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_right().X);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 14] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_right().Y);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 18] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_right().Z);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 22] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_up().X);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 26] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_up().Y);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 30] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_up().Z);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 34] = tempFloat[index];
                    }
                    break;

            }
        }
````
Network Server Send
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/Networking_Server.cs
````
public void Thread_IO_Server(byte threadId)
        {
            Avril_FSD.ServerAssembly.Framework_Server obj = Avril_FSD.ServerAssembly.Program.Get_framework_Server();
            bool doneOnce = false;
            while (obj.Get_server().Get_execute().Get_execute_Control().Get_flag_isInitialised_ServerShell(obj) == true)
            {
                if (doneOnce == false)
                {
                    doneOnce = true;
                    obj.Get_server().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(threadId, false);
                }
            }
            while (obj.Get_server().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                //System.Console.WriteLine("Thread[" + (threadId).ToString() + "] ALPHA");
                uint pollGroup = _server_SOCKET.CreatePollGroup();

                StatusCallback status = (ref StatusInfo info) => {
                    switch (info.connectionInfo.state)
                    {
                        case ConnectionState.None:
                            break;

                        case ConnectionState.Connecting:
                            _server_SOCKET.AcceptConnection(info.connection);
                            _server_SOCKET.SetConnectionPollGroup(pollGroup, info.connection);
                            break;

                        case ConnectionState.Connected:
                            System.Console.WriteLine("Thread[" + (threadId).ToString() + "] => Get_flag_IsLoaded_Stack_OutputAction = " + obj.Get_server().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputAction());//TestBench
                            if (obj.Get_server().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputAction())
                            {
                                Console.WriteLine("Client connected - ID: " + info.connection + ", IP: " + info.connectionInfo.address.GetIP());
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE.Write_Start(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerOutputRecieve(), 0);
                                byte[] data = new byte[64];
                                var output = obj.Get_server().Get_data().Get_output_Instnace().Get_FRONT_outputDoubleBuffer(obj);
                                output.Get_output_Control().SelectSetOutputSubset(obj, output.Get_praiseEventId());
                                obj.Get_server().Get_algorithms().Get_io_ListenRespond().Encode_NetworkingSteam_At_Server_Output(obj, output, data);
                                address_CLIENT.SetAddress(info.connectionInfo.address.GetIP(), 27001);
                                uint connection = _server_SOCKET.Connect(ref address_CLIENT);
                                _server_SOCKET.SendMessageToConnection(connection, data);
                                _server_SOCKET.CloseConnection(info.connection);
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVEROUTPUTRECIEVE.Write_End(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerOutputRecieve(), 0);
                            }
                            break;
                    }
                };

                _utils.SetStatusCallback(status);



#if VALVESOCKETS_SPAN
MessageCallback message = (in NetworkingMessage netMessage) => {
	Console.WriteLine("Message received from - ID: " + netMessage.connection + ", Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
};
#else
                const int maxMessages = 20;

                NetworkingMessage[] netMessages = new NetworkingMessage[maxMessages];
#endif

                while (!Console.KeyAvailable)
                {
                    _server_SOCKET.RunCallbacks();

#if VALVESOCKETS_SPAN
	server.ReceiveMessagesOnPollGroup(pollGroup, message, 20);
#else
                    int netMessagesCount = _server_SOCKET.ReceiveMessagesOnPollGroup(pollGroup, netMessages, maxMessages);

                    if (netMessagesCount > 0)
                    {
                        for (int i = 0; i < netMessagesCount; i++)
                        {
                            ref NetworkingMessage netMessage = ref netMessages[i];

                            Console.WriteLine("Message received from - ID: " + netMessage.connection + ", Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVERINPUTACTION.Write_Start(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerInputAction(), 0);
                            byte[] buffer = new byte[1024];
                            netMessage.CopyTo(buffer);
                            Avril_FSD.Library_For_Server_Concurrency.Select_Set_Intput_Subset(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), buffer[0]);
                            obj.Get_server().Get_algorithms().Get_io_ListenRespond().Decode_NetworkingSteam_At_Server_Input(obj, obj.Get_server().Get_data().Get_input_Instnace().Get_FRONT_inputDoubleBuffer(obj), buffer);
                            Avril_FSD.Library_For_Server_Concurrency.Flip_InBufferToWrite(obj.Get_server().Get_execute().Get_program_ServerConcurrency());
                            Avril_FSD.Library_For_Server_Concurrency.Push_Stack_InputPraises(obj.Get_server().Get_execute().Get_program_ServerConcurrency());
                            if (Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_Flag_ConcurrentCoreState(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_coreId_To_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency())) == Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_Flag_Idle(obj.Get_server().Get_execute().Get_program_ServerConcurrency()))
                            {
                                Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Request_Wait_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_SERVER.Get_coreId_To_Launch(obj.Get_server().Get_execute().Get_program_ServerConcurrency()));
                            }
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_SERVERINPUTACTION.Write_End(Avril_FSD.Library_For_Server_Concurrency.Get_program_WriteEnableStack_ServerInputAction(), 0);
                            netMessage.Destroy();
                        }
                    }
#endif
                    Thread.Sleep(15);
                }
                _server_SOCKET.DestroyPollGroup(pollGroup);
            }
        }
````
Do Stack Server Input Action Praise Event Id
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/engine/IO_Listen_Respond.cs
````
        public void Decode_NetworkingSteam_At_Server_Input(Avril_FSD.ServerAssembly.Framework_Server obj, Avril_FSD.ServerAssembly.Inputs.Input input, byte[] buffer)
        {
            input.Set_praiseEventId(buffer[0]);
            input.Set_in_playerId(buffer[1]);
            input.Get_input_Control().SelectSetIntputSubset(obj, input.Get_praiseEventId());
            switch (input.Get_praiseEventId())
            {
                case 0:
                    break;

                case 1:
                    var in_subset_praise1 = (Avril_FSD.ServerAssembly.Praise_Files.Praise1_Input)input.Get_praiseInputBuffer_Subset();
                    in_subset_praise1.Set_Mouse_X(BitConverter.ToSingle(buffer, 2));
                    in_subset_praise1.Set_Mouse_Y(BitConverter.ToSingle(buffer, 6));
                    break;
            }
        }
````
Encode NetworkingSteam At Server Output Send
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ServerAssembly/engine/IO_Listen_Respond.cs
````
namespace Avril_FSD.ServerAssembly
{
    public class IO_Listen_Respond
    {
     	public void Encode_NetworkingSteam_At_Server_Output(Avril_FSD.ServerAssembly.Framework_Server obj, Avril_FSD.ServerAssembly.Outputs.Output output, byte[] data)
        {
            data[0] = output.Get_praiseEventId();
            data[1] = output.Get_out_playerId();
            switch (output.Get_praiseEventId())
            {
                case 0:
                    break;

                case 1:
                    var subset = (Avril_FSD.ServerAssembly.Praise_Files.Praise1_Output)output.GetOutputBufferSubset();
                    byte[] tempFloat = BitConverter.GetBytes(subset.Get_fowards().X);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 2] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_fowards().Y);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 6] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_fowards().Z);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 10] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_right().X);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 14] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_right().Y);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 18] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_right().Z);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 22] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_up().X);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 26] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_up().Y);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 30] = tempFloat[index];
                    }
                    tempFloat = BitConverter.GetBytes(subset.Get_up().Z);
                    for (byte index = 0; index < tempFloat.Length; index++)
                    {
                        data[index + 34] = tempFloat[index];
                    }
                    break;

            }
        }
````
Client Thread Input/Output Loop
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/Networking_Client.cs
````
public void Thread_IO_Client(byte threadId)
        {
            Avril_FSD.ClientAssembly.Framework_Client obj = Avril_FSD.ClientAssembly.Program.Get_framework_Client();
            bool doneOnce = false;
            while (obj.Get_client().Get_execute().Get_execute_Control().Get_flag_SystemInitialised() == true)
            {
                if (doneOnce == false)
                {
                    doneOnce = true;
                    obj.Get_client().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(obj, threadId, false);
                }
            }
            while (obj.Get_client().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                StatusCallback status = (ref StatusInfo info) => {
                    switch (info.connectionInfo.state)
                    {
                        case ConnectionState.None:
                            break;

                        case ConnectionState.Connected:
                            Console.WriteLine("Client connected to server - ID: " + _connection);
                            if (obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_InputAction() == true)
                            {
                                System.Console.WriteLine("Thread[" + (threadId).ToString() + "] => IsLoaded_Stack_InputAction = " + obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_InputAction());//TestBench
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTINPUTACTION.Write_Start(obj.Get_client().Get_execute().Get_program_WriteQue_C_IA(), 1);
                                byte[] data = new byte[64];
                                obj.Get_client().Get_data().Get_data_Control().Pop_Stack_InputAction(obj, obj.Get_client().Get_data().Get_input_Instnace().Get_FRONT_inputDoubleBuffer(obj), obj.Get_client().Get_data().Get_input_Instnace().Get_stack_Client_InputSend());
                                obj.Get_client().Get_data().Flip_InBufferToWrite();
                                obj.Get_client().Get_algorithms().Get_io_ListenRespond().Encode_NetworkingSteam_At_Client_Input(obj, obj.Get_client().Get_data().Get_input_Instnace().Get_BACK_inputDoubleBuffer(obj), data);
                                _client_SOCKET.SendMessageToConnection(_connection, data);
                                Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTINPUTACTION.Write_End(obj.Get_client().Get_execute().Get_program_WriteQue_C_IA(), 1);
                            }
                            break;

                        case ConnectionState.ClosedByPeer:
                        case ConnectionState.ProblemDetectedLocally:
                            _client_SOCKET.CloseConnection(_connection);
                            Console.WriteLine("Client disconnected from server");
                            break;
                    }
                };
                _utils = new NetworkingUtils();
                _utils.SetStatusCallback(status);
                    
                _connection = _client_SOCKET.Connect(ref address_SERVER);
                System.Console.WriteLine("Thread[" + (threadId).ToString() + "] :: _connection = " + (_connection).ToString());//TestBench
#if VALVESOCKETS_SPAN
MessageCallback message = (in NetworkingMessage netMessage) => {
	Console.WriteLine("Message received from server - Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
};
#else
                const int maxMessages = 20;
                NetworkingMessage[] netMessages = new NetworkingMessage[maxMessages];
#endif
                System.Console.WriteLine("Thread[" + (threadId).ToString() + "] :: ALPHA");//TestBench
                while (!Console.KeyAvailable)
                {
                    System.Console.WriteLine("Thread[" + (threadId).ToString() + "] :: BRAVO");//TestBench
                    _client_SOCKET.RunCallbacks();

#if VALVESOCKETS_SPAN
	client.ReceiveMessagesOnConnection(connection, message, 20);
#else
                    int netMessagesCount = _client_SOCKET.ReceiveMessagesOnConnection(_connection, netMessages, maxMessages);
                    System.Console.WriteLine("Thread[" + (threadId).ToString() + "] :: netMessagesCount = " + netMessagesCount);//TestBench
                    if (netMessagesCount > 0)
                    {
                        for (int i = 0; i < netMessagesCount; i++)
                        {
                            ref NetworkingMessage netMessage = ref netMessages[i];

                            Console.WriteLine("Message received from server - Channel ID: " + netMessage.channel + ", Data length: " + netMessage.length);
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE.Write_Start(obj.Get_client().Get_execute().Get_program_WriteQue_C_OR(), 1);
                            byte[] buffer = new byte[1024];
                            netMessage.CopyTo(buffer);
                            obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj).Set_praiseOutputBuffer_Subset(buffer[0]);
                            obj.Get_client().Get_algorithms().Get_io_ListenRespond().Decode_NetworkingSteam_At_Client_Recieve(obj, obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj), buffer);
                            obj.Get_client().Get_data().Flip_OutBufferToWrite();
                            obj.Get_client().Get_data().Get_data_Control().Push_Stack_Client_OutputRecieve(obj, obj.Get_client().Get_data().Get_output_Instnace().Get_stack_Client_OutputRecieves(), obj.Get_client().Get_data().Get_output_Instnace().Get_FRONT_outputDoubleBuffer(obj));
                            if (obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputRecieve())
                            {
                                if (Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Get_State_LaunchBit(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C()))
                                {
                                    Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Request_Wait_Launch(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Get_coreId_To_Launch(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C()));
                                }
                            }
                            Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE.Write_End(obj.Get_client().Get_execute().Get_program_WriteQue_C_OR(), 1);
                            netMessage.Destroy();
                        }
                    }
#endif

                    Thread.Sleep(15);
                }
            }
        }
````
Decode NetworkingSteam At Client Recieve
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/engine/IO_Listen_Respond.cs
````
public void Decode_NetworkingSteam_At_Client_Recieve(Avril_FSD.ClientAssembly.Framework_Client obj, Avril_FSD.ClientAssembly.Outputs.Output output, byte[] buffer)
        {
            output.Set_praiseEventId(buffer[0]);
            output.Set_playerId(buffer[1]);
            switch (output.Get_praiseEventId())
            {
                case 0:

                    break;

                case 1:
                    var subset = (Avril_FSD.ClientAssembly.Praise_Files.Praise1_Output)output.Get_praiseOutputBuffer_Subset();
                    Vector3 tempVector = new Vector3(0);
                    tempVector.X = BitConverter.ToSingle(buffer, 2);
                    tempVector.Y = BitConverter.ToSingle(buffer, 6);
                    tempVector.Z = BitConverter.ToSingle(buffer, 10);
                    subset.Set_fowards(tempVector);
                    tempVector.X = BitConverter.ToSingle(buffer, 14);
                    tempVector.Y = BitConverter.ToSingle(buffer, 18);
                    tempVector.Z = BitConverter.ToSingle(buffer, 22);
                    subset.Set_right(tempVector);
                    tempVector.X = BitConverter.ToSingle(buffer, 26);
                    tempVector.Y = BitConverter.ToSingle(buffer, 30);
                    tempVector.Z = BitConverter.ToSingle(buffer, 34);
                    subset.Set_up(tempVector);
                    break;
            }
        }
````
Do Stack of Server Output Recieve Praise Event Id Event
https://github.com/OpenFSD/Avril_Full_Stack_Development_Template/blob/master/APP_ClientAssembly/engine/Concurrent.cs
````
public void Thread_Concurrent(byte threadId)
        {
            byte _concurrentThreadId = (byte)(threadId - (byte)2);
            Avril_FSD.ClientAssembly.Framework_Client obj = Avril_FSD.ClientAssembly.Program.Get_framework_Client();
            obj.Get_client().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(obj, threadId, false);
            bool doneOnce_A = false;
            while (obj.Get_client().Get_execute().Get_execute_Control().Get_flag_SystemInitialised() == true)
            {
                if (doneOnce_A == false)
                {
                    doneOnce_A = true;
                    obj.Get_client().Get_execute().Get_execute_Control().Set_flag_ThreadInitialised(obj, threadId, false);
                }
            }
            while (obj.Get_client().Get_execute().Get_execute_Control().Get_exitApplication() == false)
            {
                if (obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputRecieve())
                {
                    System.Console.WriteLine("Thread[" + (threadId).ToString() + "] => Get_flag_IsLoaded_Stack_OutputRecieve = " + obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputRecieve());//TestBench
                    Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE.Write_Start(obj.Get_client().Get_execute().Get_program_WriteQue_C_OR(), (byte)(_concurrentThreadId + (byte)1));
                    obj.Get_client().Get_algorithms().Get_concurrent(_concurrentThreadId).Get_concurrent_Control().SelectSet_Algorithm_Subset(obj, obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj).Get_praiseEventId(), _concurrentThreadId);
                    obj.Get_client().Get_data().Get_data_Control().Pop_Stack_OutputRecieve(obj, obj.Get_client().Get_data().Get_output_Instnace().Get_BACK_outputDoubleBuffer(obj), obj.Get_client().Get_data().Get_output_Instnace().Get_stack_Client_OutputRecieves());
                    obj.Get_client().Get_data().Flip_OutBufferToWrite();
                    obj.Get_client().Get_data().Get_data_Control().Do_Store_PraiseOutputRecieve_To_GameInstanceData(obj, obj.Get_client().Get_data().Get_output_Instnace().Get_stack_Client_OutputRecieves().ElementAt(1));
                    obj.Get_client().Get_data().Get_data_Control().Set_isPraiseActive(obj.Get_client().Get_data().Get_output_Instnace().Get_FRONT_outputDoubleBuffer(obj).Get_praiseEventId(), false);
                    Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Thread_End(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C(), _concurrentThreadId);
                    if (obj.Get_client().Get_data().Get_data_Control().Get_flag_IsLoaded_Stack_OutputRecieve())
                    {
                        if(Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Get_State_LaunchBit(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C()) == Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Get_Flag_Idle(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C()))
                        {
                            Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Request_Wait_Launch(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C(), Avril_FSD.Library_For_LaunchEnableForConcurrentThreadsAt_CLIENT.Get_coreId_To_Launch(obj.Get_client().Get_execute().Get_program_ConcurrentQue_C()));
                        }
                    }
                    Avril_FSD.Library_For_WriteEnableForThreadsAt_CLIENTOUTPUTRECIEVE.Write_End(obj.Get_client().Get_execute().Get_program_WriteQue_C_OR(), (byte)(_concurrentThreadId + (byte)1));
                }
            }
        }
````
