using System.Text;

namespace Core
{
    public class TextManager
    {
        // ----------
        // Start menu
        // ----------
        public static string GetStartScreenArt()
        {
            return @"   _____ ____  _   _  _____  ____  _      ______    _____       _______ ______ 
  / ____/ __ \| \ | |/ ____|/ __ \| |    |  ____|  / ____|   /\|__   __|  ____|
 | |   | |  | |  \| | (___ | |  | | |    | |__    | |  __   /  \  | |  | |__   
 | |   | |  | | . ` |\___ \| |  | | |    |  __|   | | |_ | / /\ \ | |  |  __|  
 | |___| |__| | |\  |____) | |__| | |____| |____  | |__| |/ ____ \| |  | |____ 
  \_____\____/|_| \_|_____/ \____/|______|______|  \_____/_/    \_\_|  |______|
";
        }

        
        // ------------------------
        // Intro (level 0)
        // ------------------------
        public static string GetLevel0Intro1()
        {
            return @"Welcome to Console Gate!
This game is created by Jens Bouman and Bas van Rossem.";
        }

        public static string GetLevel0Intro2()
        {
            return @"Because of some pandemic, a lot of your classes are online.
However, the professor has not arrived in the chat.
No one knows where he is.
You decide to take initiative in finding him.";
        }
        
        public static string GetLevel0Intro3()
        {
            return @"The game is on.";
        }

        
        // -----------------------------
        // Connect through ssh (level 1)
        // -----------------------------
        public static string GetLevel1Chatlog()
        {
            return @"-------------------
Chatlog 23 - 10 - 2020
------------------ -
23 - 10 - 2020 16:03, Docent:
Bij deze geef ik jullie een extra bijlage als toevoeging op de les

23 - 10 = 2020 16:04, Remco:
ligt het aan mij, of kan ik het bestand nog niet downloaden ?

23 - 10 - 2020 16:04, Julianne:
De docent heeft nog geen download - link gegeven....

23 - 10 - 2020 16:05, Docent:
Oeps, foutje, bij deze is hij meegedeeld.

23 - 10 - 2020 16:06, Julianne:
Bedankt!

23 - 10 - 2020 16:06, Docent:
Ik wil graag dat jullie deze stof voor de volgende les(26 - 10 - 2020) doornemen.

------------------ -
Chatlog 24 - 10 - 2020
------------------ -

-------------------
Chatlog 25 - 10 - 2020
------------------ -
25 - 10 - 2020 20:48, SYSTEM:
Succesfully downloaded ""appendix.txt""";
        }
        
        public static string GetLevel1AppendixMetadata()
        {
            return @"Filename: appendix
File extension: .txt
Path to file: / Downloads / apendix

Date created: 20 - 03 - 2014
Size: 12381 bytes
Author: Docent
IP - adress owner: 52.232.56.79";
        }
        
        public static string GetLevel1AppendixContent()
        {
            return @"Content:

Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Nulla vehicula et ex id eleifend.
Sed cursus, eros non fringilla finibus, augue velit aliquam felis,
ac mollis augue sem in arcu.
Nunc odio sapien, varius in vestibulum in, ultrices a diam.
Nulla vestibulum ac dolor quis eleifend.
Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere curae;
Praesent suscipit feugiat felis id viverra.
Praesent a lectus sapien.
Nunc ac mollis ipsum.
Ut mauris dolor, maximus id diam in, auctor suscipit neque.
Nam velit eros, accumsan non libero at, placerat semper augue.
Maecenas scelerisque semper venenatis.
Suspendisse vel dolor velit.
Nunc imperdiet cursus velit eget porta.
In eget ex purus.
Vivamus pellentesque quam in arcu ultrices varius.

Nunc egestas pellentesque pulvinar. 
Nulla euismod, nulla non consequat dapibus, nisi neque lacinia sapien, 
eget sodales quam ante a lacus. 
Etiam rhoncus, ipsum ut maximus gravida, ligula nulla congue magna, 
nec blandit sapien risus iaculis ligula. Morbi pulvinar lorem 
gravida mi eleifend convallis. Nunc lacinia fringilla gravida. 
Donec molestie arcu id ex ullamcorper sagittis.
Pellentesque tincidunt urna sit amet ex cursus suscipit.";
        }
        
        public static string GetLevel1SshWrongConnect()
        {
            return @"That IP-address can't be connected to";
        }
        
        public static string GetLevel1CatWrongFile()
        {
            return @"Can't find that file, try searching recently downloaded files.
(cat chatlog.txt might contain useful info)";
        }

        public static string GetLevel1DirContent()
        {
            return @"/:
-chatlog.txt
-appendix.txt";
        }

        public static string GetLevel1Help1()
        {
            return @"+------------------------------------------------------------------------------+
|                                  Helpmenu:                                   |
|                                                                              |
|                                                                              |
| Try finding out the address (ip4) to the teacher's pc. Using metadata from   |
| recently downloaded files might give more information                        |
|                                                                              |
|                                                                              |
| (press 'm' to go back to the starting menu)                                  |
+------------------------------------------------------------------------------+";
        }
        
        public static string GetLevel1Help2()
        {
            return @"+------------------------------------------------------------------------------+
|                                  Helpmenu:                                   |
|                                                                              |
|                                                                              |
| Try finding out the address (ip4) to the teacher's pc. Using metadata from   |
| recently downloaded files might give more information                        |
|                                                                              |
|                                                                              |
| SSH can be used to connect to other computers through the internet.          |
|                                                                              |
|                                                                              |
| (press 'm' to go back to the starting menu)                                  |
+------------------------------------------------------------------------------+";
        }
        
        
        // -----------------------------------
        // Complete the logic puzzle (level 2)
        // -----------------------------------
        public static string GetLevel2Preface()
        {
            return @"Chapter 2

Nice work!
You have made a connection to the PC of the professor.
There is only one thing that stops you from ogettin gin entirely.
He has some sort of puzzle as a login.

You take a crack at it nevertheless.";
        }

        public static string GetLevel2Controls()
        {
            return @"[Space] Toggle switch
[Left arrow] Move selection to the left
[Right arrow] Move selection to the right";
        }

        public static string GetLevel2Puzzle()
        {
            return @"                 [ Login ]
                     .
                     .
                 [  And  ]
             .....       .....
             .               .
         [  And  ]       [  OR   ]
     .....       .........       .....
     .               .               .
 [  And  ]       [  And  ]       [  OR   ]
 .       .       .       .       .       .
 .       .       .       .       .       .
[ ]     [ ]     [ ]     [ ]     [ ]     [ ]";
        }

        public static string GetLevel2Continue()
        {
            return @"Press [space] to continue...";
        }

        // ------------------------------------
        // Complete the binary puzzle (level 3)
        // ------------------------------------
        public static string GetLevel3Intro()
        {
            return @"                                                                               
                      Chapter 3: Breaking the encryption.                      
                                                                               
                                                                               
Na het verbinden met de computer van je docent, 
en het hacken van zijn logische poorten sta je nu voor een volgende uitdaging.

Om bij de agenda van de docent te komen moet je dit keer een wachtwoord zien te achterhalen. 

Gelukkig staan alle wachtwoorden opgeslagen op een centrale locatie.

(Als je er even niet uit komt, [escape] kan je helpen)";
        }

        public static string GetLevel3Help1()
        {
            return @"+------------------------------------------------------------------------------+
|                                  Helpmenu:                                   |
|                       (Druk [escape] om terug te gaan)                       |
|                                                                              |
| Alle wachtwoorden zijn opgeslagen op de centrale plaats van het systeem.     |
| (Probeer ""passwd"" te openen, of ""dir"" / ""ls"" te gebruiken).                  |
|                                                                              |
+------------------------------------------------------------------------------+";
        }

        public static string GetLevel3Help2()
        {
            return @"+------------------------------------------------------------------------------+
|                                  Helpmenu:                                   |
|                       (Druk [escape] om terug te gaan)                       |
|                                                                              |
| Alle wachtwoorden zijn opgeslagen op de centrale plaats van het systeem.     |
| (Probeer ""passwd"" te openen, of ""dir"" / ""ls"" te gebruiken).                  |
|                                                                              |
|                                                                              |
| Het lijkt erop dat zijn binaire wachtwoord versleuteld is.                   |
| Tijd om zijn stappen op te volgen.                                           |
|                                                                              |
| Voorbeeld binair optellen: 1001 1111 + 1010 0101                             |
| 1001 1111                                                                    |
| 1010 0101                                                                    |
| --------- +                                                                  |
| 0100 0100                                                                    |
|                                                                              |
| (Altijd afronden naar 8 bits)                                                |
+------------------------------------------------------------------------------+";
        }

        public static string GetLevel3Passwd()
        {
            return @"                                                                                
                                     passwd                                     
                                                                                
root:x:0:1:System Operator:/:/bin/ksh
daemon:x:1:1::/tmp:
uucp:x:4:4::/var/spool/uucppublic:/usr/lib/uucp/uucico
user:01101010:181:100:Rik de Jong:/u/user:/bin/ksh

# Je bent aardig ver gekomen, als jij in mijn passwd file zit te rommelen.
# Tel de basis bij de som van de gebruiker op
# Haal hier de som van de lagen af.
# Vermenigvuldig dit met 2, dit leidt je naar je toekomst.";
        }

        public static string GetLevel3CatWrong()
        {
            return @"Can't find that file.
(Opening the menu might provide additional information)";
        }

        public static string GetLevel3Dir()
        {
            return @":/
-passwd
-decryptor.exe";
        }

        public static string GetLevel3Decryptor(int progress)
        {
            StringBuilder decryptorText = new StringBuilder(
                @"                                                                                
Voer de juiste antwoorden in om het wachtwoord te ontgrendelen als parameter:
(Bijvoorbeeld ""decryptor 1111 1111"" of ""decryptor 11111111"")   
Correcte antwoorden komen hieronder te staan

                                                                                
Stap 1: Basiswachtwoord:                                                    ");
        
            if (progress >= 3)
            {
                decryptorText.Append(@"
0110 1010                                 

Stap 2: Som der gebruiker ");
            }

            if (progress >= 4)
            {
                decryptorText.Append(@"
1011 1111
                                                                                
Stap 3: Basiswachtwoord + som der gebruiker");
            }

            if (progress >= 5)
            {
                decryptorText.Append(@"
0010 1001                                                                                
                                                                                
Stap 4: De som van de lagen");
            }

            if (progress >= 6)
            {
                decryptorText.Append(@"
0001 1100
                                                                                
Stap 5: Stap 3 - Stap 4");
            }

            if (progress >= 7)
            {
                decryptorText.Append(@"
0000 1101
                                                                                
Stap 6: Stap 5 x 2");
            }

            if (progress >= 8)
            {
                decryptorText.Append(@"
0001 1010

Gefeliciteerd! Het wachtwoord is ""De geit is gevlogen""");
            }

            return decryptorText.ToString();
        }
    }
}