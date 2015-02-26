using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeAndTravel
{
    public class ExtendedInteractionManager : InteractionManager
    {
        protected override Item CreateItem(string itemTypeString, string itemNameString, Location itemLocation, Item item)
        {
            switch (itemTypeString.ToLower())
            {
                case "weapon":
                    item = new Weapon(itemNameString, itemLocation);
                    break;
                case "wood":
                    item = new Wood(itemNameString, itemLocation);
                    break;
                case "iron":
                    item = new Iron(itemNameString, itemLocation);
                    break;
                default:
                    item = base.CreateItem(itemTypeString, itemNameString, itemLocation, item);
                    break;
            }
            return item;
        }

        protected override Location CreateLocation(string locationTypeString, string locationName)
        {
            Location location = null;
            switch (locationTypeString.ToLower())
            {
                case "mine":
                    location = new Mine(locationName);
                    break;
                case "forest":
                    location = new Forest(locationName);
                    break;
                default:
                    location = base.CreateLocation(locationTypeString, locationName);
                    break;
            }
            return location;
        }

        protected override void HandlePersonCommand(string[] commandWords, Person actor)
        {
            switch (commandWords[1].ToLower())
            {
                case "gather":
                    HandleGatherInteraction(actor, commandWords[2]);
                    break;
                case "craft":
                    HandleCraftInteraction(actor, commandWords[2], commandWords[3]);
                    break;
                default:
                    base.HandlePersonCommand(commandWords, actor);
                    break;
            }
        }

        private void HandleCraftInteraction(Person actor, string itemType, string itemName)
        {
            switch (itemType.ToLower())
            {
                case "weapon":
                    var ironForWeapon = actor.ListInventory().FirstOrDefault(i => i is Iron) as Iron;
                    var woodForWeapon = actor.ListInventory().FirstOrDefault(w => w is Wood) as Wood;
                    if (ironForWeapon != null && woodForWeapon != null)
                    {
                        Weapon weapon = new Weapon(itemName);
                        actor.AddToInventory(weapon);
                        this.ownerByItem[weapon] = actor;
                    }
                    break;
                case "armor":
                    var ironForArmor = actor.ListInventory().FirstOrDefault(i => i is Iron) as Iron;
                    if (ironForArmor != null)
                    {
                        Armor armor = new Armor(itemName);
                        actor.AddToInventory(armor);
                        this.ownerByItem[armor] = actor;
                    }
                    break;
                default:
                    break;

            }
        }

        private void HandleGatherInteraction(Person actor, string itemName)
        {
            switch (actor.Location.LocationType)
            {
                case LocationType.Forest:
                    var weapon = actor.ListInventory().FirstOrDefault(w => w is Weapon) as Weapon;
                    if (weapon != null)
                    {
                        Wood wood = new Wood(itemName);
                        actor.AddToInventory(wood);
                        this.ownerByItem[wood] = actor;
                    }
                    break;
                case LocationType.Mine:
                    var armor = actor.ListInventory().FirstOrDefault(a => a is Armor) as Armor;
                    if (armor != null)
                    {
                        Iron iron = new Iron(itemName);
                        actor.AddToInventory(iron);
                        this.ownerByItem[iron] = actor;
                    }
                    break;
                default:
                    break;
            }
        }

        protected override Person CreatePerson(string personTypeString, string personNameString, Location personLocation)
        {
            Person person = null;
            switch (personTypeString.ToLower())
            {
                case "merchant":
                    person = new Merchant(personNameString, personLocation);
                    break;
                default:
                    person = base.CreatePerson(personTypeString, personNameString, personLocation);
                    break;
            }
            return person;
        }
    }
}
