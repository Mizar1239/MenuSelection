namespace NotStatic
{
	public class ColorSelection
	{
		public ConsoleColor Background { get; set; }
		public ConsoleColor Foreground { get; set; }
		public ConsoleColor HighlightBackground { get; set; }
		public ConsoleColor SelectionHighLightBackground { get; set; }
		public ConsoleColor HighLightForeground { get; set; }
	}

	public class ConsoleHandler<T>
	{
		private ConsoleColor _BackgroundColor, 
							_ForegroundColor,
							_HighlightForegroundColor,
							_HighlightBackgroundColor,
							_SelectionHighlightBackgroundColor;
		private readonly T[] _options;
		private readonly Func<T, string> _toString;
		private bool _sortingMode;

		public ConsoleHandler(T[] options, Func<T, string> toString)
		{
			this._options = options;
			this._toString = toString;

			this._sortingMode = false;

			this._BackgroundColor = Console.BackgroundColor;
			this._ForegroundColor = Console.ForegroundColor;
			this._HighlightBackgroundColor = Console.ForegroundColor;
			this._HighlightForegroundColor = Console.BackgroundColor;
			this._HighlightForegroundColor = Console.BackgroundColor;
			this._SelectionHighlightBackgroundColor = ConsoleColor.Cyan;
		}
		
		public ConsoleHandler(List<T> options, Func<T, string> toString) 
			: this(options.ToArray(), toString)
		{ }

		public T DisplaySelectionMenu()
		{
			int previousLineIndex = -1, selectedLineIndex = 0;
			ConsoleKey pressedKey = ConsoleKey.None;

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
					if (this._sortingMode)
						this._swap(selectedLineIndex+1, selectedLineIndex);

					selectedLineIndex++;
				}
				else if (pressedKey == ConsoleKey.UpArrow && selectedLineIndex-1 >= 0)
				{
					if (this._sortingMode)
						this._swap(selectedLineIndex-1, selectedLineIndex);
					
					selectedLineIndex--;
				}
				else if (pressedKey == ConsoleKey.Spacebar)
				{
					this._sortingMode = !this._sortingMode;
					this._updateMenu(selectedLineIndex);
				}
			}
			while (pressedKey != ConsoleKey.Enter);

			return this._options[selectedLineIndex];
		}

		public void SetColors(ColorSelection selection)
		{
			this._BackgroundColor = selection.Background;
			this._ForegroundColor = selection.Foreground;
			this._HighlightBackgroundColor = selection.HighlightBackground;
			this._HighlightForegroundColor = selection.HighLightForeground;
			this._SelectionHighlightBackgroundColor = selection.SelectionHighLightBackground;
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
			Console.BackgroundColor = this._sortingMode ? this._SelectionHighlightBackgroundColor : this._HighlightBackgroundColor;
			Console.ForegroundColor = this._HighlightForegroundColor;
			Console.WriteLine($"> {this._toString(item)}");
			Console.BackgroundColor = this._BackgroundColor;
			Console.ForegroundColor = this._ForegroundColor;		
		}

		private void _swap(int index1, int index2)
		{
			T tmp = this._options[index1];
			this._options[index1] = this._options[index2];
			this._options[index2] = tmp;
		}

		private const string SELECTION_TEXT = "Use arrows to select an object in the list\nPress Space to select an object and the arrows to change its place\nPress Enter to finish\n";
	}
}