using ClassLibrary2;
using System;
using System.Xml;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;



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

            Console.WriteLine($"Имя животного: {animalName}"); // Выведете имя животного leva

            Lion leva = new Lion("RU", false, "kolia", eClassificationAnimal.Carnivores);
            Console.WriteLine(leva.Country);
            leva.SayHello();
            Console.WriteLine(leva.GetFavouriteFood());

            Pig pig1 = new Pig("RU", false, "Nusha", eClassificationAnimal.Omnivores);
            pig1.SayHello();
            pig1.GetFavouriteFood();
            pig1.GetClassificationAnimal();
            string pigname;
            pig1.Deconstruct(out pigname);




            var root = new XElement("Classes");


            foreach (var cls in Assembly.LoadFrom("ClassLibrary2.dll").GetTypes().Where(t => t.IsClass))
            {
                var commentAttr = cls.GetCustomAttribute<CommentClass>();
                var comment = commentAttr?.comment ?? "без комментариев";

                var classElement = new XElement("Class",
                    new XAttribute("Name", cls.Name),
                    new XAttribute("Comment", comment));

                root.Add(classElement);
            }

            var xmlFilePath = "Classes.xml";
            root.Save(xmlFilePath);

            Console.WriteLine($"XML файл сохранен: {xmlFilePath}");



            Lion lion2 = new Lion("RU", false, "lek", eClassificationAnimal.Carnivores);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Lion));
            using (FileStream fs = new FileStream("lion.xml", FileMode.Open))
            {
                xmlSerializer.Serialize(fs, lion2);

                Console.WriteLine("Object has been serialized");
            }



        }




    }




}
