using System;
using Microsoft.Owin.Hosting;

namespace DalSoft.WebApi.HelpPage.Example.SelfHost 
{ 
    public class Program 
    { 
        static void Main() 
        { 
            const string baseAddress = "http://localhost:8080"; 

            // Start OWIN host 
            WebApp.Start<Startup>(baseAddress);
            
            Console.WriteLine("Service listening at: {0}", baseAddress);
            Console.WriteLine("Help page available at: {0}/help", baseAddress);
            Console.WriteLine("Press Enter to shutdown the service.");
                
            Console.ReadLine(); 
        } 
    } 
 }