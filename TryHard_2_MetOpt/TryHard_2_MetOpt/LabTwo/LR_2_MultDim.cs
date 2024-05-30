using System;
using MathUtils;
using Microsoft.VisualBasic;


namespace MainSpace
{
    class LabTwo
    {
        // Пример функции для передачи в другие функции (чемодан) улетел в утилиты MathUtils.
        // (так банально удобнее)

        //-----------------------------------{Половинное деление}-----------------------------------
        public static Vector HalfDevide(
            FunctionND func,
            Vector leftBorder,
            Vector rightBorder,
            double accuracy = 1e-5,
            int maxIterations = 1000)
        {
            // На этот раз почти без разницы какая сторона "левая",
            // Потому без swap
            Vector xc;  // Это всё также центр
            Vector directionVector;

            directionVector = Vector.Direction(leftBorder, rightBorder);    // Вектор-Направление из Левого в Правый
            directionVector *= accuracy;    //  Собственно, здесь он нужен чисто для завершения расчётов,
                                            //  Потому и домножаем на точность (accuracy)

            //           1
            //      |<------>|
            //       direction      // Это всегда должен быть единичный вектор
            //       ------->
            //      |\     /|\
            //        \     | r
            //       l \    | i
            //        e \   | g
            //         f \  | h
            //          t \ | t
            //             \|

            int iteration = 0;
            for (; iteration <= maxIterations; ++iteration)
            {
                //  Если длинна вектора между граничными векторами уже точнее точности (accuracy),
                //  то можно смело выходить из цикла.
                if ((rightBorder - leftBorder).Magnitude < accuracy) break;

                xc = (rightBorder + leftBorder) * 0.5;  // И Формула для центра всё та же

                //  Проход по функции раз,      Проход по функции два.
                if (func(xc - directionVector) < func(xc + directionVector))
                    rightBorder = xc;
                else
                    leftBorder = xc;
            }
#if DEBUG
            Console.Write($" bisect::  iterations {iteration}\n");
            Console.Write($" bisect::  func probes {iteration * 2}\n");
#endif
            return (rightBorder + leftBorder) * 0.5;
        }

        //-----------------------------------{Золотое сечение}-----------------------------------
        public static Vector GoldenRatio(
            FunctionND func,
            Vector leftBorder,
            Vector rightBorder,
            double accuracy = 1e-5,
            int maxIterations = 1000)
        {
            Vector left = new Vector(leftBorder);
            Vector right = new Vector(rightBorder);

            left = rightBorder - NumericCommon.ONE_OVER_PHI * (rightBorder - leftBorder);
            double funcLeft = func(left);

            right = leftBorder + NumericCommon.ONE_OVER_PHI * (rightBorder - leftBorder);
            double funcRight = func(right);

            int i= 0;
            for (; i < maxIterations; ++i)
            {
                if ((rightBorder - leftBorder).Magnitude < accuracy) break;
                
                if (funcLeft >= funcRight)
                {
                    leftBorder = left;
                    left = right;
                    funcLeft = funcRight;
                    right = leftBorder + NumericCommon.ONE_OVER_PHI * (rightBorder - leftBorder);
                    funcRight = func(right);
                    continue;
                }
                rightBorder = right;
                right = left;
                funcRight = funcLeft;
                left = rightBorder - NumericCommon.ONE_OVER_PHI * (rightBorder - leftBorder);
                funcLeft = func(left);
            }
#if DEBUG
            Console.Write($" GoldenRatio::  iterations {i}\n");
            Console.Write($" GoldenRatio::  func probes {i + 2}\n");
#endif
            return (rightBorder + leftBorder) * 0.5;
        }

        //-----------------------------------{Метод Фибоначчи}-----------------------------------
        public static Vector Fibonacci(
            FunctionND func,
            Vector leftBorder,
            Vector rightBorder,
            double accuracy = 1e-5,
            int maxIterations = 1000)
        {
            
            Vector left, right;
            double funcLeft, funcRight;

            int fibBuff = 0, fib_1 = 1, fib_2 = 1;
            NumericUtils.ClosestFibonacciPair((rightBorder - leftBorder).Magnitude / accuracy, ref fib_1, ref fib_2);

            left = leftBorder + (rightBorder - leftBorder) * ((double)(fib_2 - fib_1) / fib_2);
            right = leftBorder + (rightBorder - leftBorder) * ((double)fib_1 / fib_2);

            funcLeft = func(left);
            funcRight = func(right);

            fibBuff = fib_2 - fib_1;
            fib_2 = fib_1;
            fib_1 = fibBuff;

            int iterationsCount = 0;
            while (fib_1 != fib_2)
            {
                ++iterationsCount;

                if ((rightBorder - leftBorder).Magnitude < accuracy) break;
                
                if (func(leftBorder) > func(rightBorder))
                {
                    leftBorder = left;
                    funcLeft = funcRight;
                    left = right;
                    right = leftBorder + (rightBorder - leftBorder) * ((double)fib_1 / fib_2);
                    funcRight = func(right);
                }
                else
                {
                    rightBorder = right;
                    funcRight = funcLeft;
                    right = left;
                    left = leftBorder + (rightBorder - leftBorder) * ((double)(fib_2 - fib_1) / fib_2);
                    funcLeft = func(left);
                }
                fibBuff = fib_2 - fib_1;
                fib_2 = fib_1;
                fib_1 = fibBuff;
            }
#if DEBUG
            Console.Write($" Fibonacci::  iterations {iterationsCount}\n");
            Console.Write($" Fibonacci::  func probes {iterationsCount + 2}\n");
#endif
            return (rightBorder + leftBorder) * 0.5;
        }


        //-----------------------------------{Покоординатный спуск}-----------------------------------
        public static Vector CoordinateDescent(
            FunctionND func,
            Vector startDot,
            double accuracy = 1e-5,
            int maxIterations = 1000)
        {
            Vector right;
            Vector left = right = startDot;

            double step = 1.0;

            double x_i, y_1, y_0;

            int opt_coord_n = 0, coord_id;

            int i = 0;

            for (; i < maxIterations; i++)
            {
                coord_id = i % left.Count;

                right[coord_id] -= accuracy;

                y_0 = func(right);

                right[coord_id] += 2 * accuracy;

                y_1 = func(right);

                right[coord_id] -= accuracy;

                right[coord_id] = y_0 > y_1 ? right[coord_id] += step : right[coord_id] -= step;

                x_i = left[coord_id];

                right = HalfDevide(func, left, right, accuracy, maxIterations);

                left = new Vector(right);

                if (Math.Abs(right[coord_id] - x_i) < accuracy)
                {
                    opt_coord_n++;

                    if (opt_coord_n == right.Count)
                        break;
                    continue;
                }
                opt_coord_n = 0;
            }
#if DEBUG
            Console.Write($" CoordinateDescent::  iterations {i}\n");
#endif
            return left;
        }
    }
}