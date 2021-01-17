using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 Cameron Lloyd, Bradley Graves
 z1853137
 Assignment 1
 csci 473
 This is the definition of the Item class along with a testing main and an Icomparable interface
 */
namespace Assign1
{
    //The given enum for the Itemtypes
    public enum ItemType
    {
        Helmet, Neck, Shoulders, Back, Chest,
        Wrist, Gloves, Belt, Pants, Boots,
        Ring, Trinket
    };
    public class Item : IComparable
    {
        //Max variables the bottom 2 just commented out so they stopped giving me a warning about being unsused
        private static uint MAX_ILVL = 360;
        private static uint MAX_PRIMARY = 200;
        private static uint MAX_STAMINA = 275;
        private static uint MAX_LEVEL = 60;
        //private static uint GEAR_SLOTS = 14;
        //private static uint MAX_INVENTORY_SIZE = 20;
        //The Count variable so that we can keep track of how many items ther are
        public static int Count = 0;
        //the id variable that can only be changed in the constuctor and then the public property with on a get method
        private readonly uint id;
        public uint Id
        {
            get { return id; }

        }
        
        //The name of the item as well as the public property for accessing the String or changing it.
        private String name;
        public String Name
        {
            //just very simple get and set methods
            get => name;
            set => name = value;
        }
        
        //the itemType as well as the public property for accessing the itemType
        private ItemType type;
        public ItemType Type
        {
            //simple get method
            get => type;
            //The set method here checks to make sure that the Item type is somewhere between Helmet and Trinket
            //and throws an acception if the argument given is out of the given range
            set
            {
                if (value < 0 || value > ItemType.Trinket)
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be between 0 and 12");
                //otherwise changes the item type
                type = value;
            }
        }
        
        //the item level of the item and the public property for accessing it.
        private uint ilvl;
        public uint Ilvl
        {
            //simple get method
            get => ilvl;
            set
            {
                //checks to make sure that the item level that is given is between 0 and the given max item level
                //if the item level is not in that range it throws an acception.
                if (value < 0 || value > MAX_ILVL)
                    throw new ArgumentOutOfRangeException($"The item level must be between 0 and " + MAX_ILVL);
                //otherwise changes the value of ilvl
                ilvl = value;
            }
        }
        
        //The primary number(I am unsure of what this actually is at this point) and the public property of it
        private uint primary;
        public uint Primary
        {
            //simple get method
            get => primary;
            set
            {
                //checks to make sure that the primary value is between 0 and the given Max primary value and
                //throws an exception if it is not it throws an exception stating that it is not in the range
                if (value < 0 || value > MAX_PRIMARY)
                    throw new ArgumentOutOfRangeException($"The primary value must be between 0 and " + MAX_PRIMARY);
                //or it just changes the value of primary.
                primary = value;
            }
        }
        
        //the stamina cost of using the weapon(I am assuming that is what stamina is in reference to?) and it's property
        private uint stamina;
        public uint Stamina
        {
            //simple get method
            get => stamina;
            set
            {
                //checks to make sure that the value of the stamina is between 0 and the given max stamina cost
                //and throws an exception if it is not.
                if (value < 0 || value > MAX_STAMINA)
                    throw new ArgumentOutOfRangeException($"the stamina value must be between 0 and " + MAX_STAMINA);
                //or just changes the value.
                stamina = value;
            }
        }
        
        //The level requirment of the item and it's public property
        private uint requirement;
        public uint Requirement
        {
            //simple get method for the requirement
            get => requirement;
            set
            {
                //checks to make sure that the requirement level is between 0 and the max level and throws an exception if it is not
                if (value < 0 || value > MAX_LEVEL)
                    throw new ArgumentOutOfRangeException($"The level requirement must be between 0 and " + MAX_LEVEL);
                //or changes the value of requirement.
                requirement = value;
            }
        }

        //The flavor text and it's public property
        private String flavor;
        public String Flavor
        {
            //very simple set and get methods for this property
            get => flavor;
            set => flavor = value;
        }
       //the override for ToString that just prints out all of the item stats.
        public override String ToString()
        {
            return "id: " + Id + "\nname: " + Name + "\ntype: " + Type + "\nilvl: " + Ilvl + "\nprimary: " + Primary + "\nstamina: " + Stamina + "\nrequirement: " + Requirement + "\nflavor text: " + Flavor;
        }
        //The default constructor that just sets everything to 0 or null and adds one to Count
        //side note both of the constructors use the properties to access everything minus id
        public Item()
        {
            id = 0;
            Name = "";
            Type = 0;
            Ilvl = 0;
            Primary = 0;
            Stamina = 0;
            Requirement = 0;
            Flavor = "";
            Count++;
        }

        //Constructor for when you want to set all the item class members and then adds one to Count
        public Item(uint nid, String nname, ItemType ntype, uint nilvl, uint nprimary, uint nstamina, uint nreq, String nflavor)
        {
            id = nid;
            Name = nname;
            Type = ntype;
            Ilvl = nilvl;
            Primary = nprimary;
            Stamina = nstamina;
            Requirement = nreq;
            Flavor = nflavor;
            Count++;
        }

        //IComparable Interface Compare to override
        public int CompareTo(object obj)
        {
            //checks if the second item is null and then returns 500 simply for debugging purposes it will still get caught because it is greater than 0
            if (obj == null) return 500;
            //sets otheritem equal to the passed in item typecasted as an item
            Item otherItem = obj as Item;
            //checks if the typecasting failed if it did then it throws an exception and if not it just compares the names.
            if (otherItem != null)
                return this.Name.CompareTo(otherItem.Name);
            else
                throw new ArgumentException("Object is not an Item");
        }

        //generic testing main
        static void Main(string[] args)
        {
            Item test = new Item();
            Console.WriteLine(test);
            Item test2 = new Item(0005, "Mjollnir", ItemType.Trinket, 50, 100, 50, 50, "This is some flavorfull text");
            Console.WriteLine(test2);
            Console.WriteLine(test2.CompareTo(test));
        }
    }
}
