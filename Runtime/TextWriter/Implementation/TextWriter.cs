using UnityEngine;

namespace Bones.TextWriter
{
    public class TextWriter
    {
        private ITextWriterInput _input;
        private ITextWriterSerializer _serializer;
        private ITextWriterUI _ui;

        public TextWriter(ITextWriterInput input, 
            ITextWriterUI ui, 
            ITextWriterSerializer serializer)
        {
            _input = input;
            _serializer = serializer;
            _ui = ui;

            _input.StartInputListeners(OnTextInput);
        }

        private void OnTextInput(char character)
        {
            string newText = character.ToString();

            if (newText == "\b")
            {
                Debug.Log("Is Backspace?");
                return;
            }
            
            _ui.UpdateWithText(newText);
            _serializer.UpdateWithText(newText);
        }
    }
}