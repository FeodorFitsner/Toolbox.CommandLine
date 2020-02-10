using System;

namespace Toolbox.CommandLine
{
    public class ParseResult
    {
        public State State { get; private set; } = State.Succeeded;
        public string Text { get; private set; } = "";
        public object Option { get; internal set; }

        private int _return;
        public int Return 
        { 
            get
            {
                if (!ReturnSet)
                    throw new InvalidOperationException("Return not set");

                return _return;
            }
            private set
            {
                if (ReturnSet)
                    throw new InvalidOperationException("Return already set");

                _return = value;
                ReturnSet = true;
            }
        }
        private bool ReturnSet { get; set; }

        internal void SetState(State state, string text = "")
        {
            State = state;
            Text = text;
        }

        public ParseResult On<T>(Func<T, int> handler) where T : class
        {
            if (State==State.Succeeded && Option is T option)
            {
                Return = handler(option);
            }
            return this;
        }

        public ParseResult OnHelp(Func<ParseResult, int> handler)
        {
            if (State == State.RequestHelp)
            {
                Return = handler(this);
            }
            return this;
        }

        public ParseResult OnError(Func<ParseResult, int> handler)
        {
            if (State.IsNoneOf(State.Succeeded, State.RequestHelp))
            {
                Return = handler(this);
            }
            return this;
        }
    }

    public enum State
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
