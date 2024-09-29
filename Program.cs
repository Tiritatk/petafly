using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.IO;

namespace PetaFlyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string ReleaseDate = "September 2024";
            string ReleaseVersion = "P.E.T 2";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory; // Ruta base del programa
            string dependenciesFilePath = Path.Combine(baseDir, ".DATA", "memory", "pfly_dependencies_installed.txt");
            string batFilePath = Path.Combine(baseDir, "InstallDependencies.bat");

            // Verificar si el archivo de dependencias existe
            if (!File.Exists(dependenciesFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{ReleaseVersion} - Dependencias no instaladas. Ejecutando el script de instalación...");
                RunBatFile(batFilePath);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{ReleaseVersion} - Dependencias ya instaladas.");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{ReleaseDate}");
            Console.WriteLine();
            Console.Title = $"Petafly - {ReleaseVersion}, {ReleaseDate}";

                string userName = Environment.UserName;

            while (true)
            {
                // Mostrar el menú principal
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(" ███████████            █████                 ██████  ████            ");
                Console.WriteLine("░░███░░░░░███          ░░███                 ███░░███░░███            ");
                Console.WriteLine(" ░███    ░███  ██████  ███████    ██████    ░███ ░░░  ░███  █████ ████");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(" ░██████████  ███░░███░░░███░    ░░░░░███  ███████    ░███ ░░███ ░███ ");
                Console.WriteLine(" ░███░░░░░░  ░███████   ░███      ███████ ░░░███░     ░███  ░███ ░███            Developed by Tiritatk");
                Console.WriteLine(" ░███        ░███░░░    ░███ ███ ███░░███   ░███      ░███  ░███ ░███            Available in MultiTask by SergiXY_");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(" █████       ░░██████   ░░█████ ░░████████  █████     █████ ░░███████ ");
                Console.WriteLine("░░░░░         ░░░░░░     ░░░░░   ░░░░░░░░  ░░░░░     ░░░░░   ░░░░░███            https://github.com/Tiritatk/petafly");
                Console.WriteLine("                                                             ███ ░███ ");
                Console.WriteLine("                         Spanish Theme (Future Update?)     ░░██████  ");
                Console.WriteLine("                                                             ░░░░░░   ");

                // Establecer color por defecto de la Consola
                Console.ForegroundColor = ConsoleColor.White;
                
                // Frase antes del nombre del usuario con otro color
                Console.Write($"Hola, ");
                
                // Cambio de color para el nombre del usuario
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{userName}");


                // Vuelta al color normal
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("! Bienvenido a Petafly.\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n--- Menú de Inicio ---");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. Crear nueva carpeta compartida");
                Console.WriteLine("2. Abrir servidor HTTP en un directorio existente");
                Console.WriteLine("3. Gestionar carpetas compartidas");
                Console.WriteLine("4. Añadir archivo a carpeta existente");
                Console.WriteLine("5. Salir");
                Console.Write("Selecciona una opción: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Clear();
                        CreateDirectoryForSharing();
                        break;
                    case "2":
                        Console.Clear();
                        OpenExistingHttpServer();
                        break;
                    case "3":
                        Console.Clear();
                        ManageSharedFolders();
                        break;
                    case "4":
                        Console.Clear();
                        AddFileToExistingFolder();
                        break;
                    case "5":
                        Console.Clear();

                        return; // Salir del programa
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción no válida. Por favor, selecciona una opción del menú.");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }
        static void RunBatFile(string batFilePath)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = batFilePath,
                UseShellExecute = true,
                CreateNoWindow = false
            };

            try
            {
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar el archivo BAT: " + ex.Message);
            }
        }
        static void CreateDirectoryForSharing()
        {
            string PetaflyBanner = " ███████████            █████                 ██████  ████            \n░░███░░░░░███          ░░███                 ███░░███░░███            \n ░███    ░███  ██████  ███████    ██████    ░███ ░░░  ░███  █████ ████\n ░██████████  ███░░███░░░███░    ░░░░░███  ███████    ░███ ░░███ ░███ \n ░███░░░░░░  ░███████   ░███      ███████ ░░░███░     ░███  ░███ ░███ \n ░███        ░███░░░    ░███ ███ ███░░███   ░███      ░███  ░███ ░███ \n █████       ░░██████   ░░█████ ░░████████  █████     █████ ░░███████ \n░░░░░         ░░░░░░     ░░░░░   ░░░░░░░░  ░░░░░     ░░░░░   ░░░░░███ \n                                                             ███ ░███ \n                                                            ░░██████  \n\n";
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir"); // Carpeta "dir"
            Directory.CreateDirectory(rootDirectory); // Asegurarse de que la carpeta "dir" existe
            string newDirectoryName = "shared_files"; // Comenzar desde "shared_files"
            int counter = 1;

            // Generar un nuevo nombre de directorio que no exista
            while (Directory.Exists(Path.Combine(rootDirectory, newDirectoryName + counter)))
            {
                counter++;
            }

            // Crear el nuevo directorio con el número correspondiente
            newDirectoryName += counter;
            string newDirectoryPath = Path.Combine(rootDirectory, newDirectoryName);
            Directory.CreateDirectory(newDirectoryPath);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{PetaflyBanner}");
            Console.WriteLine($"Directorio creado: {newDirectoryPath}");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(500);

            // Seleccionar archivo para compartir
            Console.WriteLine("Introduce la ruta del archivo que deseas compartir (puedes arrastrar el archivo a esta ventana):");
            string filePath = Console.ReadLine();

            // Comprobar si el archivo existe y copiarlo al directorio de compartición
            if (File.Exists(filePath))
            {
                string destFile = Path.Combine(newDirectoryPath, Path.GetFileName(filePath));
                try
                {
                    File.Copy(filePath, destFile, true);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Archivo copiado a {destFile}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al copiar el archivo: " + ex.Message);
                }
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"El archivo {filePath} no existe.");
                Console.WriteLine();
                return; // Terminar si el archivo no existe
            }

            // Preguntar si se desea abrir el servidor HTTP
            Console.WriteLine("¿Deseas iniciar el servidor HTTP ahora? (s/n): ");
            string startServer = Console.ReadLine().ToLower();

            if (startServer == "s")
            {
                // Ejecutar el servidor Python en una nueva consola
                StartPythonHttpServer(newDirectoryPath);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("El servidor HTTP se ha abierto correctamente.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;

            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("El servidor HTTP no se ha iniciado. Puedes abrirlo más tarde desde el menú.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void OpenExistingHttpServer()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            string[] directories = Directory.GetDirectories(rootDirectory);

            if (directories.Length == 0)
            {
                Console.WriteLine("No hay directorios creados. Por favor, selecciona la opción '1' en el menú para crear un directorio primero.");
                return;
            }

            // Listar directorios existentes
            Console.WriteLine("Directorio(s) existente(s):");
            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(directories[i])}");
            }

            // Seleccionar el directorio para abrir el servidor
            Console.Write("Selecciona el número del directorio donde deseas abrir el servidor: ");
            if (int.TryParse(Console.ReadLine(), out int selectedFolderIndex) && selectedFolderIndex > 0 && selectedFolderIndex <= directories.Length)
            {
                string selectedFolderPath = directories[selectedFolderIndex - 1];
                StartPythonHttpServer(selectedFolderPath);
                Console.WriteLine("El servidor HTTP se ha abierto correctamente.");
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                Console.WriteLine();
            }
        }

        static void ManageSharedFolders()
        {
            string SharedFoldersBanner = " ███████████          ████      █████                           \n░░███░░░░░░█         ░░███     ░░███                            \n ░███   █ ░   ██████  ░███   ███████   ██████  ████████   █████ \n ░███████    ███░░███ ░███  ███░░███  ███░░███░░███░░███ ███░░  \n ░███░░░█   ░███ ░███ ░███ ░███ ░███ ░███████  ░███ ░░░ ░░█████ \n ░███  ░    ░███ ░███ ░███ ░███ ░███ ░███░░░   ░███      ░░░░███\n █████      ░░██████  █████░░████████░░██████  █████     ██████ \n░░░░░        ░░░░░░  ░░░░░  ░░░░░░░░  ░░░░░░  ░░░░░     ░░░░░░  ";
            while (true)
            {
                Console.WriteLine($"{SharedFoldersBanner}");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n--- Gestión de Carpetas Compartidas ---");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. Renombrar carpeta");
                Console.WriteLine("2. Mover archivo entre carpetas");
                Console.WriteLine("3. Eliminar carpeta");
                Console.WriteLine("4. Listar todos los archivos en todas las carpetas");
                Console.WriteLine("5. Volver al menú principal");
                Console.Write("Selecciona una opción: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        RenameSharedFolder();
                        break;
                    case "2":
                        MoveFileBetweenFolders();
                        break;
                    case "3":
                        DeleteFolder();
                        break;
                    case "4":
                        ListAllFilesInSharedFolders();
                        break;
                    case "5":
                        Console.Clear();
                        return; // Volver al menú principal
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción no válida. Por favor, selecciona una opción del menú.");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        static void AddFileToExistingFolder()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            string[] directories = Directory.GetDirectories(rootDirectory);

            if (directories.Length == 0)
            {
                Console.WriteLine("No hay directorios creados. Por favor, selecciona la opción '1' en el menú para crear un directorio primero.");
                return;
            }

            // Listar directorios existentes
            Console.WriteLine("Directorio(s) existente(s):");
            for (int i = 0; i < directories.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{i + 1}. {Path.GetFileName(directories[i])}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            // Seleccionar el directorio para añadir el archivo

            Console.WriteLine();
            Console.Write("Selecciona el número del directorio donde deseas añadir un archivo: ");
            if (int.TryParse(Console.ReadLine(), out int selectedFolderIndex) && selectedFolderIndex > 0 && selectedFolderIndex <= directories.Length)
            {
                string selectedFolderPath = directories[selectedFolderIndex - 1];

                // Solicitar el archivo a añadir
                Console.WriteLine("Introduce la ruta del archivo que deseas añadir (puedes arrastrar el archivo a esta ventana):");
                string filePath = Console.ReadLine();

                // Comprobar si el archivo existe y copiarlo al directorio seleccionado
                if (File.Exists(filePath))
                {
                    string destFile = Path.Combine(selectedFolderPath, Path.GetFileName(filePath));
                    try
                    {
                        File.Copy(filePath, destFile, true);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Archivo añadido a {destFile}");
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al copiar el archivo: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine($"El archivo {filePath} no existe.");
                }
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                AddFileToExistingFolder();
            }
        }

        static void StartPythonHttpServer(string defaultPath)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K cd /d \"{defaultPath}\" && python -m http.server 80",
                UseShellExecute = true,
                CreateNoWindow = false
            };

            try
            {
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al iniciar el servidor HTTP: " + ex.Message);
            }
        }

        static void ListFolders()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            Console.WriteLine("Carpetas disponibles:");
            string[] directories = Directory.GetDirectories(rootDirectory);
            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(directories[i])}");
            }
        }

        static void RenameSharedFolder()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            ListFolders();

            // Seleccionar la carpeta a renombrar
            Console.Write("Selecciona el número de la carpeta que deseas renombrar: ");
            if (int.TryParse(Console.ReadLine(), out int selectedFolderIndex) && selectedFolderIndex > 0)
            {
                string currentFolderPath = Directory.GetDirectories(rootDirectory)[selectedFolderIndex - 1];
                string currentFolderName = Path.GetFileName(currentFolderPath);

                Console.Write("Introduce el nuevo nombre para la carpeta: ");
                string newFolderName = Console.ReadLine();
                string newFolderPath = Path.Combine(rootDirectory, newFolderName);

                // Renombrar carpeta
                Directory.Move(currentFolderPath, newFolderPath);
                Console.WriteLine($"Carpeta renombrada de {currentFolderName} a {newFolderName}.");
            }
            else
            {
                Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
            }
        }

        static void MoveFileBetweenFolders()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            ListFolders();

            // Seleccionar la carpeta de origen
            Console.Write("Selecciona el número de la carpeta de origen: ");
            if (int.TryParse(Console.ReadLine(), out int sourceFolderIndex) && sourceFolderIndex > 0)
            {
                string sourceFolderPath = Directory.GetDirectories(rootDirectory)[sourceFolderIndex - 1];

                // Seleccionar la carpeta de destino
                Console.Write("Selecciona el número de la carpeta de destino: ");
                if (int.TryParse(Console.ReadLine(), out int destinationFolderIndex) && destinationFolderIndex > 0)
                {
                    string destinationFolderPath = Directory.GetDirectories(rootDirectory)[destinationFolderIndex - 1];

                    // Listar archivos en la carpeta de origen
                    Console.WriteLine("Archivos disponibles en la carpeta de origen:");
                    string[] files = Directory.GetFiles(sourceFolderPath);
                    for (int i = 0; i < files.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                    }

                    // Seleccionar el archivo a mover
                    Console.Write("Selecciona el número del archivo que deseas mover: ");
                    if (int.TryParse(Console.ReadLine(), out int fileIndex) && fileIndex > 0 && fileIndex <= files.Length)
                    {
                        string fileToMove = files[fileIndex - 1];
                        string destinationFilePath = Path.Combine(destinationFolderPath, Path.GetFileName(fileToMove));

                        // Mover el archivo
                        File.Move(fileToMove, destinationFilePath);
                        Console.WriteLine($"Archivo movido de {sourceFolderPath} a {destinationFolderPath}.");
                    }
                    else
                    {
                        Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                    }
                }
            }
        }

        static void DeleteFolder()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            ListFolders();

            // Seleccionar la carpeta a eliminar
            Console.Write("Selecciona el número de la carpeta que deseas eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int selectedFolderIndex) && selectedFolderIndex > 0)
            {
                string folderToDelete = Directory.GetDirectories(rootDirectory)[selectedFolderIndex - 1];
                Directory.Delete(folderToDelete, true); // Eliminar la carpeta y su contenido
                Console.WriteLine("Carpeta eliminada correctamente.");
            }
            else
            {
                Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
            }
        }

        static void ListAllFilesInSharedFolders()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            string[] directories = Directory.GetDirectories(rootDirectory);

            if (directories.Length == 0)
            {
                Console.WriteLine("No hay directorios creados.");
                return;
            }

            Console.WriteLine("\n--- Listado de Todos los Archivos en Todas las Carpetas ---");

            foreach (string dir in directories)
            {
                Console.WriteLine($"Carpeta: {Path.GetFileName(dir)}");
                string[] files = Directory.GetFiles(dir);
                if (files.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("  (Sin archivos)");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    foreach (string file in files)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"  - {Path.GetFileName(file)}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
    }
}
