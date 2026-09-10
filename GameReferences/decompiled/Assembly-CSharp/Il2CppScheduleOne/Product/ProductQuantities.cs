using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Product;

public static class ProductQuantities : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_BagQuantity;

	private static readonly System.IntPtr NativeFieldInfoPtr_JarQuantity;

	private static readonly System.IntPtr NativeFieldInfoPtr_BrickQuantity;

	public unsafe static int BagQuantity
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BagQuantity, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BagQuantity, (void*)(&num));
		}
	}

	public unsafe static int JarQuantity
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JarQuantity, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JarQuantity, (void*)(&num));
		}
	}

	public unsafe static int BrickQuantity
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BrickQuantity, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BrickQuantity, (void*)(&num));
		}
	}

	static ProductQuantities()
	{
		Il2CppClassPointerStore<ProductQuantities>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "ProductQuantities");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ProductQuantities>.NativeClassPtr);
		NativeFieldInfoPtr_BagQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductQuantities>.NativeClassPtr, "BagQuantity");
		NativeFieldInfoPtr_JarQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductQuantities>.NativeClassPtr, "JarQuantity");
		NativeFieldInfoPtr_BrickQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProductQuantities>.NativeClassPtr, "BrickQuantity");
	}

	public ProductQuantities(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
