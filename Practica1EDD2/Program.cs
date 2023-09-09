using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Practica1EDD2.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Practica1EDD2
{
    class Program
    {

        public class CsvReader
        {
            public List<List<string>> ReadCsv(string filePath)
            {
                List<List<string>> csvData = new List<List<string>>();

                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        if (fields != null)
                        {
                            List<string> rowData = new List<string>(fields);
                            csvData.Add(rowData);
                        }
                    }
                }

                return csvData;
            }
        }
        static void Main(string[] args)
        {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            List<Person> personas = new List<Person>();
            BTree<Person> b = new BTree<Person>(20);

            string filePath = @"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2\lab1inge.csv";

            CsvReader csvReader = new CsvReader();
            List<List<string>> csvData = csvReader.ReadCsv(filePath);

            // Ahora puedes trabajar con los datos del archivo CSV.
            foreach (List<string> row in csvData)
            {


                foreach (string cell in row)
                {
                    var line = cell;
                    var values = line.Split(';');

                    listA.Add(values[0]);
                    listB.Add(values[1]);
                }



            }
            foreach (var item in listB)
            {
                Person input = JsonConvert.DeserializeObject<Person>(item)!;
                //Console.WriteLine($"Nombre: {input.Name}" + " " + $"Id: {input.Id}" + " " + $"Fecha: {input.BirthDate}" + " " + $"Direccion: {input.Address}" + " ");
                //Console.WriteLine("----------------------");
                //Console.WriteLine("");
                personas.Add(input);
            }

       

            for (int i = 0; i < listA.Count; i++)
            {
                Console.WriteLine("Accion: " + listA[i] + " " + $"Nombre: {personas[i].Name}" + " " + $"Id: {personas[i].Id}" + " " + $"Fecha: {personas[i].BirthDate}" + " " + $"Direccion: {personas[i].Address}" + " ");
                if (listA[i] == "INSERT")
                {
                    b.Insert(personas[i]);
                    //b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                }
                else if (listA[i] == "PATCH")
                {
                    DateTime dt = new DateTime();
                    //if (personas[i].BirthDate != dt && personas[i].Address != "")
                    if (personas[i].BirthDate != "" && personas[i].Address != "")
                    {
                        b.UpdatePersonInfo(personas[i].Name, personas[i].Id, personas[i].Address, personas[i].BirthDate);
                    }
                    else if (personas[i].BirthDate == "" && personas[i].Address != "")
                    //else if (personas[i].BirthDate == dt && personas[i].Address != "")
                    {
                        b.UpdatePersonInfo(personas[i].Name, personas[i].Id, personas[i].Address, null);
                    }
                    //else if (personas[i].BirthDate != dt && personas[i].Address == "")
                    else if (personas[i].BirthDate != "" && personas[i].Address == "")
                    {
                        
                        b.UpdatePersonInfo(personas[i].Name, personas[i].Id, null, personas[i].BirthDate);
                    }
                }
                else if (listA[i] == "DELETE")
                {
                    b.RemovePerson(personas[i].Name, personas[i].Id);

                    //b.Remove(personas[i]);
                    //b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                }
            }

            Console.WriteLine("");
          
            

            



            bool showMenu;
            bool showMenu2;
            bool showMenu3;
            bool showMenu4;
            bool showMenu5;
            //bool showMenu6;
            bool showMenuPrincipal = true;

            while (showMenuPrincipal)
            {
                Console.Clear();
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1) Insert person");
                Console.WriteLine("2) Delete person");
                Console.WriteLine("3) Update person");
                Console.WriteLine("4) Search person");
                Console.WriteLine("5) Show B Tree");
                Console.Write("\r\nEnter an option: ");
                Console.Write("");

                switch (Console.ReadLine())
                {
                    case "1":
                        showMenu = true;
                        Person persona = new Person();
                        while (showMenu)
                        {

                            Console.WriteLine("Enter name or type exit: ");

                            string userName = Console.ReadLine();

                            if (userName == "exit")
                            {
                                break;
                            }
                            else
                            {
                                persona.Name = userName;
                            }

                            Console.WriteLine("Enter Id or type exit: ");

                            string userId = Console.ReadLine();

                            if (userId == "exit")
                            {
                                break;
                            }
                            else
                            {
                                persona.Id = long.Parse(userId);
                            }

                            Console.WriteLine("Enter date of birth yyyy-mm-dd or type exit: ");

                            string userBirthDate = Console.ReadLine();

                            if (userBirthDate == "exit")
                            {

                                break;
                            }
                            else
                            {
                                if (userBirthDate != "")
                                {
                                    //DateTime dateTime = DateTime.Parse(userBirthDate);
                                    //persona.BirthDate = dateTime;
                                    persona.BirthDate = userBirthDate;
                                }
                                else
                                {

                                }

                            }

                            Console.WriteLine("Enter address or type exit: ");

                            string userAddress = Console.ReadLine();

                            if (userAddress == "exit")
                            {

                                break;
                            }
                            else
                            {
                                if (userAddress != "")
                                {
                                    persona.Address = userAddress;
                                }
                                else
                                {

                                }

                            }



                            b.Insert(persona);
                            Console.WriteLine("Person added successfully");
                            b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                            System.Threading.Thread.Sleep(1500);
                            showMenu = false;

                        }


                        break;
                    case "2":

                        showMenu2 = true;
                        while (showMenu2)
                        {
                            string thisName;
                            long thisId;
                            Console.WriteLine("Enter the name of the person or type exit: ");

                            string userName = Console.ReadLine();
                            if (userName == "exit")
                            {
                                break;
                            }
                            else
                            {
                                thisName = userName;

                            }

                            Console.WriteLine("Enter the id of the person or type exit: ");

                            string userId = Console.ReadLine();
                            if (userId == "exit")
                            {
                                break;
                            }
                            else
                            {
                                thisId = long.Parse(userId);

                            }

                            bool exitoso = b.RemovePerson(thisName, thisId);
                            if (exitoso)
                            {
                                Console.WriteLine("Person deleted successfully");
                                b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                                System.Threading.Thread.Sleep(1500);
                                showMenu2 = false;
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(1500);
                                showMenu2 = false;
                            }

                        }
                        break;
                    case "3":
                        showMenu3 = true;
                        Person personaActualizar = new Person();
                        while (showMenu3)
                        {
                            string thisName;
                            long thisId;

                            Console.WriteLine("Enter the name of the person or type exit: ");

                            string userName = Console.ReadLine();
                            if (userName == "exit")
                            {
                                break;
                            }
                            else
                            {
                                thisName = userName;

                            }

                            Console.WriteLine("Enter the id of the person or type exit: ");

                            string userId = Console.ReadLine();
                            if (userId == "exit")
                            {
                                break;
                            }
                            else
                            {
                                thisId = long.Parse(userId);

                            }

                            showMenu4 = true;
                            while (showMenu4)
                            {
                                Console.WriteLine("");
                                //Console.Clear();
                                Console.WriteLine("Choose an option: ");
                                Console.WriteLine("1) Update date of birth");
                                Console.WriteLine("2) Update address");
                                Console.WriteLine("3) Update date of birth and address");
                                Console.WriteLine("4) Exit");
                                Console.Write("\r\nEnter an option: ");

                                switch (Console.ReadLine())
                                {
                                    case "1":

                                        Console.WriteLine("Enter  date of birth yyyy-mm-dd or type exit: ");

                                        string userDateInside = Console.ReadLine();

                                        if (userDateInside == "exit")
                                        {
                                            break;
                                        }

                                        DateTime dateTimeInside = DateTime.Parse(userDateInside);
                                        //bool exitosoDate = b.UpdatePersonInfo(thisName, thisId, null, dateTimeInside);
                                        bool exitosoDate = b.UpdatePersonInfo(thisName, thisId, null, userDateInside);
                                        if (exitosoDate)
                                        {
                                            Console.WriteLine("Person updated successfully");
                                            b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                                            System.Threading.Thread.Sleep(1500);
                                            showMenu3 = false;
                                        }
                                        else
                                        {
                                            System.Threading.Thread.Sleep(1500);
                                            showMenu3 = false;
                                        }


                                        break;
                                    case "2":
                                        Console.WriteLine("Enter address or type exit: ");

                                        string userAddressInside = Console.ReadLine();

                                        if (userAddressInside == "exit")
                                        {
                                            break;
                                        }


                                        bool exitosoAddress = b.UpdatePersonInfo(thisName, thisId, userAddressInside, null);
                                        if (exitosoAddress)
                                        {
                                            Console.WriteLine("Person updated successfully");
                                            b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                                            System.Threading.Thread.Sleep(1500);
                                            showMenu3 = false;
                                        }
                                        else
                                        {
                                            System.Threading.Thread.Sleep(1500);
                                            showMenu3 = false;
                                        }


                                        break;
                                    case "3":
                                        Console.WriteLine("Enter  date of birth yyyy-mm-dd or type exit: ");

                                        string userDateInsideBoth = Console.ReadLine();

                                        if (userDateInsideBoth == "exit")
                                        {
                                            break;
                                        }

                                        Console.WriteLine("Enter address or type exit: ");

                                        string userAddressInsideBoth = Console.ReadLine();

                                        if (userAddressInsideBoth == "exit")
                                        {
                                            break;
                                        }

                                        DateTime dateTimeInsideBoth = DateTime.Parse(userDateInsideBoth);
                                        //bool exitosoBoth = b.UpdatePersonInfo(thisName, thisId, userAddressInsideBoth, dateTimeInsideBoth);
                                        bool exitosoBoth = b.UpdatePersonInfo(thisName, thisId, userAddressInsideBoth, userDateInsideBoth);
                                        if (exitosoBoth)
                                        {
                                            Console.WriteLine("Person updated successfully");
                                            b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                                            System.Threading.Thread.Sleep(1500);
                                            showMenu3 = false;
                                        }
                                        else
                                        {
                                            System.Threading.Thread.Sleep(1500);
                                            showMenu3 = false;
                                        }


                                        break;
                                    case "4":
                                        showMenu4 = false;
                                        break;

                                    default:


                                        break;
                                }

                                Console.WriteLine("");
                            }




                        }


                        break;
                    case "4":

                        showMenu5 = true;

                        while (showMenu5)
                        {
                            Console.WriteLine("Enter name or type exit: ");

                            string userNameSearch = Console.ReadLine();

                            if (userNameSearch == "exit")
                            {
                                break;
                            }
                            else
                            {
                                string searchName = userNameSearch;
                                List<Person> searchResults = b.SearchByName(searchName);

                                if (searchResults.Count > 0)
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("Bitacora:");
                                    foreach (Person person in searchResults)
                                    {

                                        Console.WriteLine($"Nombre: {person.Name}, DPI: {person.Id}, Fecha de Nacimiento: {person.BirthDate}, Dirección: {person.Address}");
                                        Console.WriteLine("");
                                    }

                                    b.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
                                }
                                else
                                {
                                    Console.WriteLine("No records found");
                                    Console.WriteLine("");

                                }

                            }
                        }



                        break;

                    case "5":
                        Console.WriteLine("");
                        Console.WriteLine("B Tree: ");
                        Console.WriteLine("");
                        b.Show();

                        Console.WriteLine("Type exit to return to menu");
                        Console.WriteLine("");

                        string userExit = Console.ReadLine();

                        if (userExit == "exit")
                        {
                            break;
                        }
                        break;
                    default:

                        break;
                }
            }
            /*

            Person persona1 = new Person();
            DateTime dateTime = DateTime.Parse("01-01-1900");
            persona1.Name = "diego";
            persona1.Id = 12345678;
            persona1.BirthDate = dateTime;
            persona1.Address = "guatemala";


            Person persona2 = new Person();
            DateTime dateTime2 = DateTime.Parse("02-01-1900");
            persona2.Name = "max";
            persona2.Id = 12345679;
            persona2.BirthDate = dateTime2;
            persona2.Address = "guatemala";

            Person persona3 = new Person();
            DateTime dateTime5 = DateTime.Parse("02-01-1900");
            persona3.Name = "diego";
            persona3.Id = 12345621;
            persona3.BirthDate = dateTime5;
            persona3.Address = "guatemala";


            Person persona4 = new Person();
            DateTime dateTime6 = DateTime.Parse("03-01-1900");
            persona4.Name = "diego";
            persona4.Id = 12345622;
            persona4.BirthDate = dateTime6;
            persona4.Address = "guatemala";

  


            
            int comparisonResult = persona1.CompareTo(persona2);

            if (comparisonResult == 0)
            {
                Console.WriteLine("Las personas son iguales.");
            }
            else if (comparisonResult < 0)
            {
                Console.WriteLine("persona1 es menor que persona2.");
            }
            else
            {
                Console.WriteLine("persona1 es mayor que persona2.");
            }

            

            BTree<int> b = new BTree<int>(3);
            b.Insert(8);
            b.Insert(9);
            b.Insert(10);
            b.Insert(11);
            b.Insert(15);
            b.Insert(20);
            b.Insert(17);

            b.Show();


            if (b.Contain(12))
            {
                Console.WriteLine("\nEncontrado");
            }
            else
            {
                Console.WriteLine("\nNo encontrado");
            }

            BTree<Person> bp = new BTree<Person>(3);

            bp.Insert(persona1);
            bp.Insert(persona2);

            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona9);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona8);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona7);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona6);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona5);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona4);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona3);
            Console.WriteLine("");
            bp.Show();

            bp.Remove(persona2);
            Console.WriteLine("");
            bp.Show();


            Console.WriteLine(bp.root.key[0].Name);

            Console.WriteLine(bp.root.key);

            bp.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
            
            if (bp.Contain(persona1))
            {
                Console.WriteLine("\nEncontrado");
            }
            else
            {
                Console.WriteLine("\nNo encontrado");
            }
            

            DateTime dateTime3 = DateTime.Parse("02-02-1900");
            
            int thisId = 12345679;
            string thisName = "max";

            DateTime? thisDay = dateTime3;
            string thisAddress = null;

           bp.UpdatePersonInfo(thisName, thisId, thisAddress, thisDay);

            int thisId2 = 12345679;
            string thisName2 = "max";

            DateTime? thisDay2 = null;
            string thisAddress2 = "el salvador";

            bp.UpdatePersonInfo(thisName2, thisId2, thisAddress2, thisDay2);
            Console.WriteLine("");
            bp.Show();

            bp.Insert(persona3);
            bp.Insert(persona4);

            Console.WriteLine("");
            bp.Show();

            int thisId3 = 12345622;
            string thisName3 = "diego";
            DateTime dateTime4 = DateTime.Parse("05-01-1900");

            DateTime? thisDay3 = dateTime4;
            string thisAddress3 = null;

            bp.UpdatePersonInfo(thisName3, thisId3, thisAddress3, thisDay3);
            Console.WriteLine("");
            bp.Show();

            bp.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");

            int thisId4 = 12345678;
            string thisName4 = "diego";

            bp.RemovePerson(thisName4, thisId4);
            Console.WriteLine("");
            bp.Show();

            string searchName = "diego";
            List<Person> searchResults = bp.SearchByName(searchName);
            Console.WriteLine("");
            Console.WriteLine("Bitacora:");
            foreach (Person person in searchResults)
            {
                
                Console.WriteLine($"Nombre: {person.Name}, DPI: {person.Id}, Fecha de Nacimiento: {person.BirthDate}, Dirección: {person.Address}");
            }

            bp.PrintTreeGraph(@"C:\Users\Rolando Ziller\Documents\Universidad\2023\segundo ciclo\edd2");
            */
        }
    }
}
