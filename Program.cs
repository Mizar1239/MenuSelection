string[] cities = {
	"Napoli",
	"Milano",
	"Bologna",
	"Roma",
	"Firenze"
};

Person[] people = {
	new Person() { Name = "Paolo", Age = 23},
	new Person() { Name = "Francesco", Age = 24},
	new Person() { Name = "Gabriele", Age = 23},
	new Person() { Name = "Mattia", Age = 26},
	new Person() { Name = "Davide", Age = 28}
};

ConsoleHandler<Person> consHand = new ConsoleHandler<Person>(people, (p) => p.Name);

Person selected = consHand.DisplaySelectionMenu();

foreach (var item in people)
{
	Console.WriteLine(item);
}

Console.WriteLine($"\nOption chosen: {selected}");