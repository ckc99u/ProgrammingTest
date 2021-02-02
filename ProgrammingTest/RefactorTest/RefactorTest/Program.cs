using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace RefactorTest
{
    public enum EffectType
    {
        None = 0,
        Add = 1,
        Multiply = 2,
        Subtact = 3,
        Divide = 4,
    }

    public class StatMod
    {
        public StatMod(string t_idnet, int t_val, EffectType t_type)
        {
            Ident = t_idnet;
            Val = t_val;
            Type = t_type;
        }
        public string Ident;
        public int Val;
        public EffectType Type;
    }

    public class Entity
    {


        private Dictionary<string, StatMod> m_appliedStatMods = default(Dictionary<string, StatMod>);
        public string Id;// Do we really need it?

        public int HPMax => GetValue("HP");
        public int HPCurrent;//initialize it
        public int ArmorMax => GetValue("Arm");
        public int ArmorCurrent;//init it
        public int Speed => GetValue("Spd");

        public void Setup(string id, int hp, int arm, int spd)
        {
            Id = id;
            m_appliedStatMods = new Dictionary<string, StatMod>();

            StatModHandler.AddStatMod(this, "HP", hp, EffectType.Add);
            StatModHandler.AddStatMod(this, "Arm", arm, EffectType.Add);
            StatModHandler.AddStatMod(this, "Spd", spd, EffectType.Add);
        }
        public ref Dictionary<string, StatMod> GetAppliedStatMod
        {
            get => ref m_appliedStatMods;//to do detect null or default
        }
        //change to private
        private int GetValue(string ident)
        {
            return StatModHandler.GetValue(this, ident);//always new the Handler
        }
    }
    public class StatModHandler
    {
        public static int GetValue(Entity ent, string ident)
        {
            return ent.GetAppliedStatMod[ident].Val;
        }
        public static int ComputeStatModValue(int t_originval,int t_val ,EffectType t_type)
        {
            switch (t_type)
            {
                case EffectType.Add:
                    t_originval += t_val;
                    break;
                case EffectType.Multiply:
                    t_originval *= t_val;
                    break;
                case EffectType.Subtact:
                    t_originval -= t_val;
                    break;
                case EffectType.Divide:
                    t_originval /= t_val;
                    break;
                default:
                    break;
                    
            }
            return t_originval;
        }
        public static void AddStatMod(Entity ent, string ident, int val, EffectType type)
        {
            if (!ent.GetAppliedStatMod.ContainsKey(ident))
            {
                StatMod _statmod = new StatMod(ident, ComputeStatModValue(0, val, type), type);
                ent.GetAppliedStatMod.Add(ident, _statmod);
            }
            else
            {
                ent.GetAppliedStatMod[ident].Val = ComputeStatModValue(ent.GetAppliedStatMod[ident].Val, val, type);
                ent.GetAppliedStatMod[ident].Type = type;
            }
        }
        //remover should track its unique ID?
        public static void RemoveStatMod(Entity ent, string ident, int val, EffectType type)
        {
            if(ent.GetAppliedStatMod.ContainsKey(ident))
            {
                ent.GetAppliedStatMod.Remove(ident);
            }
        }
    }

    public class Runner
    {
        private List<Entity> ActiveEntities;

        //private Dictionary<int, Entity> m_ActiveEntities;

        private int IdCnt;

        public Runner()
        {
            ActiveEntities = new List<Entity>();
        }

        public void CreateEntity()//dynamic assign val
        {
            var entity = new Entity();
            entity.Setup((IdCnt++).ToString(), 200, 500, 5);
            ActiveEntities.Add(entity);
        }

        public void ShowEntities()
        {
            foreach (var entity in ActiveEntities)
            {
                Console.WriteLine($"Entity '{entity.Id}' has HP of: {entity.HPMax} Armor of: {entity.ArmorMax} and Speed of: {entity.Speed}");
            }
        }

        public void AddRandomStatToAll()
        {
            foreach (var entity in ActiveEntities)
            {
                string ident = "None";
                int identRand = Randomizer.GetVal(0, 3);
                //Switch cases should almost always have a default case.
                switch (identRand)
                {
                    case 0:
                        ident = "HP";
                        break;
                    case 1:
                        ident = "Arm";
                        break;
                    case 2:
                        ident = "Spd";
                        break;
                    default:
                        break;
                }
                int val = Randomizer.GetVal(1, 50);
                Console.WriteLine("random value {0}", val);
                EffectType type = (EffectType)Randomizer.GetVal(0, 5);
                Console.WriteLine("random type {0} ", type);
                Console.WriteLine("whom want to add {0} ", ident);
                StatModHandler.AddStatMod(entity, ident, val, type);//Don't instance new;

            }
        }
    }
    /// <summary>
    /// Randomize any value, we only need one in the whole program/ or change to static func for getVal
    /// </summary>
    public class Randomizer
    {
        private readonly static Random Random = new Random();

        public static int GetVal(int min, int max)
        {
            return Random.Next(min, max);
        }
    }

    class Program
    {
        private static Runner GameRunner;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Program");


             GameRunner = new Runner();
             for (int i = 0; i < 3; i++)
             {
                 Console.WriteLine("\n create a new entity to runner: ");
                 GameRunner.CreateEntity();
             }
             GameRunner.AddRandomStatToAll();
             GameRunner.ShowEntities();

            Console.Write("\nPress any key to continue... ");
            Console.ReadLine();
        }
    }
}
