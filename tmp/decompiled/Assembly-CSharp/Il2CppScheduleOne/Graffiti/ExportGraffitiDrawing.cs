using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Graffiti;

public class ExportGraffitiDrawing : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_ContainerPath;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static string ContainerPath
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ContainerPath, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ContainerPath, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static ExportGraffitiDrawing()
	{
		Il2CppClassPointerStore<ExportGraffitiDrawing>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Graffiti", "ExportGraffitiDrawing");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ExportGraffitiDrawing>.NativeClassPtr);
		NativeFieldInfoPtr_ContainerPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExportGraffitiDrawing>.NativeClassPtr, "ContainerPath");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExportGraffitiDrawing>.NativeClassPtr, 100672734);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ExportGraffitiDrawing()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ExportGraffitiDrawing>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ExportGraffitiDrawing(IntPtr pointer)
		: base(pointer)
	{
	}
}
