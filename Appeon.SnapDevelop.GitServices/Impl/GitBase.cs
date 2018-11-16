using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Appeon.SnapDevelop.GitServices.Impl
{
    internal class GitBase : IDisposable
    {

            private readonly List<string> directories = new List<string>();

            public GitBase()
            {
        
            }

            static GitBase()
            {
                
            }



            public virtual void Dispose()
            {
                
            }

            protected static void InconclusiveIf(Func<bool> predicate, string message)
            {
                if (!predicate())
                {
                    return;
                }

                throw new Exception(message);
            }

            protected void RequiresDotNetOrMonoGreaterThanOrEqualTo(System.Version minimumVersion)
            {
                Type type = Type.GetType("Mono.Runtime");

                if (type == null)
                {
                    // We're running on top of .Net
                    return;
                }

                MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);

                if (displayName == null)
                {
                    throw new InvalidOperationException("Cannot access Mono.RunTime.GetDisplayName() method.");
                }

                var version = (string)displayName.Invoke(null, null);

                System.Version current;

                try
                {
                    current = new System.Version(version.Split(' ')[0]);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Cannot parse Mono version '{0}'.", version), e);
                }

                InconclusiveIf(() => current < minimumVersion,
                    string.Format(
                        "Current Mono version is {0}. Minimum required version to run this test is {1}.",
                        current, minimumVersion));
            }

            protected static void AssertValueInConfigFile(string configFilePath, string regex)
            {
                var text = File.ReadAllText(configFilePath);
                var r = new Regex(regex, RegexOptions.Multiline).Match(text);
                //Assert.True(r.Success, text);
            }

            protected void CreateConfigurationWithDummyUser(Repository repo, string name, string email)
            {
                Configuration config = repo.Config;
                {
                    if (name != null)
                    {
                        config.Set("user.name", name);
                    }

                    if (email != null)
                    {
                        config.Set("user.email", email);
                    }
                }
            }




            protected static void EnableRefLog(IRepository repository, bool enable = true)
            {
                repository.Config.Set("core.logAllRefUpdates", enable);
            }

            public static void CopyStream(Stream input, Stream output)
            {
                var buffer = new byte[8 * 1024];
                int len;
                while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, len);
                }
            }

            public static bool StreamEquals(Stream one, Stream two)
            {
                int onebyte, twobyte;

                while ((onebyte = one.ReadByte()) >= 0 && (twobyte = two.ReadByte()) >= 0)
                {
                    if (onebyte != twobyte)
                        return false;
                }

                return true;
            }

            public static bool AssertBelongsToARepository<T>(IRepository repo, T instance)
                where T : IBelongToARepository
            {
                return repo== ((IBelongToARepository)instance).Repository;
            }

            protected void CreateAttributesFile(IRepository repo, string attributeEntry)
            {
                //Touch(repo.Info.WorkingDirectory, ".gitattributes", attributeEntry);
            }
        }
    
}
