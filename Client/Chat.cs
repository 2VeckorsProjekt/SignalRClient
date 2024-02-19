using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

internal class Chat
{
    public HubConnection connection { get; private set; }
    string ip;
    string port;
    string endpoint;
    TimeOnly time = new TimeOnly();
    string url => $"wss://{ip}:{port}/{endpoint}";
    
    public Chat(string ip, string port, string endpoint)
    {
        this.ip = ip;
        this.port = port;
        this.endpoint = endpoint;

        this.connection = new HubConnectionBuilder().WithUrl(url).Build();
        connection.StartAsync().Wait();

        connection.On<string>("ReceiveMessage", (message) =>
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"{time.ToString()}: {message}");
            Console.Write("Enter message: ");
        });

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
