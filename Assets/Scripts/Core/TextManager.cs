using System.Text;
using UnityEngine.SceneManagement;

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
23 - 10 - 2020 16:03, Professor:
Hereby an appendix of the last lesson 

23 - 10 - 2020 16:04, Remco:
is it just me or can't i access the file??

23 - 10 - 2020 16:04, Julianne:
He hasn't uploaded it yet...

23 - 10 - 2020 16:05, Docent:
Oh, my mistake. It should be available now.

23 - 10 - 2020 16:06, Julianne:
Thx!

23 - 10 - 2020 16:06, Docent:
I'd like for you all to just go through it for the coming lesson (26-10).

------------------ -
Chatlog 24 - 10 - 2020
------------------ -
24 - 10 - 2020 15:12, Remco:
I think the file is broken??

24 - 10 - 2020 23:33, Remco:
Anyone?

-------------------
Chatlog 25 - 10 - 2020
------------------ -
25 - 10 - 2020 20:48, SYSTEM:
Successfully downloaded ""appendix.txt""";
        }
        
        public static string GetLevel1AppendixMetadata()
        {
            return @"Filename: appendix
File extension: .txt
Path to file: / Downloads / appendix.txt

Date created: 20 - 03 - 2014
Size: 12381 bytes
Author: User
IP - address owner: 52.232.56.79";
        }
        
        public static string GetLevel1AppendixContent()
        {
            return @"Content:

Corem ipsum dolor sit amet, consectetur adipiscing elit.
Oulla vehicula et ex id eleifend.
Ned cursus, eros non fringilla finibus, augue velit aliquam felis,
Nc mollis augue sem in arcu.
Eunc odio sapien, varius in vestibulum in, ultrices a diam.
Culla vestibulum ac dolor quis eleifend.
Testibulum ante ipsum primis in faucibus orci luctus et ultrices posuere curae;
Traesent suscipit feugiat felis id viverra.
Oraesent a lectus sapien.
Munc ac mollis ipsum.
Et mauris dolor, maximus id diam in, auctor suscipit neque.
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
There is only one thing that stops you from getting in entirely.
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
                 [  AND  ]
             .....       .....
             .               .
         [  AND  ]       [  OR   ]
     .....       .........       .....
     .               .               .
 [  AND  ]       [  AND  ]       [  OR   ]
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
                                                                               
After connecting (and hacking) the pc of the professor,
You are now ready to face the next challenge.                                                                               

To find out more it seems that you need to figure out some sort of password.
But you are in luck. He has saved them all in one place. Convenient.

(If you are having a bit of trouble, [escape] might help you.)";
        }

        public static string GetLevel3Help1()
        {
            return @"+------------------------------------------------------------------------------+
|                                    Help:                                     |
|                         (Press [Escape] to go back)                          |
|                                                                              |
| All passwords are saved in one single location on the system.                |
| (Try opening the ""passwd"" file, of try the command ""dir"" / ""ls"").            |
|                                                                              |
+------------------------------------------------------------------------------+";
        }

        public static string GetLevel3Help2()
        {
            return @"+------------------------------------------------------------------------------+
|                                    Help:                                     |
|                         (Press [Escape] to go back)                          |
|                                                                              |
| All passwords are saved in one single location on the system.                |
| (Try opening the ""passwd"" file, of try the command ""dir"" / ""ls"").            |
|                                                                              |
|                                                                              |
| It seems that the password is encrypted using binary.                        |
| Lets follow his steps and see what happens.                                  |
|                                                                              |
| Example binary calculation: 1001 1111 + 1010 0101                            |
| 1001 1111                                                                    |
| 1010 0101                                                                    |
| --------- +                                                                  |
| 0100 0100                                                                    |
|                                                                              |
| (Always round to 8 bits)                                                     |
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

# You have come quite far, seeing as you are messing around in my passwd file.
# Add the base to the sum of the user.
# Subtract the sum of the layers.
# Multiply by 2, this will lead you to your future.";
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
To unlock the passwords, use the answers as input for the decryptor.
(For instance ""decryptor 1111 1111"" or ""decryptor 11111111"")                                                                     
The correct answers will be shown below.

                                                                                
Step 1: Base password:                                                    ");
        
            if (progress >= 3)
            {
                decryptorText.Append(@"
0110 1010                                 

Step 2: Sum of the user");
            }

            if (progress >= 4)
            {
                decryptorText.Append(@"
1011 1111
                                                                                
Step 3: base password + sum of the user");
            }

            if (progress >= 5)
            {
                decryptorText.Append(@"
0010 1001                                                                                
                                                                                
Step 4: Sum of the layers");
            }

            if (progress >= 6)
            {
                decryptorText.Append(@"
0001 1100
                                                                                
Step 5: Step 3 - Step 4");
            }

            if (progress >= 7)
            {
                decryptorText.Append(@"
0000 1101
                                                                                
Step 6: Step 5 x 2");
            }

            return decryptorText.ToString();
        }
    }
}