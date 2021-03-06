﻿using System;
using System.Linq;
using System.Text;
using Markdown.Infrastructure;

namespace Markdown.SubstringHandlers
{
    public class FirstWorkHandler : ISubstringHandler
    {
        private readonly ISubstringHandler[] handlers;

        private Func<StringReader, bool> isEndOfSubstring;

        public FirstWorkHandler(params ISubstringHandler[] handlers)
        {
            this.handlers = handlers;
        }

        public string HandleSubstring(StringReader reader)
        {
            if (!CanHandle(reader))
                throw new InvalidOperationException("No work readers");

            var substringBuilder = new StringBuilder();
            while (!reader.AtEndOfString && !IsEndOfSubstring(reader))
            {
                var firsWorkHandler = handlers.First(handler => handler.CanHandle(reader));
                var substringPart = firsWorkHandler.HandleSubstring(reader);
                substringBuilder.Append(substringPart);
            }

            return substringBuilder.ToString();
        }

        public bool CanHandle(StringReader reader)
        {
            return handlers.Any(handler => handler.CanHandle(reader));
        }

        public static FirstWorkHandler CreateFrom(params ISubstringHandler[] handlers)
        {
            return new FirstWorkHandler(handlers);
        }

        public FirstWorkHandler WithStopRule(Func<StringReader, bool> predicate)
        {
            return new FirstWorkHandler(handlers) {isEndOfSubstring = predicate};
        }

        public string HandleString(string str)
        {
            return HandleSubstring(new StringReader(str));
        }

        private bool IsEndOfSubstring(StringReader reader)
        {
            return isEndOfSubstring?.Invoke(reader) ?? false;
        }
    }
}