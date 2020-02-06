namespace Toolbox.CommandLine
{
    public class ParseResult
    {
        public Result Result { get; private set; } = Result.Succeeded;
        public string Text { get; private set; } = "";
        public object Option { get; internal set; }

        internal void SetResult(Result result, string text = "")
        {
            Result = result;
            Text = text;
        }
    }

    public enum Result
    {
        Succeeded,
        MissingVerb,
        MissingOption,
        UnknownOption,
        MissingValue,
        BadValue,
        RequestHelp,
        MandatoryOption,
        DuplicateOption,
    }
}
