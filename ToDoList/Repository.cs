namespace ToDoList;

public class Repository : IRepository<ITask>
{
    private List<ITask> _tasks = new List<ITask>();
    private List<ITask> _completedTasks = new List<ITask>();
    public IReadOnlyCollection<ITask> Tasks => _tasks.AsReadOnly();
    public IReadOnlyCollection<ITask> CompletedTasks => _completedTasks.AsReadOnly();
    
    public void Add(ITask task)
    {
        ITask newTask = task;
        _tasks.Add(newTask);
    }

    public void AddCompletedTask(ITask task)
    {
        _completedTasks.Add(task);
    }
    public void Delete(int taskIndex)
    {
        _tasks.RemoveAt(taskIndex-1);
    }

    public void Complete(int taskIndex)
    {
        string taskName = _tasks[taskIndex].Name;
        string strikethroughText = string.Concat(taskName.Select(c => c + "\u0336")).Trim();
        ITask task = new Task(strikethroughText);
        _tasks.RemoveAt(taskIndex);
        _tasks.Insert(taskIndex, task);
    }

    public void Move(int oldIndex, int newIndex)
    {
        ITask oldTask = _tasks[oldIndex-1];
        if (oldIndex > newIndex)
        {
            _tasks.Insert(newIndex-1, oldTask);
            for (int i = 0; i < _tasks.Count; i++)
            {
                if (i != newIndex-1 && _tasks[i] == oldTask)
                {
                    _tasks.RemoveAt(i);
                }
            }
        }

        if (newIndex > oldIndex)
        {
            _tasks.Insert(newIndex, oldTask);
            _tasks.RemoveAt(oldIndex-1);
        }
        
    }

    public void Clear()
    {
        _tasks.Clear();
    }

    public void ClearCompletedTasks()
    {
        _completedTasks.Clear();
    }

    public void Print()
    {
        if (_tasks.Count == 0)
        {
            Console.WriteLine("*** List is empty. ***");
        }
        else
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("TO DO LIST:");
            for (int i = 0; i < _tasks.Count; i++)
            {
                Console.WriteLine($"{i+1}. {_tasks[i]}");
            }
            Console.WriteLine("---------------------------");
        }

        if (_completedTasks.Count > 0)
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine("---------------------------");
            }
            Console.WriteLine("*** Completed Tasks: ***");
            for (int i = 0; i < _completedTasks.Count; i++)
            {
                Console.WriteLine($"{i+1}. {_completedTasks[i]}");
            }
            Console.WriteLine("---------------------------");
        }
    }

    public void Save()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../ToDoList.txt");
        
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            if (_tasks.Count > 0)
            {
                writer.WriteLine("---------------------------");
                writer.WriteLine("TO DO LIST:");
                for (int i = 0; i < _tasks.Count; i++)
                {
                    writer.WriteLine($"{i + 1}. {_tasks[i]}");
                }
                writer.WriteLine("---------------------------");
            }

            if (_completedTasks.Count > 0)
            {
                if (_tasks.Count == 0)
                {
                    writer.WriteLine("---------------------------");
                }
                writer.WriteLine("*** Completed Tasks: ***");
                for (int i = 0; i < _completedTasks.Count; i++)
                {
                    writer.WriteLine($"{i + 1}. {_completedTasks[i]}");
                }
                writer.WriteLine("---------------------------");
            }
        }
        Console.WriteLine("Output saved to ToDoList.txt");
        
    }

    public void LoadTasks()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../ToDoList.txt");
        if (File.Exists(filePath))
        {
            string text = File.ReadAllText(filePath);
            if (!string.IsNullOrWhiteSpace(text))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string currentLine;
                    while ((currentLine = reader.ReadLine()) != null)
                    {
                        if (char.IsDigit(currentLine[0]))
                        {
                            int index = currentLine.IndexOf(' ');
                            string currentTaskName = currentLine.Substring(index+1);
                            _tasks.Add(new Task(currentTaskName));
                        }

                        if (Equals(currentLine, "*** Completed Tasks: ***"))
                        {
                            string newLine;
                            while ((newLine = reader.ReadLine()) != null)
                            {
                                if (char.IsDigit(newLine[0]))
                                {
                                    int index = newLine.IndexOf(' ');
                                    string currentTaskName = newLine.Substring(index+1);
                                    _completedTasks.Add(new Task(currentTaskName));
                                }

                                if (Equals(currentLine, "---------------------------"))
                                {
                                    //_tasks.RemoveAt(_tasks.Count - 1);
                                    return;
                                }
                            }
                            
                        }
                    }
                    
                }
            }
        }
        
    }
    
    public ITask Get(string name)
    {
        return _tasks.FirstOrDefault(t => t.Name == name);
    }

    public ITask GetByID(int taskIndex)
    {
        return this._tasks[taskIndex];
    }
}