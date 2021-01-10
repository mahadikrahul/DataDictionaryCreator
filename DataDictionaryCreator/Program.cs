using DataDictionaryCreator.Helpers;
using System;

namespace DataDictionaryCreator
{
    public class Program
    {
        public static void Main()
        {
            ConfigurationHelper.GetAppSettings();
            InteractionHelper.CreateDictionary();
            Console.WriteLine("Hello World!");
        }
    }
}
