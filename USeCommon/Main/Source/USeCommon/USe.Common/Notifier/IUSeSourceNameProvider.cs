namespace USe.Common
{
    /// <summary>
	/// USe来源者名称提供者接口。
	/// </summary>
	public interface IUSeSourceNameProvider
    {
        /// <summary>
        /// 获取来源者的名称字符串。
        /// </summary>
        string SourceName { get; }
    }
}
