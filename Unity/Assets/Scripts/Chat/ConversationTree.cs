using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Chat
{
    public class ConversationTree
    {
        string speakerText;
        string[] responses;
        ConversationTree[] children;

        public ConversationTree(string text, string[] options, ConversationTree[] next)
        {
            if (options.Length != next.Length)
            {
                throw new ArgumentException("Length of nextTrees must match length of responses");
            }

            speakerText = text;
            responses = options;
            children = next;
        }

        public string SpeakerText
        {
            get
            {
                return speakerText;
            }
        }

        public string[] Responses
        {
            get
            {
                return responses;
            }
        }

        public ConversationTree[] Children
        {
            get
            {
                return children;
            }
        }

        public override string ToString()
        {
            string text = "Text: " + speakerText;
            
            for (int i = 0; i < responses.Length; i++)
            {
                text += "\nOption" + i + ": " + responses[i];
            }

            return text;
        }
    }
}
