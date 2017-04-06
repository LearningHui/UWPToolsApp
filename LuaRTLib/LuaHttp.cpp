#include "pch.h"
#include "LuaHttp.h"

namespace LuaRTLib
{
	int LuaHttp::abort(lua_State *lua)
	{
		return LuaHttpDelegates::abort(reinterpret_cast<int>(lua));
	}

	int LuaHttp::postSyn(lua_State *lua)
	{
		return LuaHttpDelegates::postSyn(reinterpret_cast<int>(lua));
	}

	int LuaHttp::postAsyn(lua_State *lua)
	{
		return LuaHttpDelegates::postAsyn(reinterpret_cast<int>(lua));
	}

	int LuaHttp::setListener(lua_State *lua)
	{
		return LuaHttpDelegates::setListener(reinterpret_cast<int>(lua));
	}

	const struct luaL_Reg libs [] =
	{
		{"abort",LuaHttp::abort},
		{"postSyn",LuaHttp::postSyn},
		{"postAsyn",LuaHttp::postAsyn},
		{"setListener",LuaHttp::setListener},
		{NULL,NULL}
	};

	int LuaHttp::registerLib(lua_State *lua)
	{
		luaL_register(lua, "http", libs);
		return 1;
	}
}
