using System.Collections.Generic;

namespace Day18
{
    public interface IToken
    {
    }

    public class Number : IToken
    {
        public long Value;
    }

    public class LeftBracket : IToken
    {
    }

    public class RightBracket : IToken
    {
    }

    public class Plus : IToken
    {
    }

    public class Times : IToken
    {
    }

    public static class Tokeniser
    {
        public static ICollection<IToken> Tokenise(string input)
        {
            var tokens = new List<IToken>();
            foreach (var c in input)
            {
                switch (c)
                {
                    case '(':
                        tokens.Add(new LeftBracket());
                        break;
                    case ')':
                        tokens.Add(new RightBracket());
                        break;
                    case '+':
                        tokens.Add(new Plus());
                        break;
                    case '*':
                        tokens.Add(new Times());
                        break;
                    case var _ when c >= '0' && c <= '9':
                        if (tokens.Count == 0 || !(tokens[^1] is Number n))
                        {
                            tokens.Add(new Number
                            {
                                Value = c - '0'
                            });
                        }
                        else
                        {
                            n.Value = n.Value * 10 + c - '0';
                        }

                        break;
                }
            }

            return tokens;
        }
    }
}
