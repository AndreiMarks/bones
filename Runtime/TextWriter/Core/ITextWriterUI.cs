namespace Bones.TextWriter
{
    public interface ITextWriterUI
    {
        void UpdateWithText(string newText);
        void AppendText(string newText);
        void DeleteText(int charsToDelte);
    }
}
