using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanetWars.Tests
{
    public class Tests
    {
        [TestFixture]
        public class PlanetWarsTests
        {
            [Test]

            public void ConstructorPlanet()
            {
                Planet planet = new Planet("Pluto",90);
                Assert.AreEqual("Pluto", planet.Name);
                Assert.AreEqual(90, planet.Budget);
                Assert.AreEqual(new List<Weapon>(), planet.Weapons);
                Assert.AreEqual(0, planet.Weapons.Count);
                

            }
            [TestCase(null)]
            [TestCase("")]

            public void ConstructorPlanetNullName(string name)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    Planet planet = new Planet(name, 99);
                });
            }

            [Test]

            public void ConstructorPlanetBudgetInvalid()
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    Planet planet = new Planet("hi", -99);
                });
            }
            [Test]
            public void WeaponTestConstructor()
            {
                Weapon weapon = new Weapon("claymore", 90, 9);
                Assert.AreEqual("claymore", weapon.Name);
                Assert.AreEqual(90, weapon.Price);
                Assert.AreEqual(9, weapon.DestructionLevel);

            }
            [Test]
            public void WeaponNegativePrice()
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    Weapon weapon = new Weapon("claymore", -90, 9);
                });

            }
            [Test]
            public void WeaponIncreaseDestruction()
            {

                Weapon weapon = new Weapon("claymore", 90, 9);
                weapon.IncreaseDestructionLevel();
                Assert.AreEqual(10, weapon.DestructionLevel);
                Assert.AreEqual(true, weapon.IsNuclear);


            }
            [Test]
            public void WeaponIsNuclear()
            {

                Weapon weapon = new Weapon("claymore", 90, 111);

                Assert.AreEqual(true, weapon.IsNuclear);


            }
            [Test]
            public void WeaponIsNuclearNo()
            {

                Weapon weapon = new Weapon("claymore", 90, 1);

                Assert.AreEqual(false, weapon.IsNuclear);


            }
            [Test]
            public void ProfitPlanet()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);
                Assert.AreEqual(90, 9, planet.Budget);
            }
            [Test]
            public void NotEnoughtFunds()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    planet.SpendFunds(100);
                });
            }
            [Test]
            public void EnoughtFunds()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);
                
                    planet.SpendFunds(90.9);
                Assert.AreEqual(0, planet.Budget);
            }
            [Test]
            public void AddingTheSameWeapon()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);

                planet.SpendFunds(90.9);
                Weapon weapon = new Weapon("hi", 90, 1);
                planet.AddWeapon(weapon);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    planet.AddWeapon(weapon);
                });
            }
            [Test]
            public void AddingWeapon()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);

                planet.SpendFunds(90.9);
                Weapon weapon = new Weapon("hi", 90, 1);
                planet.AddWeapon(weapon);
                Assert.AreEqual(1, planet.Weapons.Count);
                Assert.AreEqual(weapon, planet.Weapons.First(w => w.Name == "hi"));
            }
            [Test]
            public void RemovingWeapon()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);

                planet.SpendFunds(90.9);
                Weapon weapon = new Weapon("hi", 90, 1);
                Weapon weapon2 = new Weapon("bye", 90, 1);

                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);
                planet.RemoveWeapon("bye");
                Assert.AreEqual(1, planet.Weapons.Count);
                Assert.AreEqual(null, planet.Weapons.FirstOrDefault(w => w.Name == "bye"));
            }
            [Test]
            public void RemovingNullWeapon()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);

                planet.SpendFunds(90.9);
                Weapon weapon = new Weapon("hi", 90, 1);
                Weapon weapon2 = new Weapon("bye", 90, 1);

                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);
                planet.RemoveWeapon("what");
                Assert.AreEqual(2, planet.Weapons.Count);
                Assert.AreEqual(weapon2, planet.Weapons.FirstOrDefault(w => w.Name == "bye"));
            }
            [Test]
            public void UprgadingNoWeapon()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);

                planet.SpendFunds(90.9);
                Weapon weapon = new Weapon("hi", 90, 1);
                Weapon weapon2 = new Weapon("bye", 90, 1);

                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    planet.UpgradeWeapon("what");
                });
              
            }
            [Test]
            public void UprgadingWeapon()
            {
                Planet planet = new Planet("Pluto", 90);
                planet.Profit(0.9);

                planet.SpendFunds(90.9);
                Weapon weapon = new Weapon("hi", 90, 9);
                Weapon weapon2 = new Weapon("bye", 90, 1);

                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);
              
                    planet.UpgradeWeapon("hi");
                Assert.AreEqual(10, weapon.DestructionLevel);
                Assert.AreEqual(true, weapon.IsNuclear);
               

            }

            [Test]
            public void DestructOpponent()
            {
                Planet planet = new Planet("Pluto", 90);
                Planet opp = new Planet("Pluto", 90);

                planet.Profit(0.9);
                planet.SpendFunds(90.9);

                Weapon weapon = new Weapon("hi", 90, 9);
                Weapon weapon2 = new Weapon("bye", 90, 1);
                Weapon weapon3 = new Weapon("no", 90, 10);
                Weapon weapon4 = new Weapon("yeah", 90, 11);


                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);
                opp.AddWeapon(weapon3);
                opp.AddWeapon(weapon4);

                planet.UpgradeWeapon("hi");

                Assert.Throws<InvalidOperationException>(() =>
                {
                    planet.DestructOpponent(opp);
                });

            }
            [Test]
            public void DestructOpponentWorks()
            {
                Planet planet = new Planet("Pluto", 90);
                Planet opp = new Planet("Pluto", 90);

                planet.Profit(0.9);
                planet.SpendFunds(90.9);

                Weapon weapon = new Weapon("hi", 90, 9);
                Weapon weapon2 = new Weapon("bye", 90, 1);
                Weapon weapon3 = new Weapon("no", 90, 10);
                Weapon weapon4 = new Weapon("yeah", 90, 11);


                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);
                opp.AddWeapon(weapon3);
                opp.AddWeapon(weapon4);

                planet.UpgradeWeapon("hi");


                Assert.AreEqual($"{planet.Name} is destructed!", opp.DestructOpponent(planet));
              

            }


        }
    }
}
