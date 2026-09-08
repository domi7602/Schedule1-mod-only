using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public static class TransformUtils : Il2CppSystem.Object
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Packed
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_position;

		private static readonly System.IntPtr NativeFieldInfoPtr_rotation;

		private static readonly System.IntPtr NativeFieldInfoPtr_lossyScale;

		private static readonly System.IntPtr NativeMethodInfoPtr_IsSame_Public_Boolean_Transform_0;

		[FieldOffset(0)]
		public Vector3 position;

		[FieldOffset(12)]
		public Quaternion rotation;

		[FieldOffset(28)]
		public Vector3 lossyScale;

		static Packed()
		{
			Il2CppClassPointerStore<Packed>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<TransformUtils>.NativeClassPtr, "Packed");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Packed>.NativeClassPtr);
			NativeFieldInfoPtr_position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Packed>.NativeClassPtr, "position");
			NativeFieldInfoPtr_rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Packed>.NativeClassPtr, "rotation");
			NativeFieldInfoPtr_lossyScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Packed>.NativeClassPtr, "lossyScale");
			NativeMethodInfoPtr_IsSame_Public_Boolean_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Packed>.NativeClassPtr, 100664373);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 73854, RefRangeEnd = 73857, XrefRangeStart = 73851, XrefRangeEnd = 73854, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool IsSame(Transform transf)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transf);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsSame_Public_Boolean_Transform_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Packed>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_GetWorldPacked_Public_Static_Packed_Transform_0;

	static TransformUtils()
	{
		Il2CppClassPointerStore<TransformUtils>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "TransformUtils");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TransformUtils>.NativeClassPtr);
		NativeMethodInfoPtr_GetWorldPacked_Public_Static_Packed_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformUtils>.NativeClassPtr, 100664372);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73860, RefRangeEnd = 73862, XrefRangeStart = 73857, XrefRangeEnd = 73860, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Packed GetWorldPacked(this Transform self)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)self);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWorldPacked_Public_Static_Packed_Transform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Packed*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public TransformUtils(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
