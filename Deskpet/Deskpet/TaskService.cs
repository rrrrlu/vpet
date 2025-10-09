using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Deskpet
{
    public class TaskService
    {
        private readonly string filePath = "tasks.json";

        public List<TaskModel> LoadTasks()
        {
            if (!File.Exists(filePath)) return new List<TaskModel>();
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<TaskModel>>(json) ?? new List<TaskModel>();
        }

        public void SaveTasks(IEnumerable<TaskModel> tasks)
        {
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}