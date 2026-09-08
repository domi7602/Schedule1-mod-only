using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Map;

public class LadderSizeSetter : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_Size;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

	static LadderSizeSetter()
	{
		Il2CppClassPointerStore<LadderSizeSetter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Map", "LadderSizeSetter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LadderSizeSetter>.NativeClassPtr);
		NativeFieldInfoPtr_Size = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LadderSizeSetter>.NativeClassPtr, "Size");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LadderSizeSetter>.NativeClassPtr, 100670148);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 141991, XrefRangeEnd = 141992, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LadderSizeSetter()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LadderSizeSetter>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LadderSizeSetter(IntPtr pointer)
		: base(pointer)
	{
	}
}
