using System;
using System.Collections.Generic;
using System.Linq;

namespace Day18
{
    public class Evaluator
    {
        public static long Evaluate(ICollection<IToken> tokens, IDictionary<Type, int> precedences)
        {
            var outputQueue = new Queue<IToken>();
            var operatorStack = new Stack<IToken>();
            foreach (var token in tokens)
            {
                switch (token)
                {
                    case Number _:
                        outputQueue.Enqueue(token);
                        break;
                    case LeftBracket _:
                        operatorStack.Push(token);
                        break;
                    case Plus _:
                    case Times _:
                        while (operatorStack.Count != 0
                               && !(operatorStack.Peek() is LeftBracket)
                               && precedences[operatorStack.Peek().GetType()] >= precedences[token.GetType()])
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Push(token);
                        break;
                    case RightBracket _:
                        while (operatorStack.Count != 0 && !(operatorStack.Peek() is LeftBracket))
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }

                        if (operatorStack.Peek() is LeftBracket)
                        {
                            operatorStack.Pop();
                        }
                        break;
                }
            }

            while (operatorStack.Count != 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            return EvaluateReversePolish(outputQueue);
        }

        private static long EvaluateReversePolish(IEnumerable<IToken> queue)
        {
            var stack = new Stack<long>();
            foreach (var token in queue)
            {
                switch (token)
                {
                    case Number n:
                        stack.Push(n.Value);
                        break;
                    case Plus _:
                        stack.Push(stack.Pop() + stack.Pop());
                        break;
                    case Times _:
                        stack.Push(stack.Pop() * stack.Pop());
                        break;
                }
            }

            return stack.Single();
        }
    }
}
