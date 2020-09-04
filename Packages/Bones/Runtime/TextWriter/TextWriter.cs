using UnityEngine.InputSystem;

namespace Bones.TextWriter
{
    public class TextWriter
    {
        private ITextWriterUI _ui;
        private ITextWriterSerializer _serializer;

        public TextWriter(ITextWriterUI ui)
        {
            _ui = ui;

            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnTextInput(char character)
        {
            _ui.UpdateWithText(character.ToString());
        }
    }
}
