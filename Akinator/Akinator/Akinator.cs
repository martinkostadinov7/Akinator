using System;
using System.IO;
namespace Akinator
{
    internal class Akinator
    {
        static List<string> memory;
        static Tree tree;
        const string memoryPath = "Memory.txt";
        static void Main(string[] args)
        {
            try
            {
                StartMenu();
                ReadMemory(memoryPath);

                tree = new Tree(null);
                int index = 0;
                tree.Root = CreateTree(null, ref index);

                while (true)
                {
                    Play(tree.Root);
                    Console.WriteLine();
                    Console.WriteLine("Play again?");
                    int selection = ChooseOptions("Play Again?");

                    if (selection == 0)
                    {
                        StartMenu();
                        Play(tree.Root);
                    }
                    else
                    {
                        break;
                    }

                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //read from file
        private static void ReadMemory(string path)
        {
            memory = new List<string>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    memory.Add(line);
                }
            }
        }

        private static Node CreateTree(Node Parent, ref int index)
        {
            if (index >= memory.Count)
            {
                return null;
            }

            string line = memory[index].Trim();
            index++;

            Node currentNode = new Node(line);
            currentNode.Parent = Parent;

            if (currentNode.IsQuestion)
            {
                // If it's a question, add children recursively
                currentNode.False = CreateTree(currentNode, ref index); // "No"
                currentNode.True = CreateTree(currentNode, ref index);  // "Yes"
            }

            return currentNode;
        }

        //play the game, using the tree
        private static void Play(Node node)
        {
            int answer;

            if (node.IsQuestion)
            {
                //asking questions
                Console.WriteLine(node.Value); // node.Value is the actual question
                answer = ChooseOptions(node.Value);

                if (answer == 0) // yes
                {
                    Play(node.True);
                }
                else if (answer == 1) // no
                {
                    Play(node.False);
                }
                else if (answer == 2) // return
                {
                    if (node.Parent != null)
                    {
                        Play(node.Parent);
                    }
                    else
                    {
                        StartMenu();
                        Play(node);
                    }
                }
            }
            else
            {
                Console.WriteLine("Is it a/an " + node.Value + "?");

                answer = ChooseOptions("Is it a/an " + node.Value + "?");

                if (answer == 0) // yes
                {
                    Console.WriteLine("Guessed!");
                    return;
                }
                else if (answer == 1) // no
                {
                    //ask for the animal
                    Console.Clear();
                    Console.WriteLine("Which was the animal?");
                    string animal = Console.ReadLine();

                    //ask for a question
                    Console.WriteLine($"Give me a question, the answer to which is true for your animal and false for {node.Value}");
                    string newQuestion = Console.ReadLine();
                    if (!newQuestion.EndsWith("?"))
                    {
                        newQuestion += "?";
                    }

                    //insert the new info in the binary tree
                    node.False = new Node(node.Value);
                    node.Value = newQuestion;
                    node.True = new Node(animal);


                    //update the file and end the game
                    UpdateMemory();
                    return;
                }
                else if (answer == 2) // return
                {
                    Play(node.Parent);
                }
            }

        }
        private static void UpdateMemory()
        {
            using (StreamWriter writer = new StreamWriter(memoryPath, false))
            {
                UpdateMemory(tree.Root, "", writer);
            }
        }
        private static void UpdateMemory(Node root, string spaces, StreamWriter writer)
        {
            if (root == null)
            {
                return;
            }
            writer.WriteLine(spaces + root.Value);
            UpdateMemory(root.False, spaces + "  ", writer);
            UpdateMemory(root.True, spaces + "  ", writer);
        }

        private static int ChooseOptions(string question)
        {
            Console.WriteLine(startSelection);
            int selection = -1;

            while (true)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();

                Console.Clear();
                Console.WriteLine(question);

                if (keyPressed.Key == ConsoleKey.LeftArrow) // <
                {
                    if (selection <= 0)
                    {
                        selection = 2;
                    }
                    else selection--;
                }
                else if (keyPressed.Key == ConsoleKey.RightArrow) // >
                {
                    selection++;
                }
                else if ((keyPressed.Key == ConsoleKey.Enter || keyPressed.Key == ConsoleKey.Spacebar) && selection != -1) // enter or spacebar
                {
                    Console.Clear();
                    return Math.Abs(selection % 3);
                }
                else //wrong input
                {
                    if (selection == -1)
                    {
                        Console.WriteLine(startSelection);
                    }
                }

                // print selection
                if (selection % 3 == 0)
                {
                    Console.WriteLine(yesSelected);
                }
                else if (selection % 3 == 1)
                {
                    Console.WriteLine(noSelected);
                }
                else if (selection % 3 == 2)
                {
                    Console.WriteLine(returnSelected);
                }
            }
        }

        static string startSelection = @"
|-----|  |------|  |--------|
| YES |  |  NO  |  | RETURN |
|_____|  |______|  |________|
";
        static string yesSelected = @"
███████   |------|  |--------|
█ YES █   |  NO  |  | RETURN |
███████   |______|  |________|
";
        static string noSelected = @"
|-----|   ████████  |--------|
| YES |   █  NO  █  | RETURN |
|_____|   ████████  |________|
";
        static string returnSelected = @"
|-----|   |------|  ██████████
| YES |   |  NO  |  █ RETURN █
|_____|   |______|  ██████████
";

        private static void StartMenu()
        {
            Console.WriteLine(@"                                        
                   @@                                      
                    @@@@ =@:-=-=#%@@@@.                    
                       @          *@ .@:@                  
                    +@   ++#=:@@@@ -+. *@                  
                . :@          @@  .+#@  .*                 
                  @         %:  .:      @+                 
                  @       @-.   ..:+@%*@                   
                  @     @@  @@@@@@@-%@@                    
                  .@@@@%@@    @    .@.%                    
                   @ @.- @ :=.. .+  %=%             █████  ██   ██ ██ ███    ██  █████  ████████  ██████  ██████ 
                     *@@.  -+ @@ . +-@%@           ██   ██ ██  ██  ██ ████   ██ ██   ██    ██    ██    ██ ██   ██
                        @@ :*. :#@@@@@@:           ███████ █████   ██ ██ ██  ██ ███████    ██    ██    ██ ██████ 
                            +-=.     -@@           ██   ██ ██  ██  ██ ██  ██ ██ ██   ██    ██    ██    ██ ██   ██
                        @@:   .   @@@@             ██   ██ ██   ██ ██ ██   ████ ██   ██    ██     ██████  ██   ██
                         +@%=%##%@ @@                      
                      =:@@*=@.@:+@@@@@#@                Think of an animal and i will guess it!
                   @@@@@=*@@. @   +%..=@@@             
                @@@@****+=+@@@.#..:@##%@#@@                
               .@#***@:#***+#@@@.  ..#=#..@@               
               @+****=@--:*++=*. @@@@@@@%.--@@             
               @##***=*@@=-:=*@%         .=.   @@.         
                @@@***+=.@@@@@-+@@@@@@=...:#+%: @@@@       
                 +@@++**-.@@+=@@-+  @@@@@@- ..:.-*@@       
                   @@@*=#@=@-++#@#@ :.%#:@-@@ +#%@@        
                     @@*=.=#-+=--#%.=:.=@=@#@.@%@.         
                       @@.::-*%%%@@.=--.@*@#@ %@#          
                        @@@%@%@%#@ .-:::@@ =@ @@          
                         @%*%+#%%@@....##@  @@@            
                          @@@-+===#@@@%*%@                 
                           +@#=****+*+**@                  
                             @@#******-@                   
          @@@@@               @@@****-%@                   
        @@@@@@@@@@@             @@**+#@                    
         @@      @@@@@           @*+%@                     
          @        @@@@@-       @%=@@                      
           @         @@@%@@@@@@@#-@@                       
         .             @@@@*%@@@@@@                        
                         #@@@@.                                     Press any key to continue
");

            Console.ReadKey();
            Console.Clear();
        }
    }
}
