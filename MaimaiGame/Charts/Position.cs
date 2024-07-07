using System.Linq;
using System.Reflection;

namespace MaimaiGame.Charts;

public class Position
{
	public static readonly Position Ring1 = new Position(0.9619f, 0.6913f);
	public static readonly Position Ring2 = new Position(0.6913f, 0.9619f);
	public static readonly Position Ring3 = new Position(0.3087f, 0.9619f);
	public static readonly Position Ring4 = new Position(0.0381f, 0.6913f);
	public static readonly Position Ring5 = new Position(0.0381f, 0.3087f);
	public static readonly Position Ring6 = new Position(0.3087f, 0.0381f);
	public static readonly Position Ring7 = new Position(0.6913f, 0.0381f);
	public static readonly Position Ring8 = new Position(0.9619f, 0.3087f);

	public static readonly Position Centre = new Position(0.5000f, 0.5000f);

	public static readonly Position A1 = new Position(0.8652f, 0.6513f);
	public static readonly Position A2 = new Position(0.6513f, 0.8652f);
	public static readonly Position A3 = new Position(0.3487f, 0.8652f);
	public static readonly Position A4 = new Position(0.1348f, 0.6513f);
	public static readonly Position A5 = new Position(0.1348f, 0.3487f);
	public static readonly Position A6 = new Position(0.3487f, 0.1348f);
	public static readonly Position A7 = new Position(0.6513f, 0.1348f);
	public static readonly Position A8 = new Position(0.8652f, 0.3487f);
	
	public static readonly Position B1 = new Position(0.6877f, 0.5777f);
	public static readonly Position B2 = new Position(0.5777f, 0.6877f);
	public static readonly Position B3 = new Position(0.4223f, 0.6877f);
	public static readonly Position B4 = new Position(0.3123f, 0.5777f);
	public static readonly Position B5 = new Position(0.3123f, 0.4223f);
	public static readonly Position B6 = new Position(0.4223f, 0.3123f);
	public static readonly Position B7 = new Position(0.5777f, 0.3123f);
	public static readonly Position B8 = new Position(0.6877f, 0.4223f);
	
	public static readonly Position C1 = new Position(0.4516f, 0.5000f);
	public static readonly Position C2 = new Position(0.5484f, 0.5000f);
	
	public static readonly Position D1 = new Position(0.9141f, 0.5000f);
	public static readonly Position D2 = new Position(0.7928f, 0.7928f);
	public static readonly Position D3 = new Position(0.5000f, 0.9141f);
	public static readonly Position D4 = new Position(0.2072f, 0.7928f);
	public static readonly Position D5 = new Position(0.0859f, 0.5000f);
	public static readonly Position D6 = new Position(0.2072f, 0.2072f);
	public static readonly Position D7 = new Position(0.5000f, 0.0859f);
	public static readonly Position D8 = new Position(0.7928f, 0.2072f);
	
	public static readonly Position E1 = new Position(0.7852f, 0.5000f);
	public static readonly Position E2 = new Position(0.7016f, 0.7016f);
	public static readonly Position E3 = new Position(0.5000f, 0.7852f);
	public static readonly Position E4 = new Position(0.2984f, 0.7016f);
	public static readonly Position E5 = new Position(0.2148f, 0.5000f);
	public static readonly Position E6 = new Position(0.2984f, 0.2984f);
	public static readonly Position E7 = new Position(0.5000f, 0.2148f);
	public static readonly Position E8 = new Position(0.7016f, 0.2984f);
	
	public static readonly Position[] Values = typeof(Position)
		.GetFields(BindingFlags.Public | BindingFlags.Static)
		.Where(x => x.FieldType == typeof(Position))
		.Select(x => (Position) x.GetValue(null)!)
		.ToArray();

	public readonly float X;
	public readonly float Y;

	private Position(float x, float y)
	{
		X = x;
		Y = y;
	}
}