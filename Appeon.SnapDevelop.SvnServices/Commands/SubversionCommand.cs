using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appeon.SnapDevelop.VersionControlServices;


namespace Appeon.SnapDevelop.SvnServices
{
    internal abstract class SubversionCommand
    {

        #region register command server
        private static Dictionary< CommandType, SubversionCommand> _collection = new Dictionary<CommandType, SubversionCommand>();
        public static void RegistCommand<T>(CommandType type) where T : SubversionCommand, new()
        {
            SubversionCommand cmd = _collection.ContainsKey(type) ? _collection[type] : null;
            if (cmd == null)
            {
                _collection.Add( type, new T() );
            }
        }
        public static SubversionCommand GetCommand(  CommandType type )
        {
            if (!_collection.ContainsKey(type)) throw new ArgumentOutOfRangeException();
                        
            return _collection[type];
        }
        #endregion


        public ISvnCommand SvnCommand
        {
            get
            {
                if (Model == 0)
                {
                    return new SvnExternalWrapper();
                }
                else 
                {
                    return new SvnClientWrapper();
                }
                
            }
        }
        internal int Model { get;} = 1;
        public virtual void CallbackInvoked() {

            Console.WriteLine("a");
        }
        
        public virtual SvnExecuteResult Run(SvnFileArgs args=null ) { return default(SvnExecuteResult); }

        struct ProjectEntry
        {
            string fileName;
            long size;
            DateTime writeTime;

            public ProjectEntry(FileInfo file)
            {
                fileName = file.FullName;
                if (file.Exists)
                {
                    size = file.Length;
                    writeTime = file.LastWriteTime;
                }
                else
                {
                    size = -1;
                    writeTime = DateTime.MinValue;
                }
            }

            public bool HasFileChanged()
            {
                FileInfo file = new FileInfo(fileName);
                long newSize;
                DateTime newWriteTime;
                if (file.Exists)
                {
                    newSize = file.Length;
                    newWriteTime = file.LastWriteTime;
                }
                else
                {
                    newSize = -1;
                    newWriteTime = DateTime.MinValue;
                }
                return size != newSize || writeTime != newWriteTime;
            }
        }

        #region //Authentication
        public bool IsAuthorizationEnabled { get; set; }
        public bool IsSharpSvnUIBinding { get; set; }

        public string UserName { get; set; }
        public string PassWord { get; set; }

        public string RemoteUri { get; set; }
        public string LocalUri { get; set; }
        #endregion
    }
}
