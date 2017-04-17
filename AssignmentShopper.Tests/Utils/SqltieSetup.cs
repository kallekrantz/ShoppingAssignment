using Microsoft.Data.Sqlite;

namespace AssignmentShopper.Tests.Utils {
    public class ManagedSqliteConnection : SqliteConnection, System.IDisposable
    {

        public ManagedSqliteConnection(string connection_string) : base(connection_string) {
            Open();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void DisposeMethod() {
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeMethod();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }
        void System.IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}