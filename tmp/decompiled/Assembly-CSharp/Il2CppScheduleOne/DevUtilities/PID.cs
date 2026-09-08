using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.DevUtilities;

[System.Serializable]
public class PID : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_pFactor;

	private static readonly System.IntPtr NativeFieldInfoPtr_iFactor;

	private static readonly System.IntPtr NativeFieldInfoPtr_dFactor;

	private static readonly System.IntPtr NativeFieldInfoPtr_integral;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastError;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Single_Single_Single_Single_0;

	public unsafe float pFactor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pFactor);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pFactor)) = num;
		}
	}

	public unsafe float iFactor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_iFactor);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_iFactor)) = num;
		}
	}

	public unsafe float dFactor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dFactor);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dFactor)) = num;
		}
	}

	public unsafe float integral
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_integral);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_integral)) = num;
		}
	}

	public unsafe float lastError
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastError);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastError)) = num;
		}
	}

	static PID()
	{
		Il2CppClassPointerStore<PID>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "PID");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PID>.NativeClassPtr);
		NativeFieldInfoPtr_pFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PID>.NativeClassPtr, "pFactor");
		NativeFieldInfoPtr_iFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PID>.NativeClassPtr, "iFactor");
		NativeFieldInfoPtr_dFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PID>.NativeClassPtr, "dFactor");
		NativeFieldInfoPtr_integral = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PID>.NativeClassPtr, "integral");
		NativeFieldInfoPtr_lastError = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PID>.NativeClassPtr, "lastError");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PID>.NativeClassPtr, 100675135);
		NativeMethodInfoPtr_Update_Public_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PID>.NativeClassPtr, 100675136);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 195737, RefRangeEnd = 195739, XrefRangeStart = 195736, XrefRangeEnd = 195737, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PID(float pFactor, float iFactor, float dFactor)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PID>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&pFactor);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &iFactor;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &dFactor;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 195739, RefRangeEnd = 195742, XrefRangeStart = 195739, XrefRangeEnd = 195739, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float Update(float setpoint, float actual, float timeFrame)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&setpoint);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &actual;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &timeFrame;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Single_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public PID(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
