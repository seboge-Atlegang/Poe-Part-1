using System;
using System.Media;
using System.Threading;
using System.IO;

namespace Poe_Part_1

{

    internal class Program
    {
        private static ConsoleColor titleColor = ConsoleColor.Cyan;
        private static ConsoleColor botColor = ConsoleColor.Green;
        private static ConsoleColor userColor = ConsoleColor.Yellow;
        private static ConsoleColor warningColor = ConsoleColor.Red;
        private static ConsoleColor infoColor = ConsoleColor.Blue;
        static void Main(string[] args)
        {
            // Initialize the ai chatbot
            InitializeChatbot();

            // used to get the users name and reuse it
            string userName = GetUserName();

            // Main conversation loop
            ChatLoop(userName);
        }


        static void InitializeChatbot()
        {
            Console.Title = "Cybersecurity Awareness Assistant";

            // Play welcome sound (assuming you have a WAV file in the same directory)
            try
            {
                //sound audio recording 
                SoundPlayer player = new SoundPlayer("C:\\Users\\atleg_lptzay4\\OneDrive\\Desktop\\prog POE Part 1\\Poe Part 1\\audio\\Welcome.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                PrintWarning("Could not play welcome sound: " + ex.Message);
            }

            // Display ASCII art with colors
            DisplayAsciiArt();

            // Typing effect for welcome message
            TypeWriterEffect("Initializing Cybersecurity Awareness Assistant...", titleColor);
            Thread.Sleep(1000);
        }

        // the ascii art is displayed when the program starts
        static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
   _____              _    _____           _    
  / ____|            | |  / ____|         | |   
 | (___   _ __   ___ | |_| (___   ___  ___| | __
  \___ \ | '_ \ / _ \| __|\___ \ / _ \/ __| |/ /
  ____) || | | | (_) | |_ ____) |  __/ (__|   < 
 |_____/ |_| |_|\___/ \__|_____/ \___|\___|_|\_\
                                                
  _____                            _            
 / ____|                          | |           
| |     ___  _ __  _ __   ___  ___| |_ ___ _ __ 
| |    / _ \| '_ \| '_ \ / _ \/ __| __/ _ \ '__|
| |___| (_) | | | | | | |  __/\__ \ ||  __/ |   
 \_____\___/|_| |_|_| |_|\___||___/\__\___|_|   
");
            Console.ResetColor();
            Console.WriteLine();
        }

        static string GetUserName()
            {
                TypeWriterEffect("Hello! Welcome to the Cybersecurity Awareness Bot.", botColor);
                TypeWriterEffect("I'm here to help you stay safe online.", botColor);
                Console.WriteLine();

                string userName = "";
                while (string.IsNullOrWhiteSpace(userName))
                {
                    Console.ForegroundColor = userColor;
                    Console.Write("Before we begin, what's your name? ");
                    Console.ResetColor();

                    userName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        PrintWarning("Please enter a valid name.");
                    }
                }

                Console.WriteLine();
                TypeWriterEffect($"Nice to meet you, {userName}! Let's talk about cybersecurity.", botColor);
                Console.WriteLine();

                return userName;
            }

            static void ChatLoop(string userName)
            {
                bool continueChat = true;

                DisplayHelp();

                while (continueChat)
                {
                    Console.ForegroundColor = userColor;
                    Console.Write($"{userName}: ");
                    Console.ResetColor();

                    string input = Console.ReadLine();
                    Console.WriteLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        PrintWarning("Please enter a question or type 'help' for options.");
                        continue;
                    }

                    string response = GetResponse(input.ToLower(), userName);

                    TypeWriterEffect($"Bot: {response}", botColor);
                    Console.WriteLine();

                    // Check if user wants to exit
                    if (input.ToLower().Contains("bye") || input.ToLower().Contains("exit"))
                    {
                        continueChat = false;
                    }
                }

                // Farewell message
                TypeWriterEffect($"Thank you for using the Cybersecurity Awareness Bot, {userName}!", botColor);
                TypeWriterEffect("Stay safe online!", botColor);
                Thread.Sleep(2000);
            }

            static string GetResponse(string input, string userName)
            {
                // Basic greetings
                if (input.Contains("hello") || input.Contains("hi"))
                {
                    return $"Hello {userName}! How can I help you with cybersecurity today?";
                }
                if (input.Contains("how are you"))
                {
                    return "I'm a bot, so I don't have feelings, but I'm ready to help you with cybersecurity!";
                }

                // About the bot
                if (input.Contains("purpose") || input.Contains("what are you"))
                {
                    return "I'm here to educate South African citizens about cybersecurity threats like phishing, malware, and social engineering.";
                }
                if (input.Contains("what can i ask"))
                {
                    return "You can ask me about: \n- Password safety\n- Phishing emails\n- Safe browsing\n- Recognizing suspicious links\n- General cybersecurity tips";
                }

                // Cybersecurity topics
                if (input.Contains("password") || input.Contains("safe password"))
                {
                    return "Strong passwords should:\n- Be at least 12 characters long\n- Include uppercase, lowercase, numbers, and symbols\n- Not contain personal information\n- Be unique for each account\nConsider using a password manager!";
                }
                if (input.Contains("phishing") || input.Contains("scam email"))
                {
                    return "Phishing emails often:\n- Urge immediate action\n- Have spelling/grammar mistakes\n- Use suspicious sender addresses\n- Contain unexpected attachments\n- Ask for personal information\nNever click links in suspicious emails!";
                }
                if (input.Contains("safe browsing") || input.Contains("internet safety"))
                {
                    return "For safe browsing:\n- Look for 'https://' and padlock icon\n- Avoid public WiFi for sensitive transactions\n- Keep browser/OS updated\n- Use ad-blockers\n- Be cautious with downloads";
                }
                if (input.Contains("suspicious link") || input.Contains("dangerous link"))
                {
                    return "To check links:\n1. Hover to see actual URL\n2. Look for misspellings (like 'faceb00k.com')\n3. Check if the site has HTTPS\n4. Use online URL scanners if unsure\nWhen in doubt, don't click!";
                }

                // Help command
                if (input.Contains("help"))
                {
                    DisplayHelp();
                    return "Here are some topics you can ask about:";
                }

                // Default response for unknown queries
                 return "I didn't quite understand that. I can help with:\n- Password safety\n- Phishing emails\n- Safe browsing\nType 'help' for more options.";
            }

            static void DisplayHelp()
            {
                Console.ForegroundColor = infoColor;
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║           CYBERSECURITY HELP MENU          ║");
                Console.WriteLine("╠════════════════════════════════════════════╣");
                Console.WriteLine("║ You can ask about:                         ║");
                Console.WriteLine("║ - Password safety                          ║");
                Console.WriteLine("║ - Phishing emails                          ║");
                Console.WriteLine("║ - Safe browsing practices                  ║");
                Console.WriteLine("║ - Recognizing suspicious links             ║");
                Console.WriteLine("║                                            ║");
                Console.WriteLine("║ Type 'help' to see this menu again         ║");
                Console.WriteLine("║ Type 'bye' or 'exit' to end the conversation║");
                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
            }
        /// Function to create a typing effect for text
        static void TypeWriterEffect(string text, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                foreach (char c in text)
                {
                    Console.Write(c);
                    Thread.Sleep(20); // Adjust speed as needed
                }
                Console.WriteLine();
                Console.ResetColor();
            }
        /// Function to print warning messages in red
        static void PrintWarning(string message)
            {
                Console.ForegroundColor = warningColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }




