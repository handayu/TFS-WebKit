using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using USe.TradeDriver.Common;
using System.Diagnostics;

using USe.Common.AppLogger;
using MySql.Data.MySqlClient;
using System.IO;
using USe.Common;
using USe.Common.Manager;

namespace HistoryKLineImport
{
    abstract class ImportServices
    {
        #region
        protected string m_dbConStr = string.Empty;
        protected string m_alphaDBName = string.Empty;
        protected IAppLogger m_eventLogger = null;
        #endregion

        public ImportServices(IAppLogger eventLogger)
        {
            m_eventLogger = eventLogger;
        }

        #region methods
        public virtual bool Initialize()
        {
            try
            {
                m_dbConStr = ConfigurationManager.ConnectionStrings["MarketDataDB"].ConnectionString;
            }
            catch (Exception ex)
            {
                string error = "Not found dbConnString MarketDataDB";
                m_eventLogger.WriteError(error);
                USeConsole.WriteLine(error);
                return false;
            }

            m_alphaDBName = ConfigurationManager.AppSettings["AlphaDBName"];
            if (string.IsNullOrEmpty(m_alphaDBName))
            {
                string error = "Not found AlphaDBName";
                m_eventLogger.WriteError(error);
                USeConsole.WriteLine(error);
                return false;
            }

            return true;
        }

        public abstract bool Run();

        protected USeProductManager CreateVarietiesManager()
        {
            USeProductManager manager = new USeProductManager(m_dbConStr, m_alphaDBName);
            manager.Initialize();

            return manager;
        }
        #endregion
    }
}