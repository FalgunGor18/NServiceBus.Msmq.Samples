﻿using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("MessageBodyEncryptionEndpoint1");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.RegisterMessageEncryptor();
        var startableBus = Bus.Create(busConfiguration);
        using (var bus = startableBus.Start())
        {
            bus.Send("MessageBodyEncryptionEndpoint2", new CompleteOrder
                                  {
                                      CreditCard = "123-456-789"
                                  });
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}