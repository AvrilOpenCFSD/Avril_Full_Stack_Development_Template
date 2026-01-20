
namespace Avril_FSD.ClientAssembly.Inputs
{
    public class Input_Instance
    {
        private Avril_FSD.ClientAssembly.Inputs.Input_Instance_Control _inputInstance_Control;
        private Avril_FSD.ClientAssembly.Inputs.Input _empty_InputBuffer;
        private Avril_FSD.ClientAssembly.Inputs.Input[] _doublebuffer_Client_Input_Send;
        private List<Avril_FSD.ClientAssembly.Inputs.Input> _stack_Client_InputSend;

        public Input_Instance() 
        {
            Set_inputInstance_Control(new Avril_FSD.ClientAssembly.Inputs.Input_Instance_Control());
            while (Get_inputInstance_Control() == null) { }

            Set_empty_InputBuffer(new Avril_FSD.ClientAssembly.Inputs.Input());
            while (Get_empty_InputBuffer() == null) { }
            Get_empty_InputBuffer().InitialiseControl();

            _doublebuffer_Client_Input_Send = new Avril_FSD.ClientAssembly.Inputs.Input[2];
            for (byte index_A = 0; index_A < 2; index_A++)
            {
                _doublebuffer_Client_Input_Send[index_A] = Get_empty_InputBuffer();
                while (_doublebuffer_Client_Input_Send[index_A] == null) { }
            }

            _stack_Client_InputSend = new List<Avril_FSD.ClientAssembly.Inputs.Input>();
            while (_stack_Client_InputSend == null) { }
            _stack_Client_InputSend.Capacity = 1;
            _stack_Client_InputSend.Add(Get_empty_InputBuffer());
        }

        private UInt16 BoolToInt16(bool value)
        {
            UInt16 temp = new UInt16();
            if (value)
            {
                temp = (UInt16)1;
            }
            else if (!value)
            {
                temp = (UInt16)0;
            }
            return temp;
        }

        public Avril_FSD.ClientAssembly.Inputs.Input Get_FRONT_inputDoubleBuffer(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            return _doublebuffer_Client_Input_Send[BoolToInt16(obj.Get_client().Get_data().Get_state_Buffer_Input_ToWrite())];
        }
        public Avril_FSD.ClientAssembly.Inputs.Input Get_BACK_inputDoubleBuffer(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            return _doublebuffer_Client_Input_Send[BoolToInt16(!obj.Get_client().Get_data().Get_state_Buffer_Input_ToWrite())];
        }
        public Avril_FSD.ClientAssembly.Inputs.Input Get_empty_InputBuffer()
        {
            return _empty_InputBuffer;
        }
        public Avril_FSD.ClientAssembly.Inputs.Input_Instance_Control Get_inputInstance_Control()
        {
            return _inputInstance_Control;
        }
        public List<Avril_FSD.ClientAssembly.Inputs.Input> Get_stack_Client_InputSend()
        {
            return _stack_Client_InputSend;
        }
        public void Set_inputDoubleBuffer(Avril_FSD.ClientAssembly.Framework_Client obj, Avril_FSD.ClientAssembly.Inputs.Input value)
        {
            _doublebuffer_Client_Input_Send[BoolToInt16(obj.Get_client().Get_data().Get_state_Buffer_Input_ToWrite())] = value;
        }

        public void Set_empty_InputBuffer(Avril_FSD.ClientAssembly.Inputs.Input input)
        {
            _empty_InputBuffer = input;
        }

        public void Set_inputInstance_Control(Avril_FSD.ClientAssembly.Inputs.Input_Instance_Control input_Instance_Control)
        {
            _inputInstance_Control = input_Instance_Control;
        }
    }
}
