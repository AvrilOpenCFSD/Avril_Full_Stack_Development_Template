
namespace Avril_FSD.ServerAssembly
{
    public class Execute_Control
    {
        private bool _exitApplication;
        private bool _flag_SystemInitialised;
        private bool[] _flag_ThreadInitialised;

        public Execute_Control(int numberOfCores)
        {
            _exitApplication = false;
            _flag_ThreadInitialised = new bool[numberOfCores];
            for(byte index = 0; index < numberOfCores; index++)
            {
                Set_flag_ThreadInitialised(index, true);
            }
            _flag_SystemInitialised = true;
        }
        private void Calc_flag_SystemInitialised(Avril_FSD.ServerAssembly.Framework_Server obj)
        {
            bool isInitialised = false;
            for (byte index = 0; index < _flag_ThreadInitialised.Length; index++)
            {
                if (obj.Get_server().Get_execute().Get_execute_Control().Get_flag_ThreadInitialised(index) == true)
                {
                    isInitialised = true;
                }
            }
            if (isInitialised == false)
            {
                obj.Get_server().Get_execute().Get_execute_Control().Set_exitApplication(false);
            }
            obj.Get_server().Get_execute().Get_execute_Control().Set_flag_SystemInitialised(isInitialised);
        }
        public bool Get_exitApplication()
        {
            return _exitApplication;
        }
        public bool Get_flag_isInitialised_ServerShell(Avril_FSD.ServerAssembly.Framework_Server obj)
        {
            Calc_flag_SystemInitialised(obj);
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
        public void Set_flag_ThreadInitialised(byte coreId, bool value)
        {
            _flag_ThreadInitialised[coreId] = false;
        }
    }
}
