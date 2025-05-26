using System;
using System.Media;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Poe_Part_2
{
    // Base class containing common chatbot functionality
    internal class ChatbotBase
    {
        // Colour scheme for the bot console
        protected ConsoleColor titleColor = ConsoleColor.Cyan;
        protected ConsoleColor botColor = ConsoleColor.Green;
        protected ConsoleColor userColor = ConsoleColor.Yellow;
        protected ConsoleColor warningColor = ConsoleColor.Red;
        protected ConsoleColor infoColor = ConsoleColor.Blue;
        protected ConsoleColor memoryColor = ConsoleColor.Magenta;

        // Memory storage for user information
        protected Dictionary<string, string> userMemory = new Dictionary<string, string>();

        // Random number generator for varied responses
        protected Random random = new Random();

        // Method to display text with specific color and typewriter effect
        protected void TypeWriterEffect(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(20);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        // Method to display warning message
        protected void PrintWarning(string message)
        {
            Console.ForegroundColor = warningColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        protected void DisplayAsciiArt()
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

        // Dictionary for keyword responses with multiple variations for each topic
        // Dictionary for keyword responses with multiple variations
        protected Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
        {
            {
                "password", new List<string>
                {
                    "Strong passwords should:\n- Be at least 12 characters long\n- Include uppercase, lowercase, numbers, and symbols\n- Not contain personal information\n- Be unique for each account\nConsider using a password manager!",
                    "Password safety tip: Avoid using the same password across multiple sites. If one gets compromised, they all could be at risk!",
                    "Did you know? Using passphrases (like 'PurpleTiger$JumpsHigh') can be both secure and easier to remember than complex passwords."
                }
            },
            {
                "phishing", new List<string>
                {
                    "Phishing emails often:\n- Urge immediate action\n- Have spelling/grammar mistakes\n- Use suspicious sender addresses\n- Contain unexpected attachments\n- Ask for personal information\nNever click links in suspicious emails!",
                    "Phishing alert: Banks will never ask for your full password or PIN via email. If in doubt, contact them directly.",
                    "Watch out for phishing scams that create a sense of urgency, like 'Your account will be closed unless you act now!'"
                }
            },
            {
                "privacy", new List<string>
                {
                    "To protect your privacy:\n- Review app permissions regularly\n- Use privacy-focused browsers\n- Enable two-factor authentication\n- Be cautious about what you share online",
                    "Privacy tip: Check your social media privacy settings periodically as platforms often change their policies.",
                    "Did you know? Many 'free' apps make money by collecting and selling your data. Always check what information they access."
                }
            },
            {
                "scam", new List<string>
                {
                    "Common online scams include:\n- Fake tech support calls\n- 'You've won a prize' schemes\n- Romance scams\n- Investment fraud\nRemember: If it seems too good to be true, it probably is!",
                    "Scam warning: Never give remote access to your computer to someone who calls you unexpectedly, even if they claim to be from a well-known company.",
                    "Stay alert for scams where criminals pretend to be government officials demanding immediate payment."
                }
            },
            {
                "malware", new List<string>
                {
                    "To avoid malware:\n- Don't download software from untrusted sources\n- Keep your operating system updated\n- Use antivirus software\n- Be careful with email attachments",
                    "Malware fact: Some malware can encrypt your files and demand payment to unlock them (ransomware). Regular backups are your best defense!",
                    "Warning: Cracked or pirated software often contains hidden malware. It's not worth the risk!"
                }
            }
        };

        // Dictionary for sentiment detection and responses for all topics
        protected Dictionary<string, Dictionary<string, string>> sentimentResponses = new Dictionary<string, Dictionary<string, string>>()
{
    {
        "worried", new Dictionary<string, string>
        {
            { "password", "I understand password security can feel overwhelming. Start with small steps like enabling two-factor authentication on your most important accounts." },
            { "phishing", "It's normal to feel anxious about phishing attempts. Remember, you can always verify suspicious messages by contacting the organization directly." },
            { "privacy", "Privacy concerns are valid in today's digital world. The good news is there are many tools available to help protect your information." },
            { "scam", "It's scary how convincing some scams can be. Trust your instincts - if something feels off, it probably is." },
            { "malware", "Malware threats can be worrying, but with basic precautions like regular updates and backups, you can significantly reduce your risk." },
            { "default", "I can see you're concerned about cybersecurity. The fact that you're learning about these risks already puts you ahead of most people!" }
        }
    },
    {
        "confused", new Dictionary<string, string>
        {
            { "password", "Password security can be confusing at first. The main things to remember are: make it long, make it unique, and consider using a password manager." },
            { "phishing", "Phishing can be tricky to spot. A good rule of thumb: if an email creates a sense of urgency or asks for personal info, be suspicious." },
            { "privacy", "Privacy settings can be complex. Focus on the most important accounts first, like email and social media." },
            { "scam", "Scammers use many different tactics. The common theme is they try to get something from you - money, information, or access." },
            { "malware", "Malware comes in many forms. The key is prevention: don't open suspicious attachments or download from untrusted sites." },
            { "default", "Cybersecurity topics can be complex. Feel free to ask me to explain anything in simpler terms." }
        }
    },
    {
        "frustrated", new Dictionary<string, string>
        {
            { "password", "I hear your frustration. Managing passwords is annoying, but the inconvenience is worth it to protect your accounts. Password managers can help!" },
            { "phishing", "It's frustrating how persistent scammers are. The best defense is staying informed about their latest tactics." },
            { "privacy", "I understand your frustration about privacy. Companies do make it difficult to control your data, but small steps can make a difference." },
            { "scam", "Scams are frustrating because they prey on people's trust. Remember that legitimate organizations won't pressure you for immediate action." },
            { "malware", "Dealing with malware can be incredibly frustrating. Prevention is much easier than removal, so focus on safe browsing habits." },
            { "default", "I understand cybersecurity can be frustrating. Remember that every small step you take makes you more secure." }
        }
    },
    {
        "curious", new Dictionary<string, string>
        {
            { "password", "That's great you're curious about password security! Did you know the average person has over 100 online accounts? That's why password managers are so useful." },
            { "phishing", "Phishing is a fascinating (and scary) topic! Cybercriminals are constantly developing new tactics to trick people." },
            { "privacy", "Privacy is such an important topic these days! There's always more to learn about protecting your digital footprint." },
            { "scam", "Scams evolve constantly, which makes them interesting to study. The psychology behind how they work is particularly fascinating." },
            { "malware", "Malware development is like an arms race between hackers and security experts. The technology on both sides is amazing!" },
            { "default", "That's a great question! Cybersecurity is fascinating. Let me share some insights about that." }
        }
    }
};

        // Method to detect sentiment in user input
        protected string DetectSentiment(string input)
        {
            input = input.ToLower();
            if (input.Contains("worried") || input.Contains("concerned") || input.Contains("anxious"))
                return "worried";
            if (input.Contains("confused") || input.Contains("don't understand") || input.Contains("not sure"))
                return "confused";
            if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("angry"))
                return "frustrated";
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("fascinated"))
                return "curious";

            return null;
        }

        // Method to get a random response for a keyword
        protected string GetRandomResponse(string keyword)
        {
            if (keywordResponses.ContainsKey(keyword))
            {
                var responses = keywordResponses[keyword];
                return responses[random.Next(responses.Count)];
            }
            return null;
        }

        // Method to get a sentiment-based response
        protected string GetSentimentResponse(string sentiment, string currentTopic)
        {
            if (sentimentResponses.ContainsKey(sentiment))
            {
                var sentimentData = sentimentResponses[sentiment];
                if (sentimentData.ContainsKey(currentTopic))
                {
                    return sentimentData[currentTopic];
                }
                else if (sentimentData.ContainsKey("default"))
                {
                    return sentimentData["default"];
                }
            }
            return null;
        }

        protected virtual string GetResponse(string input, string userName, ref string currentTopic)
        {
            // Store user's name if not already stored
            if (!userMemory.ContainsKey("name"))
            {
                userMemory["name"] = userName;
            }

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

            // Check for sentiment
            string sentiment = DetectSentiment(input);
            string sentimentResponse = null;

            // Check for cybersecurity keywords
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    currentTopic = keyword;

                    // If user expressed interest in a topic, remember it
                    if (input.Contains("interested") || input.Contains("like to know") || input.Contains("tell me more"))
                    {
                        userMemory["interest"] = keyword;
                        TypeWriterEffect($"Bot: I'll remember you're interested in {keyword}.", memoryColor);
                    }

                    // Check for sentiment first
                    if (sentiment != null)
                    {
                        sentimentResponse = GetSentimentResponse(sentiment, keyword);
                        if (sentimentResponse != null)
                        {
                            return sentimentResponse;
                        }
                    }

                    // Otherwise return a random response for the keyword
                    return GetRandomResponse(keyword);
                }
            }

            // Handle follow-up questions based on current topic
            if (!string.IsNullOrEmpty(currentTopic))
            {
                if (input.Contains("more") || input.Contains("else") || input.Contains("another tip"))
                {
                    return GetRandomResponse(currentTopic);
                }

                if (sentiment != null)
                {
                    sentimentResponse = GetSentimentResponse(sentiment, currentTopic);
                    if (sentimentResponse != null)
                    {
                        return sentimentResponse;
                    }
                }
            }

            // Personalize response based on remembered interests
            if (userMemory.ContainsKey("interest"))
            {
                string interest = userMemory["interest"];
                return $"Since you're interested in {interest}, you might want to know: {GetRandomResponse(interest)}";
            }

            // Default response for unknown queries
            return "I'm not sure I understand. Could you try rephrasing? I can help with:\n- Password safety\n- Phishing emails\n- Privacy protection\n- Scam awareness\n- Malware prevention";
        }
    }

    // Derived class that inherits from ChatbotBase
    internal class Program : ChatbotBase
    {
        // Entry point of the program
        static void Main(string[] args)
        {
            Program chatbot = new Program();
            chatbot.Run();
        }

        private void Run()
        {
            // Initialize the chatbot
            InitializeChatbot();

            // Get the user's name and reuse it
            string userName = GetUserName();

            // Main conversation loop
            ChatLoop(userName);
        }

        private void InitializeChatbot()
        {
            Console.Title = "Cybersecurity Awareness Assistant";

            // Play welcome sound 
            try
            {
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

        private string GetUserName()
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

        private void ChatLoop(string userName)
        {
            bool continueChat = true;
            string currentTopic = "";

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

                string response = GetResponse(input.ToLower(), userName, ref currentTopic);

                TypeWriterEffect($"Bot: {response}", botColor);
                Console.WriteLine();

                // Check if user wants to exit
                if (input.ToLower().Contains("bye") || input.ToLower().Contains("exit"))
                {
                    continueChat = false;
                }
                else if (input.ToLower().Contains("help"))
                {
                    DisplayHelp();
                }
            }

            // Farewell message
            TypeWriterEffect($"Thank you for using the Cybersecurity Awareness Bot, {userName}!", botColor);
            TypeWriterEffect("Stay safe online!", botColor);
            Thread.Sleep(2000);
        }

        private void DisplayHelp()
        {
            Console.ForegroundColor = infoColor;
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║           CYBERSECURITY HELP MENU                  ║");
            Console.WriteLine("╠════════════════════════════════════════════════════╣");
            Console.WriteLine("║ You can ask about:                                ║");
            Console.WriteLine("║ - Password safety (e.g., 'password tips')         ║");
            Console.WriteLine("║ - Phishing emails (e.g., 'how to spot phishing')  ║");
            Console.WriteLine("║ - Privacy protection (e.g., 'privacy tips')       ║");
            Console.WriteLine("║ - Scam awareness (e.g., 'common scams')            ║");
            Console.WriteLine("║ - Malware prevention (e.g., 'avoid malware')      ║");
            Console.WriteLine("║                                                    ║");
            Console.WriteLine("║ You can express how you feel:                     ║");
            Console.WriteLine("║ - 'I'm worried about...'                          ║");
            Console.WriteLine("║ - 'I'm confused about...'                         ║");
            Console.WriteLine("║ - 'I'm frustrated with...'                        ║");
            Console.WriteLine("║                                                    ║");
            Console.WriteLine("║ Type 'help' to see this menu again               ║");
            Console.WriteLine("║ Type 'bye' or 'exit' to end the conversation     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}