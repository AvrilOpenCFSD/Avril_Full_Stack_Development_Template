// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the LIBSERVERIOCONCURRNECY_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// LIBSERVERIOCONCURRNECY_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef LIBSERVERIOCONCURRNECY_EXPORTS
#define LIBSERVERIOCONCURRNECY_API __declspec(dllexport)
#else
#define LIBSERVERIOCONCURRNECY_API __declspec(dllimport)
#endif

namespace Avril_FSD
{
	// This class is exported from the dll
	class LIBSERVERIOCONCURRNECY_API CLIBServerIOConcurrnecy {
	public:
		CLIBServerIOConcurrnecy(void);
		// TODO: add your methods here.
		static void* Initialise_Server_Concurrency();
		static void Initalise_Programs(class Avril_FSD::Framework_Server* obj);
		static void Flip_InBufferToWrite(class Avril_FSD::Framework_Server* obj);
		static void Flip_OutBufferToWrite(class Avril_FSD::Framework_Server* obj);
		static bool Get_flag_isNewInputDataReady(class Avril_FSD::Framework_Server* obj);
		static bool Get_flag_isNewOutputDataReady(class Avril_FSD::Framework_Server* obj);
		static bool Get_flag_IsStackLoaded_Server_InputAction(class Avril_FSD::Framework_Server* obj);
		static bool Get_flag_IsStackLoaded_Server_OutputRecieve(class Avril_FSD::Framework_Server* obj);
		static bool Get_flag_IsInitialised_Avril_FSD_ServerConcurrency(class Avril_FSD::Framework_Server* obj);
		static void* Get_program_WriteEnableStack_ServerInputAction();
		static void* Get_program_WriteEnableStack_ServerOutputRecieve();
		static void Pop_Stack_Output(class Avril_FSD::Framework_Server* obj);
		static void Push_Stack_InputPraises(class Avril_FSD::Framework_Server* obj);
		static void Select_Set_Intput_Subset(class Avril_FSD::Framework_Server* obj, __int8 praiseEventId);
		static void Set_flag_IsNewInputDataReady(class Avril_FSD::Framework_Server* obj, bool value);
		static void Set_flag_IsNewOutputDataReady(class Avril_FSD::Framework_Server* obj, bool value);

		// Praise Event Id
		static __int8 Get_PraiseEventId(class Avril_FSD::Framework_Server* obj);
		static void Set_PraiseEventId(class Avril_FSD::Framework_Server* obj, __int8 value);
		// Praise 0 Data
		static bool Get_Praise0_Input_IsPingActive(class Avril_FSD::Framework_Server* obj);
		static void Set_Praise0_Input_IsPingActive(class Avril_FSD::Framework_Server* obj, bool value);
		static bool Get_Praise0_Output_IsPingActive(class Avril_FSD::Framework_Server* obj);
		static void Set_Praise0_Output_IsPingActive(class Avril_FSD::Framework_Server* obj, bool value);
		// Praise 1 Data
		static float Get_Praise1_Input_mouseDelta_X(class Avril_FSD::Framework_Server* obj);
		static float Get_Praise1_Input_mouseDelta_Y(class Avril_FSD::Framework_Server* obj);
		static void Set_Praise1_Input_mouseDelta_X(class Avril_FSD::Framework_Server* obj, float value);
		static void Set_Praise1_Input_mouseDelta_Y(class Avril_FSD::Framework_Server* obj, float value);
		static Eigen::Vector3d Get_Praise1_Output_Player_Fowards(class Avril_FSD::Framework_Server* obj);
		static Eigen::Vector3d Get_Praise1_Output_Player_Up(class Avril_FSD::Framework_Server* obj);
		static Eigen::Vector3d Get_Praise1_Output_Player_Right(class Avril_FSD::Framework_Server* obj);
		static void Set_Praise1_Output_Player_Fowards(class Avril_FSD::Framework_Server* obj, Eigen::Vector3d value);
		static void Set_Praise1_Output_Player_Up(class Avril_FSD::Framework_Server* obj, Eigen::Vector3d value);
		static void Set_Praise1_Output_Player_Right(class Avril_FSD::Framework_Server* obj, Eigen::Vector3d value);
		// Praise 0 Data

		// TODO: add your methods here.


	};
}
extern LIBSERVERIOCONCURRNECY_API int nLIBServerIOConcurrnecy;

LIBSERVERIOCONCURRNECY_API int fnLIBServerIOConcurrnecy(void);
