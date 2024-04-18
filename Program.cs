using StaticDisplaying;

string[] cities = {
	"Napoli",
	"Milano",
	"Bologna",
	"Roma",
	"Firenze"
};
/* 
Person[] people = {
	new Person() { Name = "Paolo", Age = 23},
	new Person() { Name = "Francesco", Age = 24},
	new Person() { Name = "Gabriele", Age = 23},
	new Person() { Name = "Mattia", Age = 26},
	new Person() { Name = "Davide", Age = 28}
};
 */
List<Person> people = [	
	new Person() { Name = "Paolo", Age = 23},
	new Person() { Name = "Francesco", Age = 24},
	new Person() { Name = "Gabriele", Age = 23},
	new Person() { Name = "Mattia", Age = 26},
	new Person() { Name = "Davide", Age = 28}
];

ConsoleSelector<Person> consHand = new ConsoleSelector<Person>() {
	HighlightedBackground = ConsoleColor.Magenta,
	SelectionModeEnabled = true
};

var selected = consHand.DisplaySelectionMenu(people, (o) => o.Name ?? string.Empty);

Console.WriteLine("\n");
foreach (var item in consHand.GetModifiedList())
	Console.WriteLine($"{item}");

Console.WriteLine($"\nSelected: {selected}");