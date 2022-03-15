namespace TestingTechniques;
public partial class ValueSamples
{
    public string FullName = "Peter Parker";
    public int Age = 21;
    public DateOnly DateOfBirth = new(2000, 6, 9);

    public Point Point = new(10.0, 10.0);

    public User AppUser = new()
    {
        FullName = "Peter Parker",
        Age = 21,
        DateOfBirth = new(2000, 6, 9)
    };

    public IEnumerable<int> Numbers = new[] { 1, 2, 3 };

    public IEnumerable<Point> Points = new[]
    {
        new Point(10.0, 10.0),
        new Point(20.0,20.0)
    };


    public IEnumerable<User> Users = new[]
    {
        new User()
        {
            FullName = "Peter Parker",
            Age = 21,
            DateOfBirth = new(2000, 6, 9)
        },
        new User()
        {
            FullName = "Robert Smith",
            Age = 37,
            DateOfBirth = new(1984, 6, 9)
        },
        new User()
        {
            FullName = "Babara Dopstein",
            Age = 43,
            DateOfBirth = new(1978, 10, 5)
        },
    };


    // Events
    public event EventHandler ExampleEvent;

    public virtual void RaiseExampleEvent()
    {
        ExampleEvent(this, EventArgs.Empty);
    }

    internal int InternalSecretNumber = 40;

}
