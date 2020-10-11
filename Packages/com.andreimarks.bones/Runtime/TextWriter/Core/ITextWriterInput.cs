using System;

namespace Bones.TextWriter
{
    public interface ITextWriterInput
    {
        void StartInputListeners(Action<char> onTextInput);
    }
}
