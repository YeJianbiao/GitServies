using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appeon.ShapDevelop
{
    public class Config
    {


        public const string RemotePath = "http://admin@172.16.6.150:8089/r/TestApp.git";

        public const string ProjectPath = @"E:\GithubTest\ConsoleApp";

        public const string UserName = "admin";

        public const string Password = "admin123";

        public const string Email = "yejianbiao@163.com";

        public const string Name = "yejianbiao";

        public static Identity Identity = new Identity(Name, Email);

        public static readonly Signature Signature = new Signature(Identity, new DateTimeOffset(2011, 06, 16, 10, 58, 27, TimeSpan.FromHours(2)));


    }
}
