using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EmergencyResponseSimulation
{
    abstract class EmergencyUnit
    {
        public string Name { get; set; }
        public int Speed { get; set; }

        public EmergencyUnit(string name, int speed)
        {
            Name = name;
            Speed = speed;
        }

        public abstract bool CanHandle(string incidentType);
        public abstract void RespondToIncident(Incident incident);
    }
    class Police : EmergencyUnit
    {
        public Police(string name, int speed) : base(name, speed) { }

        public override bool CanHandle(string incidentType) => incidentType == "Crime";
        public override void RespondToIncident(Incident incident) =>
            Console.WriteLine($"🚓 {Name} is handling a {incident.Type} at {incident.Location}.");
    }

    class Firefighter : EmergencyUnit
    {
        public Firefighter(string name, int speed) : base(name, speed) { }

        public override bool CanHandle(string incidentType) => incidentType == "Fire";
        public override void RespondToIncident(Incident incident) =>
            Console.WriteLine($"🔥 {Name} is extinguishing a fire at {incident.Location}.");
    }

    class Ambulance : EmergencyUnit
    {
        public Ambulance(string name, int speed) : base(name, speed) { }

        public override bool CanHandle(string incidentType) => incidentType == "Medical";
        public override void RespondToIncident(Incident incident) =>
            Console.WriteLine($"🚑 {Name} is treating a patient at {incident.Location}.");
    }

    class RescueTeam : EmergencyUnit
    {
        public RescueTeam(string name, int speed) : base(name, speed) { }

        public override bool CanHandle(string incidentType) => incidentType == "Rescue";
        public override void RespondToIncident(Incident incident) =>
            Console.WriteLine($"🆘 {Name} is performing a rescue at {incident.Location}.");
    }
    class Incident
    {
        public string Type { get; set; }
        public string Location { get; set; }
        public string Difficulty { get; set; } // Easy / Medium / Hard

        public Incident(string type, string location, string difficulty)
        {
            Type = type;
            Location = location;
            Difficulty = difficulty;
        }
    }

    class Program
    {
        static Random rand = new Random();
        static string[] incidentTypes = { "Fire", "Crime", "Medical", "Rescue" };
        static string[] locations = { "Downtown", "Suburb", "Mall", "University", "Stadium", "Bridge" };
        static string[] difficulties = { "Easy", "Medium", "Hard" };

        static int GetPoints(string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    return 10;
                case "Medium":
                    return 15;
                case "Hard":
                    return 20;
                default:
                    return 10;
            }
        }

        static int GetResponseTime(int speed)
        {
            return 1000 * (100 / speed);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("🚨 Emergency Response Simulation with Bonus Features\n");
            Console.WriteLine("Select Simulation Mode:");
            Console.WriteLine("1. Auto-Respond (system picks correct unit)");
            Console.WriteLine("2. Manual-Respond (you choose which unit responds)");

            Console.Write("Your choice (1 or 2): ");
            int mode = int.TryParse(Console.ReadLine(), out int m) ? m : 1;
            var units = new List<EmergencyUnit>
            {
                new Police("Alpha Police", 90),
                new Firefighter("Blaze Squad", 70),
                new Ambulance("MediCare", 85),
                new RescueTeam("Rescue Rangers", 80)
            };

            int score = 0;

            for (int round = 1; round <= 5; round++)
            {
                Console.WriteLine($"\n--- Round {round} ---");

                string type = incidentTypes[rand.Next(incidentTypes.Length)];
                string location = locations[rand.Next(locations.Length)];
                string difficulty = difficulties[rand.Next(difficulties.Length)];

                Incident incident = new Incident(type, location, difficulty);

                Console.WriteLine($"📢 Incident: {incident.Type} at {incident.Location} (Difficulty: {incident.Difficulty})");

                EmergencyUnit selectedUnit = null;

                if (mode == 2) 
                {
                    Console.WriteLine("Available Units:");
                    for (int i = 0; i < units.Count; i++)
                        Console.WriteLine($"{i + 1}. {units[i].Name} ({units[i].GetType().Name})");

                    Console.Write("Choose a unit by number: ");
                    int choice = int.TryParse(Console.ReadLine(), out int c) ? c : 0;

                    if (choice >= 1 && choice <= units.Count)
                        selectedUnit = units[choice - 1];
                }
                else 
                {
                    foreach (var unit in units)
                    {
                        if (unit.CanHandle(incident.Type))
                        {
                            selectedUnit = unit;
                            break;
                        }
                    }
                }

                if (selectedUnit != null)
                {
                    Console.WriteLine($"🚨 Dispatching {selectedUnit.Name}...");

                    if (selectedUnit.CanHandle(incident.Type))
                    {
                        int responseTime = GetResponseTime(selectedUnit.Speed);
                        Thread.Sleep(responseTime);
                        selectedUnit.RespondToIncident(incident);

                        int basePoints = GetPoints(incident.Difficulty);
                        int bonus = selectedUnit.Speed >= 85 ? 5 : 0;
                        int total = basePoints + bonus;

                        Console.WriteLine($"✅ Incident handled successfully! (+{basePoints} pts, +{bonus} bonus for speed)");
                        score += total;
                    }
                    else
                    {
                        Console.WriteLine($"❌ {selectedUnit.Name} CANNOT handle {incident.Type}!");
                        Console.WriteLine("Penalty: -5 points.");
                        score -= 5;
                    }
                }
                else
                {
                    Console.WriteLine($"❌ No unit available to handle {incident.Type}.");
                    Console.WriteLine("Penalty: -5 points.");
                    score -= 5;
                }

                Console.WriteLine($"🔢 Current Score: {score}");
            }

            Console.WriteLine("\n✅ Simulation Complete!");
            Console.WriteLine($"🏁 Final Score: {score}");
        }
    }
}
