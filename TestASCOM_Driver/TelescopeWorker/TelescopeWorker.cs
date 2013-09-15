using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker
{
    class TelescopeWorker
    {
        public readonly static TelescopeWorker Worker = new TelescopeWorker();
        private BackgroundWorker bgWorker = new BackgroundWorker(); 

        private TelescopeWorker()
        {
            bgWorker.DoWork += BgWorkerOnDoWork;
            bgWorker.ProgressChanged += BgWorkerOnProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorkerOnRunWorkerCompleted;
            bgWorker.RunWorkerAsync();
        }

        private void BgWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            throw new System.NotImplementedException();
        }

        private void BgWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            throw new System.NotImplementedException();
        }

        private void BgWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            throw new System.NotImplementedException();
        }
    }
}
