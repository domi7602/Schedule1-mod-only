using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Lighting;

public class UsableLightSource : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_GrowSpeedMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_isEmitting;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float GrowSpeedMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowSpeedMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowSpeedMultiplier)) = num;
		}
	}

	public unsafe bool isEmitting
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isEmitting);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isEmitting)) = flag;
		}
	}

	static UsableLightSource()
	{
		Il2CppClassPointerStore<UsableLightSource>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Lighting", "UsableLightSource");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UsableLightSource>.NativeClassPtr);
		NativeFieldInfoPtr_GrowSpeedMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UsableLightSource>.NativeClassPtr, "GrowSpeedMultiplier");
		NativeFieldInfoPtr_isEmitting = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UsableLightSource>.NativeClassPtr, "isEmitting");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UsableLightSource>.NativeClassPtr, 100674942);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 193379, XrefRangeEnd = 193380, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UsableLightSource()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UsableLightSource>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UsableLightSource(IntPtr pointer)
		: base(pointer)
	{
	}
}
