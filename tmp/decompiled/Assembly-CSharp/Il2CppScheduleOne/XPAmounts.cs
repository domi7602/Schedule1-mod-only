using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne;

public static class XPAmounts : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_PLAYER_COMPLETED_DEAL;

	private static readonly System.IntPtr NativeFieldInfoPtr_DEALER_COMPLETED_DEAL;

	private static readonly System.IntPtr NativeFieldInfoPtr_SUCCESSFUL_SAMPLE;

	private static readonly System.IntPtr NativeFieldInfoPtr_HARVESTED_PLANT;

	private static readonly System.IntPtr NativeFieldInfoPtr_ESCAPED_ARREST;

	private static readonly System.IntPtr NativeFieldInfoPtr_ESCAPED_WANTED;

	private static readonly System.IntPtr NativeFieldInfoPtr_ESCAPED_WANTED2;

	private static readonly System.IntPtr NativeFieldInfoPtr_NEW_MIXTURE_DISCOVERED;

	private static readonly System.IntPtr NativeFieldInfoPtr_GRAFFITI_SPRAYED;

	public unsafe static int PLAYER_COMPLETED_DEAL
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PLAYER_COMPLETED_DEAL, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PLAYER_COMPLETED_DEAL, (void*)(&num));
		}
	}

	public unsafe static int DEALER_COMPLETED_DEAL
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DEALER_COMPLETED_DEAL, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DEALER_COMPLETED_DEAL, (void*)(&num));
		}
	}

	public unsafe static int SUCCESSFUL_SAMPLE
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SUCCESSFUL_SAMPLE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SUCCESSFUL_SAMPLE, (void*)(&num));
		}
	}

	public unsafe static int HARVESTED_PLANT
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HARVESTED_PLANT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HARVESTED_PLANT, (void*)(&num));
		}
	}

	public unsafe static int ESCAPED_ARREST
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ESCAPED_ARREST, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ESCAPED_ARREST, (void*)(&num));
		}
	}

	public unsafe static int ESCAPED_WANTED
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ESCAPED_WANTED, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ESCAPED_WANTED, (void*)(&num));
		}
	}

	public unsafe static int ESCAPED_WANTED2
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ESCAPED_WANTED2, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ESCAPED_WANTED2, (void*)(&num));
		}
	}

	public unsafe static int NEW_MIXTURE_DISCOVERED
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NEW_MIXTURE_DISCOVERED, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NEW_MIXTURE_DISCOVERED, (void*)(&num));
		}
	}

	public unsafe static int GRAFFITI_SPRAYED
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GRAFFITI_SPRAYED, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GRAFFITI_SPRAYED, (void*)(&num));
		}
	}

	static XPAmounts()
	{
		Il2CppClassPointerStore<XPAmounts>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "XPAmounts");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr);
		NativeFieldInfoPtr_PLAYER_COMPLETED_DEAL = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "PLAYER_COMPLETED_DEAL");
		NativeFieldInfoPtr_DEALER_COMPLETED_DEAL = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "DEALER_COMPLETED_DEAL");
		NativeFieldInfoPtr_SUCCESSFUL_SAMPLE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "SUCCESSFUL_SAMPLE");
		NativeFieldInfoPtr_HARVESTED_PLANT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "HARVESTED_PLANT");
		NativeFieldInfoPtr_ESCAPED_ARREST = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "ESCAPED_ARREST");
		NativeFieldInfoPtr_ESCAPED_WANTED = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "ESCAPED_WANTED");
		NativeFieldInfoPtr_ESCAPED_WANTED2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "ESCAPED_WANTED2");
		NativeFieldInfoPtr_NEW_MIXTURE_DISCOVERED = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "NEW_MIXTURE_DISCOVERED");
		NativeFieldInfoPtr_GRAFFITI_SPRAYED = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XPAmounts>.NativeClassPtr, "GRAFFITI_SPRAYED");
	}

	public XPAmounts(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
