using System;
using System.Collections.Generic;

namespace Rechner
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string input = Console.ReadLine();
                if(input == "Clear")
                {
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine(PostFixStackEvaluator(ParserV2(input)));
                }
            }
        }
        private static Queue<string> ParserV2(string input)
        {
            bool vorzeichen = false;
            char[] inputCh = input.ToCharArray();
            List<string> convInput = new List<string>();

            //Vorzeichen
            if(inputCh[0] == '-')
            {
                vorzeichen = true;
            }
            if(inputCh[0] == '+')
            {
                vorzeichen = true;
            }

            //Split string
            for (int i = 0; i < inputCh.Length; i++)
            {
                if (isOperator(inputCh[i].ToString()))
                {
                    convInput.Add(inputCh[i].ToString());
                }
                else
                {
                    if(i-1 >= 0)
                    {
                        if (!isOperator(inputCh[i - 1].ToString()))
                        {
                            convInput[convInput.Count - 1] += inputCh[i].ToString();
                        }
                        else
                        {
                            convInput.Add(inputCh[i].ToString());
                        }
                    }
                    else
                    {
                        convInput.Add(inputCh[i].ToString());
                    }
                   
                }
            }

            //minus mitten in rechnung
            for (int i = 0; i < convInput.Count; i++)
            {
                if(i - 2 >= 0)
                {
                    if (!isOperator(convInput[i]) && isOperator(convInput[i - 1]) && isOperator(convInput[i - 2]))
                    {
                        convInput[i] = convInput[i - 1] + convInput[i];
                        convInput.RemoveAt(i - 1);
                    }

                }
            }

            //Vorzeichen Berechnung 
            if (vorzeichen)
            {
                if(convInput[0] == "-")
                {
                    convInput.RemoveAt(0);
                    convInput[0] = "-" + convInput[0];
                }
                else
                {
                    convInput.RemoveAt(0);
                }

            }

            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>();

            //Converting to PostFix
            foreach(string element in convInput)
            {
                if (!isOperator(element))
                {
                    queue.Enqueue(element);
                }
                else if (isOperator(element))
                {
                    if(stack.Count == 0)
                    {
                        stack.Push(element);
                    }
                    else
                    {
                        if(element == ")")
                        {
                            for(int i = 0; i < stack.Count; i += 0)
                            {
                                if(stack.Peek() == "(")
                                {
                                    stack.Pop();
                                    break;
                                }
                                else
                                {
                                    queue.Enqueue(stack.Pop());
                                }
                            }
                            continue;
                        }
                        if(element == "(")
                        {
                            stack.Push(element);
                        }
                        else
                        {
                            if (Importence(element) > Importence(stack.Peek()))
                            {
                                stack.Push(element);
                            }
                            else
                            {
                                queue.Enqueue(stack.Pop());
                                stack.Push(element);
                            }
                        }
                    }           
                }
            }
            for (int i = 0; i < stack.Count; i += 0)
            {
                queue.Enqueue(stack.Pop());
            }

            return queue;
        }
        private static double PostFixStackEvaluator(Queue<string> queue)
        {
            Stack<double> stack = new Stack<double>();

            for (int i = 0; i < queue.Count; i += 0)
            {
                string element = queue.Dequeue();
                if (!isOperator(element))
                {
                    stack.Push(Convert.ToDouble(element.ToString()));
                }
                else if (isOperator(element))
                {
                    double n = stack.Pop();
                    double n1 = stack.Pop();
                    stack.Push(CalculateOperator(element, n1, n));
                }
            }

            if(stack.Count == 1)
            {
                return stack.Pop();
            }
            throw new Exception();
        }

        private static double CalculateOperator(string op, double n, double n1)
        {
            switch (op)
            {
                case "+":
                    return n + n1;
                case "-":
                    return n - n1;
                case "*":
                    return n * n1;
                case "/":
                    return n / n1;
                case "^":
                    double ergebnis = n;
                    for(int x = 1; n1 > x; x++)
                    {
                        ergebnis *= n;
                    }
                    return ergebnis;
            }
            throw new Exception();
        }
        private static int Importence(string ch)
        {
            switch (ch)
            {
                case "(":
                    return -1;
                case "+":
                    return 0;
                case "-":
                    return 0;
                case "*":
                    return 1;
                case "/":
                    return 1;
                case "^":
                    return 2;
            }

            throw new Exception();
                
        }
        private static bool isOperator(string input)
        {
            int i = 0;
            foreach (string element in Operatoren)
            {
                if (input == Operatoren[i])
                {
                    return true;
                }
                i++;
            }
            return false;
        }

        private static string[] Operatoren = { "-", "+", "*", "/", "^", "(", ")" };
    }
}
