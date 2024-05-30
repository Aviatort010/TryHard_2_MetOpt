using System;
using MathUtils;


namespace MainSpace
{
    class Program
    {
        static void Lab_1_test(
            LabOne.SomeOneDimFunc testFunction,
            double left = -10,
            double right = 10,
            double acc = 1e-5)
        {
            Console.WriteLine(" \tОдномерные:");

            Console.WriteLine("    Границы: [ " + left + "; " + right + " ],");
            Console.WriteLine("    Точность: " + acc);

            double HDresult = LabOne.HalfDevide(testFunction, left, right, acc);
            Console.Write(" Половинное деление - \t");
            Console.WriteLine("( " + HDresult.ToString("##.###") + " )\n");

            double GRresult = LabOne.GoldenRatio(testFunction, left, right, 2 * acc);
            Console.Write(" Золотое сечение    - \t");
            Console.WriteLine("( " + GRresult.ToString("##.###") + " )\n");

            double FBresult = LabOne.Fibonacci(testFunction, left, right, acc);
            Console.Write(" Метод Фибоначчи    - \t");
            Console.WriteLine("( " + FBresult.ToString("##.###") + " )\n");

            Console.ReadKey();
        }

        static void Lab_2_test(
            FunctionND testFunction,
            Vector left,
            Vector right,
            double acc = 1e-5)
        {
            Console.WriteLine($" \tМногомерные:");

            Console.WriteLine($"    Границы (Векторы):\n\t Слева = {left}, Справа = {right}");
            Console.WriteLine($"    Точность: {acc}\n");

            Vector HDresult = LabTwo.HalfDevide(testFunction, left, right, acc);
            Console.Write($" Половинное деление (Бисекции) ......\t");
            Console.WriteLine($" {HDresult} \n");

            Vector GRresult = LabTwo.GoldenRatio(testFunction, left, right, acc);
            Console.Write($" Золотое сечение ....................\t");
            Console.WriteLine($" {GRresult} \n");

            Vector FBresult = LabTwo.Fibonacci(testFunction, left, right, acc);
            Console.Write($" Фибоначи ...........................\t");
            Console.WriteLine($" {FBresult} \n");

            Console.Write($" Покоординатный спуск ...............\t");
            Console.WriteLine($" {LabTwo.CoordinateDescent(testFunction, left)} ");

            Console.ReadKey();
        }


        /// <summary>
        /// Одномерная Функция для испытаний алгоритмов
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static double Func(double x)
        {
            return (x + 3) * (x - 5);
        }

        static double MultiDimFunc(Vector xv)
        {
            return (xv[0] - 2.0) * (xv[0] - 2.0) + (xv[1] - 2.0) * (xv[1] - 2.0);
        }

        static void Main(string[] args)
        {
            double acc = 1e-5;
            /*
            double left = -10;
            double right = 10;

            Lab_1_test(Func, left, right, acc);
            */

            Vector leftVector = new Vector(0.0, 0.0);
            Vector rightVector = new Vector(5.0, 5.0);
            Lab_2_test(MultiDimFunc, leftVector, rightVector, acc);

        }
    }
}