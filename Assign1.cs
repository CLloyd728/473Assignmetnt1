using System;
using System.Collections.Generic;
using System.IO;
/*
 Cameron Lloyd, Bradley Graves
 z1853137, z1853328
 Assignment 1
 CSCI 473
 This is a program to a test functionality for Players and Items in the gane 'World of ConflictCraft'
 */

namespace Assign1
{
    public static class Assign1
    {
        /*
         * Enum definitions
         */
        public enum ItemType
        {
            Helmet, Neck, Shoulders, Back, Chest,
            Wrist, Gloves, Belt, Pants, Boots,
            Ring, Trinket
        };

        public enum Race { Orc, Troll, Tauren, Forsaken };

        /*
         * Global variables
         */
        private static uint MAX_ILVL = 360;
        private static uint MAX_PRIMARY = 200;
        private static uint MAX_STAMINA = 275;
        private static uint MAX_LEVEL = 60;
        private static uint GEAR_SLOTS = 14;
        private static uint MAX_INVENTORY_SIZE = 20;

        /*
         * Storage for Guilds, Items, and Players
         */
        private static Dictionary<uint, string> Guilds = new Dictionary<uint, string>();
        private static Dictionary<uint, Item> Items = new Dictionary<uint, Item>();
        private static Dictionary<uint, Player> Players = new Dictionary<uint, Player>();

        /*
         * Input file paths
         *
         * You Might have to alter the path if you have the input files located in a different folder than I do.
         * Mine are in the same folder where the Assign1.cs file is located in VS.
         */
        private static string guildsFile = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName) + "\\guilds.txt";
        private static string itemsFile = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName) + "\\equipment.txt";
        private static string playersFile = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName) + "\\players.txt";


        /*
         *  This is the definition of the Main class which contains the driver program for testing Player and Item Classes
         */
        public static void Main(string[] args)
        {
            //Load data from input files
            LoadData();

            //prints initial greeting
            Console.WriteLine("Welcome to the World of ConflictCraft: Testing Enviorment!\n");

            //the case key or the variable that the user input that the switch will run on is stored.
            String casekey = "0";
            do
            {
                //prints the menu everytime the user does something in the menu.
                Console.WriteLine("\nWelcome to the World of ConflictCraft: Testing Enviorment. Please select an option from the list below.");
                Console.WriteLine("1.) Print all players");
                Console.WriteLine("2.) Print all guilds");
                Console.WriteLine("3.) Print all gear");
                Console.WriteLine("4.) Print gear list for player");
                Console.WriteLine("5.) Leave guild");
                Console.WriteLine("6.) Join guild");
                Console.WriteLine("7.) Equip gear");
                Console.WriteLine("8.) Unequip gear");
                Console.WriteLine("9.) Award experience");
                Console.WriteLine("10.) Quit");
                casekey = Console.ReadLine();
                //the switch for all the menu options will be.
                switch (casekey)
                {
                    //prints the player list
                    case "1":
                        foreach (KeyValuePair<uint, Player> pair in Players)
                            Console.WriteLine(pair.Value);
                        break;

                    //prints the guild list
                    case "2":
                        foreach (KeyValuePair<uint, string> pair in Guilds)
                            Console.WriteLine(pair.Key + " " + pair.Value);
                        break;

                    //prints the item list
                    case "3":
                        foreach (KeyValuePair<uint, Item> pair in Items)
                            Console.WriteLine(pair.Value);
                        break;

                    //prints a specified players gearlist
                    case "4":
                        //finds the key of the player whose name the user will provide
                        uint key4p = FindPlayer(Players);
                        //if player is not found it exits this run of the loop
                        if (key4p == 2147483647)
                            break;
                        //if the player is found it prints all the gear that the player has equiped
                        for (int i = 0; i < GEAR_SLOTS; i++)
                            if (Players[key4p][i] != 0)
                                Console.WriteLine(Items[(Players[key4p])[i]]);
                        break;

                    //makes sepcified player leave their guild
                    case "5":
                        uint key5p = FindPlayer(Players);
                        if (key5p == 2147483647)
                            break;
                        if (Players[key5p].GuildID == 0)
                        {
                            Console.WriteLine(Players[key5p].Name + " is not in a guild and thus cannot leave one.");
                            break;
                        }
                        Players[key5p].GuildID = 0;
                        Console.WriteLine(Players[key5p].Name + " has left their guild.");
                        break;

                    //makes specified player join specified guild.
                    case "6":
                        //finds the key of the player in the player list
                        uint key6p = FindPlayer(Players);
                        //if the player is not found it exits
                        if (key6p == 2147483647)
                            break;
                        //if the guild you want to join doesn't exist it exits
                        uint key6g = FindGuild(Guilds);
                        if (key6g == 2147483647)
                            break;
                        //otherwise it the player joins the guild and it prints a message letting you know that.
                        Players[key6p].GuildID = key6g;
                        Console.WriteLine(Players[key6p].Name + "just joined the guild " + Guilds[key6g]);
                        break;

                    //Equips gear
                    case "7":
                        
                        //finds the key of the player name entered 
                        uint key7p = FindPlayer(Players);
                        //if player name invalid, break
                        if (key7p == 2147483647)
                            break;

                        //finds the key of the item name entered
                        uint key7i = FindItem(Items);
                        //if item name invalid, break
                        if (key7i == 2147483647)
                            break;

                        //equip the gear if player and item exist
                        Players[key7p].EquipGear(key7i);
                        break;

                    //Unequip gear
                    case "8":
                        //finds the key of the player name entered 
                        uint key8p = FindPlayer(Players);
                        //if player name invalid, break
                        if (key8p == 2147483647)
                            break;
                        //get the slot type to unequip
                        Console.WriteLine("\nEnter what item slot you would like to remove.");
                        String input = Console.ReadLine();

                        //check for invalid slot type
                        if (!Enum.IsDefined(typeof(ItemType), input))
						{
                            Console.WriteLine("\nInvalid item type.");
                            break;
						}

                        int slot = (int) Enum.Parse(typeof(ItemType), input);
                        Players[key8p].UnequipGear(slot);
                        break;

                    //Award experience
                    case "9":
                        

                    case "11":
                        break;

                    default:
                        if (casekey == "10" || casekey == "q" || casekey == "Q" || casekey == "quit" || casekey == "Quit" || casekey == "exit" || casekey == "Exit")
                            break;
                        //else
                        Console.WriteLine("You entered an invalid option please try again.");
                        break;
                }

            } while (casekey != "10" && casekey != "q" && casekey != "Q" && casekey != "quit" && casekey != "Quit" && casekey != "exit" && casekey != "Exit");
        }

		//finds the player specified by the user
		public static uint FindPlayer(Dictionary<uint, Player> Players)
		{
            //asks the user for the name of the player
            Console.WriteLine("\nPlease enter the name of the Player.");
            String playername = Console.ReadLine();
            uint key = 2147483647;
            //checks through the player list for someone by that name
            foreach (KeyValuePair<uint, Player> pair in Players)
            {
                if ((pair.Value).Name == playername)
                    key = pair.Key;
            }
            //if it doesn't exist it returns the error code I decided on because it's the max 32 bit int
            //I also don't throw exceptions here just so you can keep trying/using the menu rather than 
            //having to relaunch.
            if (key == 2147483647)
            {
                Console.WriteLine("Player under that name not found.");
                return key;
            }
            //returns the key of the related player if they exist.
            return key;
        }

        //finds the guild specified by the user
        public static uint FindGuild(Dictionary<uint, String> Guilds)
        {
            //asks the user for the guild name
            Console.WriteLine("\nPlease enter the name of the Guild you would like to join/leave.");
            String guildname = Console.ReadLine();
            uint key = 2147483647;
            //searches the guild list for a guild by name
            foreach (KeyValuePair<uint, String> pair in Guilds)
            {
                if (pair.Value == guildname)
                    key = pair.Key;
            }
            //returns and says that the guild couldn't be found.
            if (key == 2147483647)
            {
                Console.WriteLine("Guild under that name not found.");
                return key;
            }
            //returns the guild key
            return key;
        }

        //find the item specified by the user
        public static uint FindItem(Dictionary<uint, Item> Items)
        {
            //asks the user for the guild name
            Console.WriteLine("\nPlease enter the name of the Item you would like to equipt.");
            String itemName = Console.ReadLine();
            uint key = 2147483647;
            //searches the guild list for a guild by name
            foreach (KeyValuePair<uint, Item> pair in Items)
            {
                if (pair.Value.Name == itemName)
                    key = pair.Key;
            }
            //returns and says that the item couldn't be found.
            if (key == 2147483647)
            {
                Console.WriteLine("Item under that name not found.");
                return key;
            }
            //returns the item key
            return key;
        }

        /*
         * Method to load data from input files
         */
        public static void LoadData()
		{
            //Read in data from Guilds file
            var lines = File.ReadLines(guildsFile);
            foreach (var line in lines)
            {
                //Seperate on tabs and add to dict of Guilds
                string[] s = line.Split('\t');
                Guilds.Add((Convert.ToUInt32(s[0])), s[1]);
            }

            //Read in data from Items file
            lines = File.ReadLines(itemsFile);
            foreach (var line in lines)
            {
                //Seperate on tabs and add to dict of Items
                string[] s = line.Split('\t');
                Item item = new Item(Convert.ToUInt32(s[0]), s[1], (ItemType)Convert.ToUInt32(s[2]), Convert.ToUInt32(s[3]), Convert.ToUInt32(s[4]), Convert.ToUInt32(s[5]),
                    Convert.ToUInt32(s[6]), s[7]);
                Items.Add(item.Id, item);
            }

            //Read in data from Players file
            lines = File.ReadLines(playersFile);
            foreach (var line in lines)
            {
                //Seperate on tabs and add to dict of Players
                string[] s = line.Split('\t');

                // Build the Gear array for the Player
                uint[] ar = new uint[MAX_INVENTORY_SIZE];
                for (int i = 0, x = 6; i < (MAX_INVENTORY_SIZE - 6); i++, x++)
                {
                    ar[i] = Convert.ToUInt32(s[x]);
                }
                Player player = new Player(Convert.ToUInt32(s[0]), s[1], (Race)Convert.ToUInt32(s[2]), Convert.ToUInt32(s[3]),
                        Convert.ToUInt32(s[4]), Convert.ToUInt32(s[5]), ar);
                Players.Add(player.Id, player);
            }
        }

        /*
         *  This is the definition of the Player class along with an Icomparable interface
         */
        public class Player : IComparable
        {
            /*
            * Player Attributes
            */
            private uint _id;
            private string _name;
            private Race _race;
            private uint _level;
            private uint _exp;
            private uint _guildID;
            private uint[] _gear;
            private List<uint> _inventory;
            bool firstRingNext = true;
            bool firstTrinkNext = true;

            /*
             * Default Constructor for Player Class
             */
            public Player()
            {
                _id = 0;
                _name = "";
                _race = 0;
                _level = 0;
                _exp = 0;
                _guildID = 0;
                _gear = new uint[GEAR_SLOTS];
                _inventory = new List<uint>();
            }

            /*
             * Custom Constructor for Player Class
             */
            public Player(uint id, string name, Race race, uint level, uint exp, uint guildID, uint[] gear)
            {
                _id = id;
                _name = name;
                _race = race;
                _level = level;
                _exp = exp;
                _guildID = guildID;
                _gear = new uint[GEAR_SLOTS];
                //If passed in gear arrray is not empty, copy it into Player gear array
                if (gear != null)
                    Array.Copy(gear, gear.GetLowerBound(0), _gear, _gear.GetLowerBound(0), GEAR_SLOTS);
                _inventory = new List<uint>();
            }

            /*
             * Id Property
             */
            public uint Id
            {
                get => _id;
            }

            /*
             * Name Property
             */
            public string Name
            {
                get => _name;
            }

            /*
             * Player type Property
             */
            public Race Race
            {
                get => _race;

            }

            /*
             * Level property
             */
            public uint Level
            {
                get => _level;
                set
                {
                    if (value < 0 || value > MAX_LEVEL)
                        throw new ArgumentOutOfRangeException($"Player level must be between 1 and " + MAX_LEVEL);
                    else
                        _level = value;
                }
            }

            /*
             * Experience property
             */
            public uint Exp
            {
                get => _exp;
                set
                {
                    //Add assigned exp value to Players exp value
                    _exp += value;

                    //Check to see if new exp amount is over threshhold to level up
                    if (_level < MAX_LEVEL)
                        LevelUp(_exp);
                }
            }
            //Guild ID Property
            public uint GuildID
            {
                get => _guildID;
                set => _guildID = value;
            }

            /*
             * Method to handle leveling up a player
             */
            public void LevelUp(uint exp)
            {
                //Calculate required xp for level up
                uint expRequired = _level * 1000;

                while (exp >= expRequired && _level < MAX_LEVEL)
                {
                    if (_level < MAX_LEVEL)
                        _level++;
                    expRequired = _level * 1000;
                }
            }

            /*
             * Method to equip gear to players 
             */ 
            public void EquipGear(uint newGearID)
			{                
                //if item and player exist, equipt item id to corresponding slot in player gear array
                int itype = (int)Items[newGearID].Type;

                //make sure player meets required level for item
                if (this.Level < Items[newGearID].Requirement)
                {
                    //console message instead of exception so that level requirements don't break program and allow user to try again
                    Console.WriteLine("\n" + this.Name + " does not meet required level for " + Items[newGearID].Name + ".\n");
                    return;
                }

                //check if item is ring or trinket, if so then special case
                if ((itype == 10) || (itype == 11))
				{
                    switch (itype)
                    {
                        case 10:
                            if (firstRingNext && this._gear[itype] == 0)
                            {
                                this._gear[itype] = newGearID;
                                firstRingNext = false;
                            }
                            else
                            {
                                this._gear[itype + 1] = newGearID;
                                firstRingNext = true;
                            }
                            break;

                        case 11:
                            if (firstTrinkNext && this._gear[itype] == 0)
                            {
                                this._gear[itype] = newGearID;
                                firstTrinkNext = false;
                            }
                            else
                            {
                                this._gear[itype + 1] = newGearID;
                                firstTrinkNext = true;
                            }
                            break;
                    }
                }

                //equip to only slot otherwise
                this._gear[itype] = newGearID;
                Console.WriteLine("\n" + this.Name + " is now equipped with " + Items[newGearID].Name);
            }

            public void UnequipGear(int gearSlot)
			{
                //Special case
                if (gearSlot == 10 || gearSlot == 11)
                {
                    if (this[gearSlot] == 0 && this[gearSlot + 1] == 0)
                        Console.WriteLine("\nThere was no item in that slot. Nothing changed.");
                    else
                    {
                        if (this[gearSlot] == 0)
                        {
                            Console.WriteLine("\n" + Items[this._gear[gearSlot + 1]].Name + " was removed from player and added to inventory");
                            this[gearSlot + 1] = 0;
                        }

                        else
                        {
                            Console.WriteLine("\n" + Items[this._gear[gearSlot]].Name + " was removed from player and added to inventory");
                            this[gearSlot] = 0;
                        }
                    }
                }
                //Non special case
                else
                {                    
                    if (this[gearSlot] == 0)
                        Console.WriteLine("\nThere was no item in that slot. Nothing changed.");
                    else
                    {
                        Console.WriteLine("\n" + Items[this._gear[gearSlot]].Name + " was removed from player and added to inventory");
                        this[gearSlot] = 0;
                    }
                }
            }


            /*
             * Indexer for the Players Gear array
             */
            public uint this[int i]
            {
                get { return _gear[i]; }
                set { _gear[i] = value; }
            }

            /*
             * IComparable interface for the Player Class
             */
            public int CompareTo(object obj)
            {
                //Make sure object not null
                if (obj == null)
                    return 1;

                //Compare by name if valid Player object
                if (obj is Player toCompare)
                    return this.Name.CompareTo(toCompare.Name);
                else
                    throw new ArgumentException("Object is not a Player");
            }

            /*
             * toString method for the Player class
             */
            public override String ToString()
            {
                string message = "name: " + _name + "\nrace: " + _race + "\nlevel: " + _level;
                return _guildID == 0 ? message + "\n" : message + "\nguild: " + Guilds[_guildID] + "\n";
            }
        }

        /*
         *  This is the definition of the Item class along with an Icomparable interface
         */
        public class Item : IComparable
        {
            //The Count variable so that we can keep track of how many items there are
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
                return "id: " + Id + "\nname: " + Name + "\ntype: " + Type + "\nilvl: " + Ilvl + "\nprimary: " + Primary + "\nstamina: " + Stamina + "\nrequirement: " + Requirement + "\nflavor text: " + Flavor + "\n";
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
        }
    }
}
