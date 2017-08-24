using System;
using System.Diagnostics;

namespace TestWell.Mongo.Processes
{
    public interface IProcessStarter : IDisposable
    {
        #region Public Methods

        IProcess Start(ProcessStartInfo processStartInfo);

        #endregion Public Methods
    }
}
