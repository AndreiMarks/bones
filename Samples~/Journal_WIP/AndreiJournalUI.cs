using TMPro;
using UnityEngine;

namespace Bones.TextWriter
{
    using System.Text;

    public class AndreiJournalUI : WordProcessorUI
    {
        public AndreiJournalUI() : base()
        {
        }

        public override void AppendText(string newText)
        {
            Debug.Log(newText);
            base.AppendText(newText);
        }

        public override void DeleteText(int numCharsToDelete)
        {
            base.DeleteText(numCharsToDelete);
        }
    }
}
