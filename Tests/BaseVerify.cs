using System.Threading.Tasks;
using DiffEngine;
using VerifyNUnit;
using VerifyTests;

namespace NServiceBus.Mailer
{
    public abstract class BaseVerifyTest
    {
        public BaseVerifyTest()
        {
            DiffTools.UseOrder(DiffTool.Rider, DiffTool.VisualStudioCode, DiffTool.VisualStudio);
        }
    }
}