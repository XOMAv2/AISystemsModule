using AISystemsModule.Models;
using DataGenerator.Extensions;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Title каждой ноды должен быть уникальным (в файлах .dot и .dot.svg это не так).
            // В .dot.svg неправильно отображается "Соколина Гора" и "Марьина роща".
            // Правильный и актуальный .json находится в AISystemsModule/Resources.
            string sourcePath = "./Resources/moscow.json";
            string outputPath = sourcePath.Replace(".json", ".attributes.json");

            // Считываем дерево из файла.
            string text = File.ReadAllText(sourcePath);
            Node root = JsonSerializer.Deserialize<Node>(text);

            // Заполняем листья случайными значениями.
            root.AddAttributesToLeafs();

            // Записываем дерево в файл.
            var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
            var result = JsonSerializer.SerializeToUtf8Bytes(root, options);
            File.WriteAllBytes(outputPath, result);
        }
    }
}
