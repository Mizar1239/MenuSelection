using Microsoft.Win32.SafeHandles;

public interface IDisplayable
{
	public string displayString();
}

public class ConsoleHandler<T>
{
	private readonly ConsoleColor _defaultBackgroundColor, _defaultForegroundColor;
	private readonly T[] _options;
	private readonly Func<T, string> _toString;

	public ConsoleHandler(T[] options, Func<T, string> stringingOptions)
	{
		this._options = options;
		this._toString = stringingOptions;

		this._defaultBackgroundColor = Console.BackgroundColor;
		this._defaultForegroundColor = Console.ForegroundColor;
	}
	
	public ConsoleHandler(List<T> options, Func<T, string> stringingOptions) 
		: this(options.ToArray(), stringingOptions)
	{ }

	public T DisplaySelectionMenu()
	{
		int previousLineIndex = -1, selectedLineIndex = 0;
		ConsoleKey pressedKey = ConsoleKey.None;

		bool sortingMode = false;

		Console.WriteLine(SELECTION_TEXT);

		do 
		{
			if (previousLineIndex != selectedLineIndex)
			{
				this._updateMenu(selectedLineIndex);
				previousLineIndex = selectedLineIndex;
			}

			pressedKey = Console.ReadKey().Key;

			if (pressedKey == ConsoleKey.DownArrow && selectedLineIndex+1 < this._options.Length)
			{
				if (sortingMode)
					this._swap(selectedLineIndex+1, selectedLineIndex);

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

		return this._options[selectedLineIndex];
	}

	private void _updateMenu(int index)
	{
		Console.Clear();
		Console.WriteLine("\x1b[3J");

		Console.WriteLine(SELECTION_TEXT);

		foreach (var item in this._options)
		{
			bool isSelected = item != null && item.Equals(this._options[index]);

			if (isSelected)
				this._drawSelectedLine(item);
			else
				Console.WriteLine($"  {this._toString(item)}");
		}
	}

	private void _drawSelectedLine(T item)
	{
		Console.BackgroundColor = ConsoleColor.White;
		Console.ForegroundColor = ConsoleColor.Black;
		Console.WriteLine($"> {this._toString(item)}");
		Console.BackgroundColor = this._defaultBackgroundColor;
		Console.ForegroundColor = this._defaultForegroundColor;		
	}

	private void _swap(int index1, int index2)
	{
		T tmp = this._options[index1];
		this._options[index1] = this._options[index2];
		this._options[index2] = tmp;
	}

	private const string SELECTION_TEXT = "Use arrows to select an object in the list\nPress Space to select an object and the arrows to change its place\nPress Enter to finish\n";
}