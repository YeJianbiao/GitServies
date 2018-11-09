using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Appeon.SnapDevelop.SvnServices
{
    internal class UnignoreCommand : SubversionCommand
    {
        public override SvnExecuteResult Run(SvnFileArgs args)
        {
            SvnExecuteResult result = new SvnExecuteResult() { ErrorCode = 0 };

            try
            {
                var filename = args.FileName;
                using (SvnClientWrapper client = new SvnClientWrapper())
                {
                    string propertyValue = client.GetPropertyValue(Path.GetDirectoryName(filename), "svn:ignore");
                    if (propertyValue != null)
                    {
                        string shortFileName = Path.GetFileName(filename);
                        StringBuilder b = new StringBuilder();
                        using (StringReader r = new StringReader(propertyValue))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                if (!string.Equals(line, shortFileName, StringComparison.OrdinalIgnoreCase))
                                {
                                    b.AppendLine(line);
                                }
                            }
                        }
                        client.SetPropertyValue(Path.GetDirectoryName(filename), "svn:ignore", b.ToString());

                        CallbackInvoked();
                    }
                }
            }catch ( Exception ex)
            {
                result.ErrorCode = -3;
                result.Message = ex.Message;
            }

            return result;
        }
    }
     
}
