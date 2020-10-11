using System;
using Bones.TextWriter;
using UnityEngine.InputSystem;

public class KeyboardInput : ITextWriterInput
{
    private Action<char> _onTextInput;
    
    public void StartInputListeners(Action<char> onTextInput)
    {
        _onTextInput = onTextInput;
        Keyboard.current.onTextInput += _onTextInput;
    }
}
