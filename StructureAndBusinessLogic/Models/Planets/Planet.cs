using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Repositories;
using PlanetWars.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetWars.Models.Planets
{
    public class Planet : IPlanet
    {
        //private readonly List<IMilitaryUnit>units;
        //private readonly List<IWeapon> weapons;
        private string name;
        private double budget;
        private UnitRepository units;
        private WeaponRepository weapons;
      

        public Planet(string name, double budget)
        {
            Name = name;
            Budget = budget;
            units = new UnitRepository();
            weapons = new WeaponRepository();
            //units = new List<IMilitaryUnit>();
            //weapons = new List<IWeapon>();
        }
        public string Name
        {
            get => name;
            private set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages
                        .InvalidPlanetName);
                }
                name = value;
            }
        }

        public double Budget
        {
            get => budget;
            private set
            {
                if(value<0)
                {
                    throw new ArgumentException(ExceptionMessages
                        .InvalidBudgetAmount);
                }
                budget = value;
            }
        }

        public double MilitaryPower => CalculateMilitaryPower();

        private double CalculateMilitaryPower()
        {
            double sum = Army.Sum(u => u.EnduranceLevel)
            + Weapons.Sum(w => w.DestructionLevel);

            if(units.FindByName("AnonymousImpactUnit") !=null)
            {
                sum = sum + sum*0.3;
            }

            if (weapons.FindByName("NuclearWeapon") != null)
            {
                sum = sum + sum * 0.45;
            }
            sum = Math.Round(sum, 3);
            return sum;
        }
        public IReadOnlyCollection<IMilitaryUnit> Army => units.Models;

        public IReadOnlyCollection<IWeapon> Weapons => weapons.Models;

        public void AddUnit(IMilitaryUnit unit)
        {
            units.AddItem(unit);
        }

        public void AddWeapon(IWeapon weapon)
        {
            weapons.AddItem(weapon);
        }
        public void TrainArmy()
        {
            foreach (var item in Army)
            {
                item.IncreaseEndurance();
            }
        }
        public void Spend(double amount)
        {
            if(Budget-amount<0)
            {
                throw new InvalidOperationException(ExceptionMessages
                    .UnsufficientBudget);
            }
            Budget -= amount;
        }
        public void Profit(double amount)
        {
            Budget += amount;
        }
        public string PlanetInfo()
        {
            StringBuilder sb = new StringBuilder();
            string forces = Army.Count == 0 ? "No units"
                : string.Join(", ", Army.Select(u => u.GetType().Name));
            string equipment = Weapons.Count == 0 ? "No weapons"
                : string.Join(", ", Weapons.Select(w => w.GetType().Name));

            sb.AppendLine($"Planet: {Name}");
            sb.AppendLine($"--Budget: {Budget} billion QUID");
            sb.AppendLine($"--Forces: {forces}");
            sb.AppendLine($"--Combat equipment: {equipment}");
            sb.AppendLine($"--Military Power: {MilitaryPower}");

            return sb.ToString().TrimEnd();
        }

      

     

       
    }
}
