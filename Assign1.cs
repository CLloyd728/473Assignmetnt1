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
            Console.WriteLine("Welcome to the World of ConflictCraft: Testing Environment!");

            //the case key or the variable that the user input that the switch will run on is stored.
            String casekey = "0";
            do
            {
                //prints the menu everytime the user does something in the menu.
                Console.WriteLine("\nWelcome to the World of ConflictCraft: Testing Environment. Please select an option from the list below:");
                Console.WriteLine("\t1.) Print all players");
                Console.WriteLine("\t2.) Print all guilds");
                Console.WriteLine("\t3.) Print all gear");
                Console.WriteLine("\t4.) Print gear list for player");
                Console.WriteLine("\t5.) Leave guild");
                Console.WriteLine("\t6.) Join guild");
                Console.WriteLine("\t7.) Equip gear");
                Console.WriteLine("\t8.) Unequip gear");
                Console.WriteLine("\t9.) Award experience");
                Console.WriteLine("\t10.) Quit");
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
                            Console.WriteLine(pair.Value);
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
                        Console.WriteLine(Players[key4p]);
                        for (uint i = 0; i < GEAR_SLOTS; i++)
                            if (Players[key4p][i] != 0)
                                Console.WriteLine(Items[(Players[key4p])[i]]);

                        break;

                    //makes sepcified player leave their guild
                    case "5":
                        //finds the player specified by the user
                        uint key5p = FindPlayer(Players);
                        //if player not found it breaks out of the case.
                        if (key5p == 2147483647)
                            break;
                        //looks for the guild specified by the user
                        if (Players[key5p].GuildID == 0)
                        {
                            //if the player isn't in a guild it tells you that it can't leave a guild
                            Console.WriteLine(Players[key5p].Name + " is not in a guild and thus cannot leave one.");
                            break;
                        }
                        //prints a message if leaving the guild was successfull
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
                        Console.WriteLine("\n" + Players[key6p].Name + " just joined " + Guilds[key6g] + "!");
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
                        Console.WriteLine("Enter what item slot you would like to remove:\n\tHelmet\n\tNeck\n\tShoulders\n\tBack\n\tChest" +
                                            "\n\tWrist\n\tGloves\n\tBelt\n\tPants\n\tBoots\n\tRing\n\tTrinket");
                        String slotType = Console.ReadLine();

                        //check for invalid slot type
                        if (!Enum.IsDefined(typeof(ItemType), slotType))
                        {
                            Console.WriteLine("\nInvalid item type.");
                            break;
                        }

                        uint slot = (uint)(ItemType)Enum.Parse(typeof(ItemType), slotType);
                        Players[key8p].UnequipGear(slot);
                        break;

                    //Award experience
                    case "9":
                        //Flag to terminate program with bad input
                        bool isValidExp = true;

                        //Find the key of player name entered
                        uint key9p = FindPlayer(Players);

                        //If player name invalid, break
                        if (key9p == 2147483647)
                            break;

                        //Get amount from user and check for valid number
                        Console.Write("Please enter the amount of experience to award: ");
                        String expAmt = Console.ReadLine();
                        for (int i = 0; i < expAmt.Length; i++)
                        {
                            if (!Char.IsDigit(expAmt[i]))
                            {
                                isValidExp = false;
                                break;
                            }
                        }

                        //Terminate if bad input
                        if (!isValidExp)
                        {
                            Console.WriteLine("\nInvalid input.");
                            break;
                        }

                        //Terminate if player is max level
                        if (Players[key9p].Level == MAX_LEVEL)
                            break;

                        //Award entered amount of experience
                        Players[key9p].LevelUp(UInt32.Parse(expAmt));
                        Console.WriteLine();
                        break;

                    case "T":
                        //creates a new sortedset and then adds all the items to it
                        SortedSet<Item> sortedItems = new SortedSet<Item>();
                        foreach (KeyValuePair<uint, Item> pair in Items)
                            sortedItems.Add(pair.Value);
                        //prints out all the items in the sorted set
                        foreach (Item value in sortedItems)
                            Console.WriteLine(value);
                        //creates a new sortedset and then adds all the players to it
                        SortedSet<Player> sortedPlayers = new SortedSet<Player>();
                        foreach (KeyValuePair<uint, Player> pair in Players)
                            sortedPlayers.Add(pair.Value);
                        //prints all the players
                        foreach (Player value in sortedPlayers)
                            Console.WriteLine(value);
                        break;

                    default:
                        if (casekey == "10" || casekey == "q" || casekey == "Q" || casekey == "quit" || casekey == "Quit" || casekey == "exit" || casekey == "Exit")
                            break;
                        //else
                        Console.WriteLine("\nYou entered an invalid option please try again.");
                        break;
                }

            } while (casekey != "10" && casekey != "q" && casekey != "Q" && casekey != "quit" && casekey != "Quit" && casekey != "exit" && casekey != "Exit");
        }

        //finds the player specified by the user
        public static uint FindPlayer(Dictionary<uint, Player> Players)
        {
            //asks the user for the name of the player
            Console.Write("\nPlease enter player name: ");
            String playername = Console.ReadLine();
            Console.WriteLine();
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
                Console.WriteLine("\nPlayer under that name not found.");
                return key;
            }
            //returns the key of the related player if they exist.
            return key;
        }

        //finds the guild specified by the user
        public static uint FindGuild(Dictionary<uint, String> Guilds)
        {
            //asks the user for the guild name
            Console.Write("Please enter the name of the Guild you would like to join/leave: ");
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
                Console.WriteLine("\nGuild under that name not found.");
                return key;
            }
            //returns the guild key
            return key;
        }

        //find the item specified by the user
        public static uint FindItem(Dictionary<uint, Item> Items)
        {
            //asks the user for the item name
            Console.WriteLine("\nPlease enter the name of the Item you would like to equip.");
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
                Console.WriteLine("\nItem under that name not found.");
                return key;
            }
            //returns the item key
            return key;
        }

        /*
         * Method to read and load data from input files
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
                    {
                        //Increment level and subtract required amount from awarded amount
                        _level++;
                        Console.Write("\n" + this.Name + " has reached level " + _level + "!");
                        exp -= expRequired;
                    }
                    //Calculate new required amount for next iteration
                    expRequired = _level * 1000;
                }                
            }

            /*
             * Method to equip (add) gear to players 
             */
            public void EquipGear(uint newGearID)
            {
                //if item and player exist, equip item id to corresponding slot in player gear array
                uint itype = (uint)Items[newGearID].Type;

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
                            if (firstRingNext || this._gear[itype] == 0)
                            {
                                if (this._gear[itype] != 0)
                                    this._inventory.Add(this._gear[itype]);
                                this._gear[itype] = newGearID;
                                firstRingNext = false;
                                break;
                            }
                            else
                            {
                                if (this._gear[itype + 1] != 0)
                                    this._inventory.Add(this._gear[itype + 1]);
                                this._gear[itype + 1] = newGearID;
                                firstRingNext = true;
                                break;

                            }

                        case 11:
                            if (firstTrinkNext || this._gear[itype + 1] == 0)
                            {
                                if (this._gear[itype + 1] != 0)
                                    this._inventory.Add(this._gear[itype + 1]);
                                this._gear[itype + 1] = newGearID;
                                firstTrinkNext = false;
                                break;
                            }
                            else
                            {
                                if (this._gear[itype + 2] != 0)
                                    this._inventory.Add(this._gear[itype + 2]);
                                this._gear[itype + 2] = newGearID;
                                firstTrinkNext = true;
                                break;
                            }
                    }
                }
                //Add to only slot if not special case
                else
                {
                    if (this._gear[itype] != 0)
                        this._inventory.Add(this._gear[itype]);
                    this._gear[itype] = newGearID;
                }
                Console.WriteLine("\n" + this.Name + " is now equipped with " + Items[newGearID].Name + "!");
            }

            /*
             * Method to unequip (remove) gear from players
             */
            public void UnequipGear(uint gearSlot)
            {
                //Special case (ring)
                if (gearSlot == 10)
                {
                    //Check if ring 1 is empty
                    if (this[gearSlot] == 0)
                    {
                        //Check if ring 2 is empty, if both empty do nothing
                        if (this[gearSlot + 1] == 0)
                            Console.WriteLine("There was nothing in that slot. Nothing has changed.");

                        //If trinket 1 empty, unequip trinket 2
                        else
                        {
                            this._inventory.Add(this[gearSlot + 1]);
                            Console.WriteLine("\n" + Items[this._gear[gearSlot + 1]].Name + " was removed from player and added to inventory");
                            this[gearSlot + 1] = 0;
                        }
                    }

                    //No special case
                    else
                    {
                        this._inventory.Add(this[gearSlot]);
                        Console.WriteLine("\n" + Items[this._gear[gearSlot]].Name + " was removed from player and added to inventory");
                        this[gearSlot] = 0;
                    }
                }

                //Special case (trinket)
                else if (gearSlot == 11)
                {
                    //Check if trinket 1 is empty
                    if (this[gearSlot + 1] == 0)
                    {
                        //Check if trinket 2 is empty, if both empty do nothing
                        if (this[gearSlot + 2] == 0)
                            Console.WriteLine("There was nothing in that slot. Nothing has changed.");

                        //If trinket 1 empty, uneqip trinket 2
                        else
                        {
                            this._inventory.Add(this[gearSlot + 2]);
                            Console.WriteLine("\n" + Items[this._gear[gearSlot + 2]].Name + " was removed from player and added to inventory");
                            this[gearSlot + 2] = 0;
                        }
                    }

                    //If trinket 1 not empty, unequip trinket 1
                    else
                    {
                        this._inventory.Add(this[gearSlot + 1]);
                        Console.WriteLine("\n" + Items[this._gear[gearSlot + 1]].Name + " was removed from player and added to inventory");
                        this[gearSlot + 1] = 0;
                    }
                }

                //No special case
                else
                {
                    //Unequip item 
                    if (this[gearSlot] != 0)
                        this._inventory.Add(this[gearSlot]);
                    Console.WriteLine("\n" + Items[this._gear[gearSlot]].Name + " was removed from player and added to inventory");
                    this[gearSlot] = 0;
                }
            }

            /*
             * Indexer for the Players Gear array
             */
            public uint this[uint i]
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
                string message = "Name: " + String.Format("{0,-12}", _name) + "\tRace: " + _race + "\tLevel: " + _level;
                return _guildID == 0 ? message  : message + "\tGuild: " + Guilds[_guildID];
                
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
                return "(" + Type + ") " + Name + " |" + Ilvl + "| " + "--" + Requirement + "--" + "\n\t" + "\"" + Flavor + "\"";
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


