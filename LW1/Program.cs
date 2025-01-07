
using System; 
using System.Collections.Generic; 
using System.Data; 
using System.Globalization; 
using System.Runtime.InteropServices.ObjectiveC; 
 
class SimplexMethod
{
    static void Main(string[] args)
    {
        // double[] c = { 1, -1 }; // Коэффициенты целевой функции 
        // List<List<double>> A = new List<List<double>> 
        // { 
        //     new List<double> { 1, -2, 1}, 
        //     new List<double> { -2, 1 }, 
        //     new List<double> { 1, 1} 
        // }; // Коэффициенты ограничений 
        // double[] b = { 2, -2, 5 }; // Ограничения 
        // double[] c = {8,6,2}; // Коэффициенты целевой функции 
        // List<List<double>> A = new List<List<double>> 
        // { 
        //     new List<double> { 2,1,1}, 
        //     new List<double> { 1,4,0 },  
        //     new List<double> { 0, 0.5,1} 
        // // }; // Коэффициенты ограничений 
        // // double[] b = { 4, 3, 6 }; // Ограничения 
        // double[] c = { 6, 8, 5 }; // Коэффициенты целевой функции 
        // List<List<double>> A = new List<List<double>> 
        // { 
        //     new List<double> { 4, 1, 1}, 
        //     new List<double> { 1, 3, 0}, 
        //     new List<double> { 0, 0.5, 3} 
        // }; // Коэффициенты ограничений 
        // double[] b = { 3, 4, 5}; // Ограничения 

        double[] c = { 5, 6, 4 }; // Коэффициенты целевой функции 
        List<List<double>> A = new List<List<double>>
        {
            new List<double> { 1, 1, 1},
            new List<double> { 1, 3, 0},
            new List<double> { 0, 0.5, 4}
        }; // Коэффициенты ограничений 
        double[] b = { 7, 8, 6 }; // Ограничения 

        string goal = "max"; // Цель ("min" или "max") 
        int maxlength = -1;

        for (int i = 0; i < A.Count; i++)
        {
            maxlength = Math.Max(maxlength, A[i].Count);
        }

        Simplex(c, A, b, goal, maxlength);
        Console.WriteLine();
        GenerateDualfunction(c, A, b, goal);
    }
    static void PrintTableau(double[,] tableau, int rows, int cols, string[] base_var, string[] free_var)
    {
        Console.Write("   ");

        for (int i = 0; i < base_var.Length; i++)
        {
            Console.Write("        " + base_var[i]);
        }

        Console.WriteLine();

        for (int i = 0; i < rows; i++)
        {
            Console.Write(free_var[i]);
            for (int j = 0; j < cols - 1; j++)
            {

                Console.Write($"{tableau[i, j],10:F2} ");
            }
            Console.WriteLine();
        }
    }
    // Метод для составления двойственной задачи 
    static void GenerateDualfunction(double[] c, List<List<double>> A, double[] b, string goal)
    {
        // Двойственная задача имеет инвертированную цель 
        string dualGoal = goal == "max" ? "min" : "max";

        // Число переменных в двойственной задаче равно числу ограничений в исходной 
        int numDualVars = b.Length;

        // Число ограничений в двойственной задаче равно числу переменных исходной задачи 
        int numDualConstraints = c.Length;

        // Коэффициенты целевой функции двойственной задачи — это вектор ограничений исходной задачи (b) 
        double[] dualObjective = new double[numDualVars];
        for (int i = 0; i < numDualVars; i++)
        {
            dualObjective[i] = b[i];
        }

        // Коэффициенты ограничений двойственной задачи — это транспонированная матрица A 
        List<List<double>> dualConstraints = new List<List<double>>();
        for (int i = 0; i < numDualConstraints; i++)
        {
            List<double> dualRow = new List<double>();
            for (int j = 0; j < numDualVars; j++)
            {
                dualRow.Add(A[j][i]);
            }
            dualConstraints.Add(dualRow);
        }

        // Правая часть ограничений двойственной задачи — это коэффициенты целевой функции исходной задачи (c) 
        double[] dualR = new double[c.Length];
        for (int i = 0; i < c.Length; i++)
        {

dualR[i] = c[i];
        }

        // Печатаем двойственную задачу 
        Console.Write("Целевая функция:");
        string ObjectiveC = "";
        for (int i = 0; i < dualObjective.Length; i++)
        {
            ObjectiveC += $"{dualObjective[i]} x{i + 1} ";
            if (i < dualObjective.Length - 1) ObjectiveC += "+ ";
        }
        Console.Write($" F = {ObjectiveC}");
        Console.WriteLine();

        Console.WriteLine("Ограничения:");
        for (int i = 0; i < dualConstraints.Count; i++)
        {
            string constraint = "";
            for (int j = 0; j < dualConstraints[i].Count; j++)
            {
                constraint += $"{dualConstraints[i][j]} y{j + 1} ";
                if (j < dualConstraints[i].Count - 1) constraint += "+ ";
            }
            Console.WriteLine($"{constraint} <= {dualR[i]}");
        }
        Console.WriteLine();

        int maxlength = -1;
        for (int i = 0; i < dualConstraints.Count; i++)
        {
            maxlength = Math.Max(maxlength, dualConstraints[i].Count);
        }

        Simplex(dualObjective, dualConstraints, dualR, dualGoal, maxlength);

    }

    //Симплекс метод 
    static void Simplex(double[] c, List<List<double>> A, double[] b, string goal, int maxlength)
    {
        int m = b.Length; // количество ограничений 
        int n = c.Length; // количество переменных 
        int rows = m + 1; // строки таблицы (включая строку функции F) 
        int cols = n + 2; // столбцы таблицы (включая столбец свободных членов) 

        double[,] tableau = new double[rows, cols];
        string[] base_var = new string[c.Length + 1];
        string[] free_var = new string[A.Count + 1];
        // Заполнение обозначния переменных 
        base_var[0] = "s0";

        for (int i = 1; i < c.Length + 1; i++)
        {
            base_var[i] = $"x{i}";
        }

        int index = c.Length + 1;

        for (int i = 0; i < A.Count; i++)
        {
            free_var[i] = $"x{index}";
            index += 1;
        }
        free_var[^1] = "F ";

        // Инициализация симплекс-таблицы 
        for (int i = 0; i < m; i++)
        {
            tableau[i, 0] = b[i]; // столбец s_0 — свободные члены 
            if (A[i].Count == maxlength)
            {
                for (int j = 1; j <= A[i].Count; j++)
                {
                    tableau[i, j] = A[i][j - 1];
                }
            }
            else
            {
                for (int j = 1; j < A[i].Count + 1; j++)
                {
                    tableau[i, j] = A[i][j - 1];
                }

            }

        }

        for (int j = 0; j < n; j++)
        {
            tableau[rows - 1, j + 1] = c[j]; // коэффициенты целевой функции 
        }

        PrintTableau(tableau, rows, cols, base_var, free_var);
        Console.WriteLine();

        while (true)
        {
            int pivotCol = FindPivotColumn(tableau, rows, cols);
            if (pivotCol == -1) break; // Оптимальное решение найдено 

            int pivotRow = FindPivotRow(tableau, rows, pivotCol);
            if (pivotRow == -1)
            {
                Console.WriteLine("Неограниченное решение");
                return;
            }
            string old_base_var = base_var[pivotCol];
            base_var[pivotCol] = free_var[pivotRow];
            free_var[pivotRow] = old_base_var;
            PivotOperation(tableau, rows, cols, pivotRow, pivotCol, base_var, free_var);
        }

        PrintSolution(tableau, rows, cols, n, goal, base_var, free_var);
    }

    static int FindPivotColumn(double[,] tableau, int rows, int cols)
    {
        int pivotCol = -1;

        for (int j = 1; j < cols; j++) // Пропускаем первый столбец (свободные члены) 
        {
            if (tableau[rows - 1, j] > 0)
            {
                pivotCol = j;
                return pivotCol;
            }
        }
        return pivotCol;
    }

    static int FindPivotRow(double[,] tableau, int rows, int pivotCol)

{ 
        int pivotRow = -1;
    double minRatio = double.PositiveInfinity; 
 
        for (int i = 0; i<rows - 1; i++) 
        { 
            double ratio = tableau[i, 0] / tableau[i, pivotCol]; // Используем первый столбец (свободные члены) 
            if (0 < ratio && ratio<minRatio) 
            { 
                minRatio = ratio; 
                pivotRow = i; 
            } 
        } 
 
        return pivotRow; 
    } 
 
    static void PivotOperation(double[,] tableau, int rows, int cols, int pivotRow, int pivotCol, string[] base_var, string[] free_var)
{
    double[,] tableau_old = (double[,])tableau.Clone();

    double pivotValue = tableau[pivotRow, pivotCol];

    // Нормализуем ведущую строку 
    for (int j = 0; j < cols; j++)
    {
        if (j == pivotCol)
        {
            tableau[pivotRow, j] = 1 / pivotValue;
        }
        else
        {
            tableau[pivotRow, j] /= pivotValue;
        }

    }

    // Нормализуем ведущий столбец 
    for (int i = 0; i < rows; i++)
    {
        if (i != pivotRow)
        {
            tableau[i, pivotCol] /= -pivotValue;
        }

    }

    // Обновляем остальные строки 
    for (int i = 0; i < rows; i++)
    {
        if (i != pivotRow)
        {
            double factor = tableau_old[i, pivotCol];
            for (int j = 0; j < cols; j++)
            {
                if (j != pivotCol)
                {
                    tableau[i, j] -= factor * tableau_old[pivotRow, j] / pivotValue;
                }
            }
        }
    }
    PrintTableau(tableau, rows, cols, base_var, free_var);
    Console.WriteLine();
}

static void PrintSolution(double[,] tableau, int rows, int cols, int n, string goal, string[] base_var, string[] free_var)
{
    Console.WriteLine(goal == "max" ? "Максимизация" : "Минимизация");
    double optimalValue = goal == "max" ? -tableau[rows - 1, 0] : tableau[rows - 1, 0];
    Console.WriteLine("Оптимальное значение целевой функции: " + optimalValue);

    PrintTableau(tableau, rows, cols, base_var, free_var);

    for (int i = 1; i < base_var.Length; i++)
    {
        Console.WriteLine($"{base_var[i]} = 0");
    }

    for (int i = 0; i < free_var.Length - 1; i++)
    {
        Console.WriteLine($"{free_var[i]} = {tableau[i, 0]}");
    }
} 
}