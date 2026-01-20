
namespace Avril_FSD.ClientAssembly
{
    public class Execute_Control
    {
        private bool _exitApplication;
        private bool _flag_SystemInitialised;
        private bool[] _flag_ThreadInitialised;

        public Execute_Control(int numberOfCores)
        {
            _exitApplication = false;
            _flag_SystemInitialised = true;
            _flag_ThreadInitialised = new bool[numberOfCores];
            for(byte index = 0; index < numberOfCores; index++)
            {
                _flag_ThreadInitialised[index] = true;
            }
        }

        private void Calc_flag_isInitialised_ClientApp(Avril_FSD.ClientAssembly.Framework_Client obj)
        {
            bool isInitialised = false;
            for (byte index = 0; index < _flag_ThreadInitialised.Length; index++)
            {
                if (obj.Get_client().Get_execute().Get_execute_Control().Get_flag_ThreadInitialised(index) == true)
                {
                    isInitialised = true;
                }
            }
            if (isInitialised == false)
            {
                obj.Get_client().Get_execute().Get_execute_Control().Set_exitApplication(false);
            }
            obj.Get_client().Get_execute().Get_execute_Control().Set_flag_SystemInitialised(isInitialised);
        }
        public bool Get_exitApplication()
        {
            return _exitApplication;
        }
        public bool Get_flag_SystemInitialised()
        {
            return _flag_SystemInitialised;
        }
        public bool Get_flag_ThreadInitialised(byte coreId)
        {   
            return _flag_ThreadInitialised[coreId];
        }
        public void Set_exitApplication(bool flag)
        {
            _exitApplication = flag;
        }
        private void Set_flag_SystemInitialised(bool flag)
        {
            _flag_SystemInitialised = flag;
        }
        public void Set_flag_ThreadInitialised(Avril_FSD.ClientAssembly.Framework_Client obj, byte coreId, bool value)
        {
            _flag_ThreadInitialised[coreId] = value;
            Calc_flag_isInitialised_ClientApp(obj);
        }
    }
}
