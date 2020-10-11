using TMPro;
using UnityEngine;

namespace Bones.TextWriter
{
    public class TextWriterUI : ITextWriterUI
    {
        private TextMeshPro _mainText;

        public TextWriterUI()
        {
            _mainText = CreateNewTextMesh();
        }

        private TextMeshPro CreateNewTextMesh()
        {
            var gameObject = new GameObject("TextMeshPro");
            var tmp = gameObject.AddComponent<TextMeshPro>();
            tmp.alignment = TextAlignmentOptions.Center;

            return tmp;
        }

        public void UpdateWithText(string newText)
        {
            _mainText.text = newText;
        }

        public void AppendText(string newText)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteText(int charsToDelte)
        {
            throw new System.NotImplementedException();
        }
    }
}
