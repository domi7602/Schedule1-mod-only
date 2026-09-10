using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.UI.Input;

public class InputPromptsSpriteData : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_sprite;

	private static readonly IntPtr NativeFieldInfoPtr_Size;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Sprite sprite
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sprite);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Sprite>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sprite)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite));
		}
	}

	public unsafe Vector2 Size
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Size);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Size)) = vector;
		}
	}

	static InputPromptsSpriteData()
	{
		Il2CppClassPointerStore<InputPromptsSpriteData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.Input", "InputPromptsSpriteData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InputPromptsSpriteData>.NativeClassPtr);
		NativeFieldInfoPtr_sprite = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsSpriteData>.NativeClassPtr, "sprite");
		NativeFieldInfoPtr_Size = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptsSpriteData>.NativeClassPtr, "Size");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InputPromptsSpriteData>.NativeClassPtr, 100689181);
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 79617, RefRangeEnd = 79648, XrefRangeStart = 79617, XrefRangeEnd = 79648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InputPromptsSpriteData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InputPromptsSpriteData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public InputPromptsSpriteData(IntPtr pointer)
		: base(pointer)
	{
	}
}
