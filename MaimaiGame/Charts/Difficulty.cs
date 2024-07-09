using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaimaiGame.Charts.Objects;

namespace MaimaiGame.Charts;

public class Difficulty
{
	private static readonly char[] NoteSeparators = new[] { ',', '/', '`' };

	private readonly List<BaseObject> _objects = new List<BaseObject>();
	public IReadOnlyList<BaseObject> Objects => _objects;

	public string? ShortMessage { get; private set; }
	public string? Designer { get; private set; }
	public float? Offset { get; private set; }
	public string? Level { get; private set; }


	public readonly string Category;

	public Difficulty(DifficultyCategory category)
	{
		Category = category.ToString();
	}

	public bool Parse(string name, object value)
	{
		switch (name)
		{
			case "smsg":
				ShortMessage = (string) value;
				return true;
			case "des":
				Designer = (string) value;
				return true;
			case "first":
				if (value is float offset)
				{
					Offset = offset;
					return true;
				}

				string offsetStr = (string) value;

				if (!float.TryParse(offsetStr, out offset))
				{
					Logger.Error($"Invalid \"first\" value \"{offsetStr}\"");
					return false;
				}

				Offset = offset;
				return true;
			case "lv":
				Level = (string) value;
				return true;
		}

		Logger.Error($"Unknown token name \"{name}\"");
		return false;
	}

	public bool Load(string contents)
	{
		contents = contents
			.Replace("\r", string.Empty)
			.Replace("\n", string.Empty)
			.Replace(" ", string.Empty);

		CustomCharEnumerator enumerator = new CustomCharEnumerator(contents);

		int currentBpm, bpm;

		if (!TryParseBpm(enumerator, out bpm, false))
			throw new Exception("Bpm must be set at the beginning");

		currentBpm = bpm;

		int currentDivisor = 4, divisor;
		float currentTime = Offset ?? 0.0f;

		List<BaseObject> objects = new List<BaseObject>();

		while (enumerator.Peek() != null)
		{
			enumerator.Take();

			if (TryParseBpm(enumerator, out bpm))
				currentBpm = bpm;
			else if (TryParseDivisor(enumerator, out divisor))
				currentDivisor = divisor;

			if (!TryParseTap(enumerator, currentTime, objects))
				Logger.Error($"Unknown token \"{enumerator.Current}\"");

			if (enumerator.Current == ',')
			{
				if (objects.Count > 1)
				{
					foreach (BaseObject baseObject in objects)
						baseObject.Modifiers |= Modifiers.Each;
				}

				_objects.AddRange(objects);
				objects.Clear();

				currentTime += 60.0f / currentBpm * 4.0f / currentDivisor;
			}
		}

		return true;
	}

	private bool TryParseBpm(CustomCharEnumerator enumerator, out int result, bool takeAfter = true)
	{
		result = 0;

		if (enumerator.Current != '(')
			return false;

		string bpmStr = enumerator.TakeUntil(')');

		if (!int.TryParse(bpmStr, out int bpm))
			throw new Exception($"Invalid bpm value \"{bpmStr}\"");

		result = bpm;

		if (takeAfter)
			enumerator.Take();

		return true;
	}

	private bool TryParseDivisor(CustomCharEnumerator enumerator, out int result)
	{
		result = 0;

		if (enumerator.Current != '{')
			return false;

		string bpmStr = enumerator.TakeUntil('}');

		if (!int.TryParse(bpmStr, out int bpm))
			throw new Exception($"Invalid bpm value \"{bpmStr}\"");

		result = bpm;

		enumerator.Take();
		return true;
	}

	private bool TryParseTap(CustomCharEnumerator enumerator, float time, List<BaseObject> objects)
	{
		if (!Position.TryParseIndex(enumerator.Current, out int index))
			return false;

		Position? position = Position.GetPositionOrDefault('R', index);

		if (position == null)
			return false;

		enumerator.Take();

		Tap tap = new Tap(time, position);
		objects.Add(tap);

		if (Position.TryParseIndex(enumerator.Current, out int secondIndex))
		{
			Position? secondPosition = Position.GetPositionOrDefault('R', secondIndex);

			if (secondPosition == null)
				throw new Exception();

			tap.Modifiers = Modifiers.Each;

			Tap secondTap = new Tap(time, secondPosition, Modifiers.Each);
			objects.Add(secondTap);

			enumerator.Take();

			return true;
		}

		while (!NoteSeparators.Contains(enumerator.Current))
		{
			if (enumerator.Current == 'b')
				tap.Modifiers |= Modifiers.Break;
			else if (enumerator.Current == 'x')
				tap.Modifiers |= Modifiers.Ex;

			enumerator.Take();
		}

		return true;
	}

	private class CustomCharEnumerator
	{
		private readonly char[] _data;
		private int _index;

		public char Current => _data[_index];

		public CustomCharEnumerator(string value)
		{
			_data = value.ToCharArray();
			_index = 0;
		}

		public char Take()
		{
			if (_index >= _data.Length - 1)
				throw new IndexOutOfRangeException("Index is larger than the enumerator length");

			_index++;
			return _data[_index];
		}

		public string TakeUntil(char[] matches, bool includeCurrent = false, bool includeMatch = false)
		{
			StringBuilder builder = new StringBuilder();

			if (includeCurrent)
				builder.Append(Current);

			char current = Take();

			while (!matches.Contains(current))
			{
				builder.Append(current);
				current = Take();
			}

			if (includeMatch)
				builder.Append(current);

			return builder.ToString();
		}

		public string TakeUntil(char match, bool includeCurrent = false, bool includeMatch = false)
		{
			return TakeUntil(new char[] { match }, includeCurrent, includeMatch);
		}

		public char? Peek(int offset = 1)
		{
			if (offset < 1)
				throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be larger than 1");

			int index = _index + offset;

			if (index > _data.Length - 1)
				return null;

			return _data[index];
		}

		public string? PeekUntil(char[] matches, bool includeCurrent = false, bool includeMatch = false)
		{
			StringBuilder builder = new StringBuilder();

			if (includeCurrent)
				builder.Append(Current);

			int offset = 1;
			char? current = Peek(offset);

			while (current.HasValue && !matches.Contains(current.Value))
			{
				builder.Append(current.Value);

				offset++;
				current = Peek(offset);
			}

			if (!current.HasValue)
				return null;

			if (includeMatch)
				builder.Append(current.Value);

			return builder.ToString();
		}

		public string? PeekUntil(char match, bool includeCurrent = false, bool includeMatch = false)
		{
			return PeekUntil(new char[] { match }, includeCurrent, includeMatch);
		}
	}
}