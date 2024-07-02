using System;
using System.Diagnostics;
using System.Text;

namespace MaimaiGame;

public static class Logger
{
	private static LogLevel _level;

	private static bool _initialised;

	public static void Initialise()
	{
		if (_initialised)
			return;

		_initialised = true;

		_level = Configuration.Instance.LogLevel;
	}

	public static void Assert(bool condition, params object[] args)
	{
		if (condition)
			return;

		Critical(args);
	}

	public static void Critical(params object[] args) => Log(LogLevel.Critical, args);
	public static void Error(params object[] args) => Log(LogLevel.Error, args);
	public static void Information(params object[] args) => Log(LogLevel.Information, args);
	public static void Debug(params object[] args) => Log(LogLevel.Debug, args);

	private static void Log(LogLevel level, params object[] args)
	{
		if (level < _level)
			return;

		string message = BuildMessage(level, args);

		if (level == LogLevel.Critical)
			Trace.Fail(message);
		if (level == LogLevel.Error)
			Console.Error.WriteLine(message);
		else
			Console.WriteLine(message);
	}

	private static string BuildMessage(LogLevel level, params object[] args)
	{
		StringBuilder builder = new StringBuilder($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] ");

		foreach (object arg in args)
			builder.Append(arg);

		builder.AppendLine();

		return builder.ToString();
	}
}

public enum LogLevel
{
	Debug,
	Information,
	Error,
	Critical
}