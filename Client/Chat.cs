using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

// Klassen representerar en chattklient som ansluter till en SignalR-server.
internal class Chat
{
    // Fält för att lagra anslutningsinformation
    public HubConnection connection { get; private set; } // Anslutning till SignalR-servern
    string ip;
    string port;
    string endpoint; // SignalR-slutpunkt
    string username;
    string password;

    // Hjälpvariabel för att formatera tid
    TimeOnly time = new TimeOnly();

    string url => $"wss://{ip}:{port}/{endpoint}";

    public Chat(string ip, string port, string endpoint, string username, string password)
    {
        this.ip = ip;
        this.port = port;
        this.endpoint = endpoint;
        this.username = username;
        this.password = password;

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

        // Autentiserar användaren mot servern
        connection.InvokeAsync("Login", username, password).Wait();

        // Registrerar en callback för att ta emot meddelanden från servern
        connection.On<string>("ReceiveMessage", (message) =>
        {
            // Skriver ut meddelandet med formaterad tid
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"{time.ToString()}: {message}");
            Console.Write("Enter message: ");
        });

        Console.WriteLine("Connected! Write 'quitchat' to exit.");
        Sender();
    }

    // Metod för att skicka meddelanden till servern och ta emot svar.
    void Sender()
    {
        string message = string.Empty;

        // Loopar tills användaren skriver "quitchat"
        while (message != "quitchat")
        {
            message = userInput("Enter message: ");

            // Skickar meddelandet till servern
            connection.InvokeAsync("SendMessage", message);
        }
    }
    
    // Hjälpmetod för att läsa användarinput.
    string userInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}
