using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace MaimaiGame.Charts;

public class Position
{
	public static readonly Position Ring1 = new Position('R', 1, 0.6913f, 0.0381f);
	public static readonly Position Ring2 = new Position('R', 2, 0.9619f, 0.3087f);
	public static readonly Position Ring3 = new Position('R', 3, 0.9619f, 0.6913f);
	public static readonly Position Ring4 = new Position('R', 4, 0.6913f, 0.9619f);
	public static readonly Position Ring5 = new Position('R', 5, 0.3087f, 0.9619f);
	public static readonly Position Ring6 = new Position('R', 6, 0.0381f, 0.6913f);
	public static readonly Position Ring7 = new Position('R', 7, 0.0381f, 0.3087f);
	public static readonly Position Ring8 = new Position('R', 8, 0.3087f, 0.0381f);

	public static readonly Position Centre = new Position('C', 0, 0.5000f, 0.5000f);

	public static readonly Position A1 = new Position('A', 1, 0.6513f, 0.1348f);
	public static readonly Position A2 = new Position('A', 2, 0.8652f, 0.3487f);
	public static readonly Position A3 = new Position('A', 3, 0.8652f, 0.6513f);
	public static readonly Position A4 = new Position('A', 4, 0.6513f, 0.8652f);
	public static readonly Position A5 = new Position('A', 5, 0.3487f, 0.8652f);
	public static readonly Position A6 = new Position('A', 6, 0.1348f, 0.6513f);
	public static readonly Position A7 = new Position('A', 7, 0.1348f, 0.3487f);
	public static readonly Position A8 = new Position('A', 8, 0.3487f, 0.1348f);

	public static readonly Position B1 = new Position('B', 1, 0.5777f, 0.3123f);
	public static readonly Position B2 = new Position('B', 2, 0.6877f, 0.4223f);
	public static readonly Position B3 = new Position('B', 3, 0.6877f, 0.5777f);
	public static readonly Position B4 = new Position('B', 4, 0.5777f, 0.6877f);
	public static readonly Position B5 = new Position('B', 5, 0.4223f, 0.6877f);
	public static readonly Position B6 = new Position('B', 6, 0.3123f, 0.5777f);
	public static readonly Position B7 = new Position('B', 7, 0.3123f, 0.4223f);
	public static readonly Position B8 = new Position('B', 8, 0.4223f, 0.3123f);


	public static readonly Position C1 = new Position('C', 1, 0.4516f, 0.5000f);
	public static readonly Position C2 = new Position('C', 2, 0.5484f, 0.5000f);

	public static readonly Position D1 = new Position('D', 1, 0.5000f, 0.0859f);
	public static readonly Position D2 = new Position('D', 2, 0.7928f, 0.2072f);
	public static readonly Position D3 = new Position('D', 3, 0.9141f, 0.5000f);
	public static readonly Position D4 = new Position('D', 4, 0.7928f, 0.7928f);
	public static readonly Position D5 = new Position('D', 5, 0.5000f, 0.9141f);
	public static readonly Position D6 = new Position('D', 6, 0.2072f, 0.7928f);
	public static readonly Position D7 = new Position('D', 7, 0.0859f, 0.5000f);
	public static readonly Position D8 = new Position('D', 8, 0.2072f, 0.2072f);


	public static readonly Position E1 = new Position('E', 1, 0.5000f, 0.2148f);
	public static readonly Position E2 = new Position('E', 2, 0.7016f, 0.2984f);
	public static readonly Position E3 = new Position('E', 3, 0.7852f, 0.5000f);
	public static readonly Position E4 = new Position('E', 4, 0.7016f, 0.7016f);
	public static readonly Position E5 = new Position('E', 5, 0.5000f, 0.7852f);
	public static readonly Position E6 = new Position('E', 6, 0.2984f, 0.7016f);
	public static readonly Position E7 = new Position('E', 7, 0.2148f, 0.5000f);
	public static readonly Position E8 = new Position('E', 8, 0.2984f, 0.2984f);


	public static readonly Position[] Values = typeof(Position)
		.GetFields(BindingFlags.Public | BindingFlags.Static)
		.Where(x => x.FieldType == typeof(Position))
		.Select(x => (Position) x.GetValue(null)!)
		.ToArray();

	public readonly char Category;
	public readonly int Index;
	public readonly float X;
	public readonly float Y;

	public readonly Vector2 Vector;

	private Position(char category, int index, float x, float y)
	{
		Category = category;
		Index = index;
		X = x;
		Y = y;

		Vector = new Vector2(X, Y);
	}

	public static bool TryParseIndex(char value, out int result)
	{
		result = default(int);

		if (!int.TryParse("" + value, out int valueInt))
			return false;

		if (Values.All(x => x.Index != valueInt))
			return false;

		result = valueInt;
		return true;
	}

	public static Position? GetPositionOrDefault(char category, int index)
	{
		return Values.FirstOrDefault(x => x.Category == category && x.Index == index);
	}
}