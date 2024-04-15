public class ConsoleHandler
{
	private ConsoleColor _defaultBackgroundColor, _defaultForegroundColor;
	public ConsoleHandler()
	{
		this._defaultBackgroundColor = Console.BackgroundColor;
		this._defaultForegroundColor = Console.ForegroundColor;
	}

	public ConsoleHandler(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
	{
		this._defaultBackgroundColor = backgroundColor;
		this._defaultForegroundColor = foregroundColor;
	}

	public void UpdateMenu(int index, string[] cities)
	{
		Console.Clear();

		foreach (var city in cities)
		{
			bool isSelected = city == cities[index];

			if (isSelected)
				DrawSelectedLine(city);
			else
				Console.WriteLine($"  {city}");
		}
	}

	private void DrawSelectedLine(string city)
	{
		Console.BackgroundColor = ConsoleColor.White;
		Console.ForegroundColor = ConsoleColor.Black;
		Console.WriteLine($"> {city}");
		Console.BackgroundColor = this._defaultBackgroundColor;
		Console.ForegroundColor = this._defaultForegroundColor;		
	}
}