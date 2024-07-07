namespace MaimaiGame;

public static class FloatExtensions
{
	public static float MapBetween(this float value, float oldMin, float oldMax, float newMin, float newMax) =>
		(value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
}