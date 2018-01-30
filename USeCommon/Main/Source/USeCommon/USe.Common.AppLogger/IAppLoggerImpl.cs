using System;
using System.Text;

namespace USe.Common.AppLogger
{
	/// <summary>
	/// Ӧ�ó�����־�ڲ�ʵ����Ľӿڡ�
	/// </summary>
	public interface IAppLoggerImpl : IDisposable
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
		/// <returns>ˢ�³ɹ�����־��</returns>
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
		/// <param name="lineFeed">���б�־��</param>
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
		/// <param name="lineFeed">���б�־��</param>
		/// <returns>
		/// ��־д��ɹ�����־��
		/// </returns>
		bool WriteEvent(LogEventType eventType, byte[] bytes, int startIndex, int count, bool lineFeed);
	}
}
