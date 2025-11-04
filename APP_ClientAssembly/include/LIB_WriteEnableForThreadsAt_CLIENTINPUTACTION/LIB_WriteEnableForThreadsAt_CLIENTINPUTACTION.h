#ifdef LIBWRITEENABLEFORTHREADSATCLIENTINPUTACTION_EXPORTS
#define LIBWRITEENABLEFORTHREADSATCLIENTINPUTACTION_API __declspec(dllexport)
#else
#define LIBWRITEENABLEFORTHREADSATCLIENTINPUTACTION_API __declspec(dllimport)
#endif

namespace Avril_FSD
{
	class LIBWRITEENABLEFORTHREADSATCLIENTINPUTACTION_API Library_WriteEnableForThreadsAt_CLIENTINPUTACTION {
	public:
		Library_WriteEnableForThreadsAt_CLIENTINPUTACTION();
		static void* Initialise_WriteEnable();
		static void Write_End(class WriteEnableForThreadsAt_CLIENTINPUTACTION_Framework* obj, unsigned char coreId);
		static void Write_Start(class WriteEnableForThreadsAt_CLIENTINPUTACTION_Framework* obj, unsigned char coreId);

	private:
		static class WriteEnableForThreadsAt_CLIENTINPUTACTION_Framework* Get_Framework_WriteEnable();
		static void Set_writeEnable(class WriteEnableForThreadsAt_CLIENTINPUTACTION_Framework* writeEnable);
	};
}