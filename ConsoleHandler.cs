public class ConsoleHandler
{
	private readonly ConsoleColor _defaultBackgroundColor, _defaultForegroundColor;
	string[] options;

	public ConsoleHandler(List<string> options)
	{
		this.options = options.ToArray();

		this._defaultBackgroundColor = Console.BackgroundColor;
		this._defaultForegroundColor = Console.ForegroundColor;
	}

	public ConsoleHandler(string[] options)
	{
		this.options = options;

		this._defaultBackgroundColor = Console.BackgroundColor;
		this._defaultForegroundColor = Console.ForegroundColor;
	}

	public ConsoleHandler(List<string> options, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
	{
		this.options = options.ToArray();

		this._defaultBackgroundColor = backgroundColor;
		this._defaultForegroundColor = foregroundColor;
	}

	public string DisplaySelectionMenu()
	{
		int previousLineIndex = -1, selectedLineIndex = 0;
		ConsoleKey pressedKey = ConsoleKey.None;

		bool sortingMode = false;

		do 
		{
			if (previousLineIndex != selectedLineIndex)
			{
				this._updateMenu(selectedLineIndex, this.options);
				previousLineIndex = selectedLineIndex;
			}

			pressedKey = Console.ReadKey().Key;

			if (pressedKey == ConsoleKey.DownArrow && selectedLineIndex+1 < this.options.Length)
			{
				if (sortingMode)
				{
					// var tmp = this.options[selectedLineIndex+1];
					// this.options[selectedLineIndex+1] = this.options[selectedLineIndex];
					// this.options[selectedLineIndex] = tmp;
					this._swap(selectedLineIndex+1, selectedLineIndex);
				}

				selectedLineIndex++;
			}
			else if (pressedKey == ConsoleKey.UpArrow && selectedLineIndex-1 >= 0)
			{
				if (sortingMode)
					this._swap(selectedLineIndex-1, selectedLineIndex);
				
				selectedLineIndex--;
			}
			else if (pressedKey == ConsoleKey.Spacebar)
				sortingMode = !sortingMode;
		}
		while (pressedKey != ConsoleKey.Enter);

		Console.WriteLine($"Option chosen: {this.options[selectedLineIndex]}");

		return this.options[selectedLineIndex];
	}

	private void _updateMenu(int index, string[] cities)
	{
		Console.Clear();

		foreach (var city in cities)
		{
			bool isSelected = city == cities[index];

			if (isSelected)
				this._drawSelectedLine(city);
			else
				Console.WriteLine($"  {city}");
		}
	}

	private void _drawSelectedLine(string city)
	{
		Console.BackgroundColor = ConsoleColor.White;
		Console.ForegroundColor = ConsoleColor.Black;
		Console.WriteLine($"> {city}");
		Console.BackgroundColor = this._defaultBackgroundColor;
		Console.ForegroundColor = this._defaultForegroundColor;		
	}

	private void _swap(int index1, int index2)
	{
		string tmp = this.options[index1];
		this.options[index1] = this.options[index2];
		this.options[index2] = tmp;
	}
}