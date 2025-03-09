namespace ToDoList;

public class Task : ITask
{
    private string name;

    public Task(string name)
    {
        this.Name = name;
    }
    public string Name
    {
        get => name;
        private set
        {
            while (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine ("Input task name cannot be null or whitespace.");
            }
            name = value;
        }
    }

    public void RenameTask(string newName)
    {
        this.Name = newName;
    }

    public override string ToString()
    {
        return Name;
    }
}