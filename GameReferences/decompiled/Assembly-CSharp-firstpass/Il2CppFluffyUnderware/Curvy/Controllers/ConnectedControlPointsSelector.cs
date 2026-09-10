using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppFluffyUnderware.Curvy.Controllers;

public class ConnectedControlPointsSelector : DTVersionedMonoBehaviour
{
	private static readonly IntPtr NativeMethodInfoPtr_SelectConnectedControlPoint_Public_Abstract_Virtual_New_CurvySplineSegment_SplineController_CurvyConnection_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	static ConnectedControlPointsSelector()
	{
		Il2CppClassPointerStore<ConnectedControlPointsSelector>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Controllers", "ConnectedControlPointsSelector");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ConnectedControlPointsSelector>.NativeClassPtr);
		NativeMethodInfoPtr_SelectConnectedControlPoint_Public_Abstract_Virtual_New_CurvySplineSegment_SplineController_CurvyConnection_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConnectedControlPointsSelector>.NativeClassPtr, 100665351);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConnectedControlPointsSelector>.NativeClassPtr, 100665352);
	}

	[CallerCount(0)]
	public unsafe virtual CurvySplineSegment SelectConnectedControlPoint(SplineController caller, CurvyConnection connection, CurvySplineSegment currentControlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)caller);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)connection);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)currentControlPoint);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SelectConnectedControlPoint_Public_Abstract_Virtual_New_CurvySplineSegment_SplineController_CurvyConnection_CurvySplineSegment_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 34977, RefRangeEnd = 34989, XrefRangeStart = 34977, XrefRangeEnd = 34989, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ConnectedControlPointsSelector()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ConnectedControlPointsSelector>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ConnectedControlPointsSelector(IntPtr pointer)
		: base(pointer)
	{
	}
}
