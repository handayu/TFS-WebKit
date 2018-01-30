using System;
using System.Text;

namespace USe.Common.AppLogger
{
	/// <summary>
	/// Ӧ�ó�����־�ӿڡ�
	/// </summary>
	public interface IAppLogger : IDisposable
	{
		/// <summary>
		/// ��ȡӦ�ó�����־�Ķ������ơ�
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ��ȡ/����Ӧ�ó�����־�ı����ʽ��
		/// </summary>
		Encoding Encoding { get; set; }

		/// <summary>
		/// ��ȡӦ�ó�����־���̰߳�ȫ��־��
		/// </summary>
		bool IsThreadSafe { get; }

		/// <summary>
		/// ˢ��Ӧ�ó�����־�����������������д����־�洢��
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool Flush();

		/// <summary>
		/// Ӧ�ó�����־д��س����С�
		/// </summary>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool LineFeed();

		/// <summary>
		/// Ӧ�ó�����־д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="eventText">��־�¼���¼�ı���</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteEvent(LogEventType eventType, string eventText, bool lineFeed);

		/// <summary>
		/// Ӧ�ó�����־д��һ���¼���Ϣ��
		/// </summary>
		/// <param name="eventType">��־�¼����͡�</param>
		/// <param name="bytes">��־�¼���¼���ֽ����л�������</param>
		/// <param name="startIndex">��־�¼���¼���ֽ����л����������ʼ������</param>
		/// <param name="count">��־�¼���¼���ֽ�������</param>
		/// <param name="lineFeed">�س����б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Critical�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteCritical(string text);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Error�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteError(string text);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Warning�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteWarning(string text);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Information�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteInformation(string text);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Verbose�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteVerbose(string text);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Notice�����¼���Ϣ��
		/// </summary>
		/// <param name="text">��־�¼���¼�ı���</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteNotice(string text);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Inbound�����¼���Ϣ��
		/// </summary>
		/// <param name="message">��վ��Ϣ�ַ�����</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteInbound(string message);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Inbound�����¼���Ϣ��
		/// </summary>
		/// <param name="bytes">��д�����־��Ϣ�ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteInbound(byte[] bytes, int startIndex, int count);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Outbound�����¼���Ϣ��
		/// </summary>
		/// <param name="message">��վ��Ϣ�ַ�����</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteOutbound(string message);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Outbound�����¼���Ϣ��
		/// </summary>
		/// <param name="bytes">��д�����־��Ϣ�ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteOutbound(byte[] bytes, int startIndex, int count);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Message�����¼���Ϣ��
		/// </summary>
		/// <param name="message">��Ϣ�ַ�����</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteMessage(string message);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Message�����¼���Ϣ��
		/// </summary>
		/// <param name="bytes">��д�����־��Ϣ�ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteMessage(byte[] bytes, int startIndex, int count);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Audit�����¼���Ϣ��
		/// </summary>
		/// <param name="message">��Ϣ�ַ�����</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteAudit(string message);

		/// <summary>
		/// Ӧ�ó�����־д��һ��Audit�����¼���Ϣ��
		/// </summary>
		/// <param name="bytes">��д�����־��Ϣ�ֽ����л�������</param>
		/// <param name="startIndex">��д���ֽ������ڻ������е���ʼ������</param>
		/// <param name="count">��д���ֽ����е�������</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteAudit(byte[] bytes, int startIndex, int count);
	}
}
