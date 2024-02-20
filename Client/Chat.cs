using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

internal class Chat
{
    public HubConnection connection { get; private set; }
    string ip;
    string port;
    string endpoint;
    string username; // LOGIN TEST
    string password; // LOGIN TEST
    TimeOnly time = new TimeOnly();
    string url => $"wss://{ip}:{port}/{endpoint}";
    
    public Chat(string ip, string port, string endpoint, string username, string password)
    {
        this.ip = ip;
        this.port = port;
        this.endpoint = endpoint;
        this.username = username; // LOGIN TEST
        this.password = password; // LOGIN TEST

        this.connection = new HubConnectionBuilder().WithUrl(url, (opts) =>
        {
            opts.HttpMessageHandlerFactory = (message) =>
            {
                if (message is HttpClientHandler clientHandler)
                    // always verify the SSL certificate
                    clientHandler.ServerCertificateCustomValidationCallback += // Metod för att fulhacka SSL-verifiering
                        (sender, certificate, chain, sslPolicyErrors) => { return true; };  // TODO: Fixa fungerande på servern
                return message;
            };
        }).Build();
        connection.StartAsync().Wait();


        connection.InvokeAsync("Login", username, password).Wait(); // LOGIN TEST


        connection.On<string>("ReceiveMessage", (message) =>
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"{time.ToString()}: {message}");
            Console.Write("Enter message: ");
        });
        Console.WriteLine("Connected! Write 'quitchat' to exit.");
        Sender();
    }

    void Sender()
    {
        string message = string.Empty;

        while (message != "quitchat")
        {
            message = userInput("Enter message: ");
            connection.InvokeAsync("SendMessage", message);
        }
    }

    string userInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}
