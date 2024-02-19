using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

internal class Program
{


    static async Task Main(string[] args)
    {        
        string ip = userInput("Enter IP: ");
        string port = userInput("Enter port: ");
        string endpoint = userInput("Enter endpoint: ");

        var test = new Chat(ip, port, endpoint);



        /*
        string url = $"wss://{ip}:{port}/{endpoint}";

        HubConnection connection = new HubConnectionBuilder().WithUrl("wss://localhost:5001/chathub").Build();


        // Ny tråd

        Thread t;

        try
        {
            connection = new HubConnectionBuilder().WithUrl(url).Build();
            connection.StartAsync().Wait();

            t = new Thread(Listener);
            t.Start();
        }
        catch (Exception)
        {

            Console.WriteLine("Connection failed");
        }

        var workT = new Thread(Worker);
        workT.Start();


        string message = "";

        while (message != "quitchat")
        {
            message = userInput("Enter message: ");
            connection.InvokeAsync("SendMessage", message);
            
        }

        */
        //connection.InvokeAsync("SendMessage", "Andra datorn");
        Console.ReadKey();
    }

    

    static string userInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}


/*
internal class Program
{
    static void Main(string[] args)
    {      
        var connection = new HubConnectionBuilder().WithUrl("wss://localhost:5001/chathub").Build();

        connection.StartAsync().Wait();

        connection.InvokeAsync("SendMessage", "Andra datorn");
        Console.ReadKey();
    }
}
*/