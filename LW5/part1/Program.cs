using System;
using System.Data;

namespace CsharpProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Введите число 1:");
            string input = Console.ReadLine(); // Читаем строку с клавиатуры

            int l;
            if (!int.TryParse(input, out l))
            {
                Console.WriteLine("Неверный ввод. Введите целое число.");
                return;
            }

            Console.WriteLine("Введите число 2:");
            string input2 = Console.ReadLine(); // Читаем строку с клавиатуры

            int k;
            if (!int.TryParse(input2, out k))
            {
                Console.WriteLine("Неверный ввод. Введите целое число.");
                return;
            }

            if (l >= k)
            {
                Console.WriteLine("Минимальное число должно быть меньше максимального.");
                return;
            }

            MyMatrix obj = new(3, 2, l, k);
            obj.Show();
            obj.Fill(obj);
            obj.Show();
            obj.ChangeSize(2,5);
            obj.Show();
            obj.ShowPartialy(0,1,1,3);
            Console.WriteLine();
            int subject1 = obj[1, 2];
            Console.WriteLine(subject1);
            Console.WriteLine();
            int value = 3;
            obj[1, 3] = value;
            obj.Show();
        }



    }




    public class MyMatrix
    {
        public int[,] matrix;
        public int m;
        public int n;

        public MyMatrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            this.matrix = new int[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        public MyMatrix(int m, int n, int l, int k)
        {
            this.m = m;
            this.n = n;
            this.matrix = new int[m, n];

            Random random = new Random();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int r = random.Next(l, k);
                    matrix[i, j] = r;
                }
            }
        }


        public void Fill(MyMatrix obj)
        {
            Random random = new Random();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int r = random.Next();
                    matrix[i, j] = r;
                }
            }


        }


        public void Show()
        {
            for (int i = 0; i < m; i++)
            {
                Console.Write("(");
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.Write(")");
                Console.WriteLine();
            }
            Console.WriteLine();
        }



        public void ChangeSize(int rows,int cols)
        {
            if(rows<=0 || cols<=0)
            {
                throw new ArgumentException("Rows and cols must be positive.");
            }
            if(rows==m && cols == m)
            {
                return;
            }
            int[,] newMatrix = new int[rows, cols];
            for (int i = 0; i < Math.Min(rows, m); i++)
            {
                for (int j = 0; j < Math.Min(cols, n); j++)
                {
                    
                    newMatrix[i, j] = matrix[i, j];

                }
            }



            Random random = new Random();
            for (int i = Math.Min(rows, m); i < rows; i++)
            {
                for (int j = Math.Min(cols, n); j < cols; j++)
                {
                    int r = random.Next();
                        matrix[i, j] = r;
                    
                }
            }
   
            matrix = newMatrix;

            m = rows;
            n = cols;
        }

        public void ShowPartialy(int r1, int r2, int c1, int c2)
        {

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (r1<=i && i<= r2 && j>=c1 && j<=c2)
                    {
                        Console.Write(matrix[i, j] + " ");
                    }
                }
                Console.WriteLine();
            }
        }


        public int this[int index1, int index2]
        {
            
            get
            {
                return this.matrix[index1, index2];
            }
            set
            {
                this.matrix[index1, index2] = value;
            }
        }


    }




}



