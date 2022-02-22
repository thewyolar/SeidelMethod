using System;

namespace SeidelMethod
{
    class Program
    {
        //Метод, печатающий массив.
        public static void PrintArray(double[] Array)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                Console.WriteLine($"{Array[i]} ");
            }
        }

        //Метод, печатающий матрицу.
        public static void PrintMatrix(double[,] Matrix)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    Console.Write($"{Math.Round(Matrix[i, j], 10)} \t");
                }
                Console.WriteLine();
            }
        }

        //Метод для преобразования матрицы B.
        public static double[,] PB(double[,] A, int n)
        {
            double[,] B = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        B[i, j] = 0;
                    else if (i != j)
                    {
                        B[i, j] = -A[i, j] / A[i, i];
                    }
                }
            }

            return B;
        }

        //Метод для преобразования вектора c.
        public static double[] Pc(double[,] A, double[] b, int n)
        {
            double[] c = new double[n];

            for (int i = 0; i < n; i++)
            {
                c[i] = b[i] / A[i, i];
            }

            return c;
        }

        //Метод для нахождения макс. нормы матрицы.
        public static double Normi(double[,] Matrix)
        {
            double[] sum = new double[Matrix.GetLength(0)];
            double max = 0;

            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    sum[i] += Math.Abs(Matrix[i, j]);
                }
            }

            for (int i = 0; i < sum.Length; i++)
            {
                if (sum[i] > max)
                    max = sum[i];
            }

            return max;
        }

        /** Функция, реализующая метод последующих замещений (Гаусса - Зейделя).
         * B - матрица коэффициентов исходного уравнения;
         * c - вектор правой части исходного уравнения;
         * n - порядок матрицы B;
         * k - число итераций;
         * x0 - вектор начального прибилжения;
        */
        public static double[,] SeidelMethod(double[,] B, double[] c, int n, int k, double[] x0)
        {
            double[,] rez = new double[n, k];
            double[] y = x0;

            for (int m = 1; m <= k; m++)
            {
                double[] x = y;

                for (int i = 1; i <= n; i++)
                {
                    int j = 1;
                    double u = 0;

                    while (1 <= j && j < i)
                    {
                        u += B[i - 1, j - 1] * y[j - 1];
                        j += 1;
                    }

                    j = i + 1;

                    while (i < j && j <= n)
                    {
                        u += B[i - 1, j - 1] * x[j - 1];
                        j += 1;
                    }

                    y[i - 1] = u + c[i - 1];
                }

                for (int i = 1; i <= n; i++)
                {
                    rez[i - 1, m - 1] = y[i - 1];
                }
            }

            return rez;
        }

        static void Main(string[] args)
        {
            double[,] A = new double[4, 4] { { 118.8, -14, -5, 5 }, { -59.4, 194, 5, 5 }, { 148.5, 12, -310, 5 }, { 0, 18.5, 90, 18 } };
            double[] b = new double[4] { -92.5, -340.1, -898, 184.1 };
            double[] x0 = new double[4] { 0, -1, 1.2, 2 };

            double[,] B = PB(A, 4);
            Console.WriteLine("Матрица B:");
            PrintMatrix(B);

            double[] c = Pc(A, b, 4);
            Console.WriteLine("\nВектор с:");
            PrintArray(c);

            Console.WriteLine("\nПроверка достаточного условия сходимости Зейделя:");
            Console.WriteLine($"Normi(B) = {Normi(B)} - достаточное условие сходимости выполнено");

            Console.WriteLine("\nМетод Зейделя:");
            double[,] result = SeidelMethod(B, c, 4, 10, x0);
            PrintMatrix(result);

            Console.ReadKey();
        }
    }
}