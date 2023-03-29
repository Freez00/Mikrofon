using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LifeformAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            NPC guide = new NPC("Andrew", "The wall of flesh in disguise", 100);
            NPC dryad = new NPC("Xylia", "Leaf Girl", 100);
            NPC goblin_tinkerer = new NPC("Nort", "In love with the mechanic", 100);
            NPC mechanic = new NPC("Shayna", "In love with the goblin tinkerer", 100);
            NPC truffle = new NPC("Enoki", "Blue glowy boy", 100);

            Dictionary<string, NPC> npc_options = new Dictionary<string, NPC>()
            {
                {"Andrew",guide},
                {"Xylia", dryad },
                {"Nort", goblin_tinkerer },
                {"Shayna", mechanic },
                {"Enoki", truffle}
            };

            InspectorScreen<NPC> inspectorScreen = new InspectorScreen<NPC>(npc_options);

            inspectorScreen.Start();
        }
    }
    class InspectorScreen<T>
    {
        public Dictionary<string, T> options = new Dictionary<string, T>();

        private int index = 0;
        private int Count { get { return options.Count; } }

        public InspectorScreen(Dictionary<string, T> options)
        {
            this.options = options;
        }
        public void Start()
        {
            while (true)
            {
                Console.Clear();
                PrintOptions();
                ConsoleKey input = Console.ReadKey().Key;
                if (input == ConsoleKey.UpArrow && index > 0)
                {
                    index--;
                }
                else if (input == ConsoleKey.DownArrow && index < Count - 1)
                {
                    index++;
                }
                else if (input == ConsoleKey.Enter)
                {
                    T target = options.ElementAt(index).Value;
                    LifeformAnalyzer.Analyze(target);
                    Console.ReadLine();
                }
            }
        }
        private void PrintOptions()
        {
            for (int i = 0; i < Count; i++)
            {
                bool flag = false;
                if(i == index)
				{
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    flag = true;
                }
                
                Console.WriteLine(options.ElementAt(i).Key);

                if (flag)
                    Console.ResetColor();
            }
        }
    }
    static class LifeformAnalyzer
    {
        public static Dictionary<string, FieldInfo[]> data = new Dictionary<string, FieldInfo[]>();
        public static void Analyze(object target)
        {
            Type target_type = target.GetType();
            FieldInfo[] target_fields;
            if (data.Keys.Contains(target_type.Name))
                target_fields = data[target_type.Name];
			else
			{
                target_fields = target_type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                data.Add(target_type.Name, target_fields);
			}
            target_fields.Print(target);
        }

    }
    class NPC
    {
        private string name;
        private string description;
        private int health;
        public NPC(string name, string description, int health)
        {
            this.name = name;
            this.description = description;
            this.health = health;
        }
    }
    static class Extensions
	{
        public static void Print(this FieldInfo[] fields)
		{
			foreach (var field in fields)
			{
				Console.WriteLine(field.Name);
			}
		}
        public static void Print(this FieldInfo[] fields, object instance)
		{
            foreach (var field in fields)
            {
                Console.WriteLine($"{field.Name} = {field.GetValue(instance)}");
                try
                {
                    field.SetValue(instance, "hacked");

				}
				catch 
                {
                    field.SetValue(instance, 00000000);
                }
            }
        }
	}
}
