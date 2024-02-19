using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

internal class Program
{


    static async Task Main(string[] args)
    {
        string ip = userInput("Enter IP (Hampus: 92.34.183.213 - Lokal: localhost): ");
        string port = userInput("Enter port (Hampus: 80 - Lokal: 5001): ");
        string endpoint = userInput("Enter endpoint (chathub): ");

        var test = new Chat(ip, port, endpoint);

        Console.ReadKey();
    }

    

    static string userInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}

