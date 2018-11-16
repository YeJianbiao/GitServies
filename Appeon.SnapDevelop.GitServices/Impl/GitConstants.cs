using LibGit2Sharp;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitConstants
    {

        public static string Account { get; set; }

        public static string Password { get; set; }

        public static string ProjectPath { get; set; }

        public static string Name { get; set; } = "Appeon";

        public static string Email { get; set; } = "appeon@DESKTOP-VLBLSV1";

        public static Identity Identity = new Identity(Name, Email);

        public static void Setting()
        {

        }

    

}
}
