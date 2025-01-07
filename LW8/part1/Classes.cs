using System;
using System.Diagnostics;

namespace ClassLibrary2
{

    [AttributeUsage(AttributeTargets.Class)]
    //определяет пользовательский атрибут CommentAtt и указывает, что атрибут может применяться только к классам (AttributeTargets.Class)
    public class CommentClass : Attribute
    {
        public string comment { get; set; }

        public CommentClass(string comment)
        {
            this.comment = comment;
        }
    }

    public enum eClassificationAnimal
    {
        Herbivores,
        Carnivores,
        Omnivores
    }

    public enum eFavouriteFood
    {
        Meat,
        Plants,
        Everything
    }

    [CommentClass("Абстрактный класс, созданный для описания основных методов и полей животных.")]
    public abstract class Animal
    {
        public string Country { get; set; }
        public bool HideFromOtherAnimals { get; set; }
        public string Name { get; set; }
        public eClassificationAnimal WhatAnimal { get; set; }

        public Animal(string Country, bool HideFromOtherAnimals, string Name, eClassificationAnimal WhatAnimal)
        {
            this.Country = Country;
            this.HideFromOtherAnimals = HideFromOtherAnimals;
            this.Name = Name;
            this.WhatAnimal = WhatAnimal;
        }

        public abstract void SayHello();

        public abstract eFavouriteFood GetFavouriteFood();

        public void Deconstruct(out string out_name)
        {
            out_name = Name;
        }

        public eClassificationAnimal GetClassificationAnimal()
        {
            return this.WhatAnimal;
        }

    }

    [CommentClass("Класс описания коровы")]
    public class Cow : Animal
    {
        public Cow() : base("", true, "", eClassificationAnimal.Herbivores)
        {

        }

        public Cow(string country, bool value, string description, eClassificationAnimal WhatAnimal) :
        base(country, value, description, WhatAnimal)
        { }

        public override eFavouriteFood GetFavouriteFood()
        {
            return eFavouriteFood.Plants;
        }

        public override void SayHello()
        {
            Console.WriteLine("Mooo");
        }
    }

    [CommentClass("Класс описания льва")]
    public class Lion : Animal
    {
        public Lion() : base("", false, "", eClassificationAnimal.Carnivores)
        {
        }

        public Lion(string country, bool value, string description, eClassificationAnimal WhatAnimal) :
        base(country, value, description, WhatAnimal)
        { }


        public override eFavouriteFood GetFavouriteFood()
        {
            return eFavouriteFood.Meat;
        }

        public override void SayHello()
        {
            Console.WriteLine("RRRRRRR");
        }
    }

    [CommentClass("Класс описания свиньи")]
    public class Pig : Animal
    {
        public Pig(string country, bool value, string description, eClassificationAnimal WhatAnimal) :
        base(country, value, description, WhatAnimal)
        { }

        public override eFavouriteFood GetFavouriteFood()
        {
            return eFavouriteFood.Everything;
        }

        public override void SayHello()
        {
            Console.WriteLine("Hru Hru");
        }
    }


}