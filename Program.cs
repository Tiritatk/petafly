using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace PetaFlyApp
{
    class Program
    {
        class HttpServerInfo
        {
            public string DirectoryPath { get; set; }
            public int Port { get; set; }
            public Process HttpProcess { get; set; }
            public bool NgrokActive { get; set; }
        }

        static List<HttpServerInfo> activeServers = new List<HttpServerInfo>();

        static void Main(string[] args)
        {
            string ReleaseDate = "October 2024";
            string ReleaseVersion = "0.0.5";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory; // Ruta base del programa

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{ReleaseDate}");
            Console.WriteLine();
            Console.Title = $"Petafly - {ReleaseVersion}, {ReleaseDate}";

                string userName = Environment.UserName;

            while (true)
            {
                // Mostrar el menú principal
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(" ███████████            █████                 ██████  ████            ");
                Console.WriteLine("░░███░░░░░███          ░░███                 ███░░███░░███            ");
                Console.WriteLine(" ░███    ░███  ██████  ███████    ██████    ░███ ░░░  ░███  █████ ████");
                Console.WriteLine(" ░██████████  ███░░███░░░███░    ░░░░░███  ███████    ░███ ░░███ ░███ ");
                Console.WriteLine(" ░███░░░░░░  ░███████   ░███      ███████ ░░░███░     ░███  ░███ ░███            Developed by Tiritatk");
                Console.WriteLine(" ░███        ░███░░░    ░███ ███ ███░░███   ░███      ░███  ░███ ░███            Available in MultiTask by SergiXY_");
                Console.WriteLine(" █████       ░░██████   ░░█████ ░░████████  █████     █████ ░░███████ ");
                Console.WriteLine("░░░░░         ░░░░░░     ░░░░░   ░░░░░░░░  ░░░░░     ░░░░░   ░░░░░███            https://github.com/Tiritatk/petafly");
                Console.WriteLine("                                                             ███ ░███ ");
                Console.WriteLine("            Codename - Magenta Theme (Future Update?)       ░░██████  ");
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
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("3. Ajustes");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("4. Gestionar carpetas compartidas");
                Console.WriteLine("5. Añadir archivo a carpeta existente");
                Console.WriteLine("6. Gestionar servidores HTTP activos");
                Console.WriteLine("7. Salir");
                Console.WriteLine();
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
                        Settings();
                        break;
                    case "4":
                        Console.Clear();
                        ManageSharedFolders();
                        break;
                    case "5":
                        Console.Clear();
                        AddFileToExistingFolder();
                        break;
                    case "6":
                        Console.Clear();
                        ManageHttpServers();
                        break;
                    case "7":
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
        static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                // Devuelve la primera dirección IPv4
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No se pudo encontrar una dirección IP local.");
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
            Console.WriteLine("Introduce la ruta del archivo/s que deseas compartir (puedes arrastrar el archivo a esta ventana, o pulsar enter para crear una carpeta vacía):");
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
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error al copiar el archivo: " + ex.Message);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"No se especificó ningún archivo o la ruta {filePath} es incorrecta. Se creó un directorio vacio.");
                Console.WriteLine();
                return; // Terminar si el archivo no existe
            }

            // Preguntar si se desea abrir el servidor HTTP
            Console.WriteLine("¿Deseas iniciar el servidor HTTP ahora? (s/n): ");
            string startServer = Console.ReadLine().ToLower();

            if (startServer == "s")
            {
                int port = GetRandomAvailablePort(); // Obtener un puerto aleatorio disponible
                StartPythonHttpServer(newDirectoryPath, ref port);

                string localIp = GetLocalIPAddress(); // Obtener la dirección IP local
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"El servidor HTTP se ha abierto correctamente en el puerto {port}.");
                Console.WriteLine($"Accede a él en: http://{localIp}:{port}");
                Console.ForegroundColor = ConsoleColor.White;
            }



        }

        static Random random = new Random();

        static int GetRandomAvailablePort()
        {
            int port;
            do
            {
                // Generar un puerto aleatorio entre 1024 y 65535 (puertos no reservados)
                port = random.Next(1024, 65536);
            } while (IsPortInUse(port)); // Verificar si el puerto está en uso

            return port;
        }


        static void OpenExistingHttpServer()
        {
            string httpBanner = " █████   █████ ███████████ ███████████ ███████████ \n░░███   ░░███ ░█░░░███░░░█░█░░░███░░░█░░███░░░░░███\n ░███    ░███ ░   ░███  ░ ░   ░███  ░  ░███    ░███\n ░███████████     ░███        ░███     ░██████████ \n ░███░░░░░███     ░███        ░███     ░███░░░░░░  \n ░███    ░███     ░███        ░███     ░███        \n █████   █████    █████       █████    █████       \n░░░░░   ░░░░░    ░░░░░       ░░░░░    ░░░░░        ";
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            string[] directories = Directory.GetDirectories(rootDirectory); // Asegúrate de definir 'directories' aquí

            if (directories.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No hay directorios creados. Por favor, selecciona la opción '1' en el menú para crear un directorio primero.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            // Listar directorios existentes
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{httpBanner}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Directorio(s) existente(s):");
            Console.WriteLine();
            for (int i = 0; i < directories.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{i + 1}. {Path.GetFileName(directories[i])}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            // Seleccionar el directorio para abrir el servidor
            Console.WriteLine();
            Console.Write("Selecciona el número del directorio donde deseas abrir el servidor: ");
            if (int.TryParse(Console.ReadLine(), out int selectedFolderIndex) && selectedFolderIndex > 0 && selectedFolderIndex <= directories.Length)
            {
                string selectedFolderPath = directories[selectedFolderIndex - 1];

                int port = GetRandomAvailablePort(); // Obtener un puerto aleatorio disponible
                StartPythonHttpServer(selectedFolderPath, ref port);

                string localIp = GetLocalIPAddress(); // Obtener la dirección IP local
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"El servidor HTTP se ha abierto correctamente en el puerto {port}.");
                Console.WriteLine($"Accede a él en: http://{localIp}:{port}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }


            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static bool IsPortInUse(int port)
        {
            bool result = false;
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
            }
            catch (SocketException)
            {
                result = true; // El puerto está en uso
            }
            finally
            {
                listener?.Stop();
            }
            return result;
        }

        static void StartPythonHttpServer(string folderPath, ref int port)
        {
            while (IsPortInUse(port))
            {
                port++; // Incrementar el puerto hasta encontrar uno disponible
            }

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"-m http.server {port}",  // Pasar el puerto dinámicamente
                WorkingDirectory = folderPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            Process process = new Process
            {
                StartInfo = psi
            };

            process.Start();

            // Almacenar información del servidor en la lista
            activeServers.Add(new HttpServerInfo
            {
                DirectoryPath = folderPath,
                Port = port,
                HttpProcess = process,
                NgrokActive = false
            });

            Console.WriteLine($"Servidor HTTP abierto en '{folderPath}' en el puerto {port}.");
        }


        static void ListActiveServers()
        {
            Console.WriteLine("--- Servidores HTTP Activos ---");
            if (activeServers.Count == 0)
            {
                Console.WriteLine("No hay servidores HTTP abiertos actualmente.");
                return;
            }

            foreach (var server in activeServers)
            {
                Console.WriteLine($"Directorio: {server.DirectoryPath}, Puerto: {server.Port}, Ngrok: {(server.NgrokActive ? "Activo" : "Inactivo")}");
            }
        }

        static void ActivateNgrokForServer(int port)
        {
            var server = activeServers.FirstOrDefault(s => s.Port == port);
            if (server == null)
            {
                Console.WriteLine($"No se encontró un servidor en el puerto {port}.");
                return;
            }

            if (!server.NgrokActive)
            {
                StartNgrok(port); // Función que lanza ngrok
                server.NgrokActive = true;
                Console.WriteLine($"Ngrok activado en el puerto {port}.");
            }
            else
            {
                Console.WriteLine($"Ngrok ya está activo en el puerto {port}.");
            }
        }
        static void CloseHttpServer(int port)
        {
            var server = activeServers.FirstOrDefault(s => s.Port == port);
            if (server == null)
            {
                Console.WriteLine($"No se encontró un servidor en el puerto {port}.");
                return;
            }

            // Cerrar el proceso HTTP
            server.HttpProcess.Kill();
            activeServers.Remove(server);

            Console.WriteLine($"Servidor HTTP en el puerto {port} cerrado.");
        }



        static void Settings()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Settings aun no esta disponible en Petafly.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void ManageSharedFolders()
        {
            string SharedFoldersBanner = " ███████████          ████      █████                           \n░░███░░░░░░█         ░░███     ░░███                            \n ░███   █ ░   ██████  ░███   ███████   ██████  ████████   █████ \n ░███████    ███░░███ ░███  ███░░███  ███░░███░░███░░███ ███░░  \n ░███░░░█   ░███ ░███ ░███ ░███ ░███ ░███████  ░███ ░░░ ░░█████ \n ░███  ░    ░███ ░███ ░███ ░███ ░███ ░███░░░   ░███      ░░░░███\n █████      ░░██████  █████░░████████░░██████  █████     ██████ \n░░░░░        ░░░░░░  ░░░░░  ░░░░░░░░  ░░░░░░  ░░░░░     ░░░░░░  \n\n";
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
                Console.WriteLine();
                Console.Write("Selecciona una opción: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine($"{SharedFoldersBanner}");
                        RenameSharedFolder();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine($"{SharedFoldersBanner}");
                        MoveFileBetweenFolders();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine($"{SharedFoldersBanner}");
                        DeleteFolder();
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine($"{SharedFoldersBanner}");
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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No hay directorios creados. Por favor, selecciona la opción '1' en el menú para crear un directorio primero.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
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
        static void ManageHttpServers()
        {
            while (true)
            {
                Console.WriteLine("\n--- Gestión de Servidores HTTP ---");
                Console.WriteLine("1. Listar servidores activos");
                Console.WriteLine("2. Activar Ngrok en un servidor");
                Console.WriteLine("3. Cerrar un servidor HTTP");
                Console.WriteLine("4. Volver al menú principal");
                Console.Write("Selecciona una opción: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ListActiveServers();
                        break;
                    case "2":
                        Console.Write("Introduce el puerto del servidor para activar Ngrok: ");
                        if (int.TryParse(Console.ReadLine(), out int ngrokPort))
                        {
                            ActivateNgrokForServer(ngrokPort);
                        }
                        else
                        {
                            Console.WriteLine("Entrada no válida.");
                        }
                        break;
                    case "3":
                        Console.Write("Introduce el puerto del servidor que deseas cerrar: ");
                        if (int.TryParse(Console.ReadLine(), out int closePort))
                        {
                            CloseHttpServer(closePort);
                        }
                        else
                        {
                            Console.WriteLine("Entrada no válida.");
                        }
                        break;
                    case "4":
                        return; // Volver al menú principal
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        static void StartNgrok(int port)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K ngrok http {port}", // Mantiene la ventana de CMD abierta
                UseShellExecute = true,  // Permite abrir una nueva ventana de CMD
                CreateNoWindow = false   // Muestra la ventana de CMD
            };

            Process process = new Process
            {
                StartInfo = psi
            };

            process.Start();

            Console.WriteLine($"Ngrok iniciado en el puerto {port}. La URL pública estará disponible en la ventana de ngrok.");
        }

    static void ListFolders()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            Console.WriteLine();
            Console.WriteLine("Carpetas disponibles:");
            string[] directories = Directory.GetDirectories(rootDirectory);
            for (int i = 0; i < directories.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{i + 1}. {Path.GetFileName(directories[i])}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }

        static void RenameSharedFolder()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            ListFolders();

            // Obtener la lista de directorios
            string[] directories = Directory.GetDirectories(rootDirectory);

            // Verificar si hay directorios disponibles
            if (directories.Length == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay carpetas creadas. Por favor, crea una carpeta primero.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                return; // Salir del método si no hay carpetas
            }

            // Seleccionar la carpeta a renombrar
            Console.Write("Selecciona el número de la carpeta que deseas renombrar: ");
            if (int.TryParse(Console.ReadLine(), out int selectedFolderIndex) && selectedFolderIndex > 0 && selectedFolderIndex <= directories.Length)
            {
                string currentFolderPath = directories[selectedFolderIndex - 1];
                string currentFolderName = Path.GetFileName(currentFolderPath);
                Console.Write("Introduce el nuevo nombre para la carpeta: ");
                string newFolderName = Console.ReadLine();
                string newFolderPath = Path.Combine(rootDirectory, newFolderName);

                // Renombrar carpeta
                Directory.Move(currentFolderPath, newFolderPath);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Carpeta renombrada de {currentFolderName} a {newFolderName}.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void MoveFileBetweenFolders()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            ListFolders();

            // Verificar si hay carpetas disponibles
            string[] directories = Directory.GetDirectories(rootDirectory);
            if (directories.Length == 0)
            {
                Console.Clear();
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("No hay carpetas disponibles para mover archivos.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                return; // Salir del método si no hay carpetas
            }

            // Seleccionar la carpeta de origen
            Console.Write("Selecciona el número de la carpeta de origen: ");
            if (int.TryParse(Console.ReadLine(), out int sourceFolderIndex) && sourceFolderIndex > 0 && sourceFolderIndex <= directories.Length)
            {
                string sourceFolderPath = directories[sourceFolderIndex - 1];

                // Seleccionar la carpeta de destino
                Console.Write("Selecciona el número de la carpeta de destino: ");
                if (int.TryParse(Console.ReadLine(), out int destinationFolderIndex) && destinationFolderIndex > 0 && destinationFolderIndex <= directories.Length)
                {
                    string destinationFolderPath = directories[destinationFolderIndex - 1];

                    // Listar archivos en la carpeta de origen
                    Console.WriteLine();
                    Console.WriteLine("Archivos disponibles en la carpeta de origen:");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    string[] files = Directory.GetFiles(sourceFolderPath);
                    Console.ForegroundColor = ConsoleColor.White;
                    if (files.Length == 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No hay archivos disponibles en la carpeta de origen.");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        return; // Salir si no hay archivos
                    }

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
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void DeleteFolder()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            ListFolders();

            // Verificar si hay carpetas disponibles
            string[] directories = Directory.GetDirectories(rootDirectory);
            if (directories.Length == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay carpetas disponibles para eliminar.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                return; // Salir del método si no hay carpetas
            }

            // Seleccionar la carpeta a eliminar
            Console.Write("Selecciona el número de la carpeta que deseas eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int selectedFolderIndex) && selectedFolderIndex > 0 && selectedFolderIndex <= directories.Length)
            {
                string folderToDelete = directories[selectedFolderIndex - 1];
                Directory.Delete(folderToDelete, true); // Eliminar la carpeta y su contenido
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Carpeta eliminada correctamente.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        static void ListAllFilesInSharedFolders()
        {
            string rootDirectory = Path.Combine(Environment.CurrentDirectory, "dir");
            string[] directories = Directory.GetDirectories(rootDirectory);

            if (directories.Length == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay directorios creados.");
                Console.WriteLine();
                Console.ForegroundColor= ConsoleColor.White;
                return;
            }
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine("--- Listado de Todos los Archivos en Todas las Carpetas ---");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            foreach (string dir in directories)
            {
                Console.WriteLine($"Carpeta: {Path.GetFileName(dir)}");
                string[] files = Directory.GetFiles(dir);
                if (files.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("  (Sin archivos)");
                    Console.WriteLine() ;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                else
                {
                    foreach (string file in files)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"  - {Path.GetFileName(file)}");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            Console.WriteLine();

            // Pausa para que el usuario presione Enter antes de continuar
            Console.WriteLine("Presiona Enter para continuar...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
