using System;
using System.Collections;
using System.ComponentModel.Design;

namespace CsharProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int[] array = { 1, 2, 3, 5};
            MyList<int> list = new MyList<int>(array);
            list.Print();
            Console.WriteLine();

            list.Add(10);
            list.Print();
            Console.WriteLine();

            int value = list[3];
            Console.WriteLine(value);
            Console.WriteLine();
        }



    }




    public class MyList<T> : IEnumerable<T>
    {
        public T[] massiv; 
        private int _defaultCapacity = 4; 
        public int _count { get; set; } = 0; 
        
        public MyList()
        {
            massiv = new T[_defaultCapacity]; 
        }
        
        public MyList(params T[] massiv)
        {
            while (massiv.Length > _defaultCapacity)
            {
                Resize(); 
            }

            _count = massiv.Length;
            this.massiv = massiv;
        }

        public void Add(T item)
        {

            if (_count >= _defaultCapacity)
            {
                Resize();
            }

            massiv[_count] = item;

            _count++;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new IndexOutOfRangeException();
                }
                return massiv[index];
            }
        }

        public void Print()
        {
            for (int i = 0; i < _count; i++)
            {
                Console.Write(massiv[i] + " ");
            }
            Console.WriteLine();
        }

        private void Resize()
        {
            _defaultCapacity *= 2;

            T[] newMassiv = new T[_defaultCapacity];

            Array.Copy(massiv, newMassiv, _count);

            // Заменяем старый массив новым
            massiv = newMassiv;
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return massiv[i];
            }
        }

        // Необходимый метод для интерфейса IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }





}