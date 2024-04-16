string[] cities = {
	"Napoli",
	"Milano",
	"Bologna",
	"Roma",
	"Firenze"
};

ConsoleHandler consHand = new ConsoleHandler(cities);

string selected = consHand.DisplaySelectionMenu();

foreach (var item in cities)
{
	Console.WriteLine(item);
}

Console.WriteLine($"Option chosen: {selected}");