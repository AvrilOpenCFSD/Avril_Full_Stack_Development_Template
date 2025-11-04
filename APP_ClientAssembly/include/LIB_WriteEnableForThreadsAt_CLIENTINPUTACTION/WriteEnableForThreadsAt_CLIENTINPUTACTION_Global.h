#pragma once
#include <array>
#include <vector>

namespace Avril_FSD
{
    class WriteEnableForThreadsAt_CLIENTINPUTACTION_Global
    {
    public:
        WriteEnableForThreadsAt_CLIENTINPUTACTION_Global();
        ~WriteEnableForThreadsAt_CLIENTINPUTACTION_Global();
        unsigned char Get_NumCores();
        std::vector<bool> Get_flag_write_IDLE();
        std::vector<bool> Get_flag_write_WAIT();
        std::vector<bool> Get_flag_write_WRITE();
    };
}

