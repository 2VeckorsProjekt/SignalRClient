using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

/// <summary>
/// Klassen representerar en chattklient som ansluter till en SignalR-server.
/// </summary>
internal class Chat
{
    // Fält för att lagra anslutningsinformation
    public HubConnection connection { get; private set; } // Anslutning till SignalR-servern
    string ip; // IP-adress till servern
    string port; // Portnummer för servern
    string endpoint; // SignalR-slutpunkt
    string username; // Användarnamn för autentisering (LOGIN TEST)
    string password; // Lösenord för autentisering (LOGIN TEST)

    // Hjälpvariabel för att formatera tid
    TimeOnly time = new TimeOnly();

    // Bygger URL:en för anslutning
    string url => $"wss://{ip}:{port}/{endpoint}";

    /// <summary\>
    /// Konstruktor för att skapa en ny chattklient\.
    /// </summary\>
    /// <param name\="ip"\>IP\-adress till servern\.</param\>
    /// <param name\="port"\>Portnummer för servern\.</param\>
    /// <param name\="endpoint"\>SignalR\-slutpunkt\.</param\>
    /// <param name\="username"\>Användarnamn för autentisering\.</param\>
    /// <param name\="password"\>Lösenord för autentisering\.</param\>
    public Chat(string ip, string port, string endpoint, string username, string password)
    {
        this.ip = ip;
        this.port = port;
        this.endpoint = endpoint;
        this.username = username; // (LOGIN TEST)
        this.password = password; // (LOGIN TEST)

        // Skapar en ny HubConnection med URL:en och konfigurerar klienten
        this.connection = new HubConnectionBuilder().WithUrl(url, (opts) =>
        {
            // Anpassar HttpMessageHandler för att ignorera SSL\-certifikatfel
            opts.HttpMessageHandlerFactory = (message) =>
            {
                if (message is HttpClientHandler clientHandler)
                    // Ignorera alltid SSL\-certifikatfel
                    clientHandler.ServerCertificateCustomValidationCallback +=
                        (sender, certificate, chain, sslPolicyErrors) => { return true; };  // TODO: Implementera korrekt certifikatvalidering på servern
                return message;
            };
        }).Build();

        // Startar anslutningen asynkront och väntar på slutförande
        connection.StartAsync().Wait();

        // Autentiserar användaren mot servern (LOGIN TEST)
        connection.InvokeAsync("Login", username, password).Wait();

        // Registrerar en callback för att ta emot meddelanden från servern
        connection.On<string>("ReceiveMessage", (message) =>
        {
            // Skriver ut meddelandet med formaterad tid
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"{time.ToString()}: {message}");
            Console.Write("Enter message: ");
        });

        // Visar en bekräftelse och startar metoden för att skicka meddelanden
        Console.WriteLine("Connected! Write 'quitchat' to exit.");
        Sender();
    }

    /// <summary>
    /// Metod för att skicka meddelanden till servern och ta emot svar.
    /// </summary>
    void Sender()
    {
        string message = string.Empty;

        // Loopar tills användaren skriver "quitchat"
        while (message != "quitchat")
        {
            // Hämtar användarens meddelande
            message = userInput("Enter message: ");

            // Skickar meddelandet till servern
            connection.InvokeAsync("SendMessage", message);
        }
    }

    /// <summary>
    /// Hjälpmetod för att läsa användarinput.
    /// </summary>
    /// <param name="prompt">Prompt att visa till användaren.</param>
    /// <returns>Användarens inmatade text.</returns>
    string userInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}
