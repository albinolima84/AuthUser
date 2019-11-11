﻿namespace Sky.Auth.Command.Commands
{
    public class PhoneNumberCommand
    {
        public string DDD { get; }
        public string Number { get; }

        public PhoneNumberCommand(string ddd, string number)
        {
            DDD = ddd;
            Number = number;
        }
    }
}