
namespace Avril_FSD.ClientAssembly.Outputs
{
    public class Output_Instance
    {
        private Avril_FSD.ClientAssembly.Outputs.Output _empty_OutputBuffer;
        private Avril_FSD.ClientAssembly.Outputs.Output[] _buffer_Output_Recieve_Reference_ForCore;
        private Avril_FSD.ClientAssembly.Outputs.Output[] _doublebuffer_Client_Output_Recieve;
        private List<Avril_FSD.ClientAssembly.Outputs.Output> _stack_Client_OutputRecieves;

        public Output_Instance() 
        {
            Set_empty_OutputBuffer(new Avril_FSD.ClientAssembly.Outputs.Output());
            while (Get_empty_OutputBuffer() == null) { }
            Get_empty_OutputBuffer().Initialise_Control();

            _buffer_Output_Recieve_Reference_ForCore = new Avril_FSD.ClientAssembly.Outputs.Output[2];
            for (byte index = 0; index < 2; index++)
            {
                Set_buffer_Output_Recieve_Reference_ForCore(index,Get_empty_OutputBuffer());
                while (Get_buffer_Output_Recieve_Reference_ForCore(index) == null) { }
            }

            _stack_Client_OutputRecieves = new List<Avril_FSD.ClientAssembly.Outputs.Output>();
            while (_stack_Client_OutputRecieves == null) { }
            _stack_Client_OutputRecieves.Capacity = 1;
            _stack_Client_OutputRecieves.Add(Get_empty_OutputBuffer());

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

        public Avril_FSD.ClientAssembly.Outputs.Output Get_empty_OutputBuffer()
        {
            return _empty_OutputBuffer;
        }
        public Avril_FSD.ClientAssembly.Outputs.Output Get_buffer_Output_Recieve_Reference_ForCore(byte concurrentCoreId)
        {
            return _buffer_Output_Recieve_Reference_ForCore[concurrentCoreId];
        }
        public Avril_FSD.ClientAssembly.Outputs.Output Get_FRONT_outputDoubleBuffer(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            return _doublebuffer_Client_Output_Recieve[BoolToInt16(obj.Get_client().Get_data().Get_state_Buffer_Output_ToWrite())];
        }
        public Avril_FSD.ClientAssembly.Outputs.Output Get_BACK_outputDoubleBuffer(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            return _doublebuffer_Client_Output_Recieve[BoolToInt16(!obj.Get_client().Get_data().Get_state_Buffer_Output_ToWrite())];
        }

        public List<Avril_FSD.ClientAssembly.Outputs.Output> Get_stack_Client_OutputRecieves()
        {
            return _stack_Client_OutputRecieves;
        }

        private void Set_empty_OutputBuffer(Avril_FSD.ClientAssembly.Outputs.Output value)
        {
            _empty_OutputBuffer = value;
        }
        private void Set_buffer_Output_Recieve_Reference_ForCore(byte concurrenctCoreId, Avril_FSD.ClientAssembly.Outputs.Output input_Instance)
        {
            _buffer_Output_Recieve_Reference_ForCore[concurrenctCoreId] = input_Instance;
        }
        private void Set_stack_Client_OutputRecieves(List<Avril_FSD.ClientAssembly.Outputs.Output> stack_Client_OutputRecieves)
        {
            _stack_Client_OutputRecieves = stack_Client_OutputRecieves;
        }
    }
}
