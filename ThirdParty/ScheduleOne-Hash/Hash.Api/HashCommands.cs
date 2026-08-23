using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hash.Api;

public static class HashCommands
{
	private const string BridgeTypeName = "Hash.Bridge.HashBridge, Hash";

	private static readonly List<string[]> _pending = new List<string[]>();

	private static Action<string, string, string, string> _declare;

	private static bool _bound;

	public static bool Available
	{
		get
		{
			Bind();
			return _bound;
		}
	}

	private static string Owner
	{
		get
		{
			try
			{
				return Assembly.GetExecutingAssembly().GetName().Name ?? "";
			}
			catch
			{
				return "";
			}
		}
	}

	public static void Add(string word, string description, string example = null)
	{
		if (!string.IsNullOrEmpty(word))
		{
			Bind();
			if (_declare != null)
			{
				Safely(word, description, example);
				return;
			}
			_pending.Add(new string[3]
			{
				word,
				description ?? "",
				example ?? ""
			});
		}
	}

	private static void Safely(string word, string description, string example)
	{
		try
		{
			_declare(word, description ?? "", example ?? "", Owner);
		}
		catch
		{
		}
	}

	private static void Bind()
	{
		if (_bound)
		{
			return;
		}
		try
		{
			Type type = Type.GetType("Hash.Bridge.HashBridge, Hash", throwOnError: false);
			if (type == null)
			{
				return;
			}
			_declare = type.GetField("Declare", BindingFlags.Static | BindingFlags.Public)?.GetValue(null) as Action<string, string, string, string>;
			if (_declare == null)
			{
				return;
			}
			_bound = true;
			foreach (string[] item in _pending)
			{
				Safely(item[0], item[1], item[2]);
			}
			_pending.Clear();
		}
		catch
		{
		}
	}
}
