using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {

            int num1 = 0; int num2 = 0;


            Console.WriteLine("Напишите первое число");
            num1 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Напишите второе число");
            num2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Что вы хотите сделать?");
            Console.WriteLine("\tplus - сложить");
            Console.WriteLine("\tminus - вычесть");
            Console.WriteLine("\tmultiply - умножить");
            Console.WriteLine("\tdivide - разделить");
            Console.Write("Ваш выбор- ");

            switch (Console.ReadLine())
            {
                case "plus":
                    Console.WriteLine($"Ваш результат: {num1} + {num2} = " + (num1 + num2));
                    break;
                case "minus":
                    Console.WriteLine($"Ваш результат: {num1} - {num2} = " + (num1 - num2));
                    break;
                case "multiply":
                    Console.WriteLine($"Ваш результат: {num1} * {num2} = " + (num1 * num2));
                    break;
                case "divide":
                    Console.WriteLine($"Ваш результат: {num1} / {num2} = " + (num1 / num2));
                    break;
            }
        }
    }
}



