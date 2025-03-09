namespace ToDoList;

public interface IRepository<ITask>
{
    IReadOnlyCollection<ITask> Tasks { get; }

    void Add(ITask task);
    void AddCompletedTask(ITask task);
    void Delete(int taskIndex);
    void Complete(int taskIndex);
    void Move(int oldIndex, int newIndex);
    void Clear();
    void ClearCompletedTasks();
    void Save();
    void Print();
    void LoadTasks();

    ITask Get(string taskName);
    ITask GetByID(int taskIndex);
}