namespace StaticDisplaying
{
	public static class StaticDisplayer
	{
		public static T DisplayOptions<T>(IEnumerable<T> options)
		{
			Func<T, string> _toString = (o) => o == null ? "null" : (o.ToString() ?? string.Empty);
			ConsoleHandler<T> cons = new(options, _toString);
			return cons.DisplaySelectionMenu();
		}

		public static T DisplayOptions<T>(IEnumerable<T> options, bool enableSorting)
		{
			Func<T, string> _toString = (o) => o == null ? "null" : (o.ToString() ?? string.Empty);
			ConsoleHandler<T> cons = new ConsoleHandler<T>(options, _toString);
			cons.SelectionModeEnabled = enableSorting;
			return cons.DisplaySelectionMenu();
		}

		public static T DisplayOptions<T>(IEnumerable<T> options, bool enableSorting, Func<T, string> _toString)
		{
			ConsoleHandler<T> cons = new ConsoleHandler<T>(options, _toString);
			cons.SelectionModeEnabled = enableSorting;
			return cons.DisplaySelectionMenu();
		}

		private class ConsoleHandler<T>
		{
			public ConsoleColor HighlightedBackground { get; set; }
			public ConsoleColor HighlightedText { get; set; }
			public ConsoleColor SelectedBackground { get; set; }
			public ConsoleColor SelectedText { get; set; }
			public bool SelectionModeEnabled { get { return _selectionEnabled; } set { _selectionEnabled = value; } }
			private bool _selectionEnabled;
			private readonly T[] _options;
			private readonly Func<T, string> _toString;
			private bool _sortingMode;

			internal ConsoleHandler(IEnumerable<T> options, Func<T, string> stringingOptions)
			{
				this._options = options.ToArray();
				this._toString = stringingOptions;

				this._sortingMode = false;
				this._selectionEnabled = false;

				this.HighlightedBackground = ConsoleColor.White;
				this.HighlightedText = ConsoleColor.Black;
				this.SelectedBackground = ConsoleColor.Cyan;
				this.SelectedText = ConsoleColor.Black;
			}
			
			internal T DisplaySelectionMenu()
			{
				int previousLineIndex = -1, selectedLineIndex = 0;
				ConsoleKey pressedKey = ConsoleKey.None;

				Console.WriteLine(this._SelectionText());
				this._printOptionsList(selectedLineIndex);

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
					else if (this._selectionEnabled && pressedKey == ConsoleKey.Spacebar)
					{
						this._sortingMode = !this._sortingMode;
						this._updateMenu(selectedLineIndex);
					}
				}
				while (pressedKey != ConsoleKey.Enter);

				return this._options[selectedLineIndex];
			}

			private void _updateMenu(int index)
			{				
				this._setConsoleCursor();
				this._printOptionsList(index);
			}

			private void _setConsoleCursor()
			{
				var position = Console.GetCursorPosition();
				position.Top -= this._options.Length;				
				Console.SetCursorPosition(position.Left, position.Top);
			}

			private void _printOptionsList(int index)
			{
				foreach (var item in this._options)
				{
					bool isSelected = item != null && item.Equals(this._options[index]);

					if (isSelected)
						this._drawSelectedLine(item);
					else
						Console.WriteLine($"\x1b[2K\r  {this._toString(item)}");
				}
			}

			private void _drawSelectedLine(T item)
			{
				Console.ForegroundColor = this._sortingMode ? this.SelectedText : this.HighlightedText;
				Console.BackgroundColor = this._sortingMode ? this.SelectedBackground : this.HighlightedBackground;
				Console.WriteLine($"\x1b[2K\r> {this._toString(item)}");
				Console.ResetColor();
			}

			private void _swap(int index1, int index2)
			{
				T tmp = this._options[index1];
				this._options[index1] = this._options[index2];
				this._options[index2] = tmp;
			}

			private string _SelectionText() => 
				"\rUse arrows to select an object in the list\n" +
				$"{(this._selectionEnabled ? "Press Space to select an object and the arrows to change its place\n" : string.Empty)}" +
				"Press Enter to finish\n";
		}
	}
}