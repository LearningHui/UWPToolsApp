#pragma once
#include "pch.h"

extern "C"
{
#include <string.h>
#include <lualib.h>
#include <lauxlib.h>
}

namespace LuaRTLib
{
	class LuaHttp
	{
	public:
		static int registerLib(lua_State *lua);

		static int abort(lua_State *lua);
		static int postSyn(lua_State *lua);
		static int postAsyn(lua_State *lua);
		static int setListener(lua_State *lua);
	};
}
