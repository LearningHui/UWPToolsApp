#pragma once

namespace LuaRTLib
{
	public delegate int abortDel(int L);
	public delegate int postSynDel(int L);
	public delegate int postAsynDel(int L);
	public delegate int HTTP_setListenerDel(int L);

	public ref class LuaHttpDelegates sealed
	{
	public:

		property static abortDel^ abort;

		property static postSynDel^ postSyn;

		property static postAsynDel^ postAsyn;

		property static HTTP_setListenerDel^ setListener;
	};
}
