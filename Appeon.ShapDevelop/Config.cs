using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appeon.ShapDevelop
{
    public class Config
    {


        public const string RemotePath = "https://github.com/YeJianbiao/AppeonTest";

        public const string ProjectPath = @"C:\Code\Appeon\TestCode\TestApp";

        public const string UserName = "yejianbiao";

        public const string Password = "kfwgisfi168";

        public const string Email = "yejianbiao@163.com";

        public const string Name = "yejianbiao";

        public static Identity Identity = new Identity(Name, Email);

        public static readonly Signature Signature = new Signature(Identity, new DateTimeOffset(2011, 06, 16, 10, 58, 27, TimeSpan.FromHours(2)));


    }
}
