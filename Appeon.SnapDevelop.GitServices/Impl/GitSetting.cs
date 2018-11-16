using LibGit2Sharp;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitSetting
    {

        public void InitRepository(string projectPath)
        {
            GitConstants.ProjectPath = Repository.Init(projectPath, false);
            SetConfiguration();
        }

        public void SettingIdentity(string name, string email)
        {
            GitConstants.Name = name;
            GitConstants.Email = email;
            GitConstants.Identity = new Identity(name, email);
            SetConfiguration();
        }

        public void SettingAccount(string name, string pwd)
        {
            GitConstants.Account = name;
            GitConstants.Password = pwd;
        }

        private void SetConfiguration()
        {
            if (string.IsNullOrEmpty(GitConstants.ProjectPath))
            {
                return;
            }
            using (var repo = new Repository(GitConstants.ProjectPath))
            {
                Configuration config = repo.Config;
                if (!string.IsNullOrEmpty(GitConstants.Name))
                {
                    config.Set("user.name", GitConstants.Name);
                }

                if (!string.IsNullOrEmpty(GitConstants.Email))
                {
                    config.Set("user.email", GitConstants.Email);
                }
            }

        }

    }
}
