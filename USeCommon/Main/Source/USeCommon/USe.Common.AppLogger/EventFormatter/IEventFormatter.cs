using System;

namespace USe.Common.AppLogger.EventFormatter
{
	/// <summary>
	/// ��־�¼���Ϣ��ʽ���ӿڡ�
	/// </summary>
	public interface IEventFormatter
	{
		/// <summary>
		/// ʹ��ָ������־�¼����͸�ʽ�������¼���Ϣ��¼��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��ʽ���������¼���Ϣ��¼�ַ�����
		/// </returns>
		string Format(LogEventType eventType, string eventText, bool lineFeed);
	}
}
