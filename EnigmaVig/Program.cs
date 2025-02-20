using System.Text;

class Program
{
    private const string alphabet = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
    static void Main(string[] args)
    {
        while (true)
        {
            Console.SetWindowPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("**********************************************************");
            Console.WriteLine("                    EN1GM@V1G 1.0                         ");
            Console.WriteLine("**********************************************************");

            Console.WriteLine("\nSeleccione una opción:");
            Console.WriteLine("1. Encriptar un archivo");
            Console.WriteLine("2. Desencriptar un archivo");
            Console.WriteLine("3. Salir");
            Console.Write("Opción: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ProcessFile(true); // Encriptar
                    break;
                case "2":
                    ProcessFile(false); // Desencriptar
                    break;
                case "3":
                    Console.WriteLine("Saliendo del programa...");
                    return;
                default:
                    Console.WriteLine("Opción inválida. Intente nuevamente.");
                    Console.Beep();
                    break;
            }
            Console.Clear();
        }

    }

    static void ProcessFile(bool encrypt)
    {
        string action = encrypt ? "encriptar" : "desencriptar";
        string suffix = encrypt ? "C" : "D";

        string filePath;
        while (true)
        {
            Console.Write($"Ingrese la ruta del archivo a {action}: ");
            filePath = Console.ReadLine();

            filePath = filePath.Replace("\"", "").Trim();

            if (File.Exists(filePath))
                break;

            Console.WriteLine("Error: La ruta del archivo no es válida. Intente nuevamente.");
        }

        string key;
        while (true)
        {
            Console.Write("IMPORTANTE: la clave que utilice para encriptar debe ser la misma para desencriptar.");
            Console.Write("\nIngrese la clave de encriptación: ");
            key = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(key))
                break;

            Console.WriteLine("Error: La clave no puede estar vacía. Intente nuevamente.");
        }

        try
        {
            // Leer el contenido del archivo
            string inputText = File.ReadAllText(filePath);
            Console.WriteLine($"\nTexto original:");
            Console.WriteLine(inputText);

            // Encriptar o desencriptar el texto
            string outputText = VigenereCipher(inputText, key, encrypt);
            Console.WriteLine($"\nTexto {(encrypt ? "Encriptado" : "Desencriptado")}:");
            Console.WriteLine(outputText);

            // Guardar el texto encriptado/desencriptado en un nuevo archivo
            string outputFilePath = GetNewFilePath(filePath, suffix);
            File.WriteAllText(outputFilePath, outputText);
            Console.WriteLine($"\nArchivo {(encrypt ? "encriptado" : "desencriptado")} guardado en: {outputFilePath}");
            Console.Beep();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
        }

        Console.WriteLine("\nPresione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    static string VigenereCipher(string text, string key, bool encrypt)
    {
        text = text.ToUpper();
        key = key.ToUpper();
        string result = "";
        int keyIndex = 0;

        // Iterar por cada caracter del texto
        foreach (char c in text)
        {
            if (alphabet.Contains(c))
            {
                // Obtener el índice del caracter en el alfabeto
                int textIndex = alphabet.IndexOf(c);
                int keyShift = alphabet.IndexOf(key[keyIndex % key.Length]);
                int newIndex = encrypt ? (textIndex + keyShift) % alphabet.Length
                                       : (textIndex - keyShift + alphabet.Length) % alphabet.Length;

                // Agregar el caracter encriptado/desencriptado al resultado
                result += alphabet[newIndex];
                keyIndex++;
            }
            else
            {
                result += c;
            }
        }

        return result;
    }

    static string GetNewFilePath(string originalPath, string suffix)
    {
        // Obtener el directorio, nombre del archivo sin extensión y extensión
        string directory = Path.GetDirectoryName(originalPath) ?? string.Empty;
        string filenameWithoutExt = Path.GetFileNameWithoutExtension(originalPath);
        string extension = Path.GetExtension(originalPath);
        return Path.Combine(directory, filenameWithoutExt + suffix + extension);
    }
}