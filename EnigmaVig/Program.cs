using System.Text;

class Program
{
    private const string alphabet = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
    static void Main(string[] args)
    {
        Console.WriteLine("**********************************************************");
        Console.WriteLine("                    EN1GM@V1G 1.0                         ");
        Console.WriteLine("**********************************************************");
        
        Console.WriteLine("Ingrese la ruta del archivo de texto:");
        string filePath = Console.ReadLine();
        
        if(!File.Exists(filePath))
        {
            Console.WriteLine("El archivo no existe");
            return;
        }

        Console.WriteLine("Ingrese la clave de cifrado:");
        string clave = Console.ReadLine();

        Console.WriteLine("Opciones");
        Console.WriteLine("1. Cifrar mensaje");
        Console.WriteLine("2. Descifrar mensaje");
        Console.WriteLine("3. Salir");
        Console.WriteLine("**********************************************************");
        Console.WriteLine("Seleccione una opción: ");
        int opcion = int.Parse(Console.ReadLine());

        string contenido = File.ReadAllText(filePath, Encoding.UTF8).ToUpper();
        string resultado = opcion == 1 ? EncryptVigenere(contenido, clave) : DecryptVigenere(contenido, clave);

        string? directory = Path.GetDirectoryName(filePath);
        if (directory == null)
        {
            Console.WriteLine("Error: No se pudo obtener el directorio del archivo.");
            return;
        }
        string newFilePath = Path.Combine(directory, Path.GetFileNameWithoutExtension(filePath) + "C" + Path.GetExtension(filePath));
        File.WriteAllText(newFilePath, resultado, Encoding.UTF8);

        Console.WriteLine($"Proceso completado. Archivo guardado en: {newFilePath}");

    }


    static string EncryptVigenere( string texto, string clave)
    {
        StringBuilder result = new StringBuilder();
        int keyIndex = 0;

        foreach (char caracter in texto)
        {
            int indexText = alphabet.IndexOf(caracter);
            if(indexText >= 0)
            {
                int indexKey = alphabet.IndexOf(clave[keyIndex % clave.Length]);
                int newIndex = (indexText + indexKey) % alphabet.Length;
                result.Append(alphabet[newIndex]);

                keyIndex++;
            } else
            {
                result.Append(caracter);
            }
        }
        return result.ToString();
    }

    static string DecryptVigenere(string texto, string clave)
    {
        StringBuilder result = new StringBuilder();
        int keyIndex = 0;

        foreach (char caracter in texto)
        {
            int indexText = alphabet.IndexOf(caracter);
            if (indexText >= 0)
            {
                int indexKey = alphabet.IndexOf(clave[keyIndex % clave.Length]);
                int newIndex = (indexText - indexKey + alphabet.Length) % alphabet.Length;
                result.Append(alphabet[newIndex]);
                keyIndex++;
            }
            else
            {
                result.Append(caracter);
            }
        }
        return result.ToString();
    }
}