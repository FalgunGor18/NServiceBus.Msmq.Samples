﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

public class ValidationMessageMutator : IMessageMutator
{
    static ILog Logger = LogManager.GetLogger("ValidationMessageMutator");

    public object MutateOutgoing(object message)
    {
        ValidateDataAnnotations(message);
        return message;
    }

    public object MutateIncoming(object message)
    {
        ValidateDataAnnotations(message);
        return message;
    }

    static void ValidateDataAnnotations(object message)
    {
        var context = new ValidationContext(message, null, null);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(message, context, results, true);

        if (isValid)
        {
            Logger.Info("Validation succeeded for message: " + message);
            return;
        }

        var errorMessage = new StringBuilder();
        var error = string.Format("Validation failed for message {0}, with the following error/s: " + Environment.NewLine,message);
        errorMessage.Append(error);

        foreach (var validationResult in results)
        {
            errorMessage.Append(validationResult.ErrorMessage + Environment.NewLine);
        }

        Logger.Error(errorMessage.ToString());
        throw new Exception(errorMessage.ToString());
    }
}