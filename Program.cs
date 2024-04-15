string[] cities = {
	"Napoli",
	"Milano",
	"Bologna",
	"Roma",
	"Firenze"
};

int previousLineIndex = -1, selectedLineIndex = 0;
ConsoleKey pressedKey = ConsoleKey.None;
ConsoleHandler consHand = new ConsoleHandler();

do 
{
	if (previousLineIndex != selectedLineIndex)
	{
		consHand.UpdateMenu(selectedLineIndex, cities);
		previousLineIndex = selectedLineIndex;
	}

	pressedKey = Console.ReadKey().Key;

	if (pressedKey == ConsoleKey.DownArrow && selectedLineIndex+1 < cities.Length)
		selectedLineIndex++;
	else if (pressedKey == ConsoleKey.UpArrow && selectedLineIndex-1 >= 0)
		selectedLineIndex--;	
}
while (pressedKey != ConsoleKey.Enter);

Console.WriteLine($"Option chosen: {cities[selectedLineIndex]}");