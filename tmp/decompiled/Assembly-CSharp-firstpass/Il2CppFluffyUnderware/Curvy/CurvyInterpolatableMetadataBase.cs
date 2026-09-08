using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvyInterpolatableMetadataBase<T> : CurvyMetadataBase
{
	private static readonly System.IntPtr NativeMethodInfoPtr_get_MetaDataValue_Public_Abstract_Virtual_New_get_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Interpolate_Public_Abstract_Virtual_New_T_CurvyInterpolatableMetadataBase_1_T_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe virtual T MetaDataValue
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_MetaDataValue_Public_Abstract_Virtual_New_get_T_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
		}
	}

	static CurvyInterpolatableMetadataBase()
	{
		Il2CppClassPointerStore<CurvyInterpolatableMetadataBase<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvyInterpolatableMetadataBase`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyInterpolatableMetadataBase<T>>.NativeClassPtr);
		NativeMethodInfoPtr_get_MetaDataValue_Public_Abstract_Virtual_New_get_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyInterpolatableMetadataBase<T>>.NativeClassPtr, 100665165);
		NativeMethodInfoPtr_Interpolate_Public_Abstract_Virtual_New_T_CurvyInterpolatableMetadataBase_1_T_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyInterpolatableMetadataBase<T>>.NativeClassPtr, 100665166);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyInterpolatableMetadataBase<T>>.NativeClassPtr, 100665167);
	}

	[CallerCount(0)]
	public unsafe virtual T Interpolate(CurvyInterpolatableMetadataBase<T> nextMetadata, float interpolationTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nextMetadata);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &interpolationTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Interpolate_Public_Abstract_Virtual_New_T_CurvyInterpolatableMetadataBase_1_T_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyInterpolatableMetadataBase()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyInterpolatableMetadataBase<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvyInterpolatableMetadataBase(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
