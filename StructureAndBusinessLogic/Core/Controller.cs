using PlanetWars.Core.Contracts;
using PlanetWars.Models.MilitaryUnits;
using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Models.Planets;
using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Models.Weapons;
using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Repositories;
using PlanetWars.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetWars.Core
{
    public class Controller : IController
    {
        private PlanetRepository planets ;
        public Controller()
        {
            planets = new PlanetRepository();
        }
        public string CreatePlanet(string name, double budget)
        {
            IPlanet planet = planets.FindByName(name);
            if(planet != null)
            {
                return string.Format(string.Format(OutputMessages
                    .ExistingPlanet, name));
            }
            planet = new Planet(name, budget);
            planets.AddItem(planet);

            return String.Format(OutputMessages
                .NewPlanet, name);
        }
        public string AddUnit(string unitTypeName, string planetName)
        {
            IPlanet planet = planets.FindByName(planetName);
            IMilitaryUnit unit;

            if (planet == null)
            {
                throw new InvalidOperationException (string.Format(ExceptionMessages
                    .UnexistingPlanet,planetName));
            }
            if(unitTypeName == "StormTroopers")
            {
                unit = new StormTroopers();
            }
            else if(unitTypeName == "SpaceForces")
            {
                unit = new SpaceForces();
            }
            else if(unitTypeName == "AnonymousImpactUnit")
            {
                unit = new AnonymousImpactUnit();
            }
            else
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages
                    .ItemNotAvailable, unitTypeName));
            }
            if(planet.Army.Any(u=>u.GetType().Name == unitTypeName))
                    {
                throw new InvalidOperationException(string.Format(ExceptionMessages
                    .UnitAlreadyAdded, unitTypeName, planetName));
            }
            planet.Spend(unit.Cost);
            planet.AddUnit(unit);
            

            return string.Format(OutputMessages
                .UnitAdded, unitTypeName, planetName);
        }

        public string AddWeapon(string planetName, string weaponTypeName, int destructionLevel)
        {
            IPlanet planet = planets.FindByName(planetName);
            IWeapon weapon;

            if (planet == null)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages
                    .UnexistingPlanet, planetName));
            }
            if (weaponTypeName == "SpaceMissiles")
            {
                weapon = new SpaceMissiles(destructionLevel);
            }
            else if (weaponTypeName == "BioChemicalWeapon")
            {
                weapon = new BioChemicalWeapon(destructionLevel);
            }
            else if (weaponTypeName == "NuclearWeapon")
            {
                weapon = new NuclearWeapon(destructionLevel);
            }
            else
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages
                    .ItemNotAvailable, weaponTypeName));
            }
            if (planet.Weapons.Any(w => w.GetType().Name == weaponTypeName))
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages
                    .WeaponAlreadyAdded, weaponTypeName, planetName));
            }
            
            planet.Spend(weapon.Price);
            planet.AddWeapon(weapon);

            return string.Format(OutputMessages
                .WeaponAdded, planetName,weaponTypeName);
        }

        public string SpecializeForces(string planetName)
        {
            IPlanet planet = planets.FindByName(planetName);
           

            if (planet == null)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages
                    .UnexistingPlanet, planetName));
            }
            if(planet.Army.Count == 0)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessages
                    .NoUnitsFound));
            }
            planet.Spend(1.25);
            planet.TrainArmy();

            return string.Format(OutputMessages
                .ForcesUpgraded, planetName);
        }
        public string SpaceCombat(string planetOne, string planetTwo)
        {
            IPlanet first = planets.FindByName(planetOne);
            IPlanet second = planets.FindByName(planetTwo);
            IPlanet winner=null;
            IPlanet loser=null;

            if(first.MilitaryPower == second.MilitaryPower)
            {
                if(first.Weapons.Any(w => w.GetType().Name == "NuclearWeapon") 
                   && second.Weapons.Any(w => w.GetType().Name == "NuclearWeapon")
                   || !second.Weapons.Any(w => w.GetType().Name == "NuclearWeapon")
                   && !first.Weapons.Any(w => w.GetType().Name == "NuclearWeapon"))
                {
                    first.Spend(first.Budget / 2);
                    second.Spend(second.Budget / 2);
                    return string.Format(OutputMessages
                        .NoWinner);
                }
                if (first.Weapons.Any(w => w.GetType().Name == "NuclearWeapon"))
                {
                    winner = first;
                    loser = second;
                }
                else if (second.Weapons.Any(w => w.GetType().Name == "NuclearWeapon"))
                {
                    winner = second;
                    loser = first;
                    
                }
            }
            else
            {
                if(first.MilitaryPower > second.MilitaryPower)
                {
                    winner = first;
                    loser = second;
                }
                else
                {
                    winner = second;
                    loser = first;
                }
            }
            winner.Spend(winner.Budget / 2);
            double profit = loser.Army.Sum(u => u.Cost) + loser.Weapons.Sum(w => w.Price);
            winner.Profit(profit);
            winner.Profit(loser.Budget / 2);
            planets.RemoveItem(loser.Name);

            return String.Format(OutputMessages
                .WinnigTheWar, winner.Name, loser.Name);
        }
        public string ForcesReport()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"***UNIVERSE PLANET MILITARY REPORT***");

            foreach (var planet in planets.Models
                .OrderByDescending(p=>p.MilitaryPower)
                .ThenBy(p =>p.Name))
            {
                
                sb.AppendLine(planet.PlanetInfo());
            }

            return sb.ToString().TrimEnd();
        }

       

       
    }
}
