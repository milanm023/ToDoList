namespace ToDoList;

public interface ITask
{
    string Name { get; }
    void RenameTask(string newName);
}