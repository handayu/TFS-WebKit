using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using USe.Common;

namespace OuterMarketDataStore
{
    class MQTTStoreageViewModel : USeBaseViewModel
    {
        #region member
        private string m_name = string.Empty;
        private bool m_enable = false;
        private Image m_enableIcon = null;
        private int m_errorStoreCount = 0;
        private int m_storeCount = 0;
        private int m_unStoreCount = 0;
        #endregion

        #region property
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name != value)
                {
                    m_name = value;
                    SetProperty(() => this.Name);
                }
            }
        }

        public int ErrorStoreCount
        {
            get { return m_errorStoreCount; }
            set
            {
                if (m_errorStoreCount != value)
                {
                    m_errorStoreCount = value;
                    SetProperty(() => this.ErrorStoreCount);
                }
            }
        }

        public int StoreCount
        {
            get { return m_storeCount; }
            set
            {
                if (m_storeCount != value)
                {
                    m_storeCount = value;
                    SetProperty(() => this.StoreCount);
                }
            }
        }

        public int UnstoreCount
        {
            get { return m_unStoreCount; }
            set
            {
                if (m_unStoreCount != value)
                {
                    m_unStoreCount = value;
                    SetProperty(() => this.UnstoreCount);
                }
            }
        }

        #endregion
    }
}
