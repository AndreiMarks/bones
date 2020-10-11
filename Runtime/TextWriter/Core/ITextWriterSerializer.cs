namespace Bones.TextWriter
{
    public interface ITextWriterSerializer
    {
        void UpdateWithText(string text);
        void DeleteText(int charactersToDelete);
    }
}
