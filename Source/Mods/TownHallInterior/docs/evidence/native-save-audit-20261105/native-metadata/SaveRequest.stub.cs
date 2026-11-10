using Cpp2ILInjected;

namespace ScheduleOne.Persistence;

[Token(Token = "0x2000386")]
public class SaveRequest
{
	[Token(Token = "0x40012DE")]
	[FieldOffset(Offset = "0x10")]
	public ISaveable Saveable;

	[Token(Token = "0x40012DF")]
	[FieldOffset(Offset = "0x18")]
	public string ParentFolderPath;

	[Token(Token = "0x17000485")]
	[field: Token(Token = "0x40012E0")]
	[field: FieldOffset(Offset = "0x20")]
	public string SaveString
	{
		[Token(Token = "0x6001848")]
		[Address(RVA = "0x421DC0", Offset = "0x420FC0", Length = "0x5")]
		get;
		[Token(Token = "0x6001849")]
		[Address(RVA = "0x421E00", Offset = "0x421000", Length = "0xD")]
		private set;
	}

	[Token(Token = "0x600184A")]
	[Address(RVA = "0x5ABBC0", Offset = "0x5AADC0", Length = "0x32B")]
	public SaveRequest(ISaveable saveable, string parentFolderPath)
	{
	}

	[Token(Token = "0x600184B")]
	[Address(RVA = "0x5ABA60", Offset = "0x5AAC60", Length = "0x15C")]
	public void Complete()
	{
	}
}
