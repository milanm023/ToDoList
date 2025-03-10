using System.Text;

namespace ToDoList
{
    /*
 * Napraviti To Do listu:
   od funkcionalnosti treba da ima sledeće:
   1. Dodavanje itema na listu,
   2. Brisanje elemenata sa liste, ali dvojako. Prvi nivo je da bude item ispisan ali precrtan linijom, a drugi nivo je da bude kompletno izbrisan sa spiska (nisam siguran da li ovo može u konzoli, ali valja proveriti)
   3. Resetovanje liste
   4. Sortiranje po nivou prioriteta
   5. Beleženje liste u eksterni fajl da može da se ponovo učita.
 */

    class StartUp
    {
        static void Main(string[] args)
        {
            Tutorial();
            
            IRepository<ITask> repository = new Repository();
            repository.LoadTasks();
            if (repository.Tasks.Count == 0)
            {
                Console.WriteLine("TO DO LIST:");
            }
            repository.Print();
            Console.WriteLine();
            string name = "";

            while (true)
            {
                Console.Write("Enter command: ");
                name = Console.ReadLine();
                Console.Clear();
                if (Equals(name.ToLower(), "exit"))
                {
                    break;
                }
                ExecuteCommand(repository, name);
                repository.Print();
            }
            repository.Print();
            
        }

        private static void Tutorial()
        {
            Console.WriteLine("Welcome to To Do List console application!");
            Console.WriteLine("List of commands:");
            Console.WriteLine("\t1. Add new task - Use keyword \"add\" to add new task. (add Make a breakfast)");
            Console.WriteLine("\t2. Complete a task - Use keyword \"complete\" to complete a task following task number in the list. (complete 2)");
            Console.WriteLine("\t3. Delete a task - Use keyword \"delete\" to delete a task following task number in the list. (delete 2)");
            Console.WriteLine("\t4. Move tasks - Use keyword \"move\" to move tasks order, following old tasks number and new task number in the list. (move 2 5)");
            Console.WriteLine("\t5. Clear tasks list - Use keyword \"clear\" to clear a task list. (clear)");
            Console.WriteLine("\t6. Clear completed tasks list - Use keyword \"clearcompleted\" to clear completed task list. (clearcompleted)");
            Console.WriteLine("\t7. Save lists - Use keyword \"save\" to save a task lists.");
            Console.WriteLine("\t8. Help - Use keyword \"help\".");
            Console.WriteLine("\t9. Exit program - Use keyword \"exit\".");
            Console.WriteLine();
        }

        private static void ExecuteCommand(IRepository<ITask>? repository, string? name)
        {
            string[] commands = name.Split(" ");
            string command = commands[0];
            
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < commands.Length; i++)
            {
                sb.Append(commands[i] + " ");
            }
            string taskName = sb.ToString().Trim();
            
            switch (command.ToLower())
            {
                case "add":
                    {
                        if (repository.Get(taskName) != null)
                        {
                            Console.WriteLine("Task already exists.");
                            break;
                        }
                        ITask task = new Task(taskName);
                        repository.Add(task);
                    }
                    break;
                case "move":
                    {
                        string[] indexes = taskName.Split(" ",StringSplitOptions.RemoveEmptyEntries).ToArray();
                        if (int.TryParse(indexes[0], out int oldIndex) && int.TryParse(indexes[1], out int newIndex))
                        {
                            if (oldIndex < 1 || oldIndex > repository.Tasks.Count || newIndex < 1 || newIndex > repository.Tasks.Count)
                            {
                                Console.WriteLine("Task does not exist.");
                                break;
                            }
                            repository.Move(oldIndex, newIndex);
                        }
                        else
                        {
                            Console.WriteLine("Please enter order number to delete.");
                        }
                    }
                    break;
                case "delete":
                    {
                        if (int.TryParse(taskName, out int taskIndex))
                        {
                            if (taskIndex < 1 || taskIndex > repository.Tasks.Count)
                            {
                                Console.WriteLine("Task does not exist.");
                                break;
                            }
                            repository.Delete(int.Parse(taskName));
                        }
                        else
                        {
                            Console.WriteLine("Please enter order number to delete.");
                        }
                    }
                    break;
                case "complete":
                    {
                        if (int.TryParse(taskName, out int taskIndex))
                        {
                            if (taskIndex < 1 || taskIndex > repository.Tasks.Count)
                            {
                                Console.WriteLine("Task does not exist.");
                                break;
                            }
                            ITask task = repository.GetByID(taskIndex-1);
                            repository.AddCompletedTask(task);
                            repository.Complete(taskIndex-1);
                        }
                        else
                        {
                            Console.WriteLine("Please enter order number to complete.");
                        }
                    }  
                    break;
                case "clear":
                    {
                        repository.Clear();
                    }
                    break;
                case "help":
                {
                    Tutorial();
                }
                    break;
                case "clearcompleted":
                {
                    repository.ClearCompletedTasks();
                }
                    break;
                case "save":
                {
                    repository.Save();
                }
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }
}


    
