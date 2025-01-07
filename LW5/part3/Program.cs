using System;
using System.Collections;
using System.Collections.Generic;


namespace CsharpProject
{
    public class Program
    {
       

        public static void Main(string[] args)
        {
            int[] intArray = new int[1] { 4 };
            string[] stringArray = new string[1] { "Potato" };
            MyDictionary<int,string> Massiv = new MyDictionary<int, string> (intArray,stringArray);
            int y = Massiv.counter;
            Console.WriteLine (y);
            Massiv.Add(3, "rur");
            Massiv.Add(1, "Apple");
            Massiv.Add(2, "Banana");
            Console.WriteLine(Massiv[1]);
            Console.WriteLine(Massiv[2]);
            Console.WriteLine(Massiv[3]);
            Console.WriteLine(Massiv[4]);
        }


    }




    public class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public int counter { get; set; }
        public TKey[] keysArray;
        public TValue[] valuesArray;

        public MyDictionary()
        {
            counter = 0;
            keysArray = new TKey[0];
            valuesArray = new TValue[0];
        }

        public MyDictionary(TKey[] keys, TValue[] values)
        {
            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Количество ключей и значений должно быть одинаковым");
            }

            counter = keys.Length;
            keysArray = keys;
            valuesArray = values;
        }

        public void Add(TKey key, TValue value)
        {
            if (counter == keysArray.Length)
            {
                Resize();
            }

            keysArray[counter] = key;
            valuesArray[counter] = value;
            counter++;
        }

        private void Resize()
        {
            int newSize = counter * 2;
            TKey[] newKeysArray = new TKey[newSize];
            TValue[] newValuesArray = new TValue[newSize];
            Array.Copy(keysArray, newKeysArray, counter);
            Array.Copy(valuesArray, newValuesArray, counter);
            keysArray = newKeysArray;
            valuesArray = newValuesArray;
        }

        public TValue this[TKey key]
        {
            get
            {
                for (int i = 0; i < counter; i++)
                {
                    if (keysArray[i].Equals(key))
                    {
                        return valuesArray[i];
                    }
                }

                throw new KeyNotFoundException();
            }
           
        }






        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < counter; i++)
            {
                yield return new KeyValuePair<TKey, TValue>(keysArray[i], valuesArray[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }







}

