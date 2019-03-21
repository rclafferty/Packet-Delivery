using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.Chat
{
    public class ConversationTreeSetup
    {
        ConversationTree chatTreeCLA;
        ConversationTree chatTreeCLAHead;
        string initialText;

        public ConversationTreeSetup()
        {

        }

        void SetupCLATree()
        {
            ConversationTree temp_tree_1;
            ConversationTree temp_tree_2;

            // CLA Chat Tree
            initialText = "Welcome to the Central Lookup Agency.";

            string headText = "How may we assist you?";
            string option1 = "I'm looking for a _. Do you know where I can find them?";
            string option2 = "Can you show me the travel log for this packet?";
            string[] options = { option1, option2 };
            string action1 = "Chat";
            string action2 = "Chat";
            string[] actions = { action1, action2 };

            chatTreeCLAHead = new ConversationTree(headText, options, actions, null);

            // Left child
            headText = "Check this list and select the person you're looking for.";
            option1 = "Okay.";
            option2 = "Actually, I'll come back later";
            string[] temp_left1_options = { option1, option2 };
            action1 = "Show GUI";
            action2 = "Chat";
            string[] temp_left1_actions = { action1, action2 };

            temp_tree_1 = new ConversationTree(headText, temp_left1_options, temp_left1_actions, null);

            // Right Child
            headText = "Of course. Here you go.";
            option1 = "Thank you.";
            option2 = "";
            string[] temp_right1_options = { option1, option2 };
            action1 = "Show GUI";
            action2 = "Nothing";
            string[] temp_right1_actions = { action1, action2 };

            temp_tree_2 = new ConversationTree(headText, temp_right1_options, temp_right1_actions, null);
            
            // Assign children of head since they are now created
            ConversationTree[] childrenOfHead = { temp_tree_1, temp_tree_2 };
            chatTreeCLAHead.Children = childrenOfHead;

            // Left Left child
            headText = "Check with the [], which is to the * of us.";
            option1 = "Thank you for your help.";
            option2 = "One more thing...";
            string[] temp_left11_options = { option1, option2 };
            action1 = "Leave";
            action2 = "Chat";
            string[] temp_left11_actions = { action1, action2 };

            ConversationTree[] leftLeftChildChildren = { null, chatTreeCLAHead };
            temp_tree_1 = new ConversationTree(headText, temp_left11_options, temp_left11_actions, leftLeftChildChildren);

            // Left Right Child
            headText = "Okay. See you soon!";
            option1 = "Bye.";
            option2 = "";
            string[] temp_right12_options = { option1, option2 };
            action1 = "Leave";
            action2 = "Leave";
            string[] temp_right12_actions = { action1, action2 };

            ConversationTree[] leftRightChildChildren = { null, null };
            temp_tree_2 = new ConversationTree(headText, temp_right1_options, temp_right1_actions, leftRightChildChildren);

            // Assign children of left child
            ConversationTree[] leftChildChildren = { temp_tree_1, temp_tree_2 };
            chatTreeCLAHead.Children[0].Children = leftChildChildren;

            // Right Left Child
            headText = "Is there anything else we can do to help?";
            option1 = "One more thing...";
            option2 = "I'm okay for now. Thank you.";
            string[] temp_right21_options = { option1, option2 };
            action1 = "Chat";
            action2 = "Leave";
            string[] temp_right21_actions = { action1, action2 };

            ConversationTree[] rightLeftChildChildren = { chatTreeCLAHead, null };
            temp_tree_2 = new ConversationTree(headText, temp_right1_options, temp_right1_actions, rightLeftChildChildren);

            // Right Right Child
            temp_tree_2 = null;

            // Assign children of right child
            ConversationTree[] rightChildChildren = { temp_tree_1, temp_tree_2 };
            chatTreeCLAHead.Children[1].Children = rightChildChildren;
        }
    }

    public class ConversationTreeSetup_Working
    {
        ConversationTree chatTreeCLA;
        ConversationTree chatTreeCLA_HEAD;
        string initialText = "";

        public ConversationTreeSetup_Working()
        {
            string filepath = "Conversation/Conversation_CLA.json";
#if UNITY_EDITOR
            filepath = "Assets/Resources/" + filepath;
#endif

            string line = "";
            string[] parts = null;

            Stack<string> jsonStack = new Stack<string>();

            string chatText = "";
            string option1 = "";
            string option2 = "";
            string action1 = "";
            string action2 = "";

            ConversationTree child1 = null;
            ConversationTree child2 = null;

            bool initialSetup = true;
            bool initialTree = true;

            using (StreamReader sr = new StreamReader(filepath))
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line == "{")
                    {
                        jsonStack.Push(line);

                        if (!initialTree)
                        {
                            string[] options = { option1, option2 };
                            ConversationTree[] children = { child1, child2 };
                            //ConversationTree temp = new ConversationTree(chatText, options, children);

                        }
                    }
                    else if (line == "}")
                    {
                        if (jsonStack.Count > 0)
                        {
                            jsonStack.Pop();
                        }
                        else
                        {
                            throw new NullReferenceException("Stack is already empty");
                        }
                    }
                    else
                    {
                        parts = line.Split(':');
                        if (parts[0] == "initialText")
                        {
                            string[] temp = parts[1].Split(',');
                            line = temp[0];
                            initialText = line;
                            continue;
                        }
                    }
                }
            }
        }
    }
}
