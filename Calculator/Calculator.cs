using System;

namespace LidlCalculator
{
    public class Calculator
    {
        public void Start()
        {
            double memory = 0;
            Console.WriteLine("Напишите первое число");
            double result = EnterNumber();

            string answer;

            do
            {
                Console.WriteLine("Введите число");
                double number = EnterNumber();

                Console.WriteLine("Что вы хотите сделать?");
                Console.WriteLine("\tplus - сложить");
                Console.WriteLine("\tminus - вычесть");
                Console.WriteLine("\tmultiply - умножить");
                Console.WriteLine("\tdivide - разделить");
                Console.WriteLine("\tplusmem -  сохраненный результат");
                Console.WriteLine("\tminusmem - вычесть сохраненный результат");
                Console.WriteLine("\tmultiplymem - умножить на сохраненный результат");
                Console.WriteLine("\tdividemem - разделить на сохраненный результат");

                Console.Write("Ваш выбор- ");

                switch (Console.ReadLine())
                {
                    case "plus":
                        result = Plus(result, number);
                        break;

                    case "minus":
                        result = Minus(result, number);
                        break;

                    case "multiply":
                        result = Multiply(result, number);
                        break;

                    case "divide":
                        if (number == 0)
                        {
                            Console.WriteLine("На ноль делить нельзя!!!");
                            break;
                        }

                        result = Divide(result, number);
                        break;

                    case "plusmem":
                        result = Plus(number, memory);
                        break;

                    case "minusmem":
                        result = Minus(number, memory);
                        break;

                    case "multiplymem":
                        result = Multiply(number, memory);
                        break;

                    case "dividemem":
                        if (memory == 0)
                        {
                            Console.WriteLine("На ноль делить нельзя!!!");
                            break;
                        }

                        result = Divide(number, memory);
                        break;
                }

                Console.WriteLine("\tmemory - сохранить результат");
                Console.WriteLine("\tclearmemory - очистить сохраненный результат");

                Console.Write("Ваш выбор- ");

                switch (Console.ReadLine())
                {
                    case "memory":
                        memory = result;
                        break;

                    case "clearmemory":
                        memory = 0;
                        break;
                }

                Console.WriteLine("Вы хотите продолжить? Y или N");
                answer = Console.ReadLine();

            } while (answer.ToLower() == "y");
        }

        private double EnterNumber()
        {
            while (true)
            {
                try
                {
                    return Convert.ToDouble(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Вы ввели не число!");
                    Console.ResetColor();

                    continue;
                }
                catch (OverflowException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Вы ввели слишком большое по модулю число!");
                    Console.WriteLine($"Число должно быть больше {double.MinValue} и меньше {double.MaxValue}");
                    Console.ResetColor();

                    continue;
                }
            }
        }

        private int Plus(int num1, int num2)
        {
            int result = num1 + num2;
            Console.WriteLine($"Ваш результат: {num1} + {num2} = " + result);
            return result;
        }

        private double Plus(double num1, double num2)
        {
            double result = num1 + num2;
            Console.WriteLine($"Ваш результат: {num1} + {num2} = {result}");
            return result;
        }

        private int Minus(int num1, int num2)
        {
            int result = num1 - num2;
            Console.WriteLine($"Ваш результат: {num1} - {num2} = " + result);
            return result;
        }

        private double Minus(double num1, double num2)
        {
            double result = num1 - num2;
            Console.WriteLine($"Ваш результат: {num1} - {num2} = " + result);
            return result;
        }

        private int Multiply(int num1, int num2)
        {
            int result = num1 * num2;
            Console.WriteLine($"Ваш результат: {num1} * {num2} = " + result);
            return result;
        }

        private double Multiply(double num1, double num2)
        {
            double result = num1 * num2;
            Console.WriteLine($"Ваш результат: {num1} * {num2} = " + result);
            return result;
        }

        private int Divide(int num1, int num2)
        {
            int result = num1 / num2;
            Console.WriteLine($"Ваш результат: {num1} / {num2} = " + result);
            return result;
        }

        private double Divide(double num1, double num2)
        {
            double result = num1 / num2;
            Console.WriteLine($"Ваш результат: {num1} / {num2} = " + result);
            return result;
        }
    }
}