using System.Diagnostics;

namespace TestWell.Mongo.Processes
{
    public interface IProcess
    {
        #region Public Properties

        int Id { get; }
        ProcessStartInfo StartInfo { get; }

        #endregion Public Properties

        #region Public Methods

        void Kill();

        void WaitForExit();

        #endregion Public Methods
    }
}
