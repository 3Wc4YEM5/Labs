using System;
using ClassLibrary1;
using System.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace CsharpProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Cow cow1 = new Cow("US", false, "leva", eClassificationAnimal.Herbivores);
            cow1.SayHello();
            string animalName;
            cow1.Deconstruct(out animalName);

            Console.WriteLine($"Имя животного: {animalName}"); // Выведет: "Имя животного: Rex"

            Lion leva = new Lion("King Lion", false, "leva", eClassificationAnimal.Carnivores);
            Console.WriteLine(leva.Country);
            leva.SayHello();





            var root = new XElement("Classes");


            foreach (var cls in Assembly.LoadFrom("ClassLibrary1.dll").GetTypes().Where(t => t.IsClass))
            {
                var commentAttr = cls.GetCustomAttribute<CommentClass>();
                var comment = commentAttr?.comment ?? "Нет комментария";

                var classElement = new XElement("Class",
                    new XAttribute("Name", cls.Name),
                    new XAttribute("Comment", comment));

                root.Add(classElement);
            }

            var xmlFilePath = "Classes.xml";
            root.Save(xmlFilePath);

            Console.WriteLine($"XML файл сохранен: {xmlFilePath}");

        }




    }








}




