public class ConsoleSelector<T>
{
	public ConsoleColor HighlightedBackground { get; set; }
	public ConsoleColor HighlightedText { get; set; }
	public ConsoleColor SelectedBackground { get; set; }
	public ConsoleColor SelectedText { get; set; }
	public bool SelectionModeEnabled { get; set; }
	private bool _sortingMode;
	private T[] _options;

	public ConsoleSelector() 
	{
		this._sortingMode = false;
		this.SelectionModeEnabled = false;
		this._options = [];

		this.HighlightedBackground = ConsoleColor.White;
		this.HighlightedText = ConsoleColor.Black;
		this.SelectedBackground = ConsoleColor.Cyan;
		this.SelectedText = ConsoleColor.Black;
	}

	/**
	 * Main method of the application. It performs the options print in the console.
	 *
	 * PARAMS: 
		- options: the list of elements to print.
		- toString: a function that translates the elements into string to print them in the menu.
	 *
	 * RETURN: the selected value.
	 *
	 * the user can also change the order of the options displayed. The new order will be stored in 
	 * the _options variable of this class and can be retrieved with the apposite method.
	 */
	public T DisplaySelectionMenu(IEnumerable<T> options, Func<T, string> toString)
	{
		this._options = options.ToArray();

		int previousLineIndex = -1, selectedLineIndex = 0;
		ConsoleKey pressedKey = ConsoleKey.None;

		Console.WriteLine(this._selectionText());
		this._printOptionsList(selectedLineIndex, toString);

		while (pressedKey != ConsoleKey.Enter)
		{
			if (previousLineIndex != selectedLineIndex)
			{
				this._updateMenu(selectedLineIndex, toString);
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
			else if (this.SelectionModeEnabled && pressedKey == ConsoleKey.Spacebar)
			{
				this._sortingMode = !this._sortingMode;
				this._updateMenu(selectedLineIndex, toString);
			}
		}

		return this._options[selectedLineIndex];
	}

	/**
	 * Display method override.
	 * This one uses a default ToString display method.
	 */
	public T DisplaySelectionMenu(IEnumerable<T> options)
	{
		return this.DisplaySelectionMenu(options, (o) => o == null ? "null" : (o.ToString() ?? string.Empty));	
	}
	
	/**
	 * Set the console cursor position.
	 * Prints the options in the console. 
	 */
	private void _updateMenu(int index, Func<T, string> toString)
	{				
		this._setConsoleCursor();
		this._printOptionsList(index, toString);
	}

	/** The list gets reprinted every time it is modified (the user scrolls up or down or a new element is selected).
	 *	So the console cursor is taken back to the start of the list to perform the printing once again.
	 **/
	private void _setConsoleCursor()
	{
		var position = Console.GetCursorPosition();
		position.Top -= this._options.Length;				
		Console.SetCursorPosition(0, position.Top);
	}

	/**
	 * Prints all the options in the console.
	 */
	private void _printOptionsList(int index, Func<T, string> toString)
	{
		foreach (var item in this._options)
		{
			bool isSelected = item != null && item.Equals(this._options[index]);

			if (isSelected)
				this._drawSelectedLine(item, toString);
			else
				Console.WriteLine($"\x1b[2K\r  {toString(item)}"); // the escape sequence clears the whole line and then set the cursor at the beginning
		}
	}

	// changes briefly the console colors to print the selected line
	// then changes back to normal colors
	private void _drawSelectedLine(T item, Func<T, string> toString)
	{
		Console.ForegroundColor = this._sortingMode ? this.SelectedText : this.HighlightedText;
		Console.BackgroundColor = this._sortingMode ? this.SelectedBackground : this.HighlightedBackground;
		
		// the escape sequence clears the whole line and then set the cursor at the beginning
		Console.WriteLine($"\x1b[2K\r> {toString(item)}"); 
		Console.ResetColor();
	}

	// return the options list state
	// it takes track of the changes made in sorting mode
	public List<T> GetModifiedList()
	{
		return this._options.ToList();
	}

	// performs the row swap in the sortingMode 
	private void _swap(int index1, int index2)
	{
		T tmp = this._options[index1];
		this._options[index1] = this._options[index2];
		this._options[index2] = tmp;
	}

	private string _selectionText() => 
		"\rUse arrows to select an object in the list\n" +
		$"{(this.SelectionModeEnabled ? "Press Space to select an object and the arrows to change its place\n" : string.Empty)}" +
		"Press Enter to finish\n";
}
