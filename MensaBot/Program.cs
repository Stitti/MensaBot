// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

StreamReader reader = new StreamReader("secret.json");
string json = reader.ReadToEnd();
Console.ReadKey();