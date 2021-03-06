using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rechner
{
    class Rechner
    {
        public string ergebnis;
        public bool isEquation = false;
        public bool equals = false;

        public Rechner()
        {

        }

        public void Calculate(string input)
        {
            List<string> convInput = ConvertString(input);

            List<string> term1 = new List<string>();
            List<string> term2 = new List<string>();
            if (convInput.Contains("="))
            {
                isEquation = true;
                int index = convInput.IndexOf("=");

                for(int i = 0; i < index; i++)
                {
                    term1.Add(convInput[i]);
                }
                for(int i = index + 1; i < convInput.Count; i++)
                {
                    term2.Add(convInput[i]);
                }

                double erg1 = PostFixStackEvaluator(ParserV2(term1));
                double erg2 = PostFixStackEvaluator(ParserV2(term2));
                if (erg1 == erg2)
                {
                    ergebnis = erg1.ToString() + "=" + erg2.ToString();
                    equals = true; 
                }
            }
            else
            {
                ergebnis = PostFixStackEvaluator(ParserV2(convInput)).ToString();
            }
        }

        private static List<string> ConvertString(string input)
        {
            char[] inputCh = input.ToCharArray();
            List<string> convInput = new List<string>();

            //Split string
            for (int i = 0; i < inputCh.Length; i++)
            {
                if (isOperator(inputCh[i].ToString()))
                {
                    convInput.Add(inputCh[i].ToString());
                }
                else
                {
                    if (i - 1 >= 0)
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

            return convInput;
        }
        private static Queue<string> ParserV2(List<string> convInput)
        {
            //minus mitten in rechnung
            for (int i = 0; i < convInput.Count; i++)
            {
                if (i - 2 >= 0)
                {
                    if (!isOperator(convInput[i]) && isOperator(convInput[i - 1]) && isOperator(convInput[i - 2]))
                    {
                        convInput[i] = convInput[i - 1] + convInput[i];
                        convInput.RemoveAt(i - 1);
                    }

                }
            }

            //Vorzeichen Berechnung 
            if (convInput[0] == "-")
            {
                convInput.RemoveAt(0);
                convInput[0] = "-" + convInput[0];
            }
            else if(convInput[0] == "+")
            {
                convInput.RemoveAt(0);
            }

            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>();

            //Converting to PostFix
            foreach (string element in convInput)
            {
                if (!isOperator(element))
                {
                    queue.Enqueue(element);
                }
                else if (isOperator(element))
                {
                    if (stack.Count == 0)
                    {
                        stack.Push(element);
                    }
                    else
                    {
                        if (element == ")")
                        {
                            for (int i = 0; i < stack.Count; i += 0)
                            {
                                if (stack.Peek() == "(")
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
                        if (element == "(")
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

            if (stack.Count == 1)
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
                    for (int x = 1; n1 > x; x++)
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

        private static string[] Operatoren = { "-", "+", "*", "/", "^", "(", ")", "=" };
    }
}
