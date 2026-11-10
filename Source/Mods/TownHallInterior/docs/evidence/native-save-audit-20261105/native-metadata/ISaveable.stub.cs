using System.Collections.Generic;
using Cpp2ILInjected;
using ScheduleOne.Persistence.Loaders;

namespace ScheduleOne.Persistence;

[Token(Token = "0x2000366")]
public interface ISaveable
{
	[Token(Token = "0x17000460")]
	string SaveFolderName
	{
		[Token(Token = "0x6001778")]
		get;
	}

	[Token(Token = "0x17000461")]
	string SaveFileName
	{
		[Token(Token = "0x6001779")]
		get;
	}

	[Token(Token = "0x17000462")]
	Loader Loader
	{
		[Token(Token = "0x600177A")]
		get;
	}

	[Token(Token = "0x17000463")]
	bool ShouldSaveUnderFolder
	{
		[Token(Token = "0x600177B")]
		get;
	}

	[Token(Token = "0x17000464")]
	List<string> LocalExtraFiles
	{
		[Token(Token = "0x600177C")]
		get;
		[Token(Token = "0x600177D")]
		set;
	}

	[Token(Token = "0x17000465")]
	List<string> LocalExtraFolders
	{
		[Token(Token = "0x600177E")]
		get;
		[Token(Token = "0x600177F")]
		set;
	}

	[Token(Token = "0x17000466")]
	bool HasChanged
	{
		[Token(Token = "0x6001780")]
		get;
		[Token(Token = "0x6001781")]
		set;
	}

	[Token(Token = "0x6001782")]
	void InitializeSaveable();

	[Token(Token = "0x6001783")]
	string GetSaveString();

	[Token(Token = "0x6001784")]
	[Address(RVA = "0x583A50", Offset = "0x582C50", Length = "0x151")]
	string Save(string parentFolderPath)
	{
		return null;
	}

	[Token(Token = "0x6001785")]
	[Address(RVA = "0x583E70", Offset = "0x583070", Length = "0x38F")]
	void WriteBaseData(string parentFolderPath, string saveString)
	{
	}

	[Token(Token = "0x6001786")]
	[Address(RVA = "0x583990", Offset = "0x582B90", Length = "0xB7")]
	string GetLocalPath(out bool isFolder)
	{
		isFolder = default(bool);
		return null;
	}

	[Token(Token = "0x6001787")]
	[Address(RVA = "0x582F00", Offset = "0x582100", Length = "0x933")]
	void CompleteSave(string parentFolderPath, bool writeDataFile)
	{
	}

	[Token(Token = "0x6001788")]
	[Address(RVA = "0x584200", Offset = "0x583400", Length = "0x55")]
	List<string> WriteData(string parentFolderPath)
	{
		return null;
	}

	[Token(Token = "0x6001789")]
	[Address(RVA = "0x43FEC0", Offset = "0x43F0C0", Length = "0x3")]
	void DeleteUnapprovedFiles(string parentFolderPath)
	{
	}

	[Token(Token = "0x600178A")]
	[Address(RVA = "0x583840", Offset = "0x582A40", Length = "0x144")]
	string GetContainerFolder(string parentFolderPath)
	{
		return null;
	}

	[Token(Token = "0x600178B")]
	[Address(RVA = "0x5846A0", Offset = "0x5838A0", Length = "0x449")]
	string WriteSubfile(string parentPath, string localPath_NoExtensions, string contents)
	{
		return null;
	}

	[Token(Token = "0x600178C")]
	[Address(RVA = "0x584260", Offset = "0x583460", Length = "0x43B")]
	string WriteFolder(string parentPath, string localPath_NoExtensions)
	{
		return null;
	}

	[Token(Token = "0x600178D")]
	[Address(RVA = "0x583D80", Offset = "0x582F80", Length = "0xE9")]
	bool TryLoadFile(string parentPath, string fileName, out string contents)
	{
		contents = null;
		return false;
	}

	[Token(Token = "0x600178E")]
	[Address(RVA = "0x583BB0", Offset = "0x582DB0", Length = "0x1CC")]
	bool TryLoadFile(string path, out string contents, bool autoAddExtension = true)
	{
		contents = null;
		return false;
	}
}
