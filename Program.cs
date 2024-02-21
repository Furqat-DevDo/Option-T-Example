Person? man = Person.Create("John", "Doe");
Person? aristotle = Person.Create("Arsistotle");

Book faustus = Book.Create("The Book", man);
Book rhetoric = Book.Create("Rethoric", aristotle);
Book nights = Book.Create("One thousand nights");

Console.WriteLine();
Console.WriteLine(GteBookLabel(faustus));
Console.WriteLine();
Console.WriteLine(GteBookLabel(rhetoric));
Console.WriteLine();
Console.WriteLine(GteBookLabel(nights));


string Getlabel (Person person) => person
    .LastName
    .Map(lastName => $"{person.FirstName} {lastName}")
    .Reduce($"{person.FirstName}");

string GteBookLabel(Book book) => book
    .Author
    .Map(Getlabel)
    .Map(author => $"{book.Title} by {author}")
    .Reduce(book.Title);

class Person
{
    public string FirstName { get; private set; }
    public Option<string> LastName { get; private set; }
    private Person(string firstName,Option<string> lastName) => (FirstName,LastName) = (firstName,lastName);
    public static Person Create(string firstName, string lastName) => new (firstName,Option<string>.Some(lastName));
    public static Person Create(string firstName) => new (firstName,Option<string>.None());
}

class Book
{
    public string Title { get; set; }
    public Option<Person> Author { get; set; }
    private Book(string title,Option<Person> author) => (Title,Author) = (title,author);
    public static Book Create(string title, Person author) => new (title,Option<Person>.Some(author));
    public static Book Create(string title) => new (title,Option<Person>.None());
}

class Option<T> where T : class
{
    private T? _object;
    public static Option<T> Some(T obj) => new (){_object = obj};
    public static Option<T> None() => new ();
    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class => 
        _object is null ? Option<TResult>.None() : Option<TResult>.Some(map(_object));
    public T Reduce (T @defaukt) => _object ?? @defaukt;
}