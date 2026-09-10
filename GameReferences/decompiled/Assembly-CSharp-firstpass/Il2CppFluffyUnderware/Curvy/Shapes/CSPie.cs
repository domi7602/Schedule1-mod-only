using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Shapes;

public class CSPie : CSCircle
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "EatModeEnum")]
	public enum EatModeEnum
	{
		Left,
		Right,
		Center
	}

	private static readonly IntPtr NativeFieldInfoPtr_m_Roundness;

	private static readonly IntPtr NativeFieldInfoPtr_m_Empty;

	private static readonly IntPtr NativeFieldInfoPtr_m_Eat;

	private static readonly IntPtr NativeMethodInfoPtr_get_Roundness_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Roundness_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Empty_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Empty_Public_set_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_maxEmpty_Private_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Eat_Public_get_EatModeEnum_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Eat_Public_set_Void_EatModeEnum_0;

	private static readonly IntPtr NativeMethodInfoPtr_cpPosition_Private_Vector3_Int32_Int32_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ApplyShape_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float m_Roundness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Roundness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Roundness)) = num;
		}
	}

	public unsafe int m_Empty
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Empty);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Empty)) = num;
		}
	}

	public unsafe EatModeEnum m_Eat
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Eat);
			return *(EatModeEnum*)num;
		}
		set
		{
			*(EatModeEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Eat)) = eatModeEnum;
		}
	}

	public unsafe float Roundness
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Roundness_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42860, XrefRangeEnd = 42861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Roundness_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int Empty
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 42861, RefRangeEnd = 42871, XrefRangeStart = 42861, XrefRangeEnd = 42861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Empty_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Empty_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int maxEmpty
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38121, RefRangeEnd = 38127, XrefRangeStart = 38121, XrefRangeEnd = 38127, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_maxEmpty_Private_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe EatModeEnum Eat
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 42871, RefRangeEnd = 42874, XrefRangeStart = 42871, XrefRangeEnd = 42871, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Eat_Public_get_EatModeEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(EatModeEnum*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Eat_Public_set_Void_EatModeEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static CSPie()
	{
		Il2CppClassPointerStore<CSPie>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Shapes", "CSPie");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CSPie>.NativeClassPtr);
		NativeFieldInfoPtr_m_Roundness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CSPie>.NativeClassPtr, "m_Roundness");
		NativeFieldInfoPtr_m_Empty = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CSPie>.NativeClassPtr, "m_Empty");
		NativeFieldInfoPtr_m_Eat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CSPie>.NativeClassPtr, "m_Eat");
		NativeMethodInfoPtr_get_Roundness_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665199);
		NativeMethodInfoPtr_set_Roundness_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665200);
		NativeMethodInfoPtr_get_Empty_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665201);
		NativeMethodInfoPtr_set_Empty_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665202);
		NativeMethodInfoPtr_get_maxEmpty_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665203);
		NativeMethodInfoPtr_get_Eat_Public_get_EatModeEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665204);
		NativeMethodInfoPtr_set_Eat_Public_set_Void_EatModeEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665205);
		NativeMethodInfoPtr_cpPosition_Private_Vector3_Int32_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665206);
		NativeMethodInfoPtr_ApplyShape_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665207);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CSPie>.NativeClassPtr, 100665208);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 42880, RefRangeEnd = 42885, XrefRangeStart = 42874, XrefRangeEnd = 42880, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 cpPosition(int i, int empty, float d)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&i);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &empty;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &d;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_cpPosition_Private_Vector3_Int32_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42885, XrefRangeEnd = 42999, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ApplyShape()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ApplyShape_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42999, XrefRangeEnd = 43000, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CSPie()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CSPie>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CSPie(IntPtr pointer)
		: base(pointer)
	{
	}
}
