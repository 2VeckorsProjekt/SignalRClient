using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

internal class Program
{
    static async Task Main(string[] args)
    {        
        string ip = userInput("Enter IP (Hampus: 92.34.183.213 - Lokal: localhost): ");
        string port = userInput("Enter port (Hampus: 80 - Lokal: 5001): ");
        string endpoint = userInput("Enter endpoint (chathub): ");
        string username = userInput("Username: ");
        string password = userInput("Password: ");

        // Skapar ett nytt Chat-objekt med användaruppgifterna.
        var test = new Chat(ip, port, endpoint, username, password);

        // Väntar på knapptryckning innan avslut.
        Console.ReadKey();
    }  

    static string userInput(string prompt) // Metod för att hämta användarinmatning.
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}